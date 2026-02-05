using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.Util.Log;
using MySql.Data.MySqlClient;
using AvisSealer.Server;
using System.Diagnostics;
using System.IO;

namespace AvisSealer
{
    public class ServerManager
    {
        private CommonRepository _repository = null;
        private ProgramConfig _program = null;

        public static string CONNECT_DB = string.Empty;
        public static string CONNECT_PROC = string.Empty;
        private string DATABASE_DB_NAME = "avis_lpr_db";
        private string DATABASE_PROC_NAME = "avis_lpr_proc";
        private string ROOT_ID = "root";
        private string ROOT_PWD = "root";

        private Task _serverTask = null;
        private int _processDelay_ms = 100;
        private Task _serverChecker = null;
        private int _serverCheckInterval_ms = 5000;
        private int _SERVER_TIMEOUT_INTERVAL = 100;

        private object lockServerConnectObject = new object();

        private bool _isSavingImage = false;
        private bool _isSavingData = false;

        private int _deleteHour = -1;

        // VIP 모드
        public static string VIP_MODE_INSP_TITLE = "LIN 펌웨어 업데이트 확인";

        public ServerManager(int processDelay_ms)
        {
            _repository = CommonRepository.Instance();
            _program = ProgramConfig.Instance();

            _processDelay_ms = processDelay_ms;

            this.ROOT_ID = "root";
            this.ROOT_PWD = "123123";

            ServerManager.CONNECT_DB = string.Format("Server={0};Port={1};Database={2};Uid={3};Pwd={4};Connect Timeout=3;", _program.serverIp, _program.serverPort, DATABASE_PROC_NAME, ROOT_ID, ROOT_PWD);
            ServerManager.CONNECT_PROC = string.Format("Server={0};Port={1};Database={2};Uid={3};Pwd={4};Connect Timeout=10;", _program.serverIp, _program.serverPort, DATABASE_PROC_NAME, ROOT_ID, ROOT_PWD);
        }

        public void DoStart()
        {
            // 모니터링
            _serverChecker = Task.Run(() => _ServerMonitoring());

            // 업로드
            _serverTask = Task.Run(() => _UploadProcess());


            _DownloadInformation();

            Task.Run(() => _UploadSwStatus());
        }

        public void _UploadSwStatus()
        {
            CLog.Log("[ServerManager] SaveStatus Process");

            System.Threading.Thread.Sleep(5000);

            PerformanceCounter cpu = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            PerformanceCounter ram = new PerformanceCounter("Memory", "Available MBytes");
            PerformanceCounter prcessCpu = new PerformanceCounter("Process", "% Processor Time", Process.GetCurrentProcess().ProcessName);

            DateTime lastUpladTime = DateTime.MinValue;
            while (!_repository.IsCloseProgram)
            {
                if (DateTime.Now.Subtract(lastUpladTime).TotalHours > 6)
                {
                    lastUpladTime = DateTime.Now;

                    //SpreadSheet 저장
                    string sheetId = _program.spreadSheetId;
                    if (string.IsNullOrEmpty(sheetId)) sheetId = "133DqF7_5IS5VlKhfM66RJNRdOs9SOiczlD66-ZYOac0";

                    if (!string.IsNullOrEmpty(sheetId))
                    {
                        float percentC = 0, percentD = 0;

                        try
                        {
                            // 드라이브 정보에 엑세스하여 모든 논리 드라이브의 이름을 가져옴
                            System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
                            foreach (System.IO.DriveInfo drive in drives)
                            {
                                if (drive.Name.Contains("C"))
                                {
                                    // 드라이브 전체 용량
                                    int maxc = (int)(drive.TotalSize / 1000000);
                                    // 사용중인 용량 ( 전체 용량 - 사용 가능한 용량 )
                                    int cst = (int)((drive.TotalSize - drive.AvailableFreeSpace) / 1000000);

                                    percentC = (float)((float)cst / (float)maxc) * 100;
                                }

                                if (drive.Name.Contains("D"))
                                {
                                    // 드라이브 전체 용량
                                    int maxc = (int)(drive.TotalSize / 1000000);
                                    // 사용중인 용량 ( 전체 용량 - 사용 가능한 용량 )
                                    int cst = (int)((drive.TotalSize - drive.AvailableFreeSpace) / 1000000);

                                    percentD = (float)((float)cst / (float)maxc) * 100;
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }


                        try
                        {
                            string sheetName = string.Format("[Software Status]");
                            List<object> colNames = new List<object>
                            {
                                "시간", "S/W 버젼", "동작시간", "서버", "서버주소", "전류센서1", "전류센서1 ID", "전류센서2", "전류센서2 ID", "조도센서1", "조도센서1 ID", "조도센서2", "조도센서2 ID", "조도센서3", "조도센서3 ID", "조도센서4", "조도센서4 ID", "PUC", "PUC PORT", "바이패스", "용량(C)", "용량(D)", "CPU 사용량", "S/W CPU 사용량", "RAM 사용량"
                            };
                            List<object> datas = new List<object>
                            {
                                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                                DateTime.Now.Subtract(Program.startTime).TotalHours.ToString("0.0")+"h",
                                _repository.IsConnectServer, string.Format("{0}:{1}", _program.serverIp, _program.serverPort),
                                "false(null)",
                                "false(null)",
                                "false(null)",
                                "false(null)",
                                "false(null)",
                                "false(null)",
                                (_repository.puc != null)?_repository.puc.isConnect.ToString() : "false(null)", (_repository.puc != null)?_repository.puc.comPort : "false(null)", _program.bypass,
                                percentC.ToString("0.0") + "%", percentD.ToString("0.0") + "%",
                                cpu.NextValue().ToString("0.0") + "%", prcessCpu.NextValue().ToString("0.0") + "%", ram.NextValue().ToString("0.0") + "MB"
                            };

                            Tools.Device.GoogleDrive.WriteSpreadSheet(sheetId, sheetName, colNames, datas);
                        }catch(Exception)
                        {
                        }
                    }
                }

                if (!_repository.IsCloseProgram)
                {
                    System.Threading.Thread.Sleep(5000);
                }
            }

            CLog.Log("[ServerManager] SaveStatus ByeBye~!");
        }

        public void DoStop()
        {
        }

        private void _DownloadInformation()
        {
            Task.Run(() =>
            {
                if (_repository.standard == null) _repository.standard = new SealerStandard();


                bool isServerDownload = true;
                bool isUploadData = false;

                if (isServerDownload/* && _repository.IsConnectServer*/)
                {
                    _GetServerStandards();
                }

                // 실패시 기본값으로 설정
                if (_repository.standard == null || _repository.standard.listItemCount <= 0)
                {
                    _MakeDefaultStd(ref _repository.standard);
                    _repository.standard.Model = CommonRepository.DEFAULT_MODEL_CODE;
                }

                if (isUploadData)
                {
                    SaveStandard(_repository.standard);

                    _repository.standard.Clear();
                    _GetServerStandards();
                }


                // download last insp
                if (_repository.standard != null)
                {
                    if (_repository.lastResult != null) _repository.lastResult.Clear(); //마지막 검사결과 업데이트

                    if (!_GetServerLastResult())
                    {
                        if (_repository.lastResult == null) _repository.lastResult = new SealerResult();
                    }
                }

            });
        }


        #region ABOUT_SERVER
        private static bool _GetServerProductionInfo(int model, ref int prodSeq, ref int goalCnt, ref int currCnt, ref int okCnt, ref int ngCnt, ref int serialCode)
        {
            using (MySqlConnection conn = new MySqlConnection(ServerManager.CONNECT_DB))
            {
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    try
                    {
                        conn.Open();
                    }
                    catch (Exception e)
                    {
                        CLog.LogErr(e, "[ServerManager]서버 연결 중 에러가 발생하였습니다.");
                        return false;
                    }

                    try
                    {
                        string sqlCmd = string.Format("SELECT * from avis_lpr_db.production_info P WHERE P.Model = {0} ORDER BY P.ProdSeq DESC LIMIT 1;", (int)model);

                        cmd.CommandText = sqlCmd;
                        MySqlDataReader reader = cmd.ExecuteReader();
                        if (reader != null)
                        {
                            try
                            {
                                prodSeq = -1;
                                while (reader.Read())
                                {
                                    try
                                    {
                                        prodSeq = _GetDbInt(reader, "ProdSeq");
                                        goalCnt = _GetDbInt(reader, "GoalCnt");
                                        currCnt = _GetDbInt(reader, "CurrCnt");
                                        okCnt = _GetDbInt(reader, "OkCnt");
                                        ngCnt = _GetDbInt(reader, "NgCnt");
                                        serialCode = _GetDbInt(reader, "SerialCode");
                                    }
                                    catch (Exception) { }
                                }

                                if (prodSeq < 0) return false;

                                return true;
                            }
                            catch (Exception ex)
                            {
                                CLog.LogErr(ex, "[ServerManager] 검사설정 불러오는 중 에러가 발생하였습니다.");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        CLog.LogErr(e, "[ServerManager]서버 명령 수행 중 에러가 발생하였습니다.");
                    }
                }
            }

            return false;
        }

        private bool _GetServerStandards()
        {
            using (MySqlConnection conn = new MySqlConnection(ServerManager.CONNECT_DB))
            {
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    try
                    {
                        conn.Open();
                    }
                    catch (Exception e)
                    {
                        CLog.LogErr(e, "[ServerManager]서버 연결 중 에러가 발생하였습니다.");
                        return false;
                    }

                    try
                    {
                        string sqlCmd = string.Format("SELECT S.*, I.* FROM avis_lpr_db.standard S"
                            + " LEFT JOIN avis_lpr_db.standard_to_item M ON S.StdSeq = M.StdSeq"
                            + " LEFT JOIN avis_lpr_db.standard_item I ON M.ItemSeq = I.StdItemSeq ORDER BY I.StdItemSeq");

                        cmd.CommandText = sqlCmd;
                        MySqlDataReader reader = cmd.ExecuteReader();
                        if (reader != null)
                        {
                            try
                            {
                                _repository.standard = null;
                                while (reader.Read())
                                {
                                    try
                                    {
                                        int stdSeq = _GetDbInt(reader, "StdSeq");
                                        int model = _GetDbInt(reader, "Model");
                                        int itemSeq = _GetDbInt(reader, "StdItemSeq");
                                        string itemTitle = _GetDbString(reader, "Title");
                                        int cntPushBtn = _GetDbInt(reader, "ControlPushBtn");
                                        int virtualServer = _GetDbInt(reader, "VirtualServerConnect");
                                        int isCameraDetect = _GetDbInt(reader, "IsCameraDetect");
                                        int isBarcodeScan = _GetDbInt(reader, "IsBarcodeScan");
                                        double lightSen1 = _GetDbDouble(reader, "LightSensor1");
                                        double lightSen2 = _GetDbDouble(reader, "LightSensor2");
                                        double lightSen3 = _GetDbDouble(reader, "LightSensor3");
                                        double lightSen4 = _GetDbDouble(reader, "LightSensor4");
                                        double volSen1 = _GetDbDouble(reader, "VoltageSensor1");
                                        double volSen2 = _GetDbDouble(reader, "VoltageSensor2");
                                        int vehicleIndex = _GetDbInt(reader, "OutVehicleIndex");
                                        string vehicleNumber = _GetDbString(reader, "VehicleNumber");
                                        string vehicleType = _GetDbString(reader, "VehicleType");
                                        uint beforeDelay = (uint)_GetDbInt(reader, "InspBeforeDelay");
                                        uint afterDelay = (uint)_GetDbInt(reader, "InspAfterDelay");

                                        if (itemTitle == "출력 전원 검사1") itemTitle = "출력 전원 검사";
                                        if (itemTitle == "출력 전원 검사2") continue;

                                        if (_repository.standard == null) _repository.standard = new SealerStandard(stdSeq, model, new List<SealerStandardItem>());


                                        SealerStandardItem stdItem = _repository.standard.listItem.Find(i => i.SeqItem == itemSeq);
                                        if (stdItem == null)
                                        {
                                            _repository.standard.listItem.Add(new SealerStandardItem(itemSeq, itemTitle, cntPushBtn, virtualServer, isCameraDetect, isBarcodeScan, lightSen1, lightSen2, lightSen3, lightSen4, volSen1, volSen2, vehicleIndex, vehicleNumber, vehicleType, beforeDelay, afterDelay));
                                        }
                                        else
                                        {
                                            stdItem.Set(itemSeq, itemTitle, cntPushBtn, virtualServer, isCameraDetect, isBarcodeScan, lightSen1, lightSen2, lightSen3, lightSen4, volSen1, volSen2, vehicleIndex, vehicleNumber, vehicleType, beforeDelay, afterDelay);
                                        }
                                    }
                                    catch (Exception) { }
                                }

                                return true;
                            }
                            catch (Exception ex)
                            {
                                CLog.LogErr(ex, "[ServerManager] 검사설정 불러오는 중 에러가 발생하였습니다.");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        CLog.LogErr(e, "[ServerManager]서버 명령 수행 중 에러가 발생하였습니다.");
                    }
                }
            }

            return false;
        }

        private bool _GetServerLastResult()
        {
            using (MySqlConnection conn = new MySqlConnection(ServerManager.CONNECT_DB))
            {
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    try
                    {
                        conn.Open();
                    }
                    catch (Exception e)
                    {
                        CLog.LogErr(e, "[ServerManager]서버 연결 중 에러가 발생하였습니다.");
                        return false;
                    }

                    try
                    {
                        int lastResultSeq = -1;

                        string sqlCmd = "SELECT ResultSeq FROM avis_lpr_db.result ORDER BY ResultSeq DESC limit 1";
                        cmd.CommandText = sqlCmd;
                        MySqlDataReader reader = cmd.ExecuteReader();
                        if (reader != null)
                        {
                            try
                            {
                                if (reader.Read())
                                {
                                    lastResultSeq = _GetDbInt(reader, "ResultSeq");
                                }
                            }
                            catch (Exception) { }
                        }
                        reader.Close();

                        if (lastResultSeq < 0) return false;

                        sqlCmd = string.Format("SELECT R.*, I.* FROM avis_lpr_db.result R"
                            + " LEFT JOIN avis_lpr_db.result_to_item M ON R.ResultSeq = M.ResultSeq"
                            + " LEFT JOIN avis_lpr_db.result_item I ON M.ItemSeq = I.ItemSeq"
                            + " where {0} = R.ResultSeq ORDER BY R.ResultSeq", lastResultSeq);

                        cmd.CommandText = sqlCmd;
                        reader = cmd.ExecuteReader();
                        if (reader != null)
                        {
                            try
                            {
                                _repository.lastResult = null;
                                if (reader.Read())
                                {
                                    int ResultSeq = _GetDbInt(reader, "ResultSeq");
                                    DateTime StartTime = _GetDbDatetime(reader, "StartTime");
                                    DateTime EndTime = _GetDbDatetime(reader, "EndTime");
                                    int TotalResult = _GetDbInt(reader, "TotalResult");
                                    string Barcode = _GetDbString(reader, "Barcode");
                                    string OutBarcode = _GetDbString(reader, "OutBarcode");
                                    int Model = _GetDbInt(reader, "Model");
                                    string ResultInfo = _GetDbString(reader, "ResultInfo");
                                    int StdSeq = _GetDbInt(reader, "StdSeq");
                                    int DurationTotal = _GetDbInt(reader, "DurationTotal");

                                    if (string.IsNullOrEmpty(Barcode)) Barcode = SealerResult.DEFAULT_BARCODE;
                                    if (string.IsNullOrEmpty(OutBarcode)) OutBarcode = SealerResult.DEFAULT_BARCODE;

                                    if (_repository.lastResult == null)
                                    {
                                        _repository.lastResult = new SealerResult(0, ResultSeq, StartTime, EndTime, TotalResult, Barcode, OutBarcode, Model, StdSeq, ResultInfo, DurationTotal, string.Empty, new List<ResultItem>());

                                        if (_repository.standard.Model == Model) _repository.lastResult.std.Set(_repository.standard);
                                    }

                                    _repository.lastResult.listItem.Clear();
                                    do
                                    {
                                        try
                                        {
                                            int ItemSeq = _GetDbInt(reader, "ItemSeq");
                                            DateTime ItemStartTime = _GetDbDatetime(reader, "ItemStartTime");
                                            DateTime ItemEndTime = _GetDbDatetime(reader, "ItemEndTime");
                                            int ItemResult = _GetDbInt(reader, "ItemResult");
                                            int VirtualServerConnect = _GetDbInt(reader, "VirtualServerConnect");
                                            int IsCameraDetect = _GetDbInt(reader, "IsCameraDetect");
                                            string VehicleNumber = _GetDbString(reader, "VehicleNumber");
                                            string DetectInfo = _GetDbString(reader, "DetectInfo");
                                            double LightSensor1 = _GetDbDouble(reader, "LightSensor1");
                                            double LightSensor2 = _GetDbDouble(reader, "LightSensor2");
                                            double LightSensor3 = _GetDbDouble(reader, "LightSensor3");
                                            double LightSensor4 = _GetDbDouble(reader, "LightSensor4");
                                            double VoltageSensor1 = _GetDbDouble(reader, "VoltageSensor1");
                                            double VoltageSensor2 = _GetDbDouble(reader, "VoltageSensor2");
                                            string LiveCapturePath = _GetDbString(reader, "LiveCapturePath");
                                            string DetectCapturePath = _GetDbString(reader, "DetectCapturePath");
                                            int StdItemSeq = _GetDbInt(reader, "StdItemSeq");
                                            string ItemResultInfo = _GetDbString(reader, "ItemResultInfo");
                                            int DurationItem = _GetDbInt(reader, "DurationItem");

                                            ResultItem r = _repository.lastResult.listItem.Find(i => i.ItemSeq == ItemSeq);
                                            if (r == null)
                                            {
                                                ResultItem item = new ResultItem(ItemSeq, ItemStartTime, ItemEndTime, ItemResult, VirtualServerConnect, IsCameraDetect, VehicleNumber, DetectInfo,
                                                    LightSensor1, LightSensor2, LightSensor3, LightSensor4, VoltageSensor1, VoltageSensor2, LiveCapturePath, DetectCapturePath, StdItemSeq, ItemResultInfo, DurationItem);
                                                SealerStandardItem stdItem = _repository.standard.listItem.Find(i => i.SeqItem == StdItemSeq);
                                                if (stdItem != null) item.stdItem = stdItem;

                                                _repository.lastResult.listItem.Add(item);
                                            }
                                            else
                                            {
                                                SealerStandardItem stdItem = _repository.standard.listItem.Find(i => i.SeqItem == StdItemSeq);
                                                if (stdItem != null) r.stdItem = stdItem;

                                                r.Set(ItemSeq, ItemStartTime, ItemEndTime, ItemResult, VirtualServerConnect, IsCameraDetect, VehicleNumber, DetectInfo,
                                                    LightSensor1, LightSensor2, LightSensor3, LightSensor4, VoltageSensor1, VoltageSensor2, LiveCapturePath, DetectCapturePath, StdItemSeq, ItemResultInfo, DurationItem);
                                            }
                                        }
                                        catch (Exception) { }
                                    } while (reader.Read());
                                }


                                if (_repository.lastResult == null)
                                {
                                    _repository.lastResult = new SealerResult();
                                }

                                return true;
                            }
                            catch (Exception ex)
                            {
                                CLog.LogErr(ex, "[ServerManager] 마지막 검사 결과를 불러오는 중 에러가 발생하였습니다.");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        CLog.LogErr(e, "[ServerManager]서버 명령 수행 중 에러가 발생하였습니다.");
                    }
                }
            }

            return false;
        }

        public static SealerResults GetResults(int seq, DateTime startTime, DateTime endTime, CommonRepository.EnumInspResult result = CommonRepository.EnumInspResult.Unknown, string barcode = null, string outBarcode = null)
        {
            SealerResults results = new SealerResults();

            using (MySqlConnection conn = new MySqlConnection(ServerManager.CONNECT_DB))
            {
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    try
                    {
                        cmd.CommandTimeout = 10000;

                        conn.Open();
                    }
                    catch (Exception e)
                    {
                        CLog.LogErr(e, "[ServerManager]서버 연결 중 에러가 발생하였습니다.");
                        return null;
                    }

                    try
                    {
                        if (startTime == DateTime.MinValue) startTime = new DateTime(2000, 1, 1, 8, 0, 0);
                        if (endTime == DateTime.MinValue) endTime = new DateTime(2035, 12, 31, 23, 59, 59);

                        int rowCount = 99999;
                        int startOffset = 0;

                        string sqlCmd = string.Format("SELECT " +
                                                        "R.*, COUNT(DISTINCT M.ItemSeq) AS ItemCount " +
                                                      "FROM " +
                                                        "avis_lpr_db.result R " +
                                                        "LEFT JOIN avis_lpr_db.result_to_item M ON R.ResultSeq = M.ResultSeq " +
                                                      "WHERE " +
                                                        "{0}(R.StartTimeTimeStamp between UNIX_TIMESTAMP(\"{1}\") and UNIX_TIMESTAMP(\"{2}\")) and " +
                                                        "if ({3} <= 0, true, R.TotalResult = {3}) and " +
                                                        "if (\"{4}\" is null or \"{4}\" = \" \", true, R.Barcode like \"%{4}%\") and " +
                                                        "if (\"{7}\" is null or \"{7}\" = \" \", true, R.OutBarcode like \"%{7}%\") " +
                                                      "GROUP BY " +
                                                        "R.ResultSeq " +
                                                      "ORDER BY " +
                                                        "StartTimeTimeStamp desc " +
                                                      "LIMIT {5} offset {6};",
                                                      (seq >= 0) ? string.Format("({0} = R.ResultSeq) and ", seq) : "",
                                                      startTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                                      endTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                                      (result == CommonRepository.EnumInspResult.Unknown) ? -1 : (int)result,
                                                      barcode,
                                                      rowCount,
                                                      startOffset,
                                                      outBarcode);
                        cmd.CommandText = sqlCmd;

                        CommonRepository _rep = CommonRepository.Instance();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        if (reader != null)
                        {
                            try
                            {
                                while (reader.Read())
                                {
                                    try
                                    {
                                        int ResultSeq = _GetDbInt(reader, "ResultSeq");
                                        DateTime StartTime = _GetDbDatetime(reader, "StartTime");
                                        DateTime EndTime = _GetDbDatetime(reader, "EndTime");
                                        int TotalResult = _GetDbInt(reader, "TotalResult");
                                        string Barcode = _GetDbString(reader, "Barcode");
                                        string OutBarcode = _GetDbString(reader, "OutBarcode");
                                        int Model = _GetDbInt(reader, "Model");
                                        string ResultInfo = _GetDbString(reader, "ResultInfo");
                                        int StdSeq = _GetDbInt(reader, "StdSeq");
                                        int itemCount = _GetDbIong(reader, "ItemCount");
                                        int DurationTotal = _GetDbInt(reader, "DurationTotal");

                                        if (string.IsNullOrEmpty(Barcode)) Barcode = SealerResult.DEFAULT_BARCODE;
                                        if (string.IsNullOrEmpty(OutBarcode)) OutBarcode = SealerResult.DEFAULT_BARCODE;

                                        SealerResult r = new SealerResult(0, ResultSeq, StartTime, EndTime, TotalResult, Barcode, OutBarcode, Model, StdSeq, ResultInfo, DurationTotal, string.Empty, new List<ResultItem>());
                                        if (_rep.standard.Model == Model) r.std.Set(_rep.standard);

                                        // item 개수만큼 추가
                                        r.listItem.Clear();
                                        if (itemCount > 0)
                                        {
                                            for (int i = 0; i < itemCount; i++)
                                            {
                                                r.listItem.Add(new ResultItem());
                                            }
                                        }
                                        results.listResult.Add(r);
                                    }
                                    catch (Exception) { }
                                }

                                if (results.listInspResultCount > 0)
                                {
                                    results.listResult = results.listResult.OrderByDescending(i => i.startTime).ToList();
                                    foreach (SealerResult r in results.listResult)
                                    {
                                        if (r.listItemCount > 0)
                                        {
                                            r.listItem = r.listItem.OrderBy(i => i.ItemSeq).ToList();
                                        }
                                    }
                                }
                            }
                            catch (Exception) { }

                            return results;
                        }
                    }
                    catch (Exception e)
                    {
                        CLog.LogErr(e, "[ServerManager]서버 명령 수행 중 에러가 발생하였습니다.");
                    }
                }
            }

            return null;
        }

        public static SealerResult GetResultDetail(int in_resultSeq)
        {
            if (in_resultSeq < 0) return null;

            using (MySqlConnection conn = new MySqlConnection(ServerManager.CONNECT_DB))
            {
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    try
                    {
                        cmd.CommandTimeout = 10000;

                        conn.Open();
                    }
                    catch (Exception e)
                    {
                        CLog.LogErr(e, "[ServerManager]서버 연결 중 에러가 발생하였습니다.");
                        return null;
                    }

                    try
                    {
                        string sqlCmd = string.Format("SELECT " +
                                                                "R.*, I.* " +
                                                               "FROM " +
                                                                "avis_lpr_db.result R " +
                                                                "LEFT JOIN avis_lpr_db.result_to_item M ON R.ResultSeq = M.ResultSeq " +
                                                                "LEFT JOIN avis_lpr_db.result_item I ON M.ItemSeq = I.ItemSeq " +
                                                               "WHERE " +
                                                                "{0} = R.ResultSeq;",
                                                                in_resultSeq);
                        cmd.CommandText = sqlCmd;

                        CommonRepository _rep = CommonRepository.Instance();
                        SealerResult lprResult = null;
                        MySqlDataReader reader = cmd.ExecuteReader();
                        if (reader != null)
                        {
                            try
                            {
                                while (reader.Read())
                                {
                                    int ResultSeq = _GetDbInt(reader, "ResultSeq");
                                    DateTime StartTime = _GetDbDatetime(reader, "StartTime");
                                    DateTime EndTime = _GetDbDatetime(reader, "EndTime");
                                    int TotalResult = _GetDbInt(reader, "TotalResult");
                                    string Barcode = _GetDbString(reader, "Barcode");
                                    string OutBarcode = _GetDbString(reader, "OutBarcode");
                                    int Model = _GetDbInt(reader, "Model");
                                    string ResultInfo = _GetDbString(reader, "ResultInfo");
                                    int StdSeq = _GetDbInt(reader, "StdSeq");
                                    int DurationTotal = _GetDbInt(reader, "DurationTotal");

                                    if (string.IsNullOrEmpty(Barcode)) Barcode = SealerResult.DEFAULT_BARCODE;
                                    if (string.IsNullOrEmpty(OutBarcode)) OutBarcode = SealerResult.DEFAULT_BARCODE;


                                    lprResult = new SealerResult(0, ResultSeq, StartTime, EndTime, TotalResult, Barcode, OutBarcode, Model, StdSeq, ResultInfo, DurationTotal, string.Empty, new List<ResultItem>());
                                    if (_rep.standard.Model == Model) lprResult.std.Set(_rep.standard);

                                    lprResult.listItem.Clear();
                                    do
                                    {
                                        try
                                        {
                                            int ItemSeq = _GetDbInt(reader, "ItemSeq");
                                            DateTime ItemStartTime = _GetDbDatetime(reader, "ItemStartTime");
                                            DateTime ItemEndTime = _GetDbDatetime(reader, "ItemEndTime");
                                            int ItemResult = _GetDbInt(reader, "ItemResult");
                                            int VirtualServerConnect = _GetDbInt(reader, "VirtualServerConnect");
                                            int IsCameraDetect = _GetDbInt(reader, "IsCameraDetect");
                                            string VehicleNumber = _GetDbString(reader, "VehicleNumber");
                                            string DetectInfo = _GetDbString(reader, "DetectInfo");
                                            double LightSensor1 = _GetDbDouble(reader, "LightSensor1");
                                            double LightSensor2 = _GetDbDouble(reader, "LightSensor2");
                                            double LightSensor3 = _GetDbDouble(reader, "LightSensor3");
                                            double LightSensor4 = _GetDbDouble(reader, "LightSensor4");
                                            double VoltageSensor1 = _GetDbDouble(reader, "VoltageSensor1");
                                            double VoltageSensor2 = _GetDbDouble(reader, "VoltageSensor2");
                                            string LiveCapturePath = _GetDbString(reader, "LiveCapturePath");
                                            string DetectCapturePath = _GetDbString(reader, "DetectCapturePath");
                                            int StdItemSeq = _GetDbInt(reader, "StdItemSeq");
                                            string ItemResultInfo = _GetDbString(reader, "ItemResultInfo");
                                            int durationTotal = _GetDbInt(reader, "DurationTotal");
                                            int durationItem = _GetDbInt(reader, "DurationItem");

                                            ResultItem r = lprResult.listItem.Find(i => i.ItemSeq == ItemSeq);
                                            if (r == null)
                                            {
                                                ResultItem item = new ResultItem(ItemSeq, ItemStartTime, ItemEndTime, ItemResult, VirtualServerConnect, IsCameraDetect, VehicleNumber, DetectInfo,
                                                    LightSensor1, LightSensor2, LightSensor3, LightSensor4, VoltageSensor1, VoltageSensor2, LiveCapturePath, DetectCapturePath, StdItemSeq, ItemResultInfo, durationItem);
                                                if (lprResult.std != null)
                                                {
                                                    SealerStandardItem stdItem = lprResult.std.listItem.Find(i => i.SeqItem == StdItemSeq);
                                                    if (stdItem != null) item.stdItem = stdItem;
                                                }

                                                lprResult.listItem.Add(item);
                                            }
                                            else
                                            {
                                                r.Set(ItemSeq, ItemStartTime, ItemEndTime, ItemResult, VirtualServerConnect, IsCameraDetect, VehicleNumber, DetectInfo,
                                                    LightSensor1, LightSensor2, LightSensor3, LightSensor4, VoltageSensor1, VoltageSensor2, LiveCapturePath, DetectCapturePath, StdItemSeq, ItemResultInfo, durationItem);

                                                if (lprResult.std != null)
                                                {
                                                    SealerStandardItem stdItem = lprResult.std.listItem.Find(i => i.SeqItem == StdItemSeq);
                                                    if (stdItem != null) r.stdItem = stdItem;
                                                }
                                            }
                                        }
                                        catch (Exception) { }
                                    } while (reader.Read());

                                    if (lprResult.listItemCount > 0)
                                    {
                                        lprResult.listItem = lprResult.listItem.OrderBy(i => i.ItemSeq).ToList();
                                    }
                                }
                            }
                            catch (Exception) { }

                            return lprResult;
                        }
                    }
                    catch (Exception e)
                    {
                        CLog.LogErr(e, "[ServerManager]서버 명령 수행 중 에러가 발생하였습니다. (detail)");
                    }
                }
            }

            return null;
        }


        public static SealerResult GetResult(int seq, DateTime startTime, DateTime endTime, CommonRepository.EnumInspResult result = CommonRepository.EnumInspResult.Unknown, string barcode = null)
        {
            SealerResults results = GetResults(seq, startTime, endTime, result, barcode);

            if (results != null && results.listInspResultCount > 0)
            {
                return results.listResult[0];
            }

            return null;
        }

        public static SealerResult GetResult(int seq = int.MinValue)
        {
            return GetResult(seq, DateTime.MinValue, DateTime.MinValue);
        }

        public static bool SaveResult(SealerResult destResult)
        {
            if (destResult == null || destResult.listItem == null || destResult.listItem.Count <= 0) return false;

            using (MySqlConnection conn = new MySqlConnection(ServerManager.CONNECT_PROC))
            {
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    try
                    {
                        conn.Open();
                    }
                    catch (Exception e)
                    {
                        CLog.LogErr(e, "[ServerManager]서버 연결 중 에러가 발생하였습니다.");
                        return false;
                    }

                    try
                    {
                        string sqlCmd = string.Empty;
                        int resultSeq = -1, itemSeq = -1, idx = 0;
                        foreach (ResultItem i in destResult.listItem)
                        {
                            if (!string.IsNullOrEmpty(i.ResultInfo))
                            {
                                string d = i.ResultInfo.Replace("\r", "\r\n");
                                string path = SealerFilePath.GetItemDetailInfoPath(i.inspStartTime, idx);
                                System.IO.File.WriteAllText(path, d);

                                i.ResultInfo = d;
                            }
                            i.LiveCapturePath = i.LiveCapturePath.Replace("\\", "/");
                            i.DetectCapturePath = i.DetectCapturePath.Replace("\\", "/");
                            i.ResultInfo = i.ResultInfo.Replace("\\", "/");

                            itemSeq = -1;

                            string barcode = destResult.Barcode;
                            string outBarcode = destResult.OutBarcode;
                            string rInfo = destResult.ResultInfo;
                            string vNumber = i.VehicleNumber;
                            string vType = i.DetectType;
                            string lPath = i.LiveCapturePath;
                            string cPath = i.DetectCapturePath;
                            string iInfo = i.ResultInfo;

                            if (string.IsNullOrEmpty(barcode)) barcode = SealerResult.DEFAULT_BARCODE;
                            if (string.IsNullOrEmpty(outBarcode)) outBarcode = SealerResult.DEFAULT_BARCODE;
                            if (string.IsNullOrEmpty(rInfo)) rInfo = "-";
                            if (string.IsNullOrEmpty(vNumber)) vNumber = "-";
                            if (string.IsNullOrEmpty(vType)) vType = "-";
                            if (string.IsNullOrEmpty(lPath)) lPath = "-";
                            if (string.IsNullOrEmpty(cPath)) cPath = "-";
                            if (string.IsNullOrEmpty(iInfo)) iInfo = "-";

                            sqlCmd = string.Format("select avis_lpr_proc._addResultItem({0}, \"{1}\", \"{2}\", {3}, \"{4}\", {5}, \"{6}\", {7}, {8}, \"{9}\", \"{10}\", {11}, {12}, {13}, \"{14}\", \"{15}\", {16}, {17}, {18}, {19}, {20}, {21}, \"{22}\", \"{23}\", {24}, \"{25}\", {26}, {27}, \"{28}\");",
                                               resultSeq, destResult.StartTime, destResult.InspEndTime, destResult.TotalResult, barcode, destResult.Model, rInfo, destResult.StdSeq,
                                               itemSeq, i.InspStartTime, i.InspEndTime, i.ItemResult, i.VirtualServerConnect, i.IsCameraDetect, vNumber, vType,
                                               i.LightSensor1, i.LightSensor2, i.LightSensor3, i.LightSensor4, i.VoltageSensor1, i.VoltageSensor2, lPath, cPath, i.StdItemSeq, iInfo,
                                               destResult.inspDurationTimeMilliSec, i.inspDurationTimeMil, outBarcode);

                            cmd.CommandText = sqlCmd;
                            int result = (int)cmd.ExecuteScalar();
                            if (result > 0)
                            {
                                if (resultSeq < 0)
                                {
                                    cmd.CommandText = string.Format("select avis_lpr_proc._GetLastResultSeq();");
                                    resultSeq = (int)cmd.ExecuteScalar();
                                }

                                if (itemSeq < 0)
                                {
                                    cmd.CommandText = "select avis_lpr_proc._GetLastResultItemSeq();";
                                    itemSeq = (int)cmd.ExecuteScalar();
                                }
                            }

                            idx++;
                        }

                        return true;
                    }
                    catch (Exception e)
                    {
                        CLog.LogErr(e, "[ServerManager]서버 명령 수행 중 에러가 발생하였습니다.");
                    }
                }
            }

            return false;
        }

        public static bool SaveStandard(SealerStandard destStandard)
        {
            if (destStandard == null || destStandard.listItemCount <= 0) return false;

            using (MySqlConnection conn = new MySqlConnection(ServerManager.CONNECT_PROC))
            {
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    try
                    {
                        conn.Open();
                    }
                    catch (Exception e)
                    {
                        CLog.LogErr(e, "[ServerManager]서버 연결 중 에러가 발생하였습니다.");
                        return false;
                    }

                    try
                    {
                        string sqlCmd = string.Empty;
                        foreach (SealerStandardItem i in destStandard.listItem)
                        {
                            sqlCmd = string.Format("select avis_lpr_proc._addStandardItem({0}, {1}, {2}, \"{3}\", {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, \"{15}\", \"{16}\", {17}, {18});",
                                                destStandard.SeqStandard, destStandard.Model, i.SeqItem, i.Title, i.ControlPushBtn, i.VirtualServerConnect, i.IsCameraDetect, i.IsBarcodeScan,
                                                i.LightSensor1, i.LightSensor2, i.LightSensor3, i.LightSensor4, i.VoltageSensor1, i.VoltageSensor2,
                                                i.OutVehicleIndex, i.VehicleNumber, i.DetectType, i.InspBeforeDelay, i.InspAfterDelay);

                            cmd.CommandText = sqlCmd;
                            int result = (int)cmd.ExecuteScalar();
                            if (result > 0) destStandard.SeqStandard = result;
                        }

                        return true;
                    }
                    catch (Exception e)
                    {
                        CLog.LogErr(e, "[ServerManager]서버 명령 수행 중 에러가 발생하였습니다.");
                    }
                }
            }

            return false;
        }

        private static int _ExeProc(MySqlCommand cmd, string sqlCmd)
        {
            if (cmd == null) return -1;
            try
            {
                cmd.CommandText = sqlCmd;
                return (int)cmd.ExecuteScalar();

            }
            catch (Exception e)
            {
                CLog.LogErr(e, "[ServerManager]서버 명령 수행 중 에러가 발생하였습니다.");
            }

            return -1;
        }

        private static int _GetDbInt(MySqlDataReader r, string colName)
        {
            if (r != null)
            {
                try
                {
                    object o = r[colName];
                    if (o.GetType() == typeof(int)) return r.GetInt32(colName);
                }
                catch (Exception) { }
            }

            return -1;
        }

        private static int _GetDbIong(MySqlDataReader r, string colName)
        {
            if (r != null)
            {
                try
                {
                    object o = r[colName];
                    if (o.GetType() == typeof(long)) return r.GetInt32(colName);
                }
                catch (Exception) { }
            }

            return -1;
        }

        private static bool _GetDbBool(MySqlDataReader r, string colName)
        {
            if (r != null)
            {
                try
                {
                    object o = r[colName];
                    if (o.GetType() == typeof(bool)) return r.GetBoolean(colName);
                }
                catch (Exception) { }
            }

            return false;
        }

        private static byte _GetDbByte(MySqlDataReader r, string colName)
        {
            if (r != null)
            {
                try
                {
                    object o = r[colName];
                    if (o.GetType() == typeof(byte)) return r.GetByte(colName);
                }
                catch (Exception) { }
            }

            return 0;
        }

        private static string _GetDbString(MySqlDataReader r, string colName)
        {
            if (r != null)
            {
                try
                {
                    object o = r[colName];
                    if (o.GetType() == typeof(string)) return r.GetString(colName);
                }
                catch (Exception) { }
            }

            return string.Empty;
        }

        private static DateTime _GetDbDatetime(MySqlDataReader r, string colName)
        {
            if (r != null)
            {
                try
                {
                    object o = r[colName];
                    if (o.GetType() == typeof(DateTime)) return r.GetDateTime(colName);
                }
                catch (Exception) { }
            }

            return DateTime.MinValue;
        }

        private static double _GetDbDouble(MySqlDataReader r, string colName)
        {
            if (r != null)
            {
                try
                {
                    object o = r[colName];
                    if (o.GetType() == typeof(double)) return r.GetDouble(colName);
                }
                catch (Exception) { }
            }

            return 0;
        }
#endregion

        private void _ServerMonitoring()
        {
            bool isUseServerMonitor = true;

            // 내부 thread
            if (isUseServerMonitor)
            {
                DateTime lastUpdateTime = DateTime.MinValue;
                bool isFirst = true;
                while (!_repository.IsCloseProgram)
                {
                    bool oldConnectInfo = _repository.IsConnectServer;
                    if (isFirst)
                    {
                        System.Threading.Thread.Sleep(500);
                        oldConnectInfo = false;
                    }

                    _UpdateServerCnctState();

                    // send event
                    if (oldConnectInfo != _repository.IsConnectServer || lastUpdateTime.Day != DateTime.Now.Day)
                    {
                        if (_repository.IsConnectServer)
                        {
                            CLog.Log("[serverChecker]Connect to the server");
                            _repository.OnChgConnectServer(this, new EvtConnectInfoArgs(true));
                        }
                        else
                        {
                            CLog.Log("[serverChecker]Disconnect to the server");
                            _repository.OnChgConnectServer(this, new EvtConnectInfoArgs(false));
                        }

                        lastUpdateTime = DateTime.Now;
                    }

                    if (!_repository.IsCloseProgram) System.Threading.Thread.Sleep(_serverCheckInterval_ms);



                    if (isFirst)
                    {
                        while (!_repository.IsGuiReady && !_repository.IsCloseProgram)
                        {
                            System.Threading.Thread.Sleep(50);
                        }
                        _repository.OnChgConnectServer(this, new EvtConnectInfoArgs(false));

                        if(_repository.IsConnectServer) _repository.OnChgConnectServer(this, new EvtConnectInfoArgs(true));

                        isFirst = false;
                    }

                    //1시간마다 파일 삭제
                    if (DateTime.Now.Hour != _deleteHour)
                    {
                        // 파일 용량 확인
                        CheckDeleteData();

                        _deleteHour = DateTime.Now.Hour;
                    }
                }

                CLog.Log("[serverChecker] ByeBye~!");
            }
            else
            {
                //GUI ready 대기
                while (!_repository.IsGuiReady && !_repository.IsCloseProgram)
                {
                    System.Threading.Thread.Sleep(50);
                }

                _repository.IsConnectServer = true;
                _repository.OnChgConnectServer(this, new EvtConnectInfoArgs(true));
                CLog.Log("[serverChecker]Disable monitoring");
            }
        }

        public void CheckDeleteData()
        {
            // 익명메소드로 처리
            System.Threading.Thread t = new System.Threading.Thread
            (
                delegate ()
                {
                    #region CHECK_LOG
                    try
                    {
                        DateTime createMinTime2 = DateTime.Now.Subtract(new TimeSpan(30, 0, 0, 0));
                        string baseFilder = AjinUtil.DefaultName.GetAppDataFolderPath("log");

                        var files = from fs in System.IO.Directory.GetFiles(baseFilder, "*.txt", SearchOption.AllDirectories)
                                    let f = new System.IO.FileInfo(fs)
                                    where f.CreationTime <= createMinTime2
                                    select fs;

                        if (files != null)
                        {
                            foreach (string file in files)
                            {
                                if (!string.IsNullOrEmpty(file) && System.IO.File.Exists(file))
                                {
                                    System.IO.File.Delete(file);
                                }
                            }
                        }

                        var dirs = from ds in System.IO.Directory.GetDirectories(baseFilder, "*.*", SearchOption.AllDirectories)
                                   let d = new System.IO.DirectoryInfo(ds)
                                   where d.GetFiles().Length <= 0 && d.GetDirectories().Length <= 0
                                   select ds;

                        if (dirs != null)
                        {
                            foreach (string dir in dirs)
                            {
                                if (!string.IsNullOrEmpty(dir) && System.IO.Directory.Exists(dir))
                                {
                                    System.IO.Directory.Delete(dir);
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                    #endregion

                    #region CHECK_Image_30DAY
                    try
                    {
                        DateTime createMinTime3 = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0));
                        string baseFilder = SealerFilePath.GetImageFolderPath();

                        var dirs = from ds in System.IO.Directory.GetDirectories(baseFilder, "*.*", SearchOption.AllDirectories)
                                   let d = new System.IO.DirectoryInfo(ds)
                                   where d.GetFiles().Length <= 0 && d.GetDirectories().Length <= 0
                                   select ds;

                        if (dirs != null)
                        {
                            foreach (string dir in dirs)
                            {
                                if (!string.IsNullOrEmpty(dir) && System.IO.Directory.Exists(dir))
                                {
                                    System.IO.Directory.Delete(dir);
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                    #endregion

                    #region CHECK_tmp_1DAY
                    try
                    {
                        DateTime createMinTime3 = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0));
                        string baseFilder = SealerFilePath.GetAlgorithmLogPath();

                        var dirs = from ds in System.IO.Directory.GetDirectories(baseFilder, "*.*", SearchOption.AllDirectories)
                                   let d = new System.IO.DirectoryInfo(ds)
                                   where d.GetFiles().Length <= 0 && d.GetDirectories().Length <= 0
                                   select ds;

                        if (dirs != null)
                        {
                            foreach (string dir in dirs)
                            {
                                if (!string.IsNullOrEmpty(dir) && System.IO.Directory.Exists(dir))
                                {
                                    System.IO.Directory.Delete(dir);
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }

                    try
                    {
                        DateTime createMinTime3 = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0));
                        string baseFilder = SealerFilePath.GetTmpFolderPath();

                        var dirs = from ds in System.IO.Directory.GetDirectories(baseFilder, "*.*", SearchOption.AllDirectories)
                                   let d = new System.IO.DirectoryInfo(ds)
                                   where d.GetFiles().Length <= 0 && d.GetDirectories().Length <= 0
                                   select ds;

                        if (dirs != null)
                        {
                            foreach (string dir in dirs)
                            {
                                if (!string.IsNullOrEmpty(dir) && System.IO.Directory.Exists(dir))
                                {
                                    System.IO.Directory.Delete(dir);
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                    #endregion
                }
            );
            t.Start();
        }

        private void _UploadProcess()
        {
            CLog.Log("[ServerManager] Start process");

            while (!_repository.IsCloseProgram)
            {
                if (_repository.mainState == CommonRepository.MainState.Save)
                {
                    if (_repository.IsConnectServer)
                    {
                        int retryCount = 3;

                        if (!_repository.lastResult.IsSaveImg)
                        {
                            _UpdateImage(retryCount);
                            _repository.lastResult.IsSaveImg = true;
                        }

                        if (!_repository.lastResult.IsSaveData)
                        {
                            _UpdateData(retryCount);
                            _repository.lastResult.IsSaveData = true;
                        }

                        //inspResult.Clear();

                        if (_repository.lastResult.IsSaveImg && _repository.lastResult.IsSaveData)
                        {
                            CLog.Log("[StorageManager]Success to upload data.");
                        }
                    }
                    else
                    {
                        CLog.LogErr(string.Format("[StorageManager]Can't upload the data(disconnect server)"));
                    }
                }

                if (!_repository.IsCloseProgram)
                {
                    System.Threading.Thread.Sleep(_processDelay_ms);
                }
            }

            CLog.Log("[ServerManager] ByeBye~!");
        }

        private async void _UpdateServerCnctState()
        {
            lock (lockServerConnectObject)
            {
                int prodSeq = 0, goalCnt = 0, currCnt = 0, okCnt = 0, ngCnt = 0, serialCode = 0;
                if (_GetServerProductionInfo(CommonRepository.DEFAULT_MODEL_CODE, ref prodSeq, ref goalCnt, ref currCnt, ref okCnt, ref ngCnt, ref serialCode))
                {
                    _repository.IsConnectServer = true;
                }
                else
                {
                    _repository.IsConnectServer = false;
                }

                //_repository.IsConnectServer = AjinUtil.HttpNet.IsConnectServer(_program.serverAddress, _SERVER_TIMEOUT_INTERVAL);
            }
        }

        private void _UpdateImage(int retryCnt)
        {
            if (!_isSavingImage)
            {
                _isSavingImage = true;

                if (!_repository.lastResult.IsSaveImg)
                {
                    Task.Run(() =>
                    {
                        if (_repository.lastResult != null)
                        {
                            foreach (ResultItem r in _repository.lastResult.listItem)
                            {
                                string liveCapturePath = SealerFilePath.GetLiveImagePath(r.inspStartTime);

                                if (!string.IsNullOrEmpty(r.LiveCapturePath))
                                {
                                    try
                                    {
                                        if (System.IO.File.Exists(r.LiveCapturePath))
                                        {
                                            System.IO.File.Copy(r.LiveCapturePath, liveCapturePath);
                                            r.LiveCapturePath = liveCapturePath;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        CLog.LogErr(ex, "Exception in LiveCapturePath");
                                    }
                                }
                            }
                        }

                        _isSavingImage = false;
                    });
                }
                else
                {
                    _isSavingImage = false;
                }
            }
        }

        private void _UpdateData(int retryCnt)
        {
            if (!_isSavingData)
            {
                _isSavingData = true;

                if (!_repository.lastResult.IsSaveData)
                {
                    Task.Run(() =>
                    {
                        SaveResult(_repository.lastResult);

                        _repository.lastResult.IsSaveData = true;
                        _isSavingData = false;
                    });
                }
                else
                {
                    _isSavingData = false;
                }
            }
        }


        //make default standard
        private void _MakeDefaultStd(ref SealerStandard std)
        {
            if (std == null) std = new SealerStandard();

            std.SeqStandard = -1;
            std.Model = std.Model;
            if (std.listItem == null) std.listItem = new List<SealerStandardItem>();
            std.listItem.Clear();

            uint defaultDelay = 300, longDelay = 500;
            int onLedValue = 500;
            int offLedValue = 100;

            int n = -1;
            string s = string.Empty;
            CommonRepository.BtnType btn = CommonRepository.BtnType.Unknown;
            CommonRepository.BoolType bType = CommonRepository.BoolType.Unknown;
            CommonRepository.TestDataIndex tType = CommonRepository.TestDataIndex.Init;

            std.listItem.Add(new SealerStandardItem(n, "기본 검사", btn, CommonRepository.BoolType.True, bType, CommonRepository.BoolType.True, n, n, n, n, n, n, tType, s, s, 0, defaultDelay));

            // 전면 LED 검사
            std.listItem.Add(new SealerStandardItem(n, "PWR LED(G) 검사", btn, CommonRepository.BoolType.True, bType, bType, onLedValue, n, n, n, n, n, tType, s, s, 0, defaultDelay));
            std.listItem.Add(new SealerStandardItem(n, "SYS LED(Y) 검사", btn, CommonRepository.BoolType.True, bType, bType, n, onLedValue, n, n, n, n, tType, s, s, 0, defaultDelay));
            std.listItem.Add(new SealerStandardItem(n, "CAM LED(R) 검사", btn, CommonRepository.BoolType.True, bType, bType, n, n, onLedValue, n, n, n, tType, s, s, 0, defaultDelay));
            std.listItem.Add(new SealerStandardItem(n, "SW LED(G) 검사", btn, CommonRepository.BoolType.True, bType, bType, n, n, n, onLedValue, n, n, tType, s, s, 0, defaultDelay));

            // 출력 전압 검사
            std.listItem.Add(new SealerStandardItem(n, "출력 전원 검사1", btn, CommonRepository.BoolType.True, bType, bType, n, n, n, n, 11.5, n, tType, s, s, 0, defaultDelay));
            std.listItem.Add(new SealerStandardItem(n, "출력 전원 검사2", btn, CommonRepository.BoolType.True, bType, bType, n, n, n, n, n, 11.5, tType, s, s, 0, defaultDelay));

            // 스위치 검사
            std.listItem.Add(new SealerStandardItem(n, "스위치1 검사 (기본 상태)", btn, CommonRepository.BoolType.True, bType, bType, n, n, n, onLedValue, n, n, tType, s, s, 0, defaultDelay));
            std.listItem.Add(new SealerStandardItem(n, "스위치2 준비1 (동작)", CommonRepository.BtnType.Push, bType, bType, bType, n, n, n, n, n, n, tType, s, s, longDelay, defaultDelay));
            std.listItem.Add(new SealerStandardItem(n, "스위치2 준비2 (복귀)", CommonRepository.BtnType.UnPush, bType, bType, bType, n, n, n, n, n, n, tType, s, s, longDelay, defaultDelay));
            std.listItem.Add(new SealerStandardItem(n, "스위치2 검사 (미동작 상태)", btn, CommonRepository.BoolType.True, bType, bType, n, n, n, offLedValue, n, n, tType, s, s, 0, defaultDelay));
            std.listItem.Add(new SealerStandardItem(n, "스위치3 준비1 (동작)", CommonRepository.BtnType.Push, bType, bType, bType, n, n, n, n, n, n, tType, s, s, longDelay, defaultDelay));
            std.listItem.Add(new SealerStandardItem(n, "스위치3 준비2 (복귀)", CommonRepository.BtnType.UnPush, bType, bType, bType, n, n, n, n, n, n, tType, s, s, longDelay, defaultDelay));
            std.listItem.Add(new SealerStandardItem(n, "스위치3 검사 (동작 상태)", btn, CommonRepository.BoolType.True, bType, bType, n, n, n, onLedValue, n, n, tType, s, s, 0, defaultDelay));

            // 영상 검사
            std.listItem.Add(new SealerStandardItem(n, "수배 검사", btn, bType, bType, bType, n, n, n, n, n, n, CommonRepository.TestDataIndex.TD2, "11가1111", "수배", 0, defaultDelay));
            std.listItem.Add(new SealerStandardItem(n, "미납 검사", btn, bType, bType, bType, n, n, n, n, n, n, CommonRepository.TestDataIndex.TD3, "11가1111", "미납", 0, defaultDelay));
            std.listItem.Add(new SealerStandardItem(n, "일반 검사", btn, bType, bType, bType, n, n, n, n, n, n, CommonRepository.TestDataIndex.TD1, s, s, 0, defaultDelay));

            // 종료
            std.listItem.Add(new SealerStandardItem(n, "검사 종료", btn, bType, bType, bType, n, n, n, n, n, n, tType, s, s, 0, defaultDelay));
        }
    }


    class FileSorter : IComparer<FileInfo>
    {
        public int Compare(FileInfo x, FileInfo y)
        {
            return y.CreationTime.CompareTo(x.CreationTime);
        }
    }

    class DirectorySorter : IComparer<DirectoryInfo>
    {
        public int Compare(DirectoryInfo x, DirectoryInfo y)
        {
            return y.CreationTime.CompareTo(x.CreationTime);
        }
    }
}
