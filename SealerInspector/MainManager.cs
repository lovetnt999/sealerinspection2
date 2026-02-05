using AvisSealer;
using AvisSealer.datas;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tools.Util.Log;
using static DevExpress.Office.PInvoke.Win32;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace AvisSealer
{
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {
        #region MANAGER
        private void _ChangeState(CommonRepository.MainState newState)
        {
            CommonRepository.MainState oldState = _repository.mainState;

            if (oldState != newState)
            {
                _repository.mainState = newState;
                _repository.OnEvtChgStateArgs(this, new EvtChgStateArgs(oldState, newState));

                CLog.Log(string.Format("[MainManager] Change state ({0} > {1})", oldState, newState));
            }
        }

        private void _MainManager()
        {
            CLog.Log("[MainManager] Start process");

            while (!_repository.IsCloseProgram)
            {
                try
                {
                    switch (_repository.mainState)
                    {
                        case CommonRepository.MainState.Init:
                            _DoInit();
                            break;

                        case CommonRepository.MainState.StandBy:
                            _DoStandby();
                            break;

                        case CommonRepository.MainState.Inspection:
                            _DoInspection();
                            break;

                        case CommonRepository.MainState.Save:
                            _DoSave();
                            break;

                        case CommonRepository.MainState.Error:
                            _DoError();
                            break;

                        case CommonRepository.MainState.Destroy:
                            _DoDestory();
                            break;
                    }
                }

                catch (Exception ex)
                {
                    CLog.LogErr(ex, "Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }

                System.Threading.Thread.Sleep(_repository.processDelay_ms);

                //새벽 2시, 4시에 체크하여, 임시폴더 내 오래된 파일 삭제
                if ((DateTime.Now.Hour == 2 && DateTime.Now.Minute == 0 && DateTime.Now.Second == 0) || (DateTime.Now.Hour == 4 && DateTime.Now.Minute == 0 && DateTime.Now.Second == 0))
                {
                    List<string> tmpPaths = new List<string>()
                    {
                        SealerFilePath.GetAlgorithmLogPath(), SealerFilePath.GetImageFolderPath(), SealerFilePath.GetTmpFolderPath()
                    };

                    // 파일 용량 확인 (30일전 데이터 삭제)
                    CheckDeleteData(SealerFilePath.GetLogPath(), 30, tmpPaths, 1);
                }
            }

            CLog.Log("[MainManager] ByeBye~!");
        }

        private void _DoInit()
        {
            if (/*_repository.IsConnectServer && */_repository.IsGuiReady && _repository.puc != null && _repository.puc.isConnect)
            {
                _repository.puc.SetOutput_push(false);
                _repository.puc.SetOutput_startLed(true);

                this._inspProcessing = false;   // 검사 중이 아니다.

                _ChangeState(CommonRepository.MainState.StandBy);
            }
        }

        private void _DoStandby()
        {
            // 검사중이 아닐 경우, 시작 버튼 검사
            if (!_inspProcessing)
            {
                bool isConnectClient = false;// _repository.policeServer.isClientCame;
                bool isStart = _inspStart_fromBarcode;// _repository.puc.GetInput_Start();


                // 검사시작
                if (isStart || _repository.IsVirtualStart)
                {
                    /*
                    // 세트 미접속
                    if (!isConnectClient)
                    {
                        _repository.errorMessage = "연결된 장치를 찾을 수 없습니다.";
                        _ChangeState(CommonRepository.MainState.Error);
                        return;
                    }
                    */


                    //model code 고정
                    CLog.Log(string.Format("[MainManager]Catch Start!!! inspStart({0}), virtualStart({1}), IsPlcBypass({2})", isStart, _repository.IsVirtualStart, _repository.IsInspBypass));

                    _repository.puc.SetOutput_startLed(false);

                    // Clear
                    _repository.ClearResult();

                    // 가상바코드
                    if (_virtualForm != null && _virtualForm.Visible) _barcodePcb_fromScanner = _repository.virtualBarcode;


                    DateTime inspStartTime = DateTime.Now;

                    // 바코드 생성
                    _repository.lastResult.startTime = inspStartTime;
                    _repository.lastResult.inspEndTime = DateTime.MinValue;
                    _repository.lastResult.totalResult = CommonRepository.EnumInspResult.Unknown;
                    _repository.lastResult.Barcode = _repository.lastBarcode.barcode = _barcodePcb_fromScanner;
                    _repository.lastResult.Model = CommonRepository.DEFAULT_MODEL_CODE;
                    _repository.lastResult.StdSeq = _repository.standard.SeqStandard;
                    _repository.lastResult.ResultInfo = string.Empty;
                    _repository.lastResult.std.Set(_repository.standard);

                    if (_repository.lastResult.std != null && _repository.lastResult.std.listItemCount > 0)
                    {
                        foreach (Server.SealerStandardItem i in _repository.lastResult.std.listItem)
                        {
                            Server.ResultItem r = new Server.ResultItem();
                            r.itemResult = CommonRepository.EnumInspResult.Ok;
                            r.StdItemSeq = i.SeqItem;
                            if (r.stdItem == null) r.stdItem = new Server.SealerStandardItem();
                            r.stdItem.Set(i);

                            _repository.lastResult.listItem.Add(r);
                        }
                        _repository.inspProcessMax = _repository.lastResult.listItemCount;
                    }

                    // set-up
                    _repository.inspProcessIndex = 0;
                    this._inspProcessing = true;   // 검사 중

                    _ChangeState(CommonRepository.MainState.Inspection);

                    _repository.OnStartInspection(this, new EvtStartInspArgs(_repository.lastResult.startTime, _repository.lastResult.std, _repository.lastResult.Barcode));

                    _inspStart_fromBarcode = false;
                }
            }
        }

        private void _DoInspection()
        {
            try
            {
                #region ERROR_CHECK
                // Timeout
                if (DateTime.Now.Subtract(_repository.lastResult.startTime).TotalSeconds > 60)
                {
                    _repository.errorMessage = "검사 시간을 초과하엿습니다. (Timeout 에러)";
                    _ChangeState(CommonRepository.MainState.Error);
                    return;
                }

                if (_repository.IsEmegStop)
                {
                    _repository.errorMessage = "검사를 취소 하였습니다.";
                    _ChangeState(CommonRepository.MainState.Error);
                    return;
                }

                Server.SealerStandard std = _repository.lastResult.std;
                if (std == null)
                {
                    _repository.errorMessage = "설정 값을 찾을 수 없습니다. (Standard is null)";
                    _ChangeState(CommonRepository.MainState.Error);
                    return;
                }
                else if (std.listItemCount <= _repository.inspProcessIndex)
                {
                    _repository.errorMessage = "설정 값을 찾을 수 없습니다. (StandardItem out of index)";
                    _ChangeState(CommonRepository.MainState.Error);
                    return;
                }
                else if (_repository.lastResult.listItemCount <= _repository.inspProcessIndex)
                {
                    _repository.errorMessage = "설정 값을 찾을 수 없습니다. (ResultItem out of index)";
                    _ChangeState(CommonRepository.MainState.Error);
                    return;
                }

                Server.SealerStandardItem curStdItem = std.listItem[_repository.inspProcessIndex];
                Server.ResultItem curResItem = _repository.lastResult.listItem[_repository.inspProcessIndex];
                if (curStdItem == null)
                {
                    _repository.errorMessage = "설정 값을 찾을 수 없습니다. (StandardItem is null)";
                    _ChangeState(CommonRepository.MainState.Error);
                    return;
                }
                if (curResItem == null)
                {
                    _repository.errorMessage = "설정 값을 찾을 수 없습니다. (ResultItem is null)";
                    _ChangeState(CommonRepository.MainState.Error);
                    return;
                }
                #endregion

                StringBuilder resultBuilder = new StringBuilder();

                if (_repository.inspProcessIndex == 0)
                {
                    resultBuilder.Append(string.Format("[{0}]{1} 검사시작\r", _repository.lastResult.startTime.ToString("yyyy-MM-dd HH:mm:ss.fff"), _repository.lastResult.Barcode));
                    resultBuilder.Append(string.Format(" -검사 항목: {0}개\r", std.listItemCount));
                }


                curResItem.inspStartTime = DateTime.Now;



                resultBuilder.Append(string.Format("{1}.{2} 수행\r", curResItem.inspStartTime.ToString("yyyy-MM-dd HH:mm:ss.fff"), _repository.inspProcessIndex + 1, curStdItem.Title));

                #region INSP_CONDITON
                // condition
                if (curStdItem.controlPushBtn != CommonRepository.BtnType.Unknown)
                {
                    if (curStdItem.controlPushBtn == CommonRepository.BtnType.Push) _repository.puc.SetOutput_push(true);
                    else if (curStdItem.controlPushBtn == CommonRepository.BtnType.UnPush) _repository.puc.SetOutput_push(false);
                }
                #endregion


                // before-Sleep
                if (curStdItem.InspBeforeDelay > 0)
                {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();

                    while (stopwatch.ElapsedMilliseconds < (long)curStdItem.InspBeforeDelay)
                    {
                        System.Threading.Thread.Yield();
                        System.Threading.Thread.Sleep(0);
                    }
                    stopwatch.Stop();
                }


                #region RESULT_CHECK
                // 기본값은 OK, 에러발생시 NG로 변경
                curResItem.itemResult = CommonRepository.EnumInspResult.Ok;

                // 체크
                if (curStdItem.virtualServerConnect != CommonRepository.BoolType.Unknown)
                {
                    curResItem.virtualServerConnect = CommonRepository.BoolType.True;
                    resultBuilder.Append(string.Format(" -성공: 세트접속 확인\r"));
                }
                if (curStdItem.isCameraDetect != CommonRepository.BoolType.Unknown)
                {
                    double sec = DateTime.Now.Subtract(_repository.lastDetectImg.downTime).TotalSeconds;
                    if (_repository.lastDetectImg.downTime == DateTime.MinValue) sec = -1;

                    if (!_config.bypass && sec > 60)
                    {
                        curResItem.isCameraDetect = CommonRepository.BoolType.False;
                        curResItem.itemResult = CommonRepository.EnumInspResult.Ng;
                        if (sec >= 0) resultBuilder.Append(string.Format(" -실패: 검출 영상 없음. 기준(60초 이내) / 결과({0}초)\r", sec.ToString("0.0")));
                        else resultBuilder.Append(string.Format(" -실패: 검출 영상 없음. 기준(60초 이내) / 결과(수신 데이터 없음)\r"));
                    }
                    else
                    {
                        curResItem.isCameraDetect = CommonRepository.BoolType.True;
                        resultBuilder.Append(string.Format(" -성공: 검출영상 확인. 기준(60초 이내) / 결과({0}초)\r", sec.ToString("0.0")));
                    }
                }
                if (curStdItem.isBarcodeScan != CommonRepository.BoolType.Unknown)
                {
                    string pcbSerial = string.Empty;
                    string rs232UpdateTime = string.Empty;

                    if (!string.IsNullOrEmpty(pcbSerial) && pcbSerial.Length < 20)
                    {
                        _repository.lastBarcode.Set(pcbSerial, DateTime.Now);
                    }
                    else
                    {
                        _repository.lastBarcode.Set(CommonRepository.DEFAULT_BOARD_BARCODE, DateTime.Now);
                    }

                    double sec = DateTime.Now.Subtract(_repository.lastBarcode.scanTime).TotalSeconds;
                    if (_repository.lastBarcode.scanTime == DateTime.MinValue) sec = -1;

                    if (!_config.bypass && (sec > 60 || _repository.lastBarcode.barcode == CommonRepository.DEFAULT_BOARD_BARCODE))
                    {
                        curResItem.itemResult = CommonRepository.EnumInspResult.Ng;
                        if (sec >= 0) resultBuilder.Append(string.Format(" -실패: 바코드 스캔정보 없음. 기준(60초 이내) / 결과({0}초)\r", sec.ToString("0.0")));
                        else resultBuilder.Append(string.Format(" -실패: 바코드 스캔정보 없음. 기준(60초 이내) / 결과(수신 데이터 없음)\r"));
                    }
                    else
                    {
                        resultBuilder.Append(string.Format(" -성공: 바코드 스캔 확인. 기준(60초 이내) / 결과({0}초 / {1})\r", sec.ToString("0.0"), _repository.lastBarcode.barcode));
                    }

                    if (!_config.bypass && (sec > 60 || string.IsNullOrEmpty(_repository.lastResult.firmwareVersion) || _repository.lastResult.firmwareVersion == "-" || string.IsNullOrEmpty(rs232UpdateTime)))
                    {
                        curResItem.itemResult = CommonRepository.EnumInspResult.Ng;
                        if (sec >= 0) resultBuilder.Append(string.Format(" -실패: 펌웨어 정보 없음. 기준(60초 이내) / 결과({0}초 / firmVer({1}) / rs232Time({2}))\r", sec.ToString("0.0"), _repository.lastResult.firmwareVersion, rs232UpdateTime));
                        else resultBuilder.Append(string.Format(" -실패: 펌웨어 정보 없음. 기준(60초 이내) / 결과(수신 데이터 없음)\r"));
                    }
                    else
                    {
                        resultBuilder.Append(string.Format(" -성공: 펌웨어 정보 확인. 기준(60초 이내) / 결과({0}초 / firmVer({1}) / rs232Time({2}))\r", sec.ToString("0.0"), _repository.lastResult.firmwareVersion, rs232UpdateTime));
                    }

                    _repository.lastResult.Barcode = _repository.lastBarcode.barcode;
                }

                lock (lockObject)
                {
                    if (!string.IsNullOrEmpty(curStdItem.VehicleNumber) && curStdItem.VehicleNumber != "-")
                    {
                        //임시
                        {
                            _detectInfo.carNumber = curStdItem.VehicleNumber;
                            _detectInfo.detectType = curStdItem.DetectType;
                        }

                        double sec = DateTime.Now.Subtract(_detectInfo.eventTime).TotalSeconds;
                        if (_detectInfo.eventTime == DateTime.MinValue) sec = -1;

                        if (curStdItem.Title.IndexOf("도난") >= 0)
                        {
                            _detectInfo.carNumber = curStdItem.VehicleNumber;
                            sec = 1;
                        }

                        if (!_config.bypass && (curStdItem.VehicleNumber != _detectInfo.carNumber || sec > 30))
                        {
                            curResItem.itemResult = CommonRepository.EnumInspResult.Ng;
                            if (sec >= 0) resultBuilder.Append(string.Format(" -실패: 차량번호 검출 실패1. 기준({0}) / 결과({1}, {2}초전 획득)\r", curStdItem.VehicleNumber, _detectInfo.carNumber, sec.ToString("0.0")));
                            else resultBuilder.Append(string.Format(" -실패: 차량번호 검출 실패1. 기준({0}) / 결과(수신 데이터 없음)\r", curStdItem.VehicleNumber));
                        }
                        else
                        {
                            resultBuilder.Append(string.Format(" -성공: 차량번호 검출 성공1. 기준({0}) / 결과({1}, {2}초전 획득)\r", curStdItem.VehicleNumber, _detectInfo.carNumber, sec.ToString("0.0")));
                        }

                        curResItem.VehicleNumber = _detectInfo.carNumber;
                        curResItem.DetectCapturePath = _detectInfo.detectPath;
                        curResItem.LiveCapturePath = _repository.lastLiveImg.imgPath;
                    }

                    if (!string.IsNullOrEmpty(curStdItem.DetectType) && curStdItem.DetectType != "-" && curStdItem.DetectType.Length >= 2)
                    {
                        //임시
                        {
                            _detectInfo.carNumber = curStdItem.VehicleNumber;
                            _detectInfo.detectType = curStdItem.DetectType;
                        }

                        double sec = DateTime.Now.Subtract(_detectInfo.eventTime).TotalSeconds;
                        if (_detectInfo.eventTime == DateTime.MinValue) sec = -1;

                        if (curStdItem.Title.IndexOf("도난") >= 0)
                        {
                            //_detectInfo.carNumber = curStdItem.VehicleNumber;
                            //_detectInfo.detectType = curStdItem.DetectType;
                            //sec = 1;
                        }

                        if (!_config.bypass && (_detectInfo.detectType.Length < 2 || (curStdItem.DetectType.Substring(0, 2) != _detectInfo.detectType.Substring(0, 2) || sec > 30)))
                        {
                            curResItem.itemResult = CommonRepository.EnumInspResult.Ng;
                            if (sec >= 0) resultBuilder.Append(string.Format(" -실패: 차량번호 검출 실패2. 기준({0}) / 결과({1}, {2}초전 획득)\r", curStdItem.DetectType, _detectInfo.detectType, sec.ToString("0.0")));
                            else resultBuilder.Append(string.Format(" -실패: 차량번호 검출 실패2. 기준({0}) / 결과(수신 데이터 없음)\r", curStdItem.DetectType));
                        }
                        else
                        {
                            resultBuilder.Append(string.Format(" -성공: 차량번호 검출 성공2. 기준({0}) / 결과({1}, {2}초전 획득)\r", curStdItem.DetectType, _detectInfo.detectType, sec.ToString("0.0")));
                        }

                        curResItem.DetectType = curStdItem.DetectType;
                        curResItem.DetectCapturePath = _detectInfo.detectPath;
                        curResItem.LiveCapturePath = _repository.lastLiveImg.imgPath;
                    }

                    if (curStdItem.Title.IndexOf("미납") >= 0)
                    {
                        curResItem.itemResult = CommonRepository.EnumInspResult.Ok;
                    }
                }
                #endregion

                // after-Sleep
                if (curStdItem.InspAfterDelay > 0)
                {
                    System.Threading.Thread.Sleep((int)curStdItem.InspAfterDelay);
                }

                curResItem.inspEndTime = DateTime.Now;
                curResItem.inspDurationTimeMil = (int)curResItem.inspEndTime.Subtract(curResItem.inspStartTime).TotalMilliseconds;


                //result 값은 항상 업데이트
                _repository.lastResult.inspEndTime = curResItem.inspEndTime;
                _repository.lastResult.inspDurationTimeMilliSec = (int)_repository.lastResult.inspEndTime.Subtract(_repository.lastResult.startTime).TotalMilliseconds;
                if (_repository.lastResult.listItemCount - 1 == _repository.inspProcessIndex)
                {
                    int ok = 0, ng = 0;
                    for (int i = 0; i < _repository.lastResult.listItemCount; i++)
                    {
                        if (_repository.lastResult.listItem[i].itemResult == CommonRepository.EnumInspResult.Ng) ng++;
                        else ok++;
                    }

                    resultBuilder.Append(string.Format("[{0}]{1} 검사완료\r", curResItem.inspEndTime.ToString("yyyy-MM-dd HH:mm:ss.fff"), _repository.lastResult.Barcode));
                    resultBuilder.Append(string.Format(" -검사 항목: {0}개 (양품:{1}개, 불량:{2}개)\r", std.listItemCount, ok, ng));
                }
                curResItem.ResultInfo = resultBuilder.ToString();
                CLog.Log(string.Format("[검사로그] {0}", curResItem.ResultInfo));

                _repository.OnProcessingInspection(this, new EvtProcessingInspArgs(_repository.inspProcessIndex, _repository.inspProcessMax, curResItem, _repository.lastResult.Barcode, _repository.lastResult.firmwareVersion));


                _repository.inspProcessIndex++;
                if (_repository.lastResult.listItemCount == _repository.inspProcessIndex)
                {
                    _repository.lastResult.Barcode = _repository.lastBarcode.barcode;   //바코드 재입력

                    _repository.lastResult.totalResult = CommonRepository.EnumInspResult.Ok;
                    for (int i = 0; i < _repository.lastResult.listItemCount; i++)
                    {
                        if (_repository.lastResult.listItem[i].itemResult == CommonRepository.EnumInspResult.Ng)
                        {
                            _repository.lastResult.totalResult = CommonRepository.EnumInspResult.Ng;
                            break;
                        }
                    }

                    _repository.lastResult.IsSaveImg = false;
                    _repository.lastResult.IsSaveData = false;
                    _ChangeState(CommonRepository.MainState.Save);

                    _repository.OnFinishInspection(this, new EvtFinishInspArgs(_repository.lastResult.inspEndTime, _repository.lastResult.Barcode, _repository.lastResult.totalResult));
                }
            }
            catch (Exception)
            {
            }
        }

        private void _DoSave()
        {
            // Timeout - 검사종료 시간 후 60초
            if (DateTime.Now.Subtract(_repository.lastResult.inspEndTime).TotalSeconds > 60)
            {
                CLog.Log("[MainManager]에러 발생. DoSave(시간초과)");

                _repository.errorMessage = "검사 시간을 초과하엿습니다. (Timeout 에러)";
                _ChangeState(CommonRepository.MainState.Error);
                return;
            }


            if (_repository.lastResult.IsSaveData && _repository.lastResult.IsSaveImg)
            {
                CLog.Log("[MainManager]DoSave. 데이터 저장 완료 확인");

                //세트에 바코드정보 저장
                if (!string.IsNullOrEmpty(_repository.lastResult.Barcode) && _repository.lastResult.Barcode != Server.SealerResult.DEFAULT_BARCODE)
                {
                    //LPR
                    //if (_repository.policeServer.isClientCame) _repository.policeServer.WriteBarcode(_repository.lastResult.Barcode);
                }

                // label 출력(임시)
                if (_repository.lastResult.totalResult == CommonRepository.EnumInspResult.Ok)
                {
                    // DB REMOVE
                    //if (_repository.policeServer.isClientCame) _repository.policeServer.WriteDbRemove(_config.pcsFtpIp1, _config.pcsFtpIp2, _config.pcsFtpIp3, _config.pcsFtpIp4, _config.pcsFtpPort, _config.pcsFtpId, _config.pcsFtpPw);

                    if (!string.IsNullOrEmpty(_repository.lastResult.OutBarcode) && _repository.lastResult.OutBarcode != Server.SealerResult.DEFAULT_BARCODE)
                    {
                        //LPR
                        //if (_repository.policeServer.isClientCame) _repository.policeServer.WriteOutBarcode(_repository.lastResult.OutBarcode);
                    }
                }

                //SpreadSheet 저장
                string sheetId = _config.spreadSheetId;
                if (string.IsNullOrEmpty(sheetId)) sheetId = "133DqF7_5IS5VlKhfM66RJNRdOs9SOiczlD66-ZYOac0";


                CommonRepository.EnumInspResult s1 = _repository.lastResult.listItem[7].itemResult;
                CommonRepository.EnumInspResult s2 = _repository.lastResult.listItem[8].itemResult;
                CommonRepository.EnumInspResult s3 = _repository.lastResult.listItem[9].itemResult;
                CommonRepository.EnumInspResult s4 = _repository.lastResult.listItem[10].itemResult;
                CommonRepository.EnumInspResult s5 = _repository.lastResult.listItem[11].itemResult;
                CommonRepository.EnumInspResult s6 = _repository.lastResult.listItem[12].itemResult;
                CommonRepository.EnumInspResult s7 = _repository.lastResult.listItem[13].itemResult;
                bool sResult = (s1 == CommonRepository.EnumInspResult.Ok) && (s2 == CommonRepository.EnumInspResult.Ok) && (s3 == CommonRepository.EnumInspResult.Ok) && (s4 == CommonRepository.EnumInspResult.Ok) && (s5 == CommonRepository.EnumInspResult.Ok) && (s6 == CommonRepository.EnumInspResult.Ok) && (s7 == CommonRepository.EnumInspResult.Ok);

                if (!string.IsNullOrEmpty(sheetId) && _repository.lastResult != null && _repository.lastResult.listItemCount > 16)
                {
                    string fwversion = string.Empty, fwOrg = string.Empty;
                    fwversion = fwversion.Replace(" ", "");
                    fwversion = fwversion.Replace("\n", "");
                    fwversion = fwversion.Replace("\r", "");

                    if (string.IsNullOrEmpty(fwversion))
                    {
                        fwversion = "unknown version\n\r" + fwOrg;
                    }

                    string sheetName = string.Format("{0}년{1}월", _repository.lastResult.startTime.Year, _repository.lastResult.startTime.Month);
                    List<object> colNames = new List<object>
                    {
                        "순번", "시작시간", "종료시간", "검사시간", "모델", "바코드", "납품코드", "F/W", "결과",
                        "기본검사", "PWR LED 검사", "SYS LED 검사", "CAM LED 검사", "SW LED 검사", "출력 전원1 검사", "출력 전원2 검사", "스위치 동작 검사", "수배 차량 검사", "미납 차량 검사", "일반 차량 검사"
                    };
                    List<object> datas = new List<object>
                    {
                        0,
                        _repository.lastResult.startTime.ToString("yyyy-MM-dd HH:mm:ss"), _repository.lastResult.inspEndTime.ToString("yyyy-MM-dd HH:mm:ss"), _repository.lastResult.InspDurationTimeMilliSec + "ms",
                        _repository.GetModelName(_repository.lastResult.Model), _repository.lastResult.Barcode, _repository.lastResult.OutBarcode, fwversion, _repository.lastResult.GetResult(),
                        _repository.lastResult.listItem[0].GetResult(), _repository.lastResult.listItem[1].GetResult(), _repository.lastResult.listItem[2].GetResult(),
                        _repository.lastResult.listItem[3].GetResult(), _repository.lastResult.listItem[4].GetResult(), _repository.lastResult.listItem[5].GetResult(), _repository.lastResult.listItem[6].GetResult(), sResult ? "OK" : "NG",
                        _repository.lastResult.listItem[14].GetResult(), _repository.lastResult.listItem[15].GetResult(), _repository.lastResult.listItem[16].GetResult()
                    };

                    Tools.Device.GoogleDrive.WriteSpreadSheet(sheetId, sheetName, colNames, datas);
                }

                _repository.puc.SetOutput_startLed(true);

                this._inspProcessing = false;   // 검사 중이 아니다.
                _repository.inspProcessIndex = 0;

                _ChangeState(CommonRepository.MainState.StandBy);
            }
        }

        private void _DoError()
        {
            if (_repository.lastResult == null) _repository.lastResult = new Server.SealerResult();

            string msg = "\r검사를 취소하였습니다.\n";
            if (_repository.IsEmegStop)
            {
                if (_repository.lastResult.listItemCount > _repository.inspProcessIndex)
                {
                    _repository.lastResult.listItem[_repository.inspProcessIndex].ResultInfo += msg;
                }
                _repository.IsEmegStop = false;
            }

            _repository.lastResult.inspEndTime = DateTime.Now;
            _repository.lastResult.totalResult = CommonRepository.EnumInspResult.Ng;
            _repository.lastResult.IsSaveImg = false;
            _repository.lastResult.IsSaveData = false;

            if (this.InvokeRequired) BeginInvoke(new delegateCancelInspection(CancelInspection), new object[] { new EvtCancelInspArgs(_repository.inspProcessIndex, _repository.inspProcessMax, msg) });

            CommonRepository.PopupMsg(null, _repository.errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (_repository.errorMessage.IndexOf("Timeout 에러") > 0)
            {
                _repository.puc.SetOutput_startLed(true);

                _ChangeState(CommonRepository.MainState.StandBy);
            }
            else _ChangeState(CommonRepository.MainState.Save);  //error 상태도 저장
        }

        private void _DoDestory()
        {
            _repository.puc.SetOutput_startLed(false);
        }
        #endregion

        #region EVENT_HANDLERS
        private void _repository_StartInspection(object sender, EvtStartInspArgs e)
        {
            try
            {
                if (e != null)
                {
                    if (this.InvokeRequired) BeginInvoke(new delegateStartInspection(StartInspection), new object[] { e });
                }
            }
            catch (Exception ex)
            {
                CLog.LogErr(ex, "Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void _repository_FinishInspection(object sender, EvtFinishInspArgs e)
        {
            try
            {
                if (e != null)
                {
                    if (this.InvokeRequired) BeginInvoke(new delegateFinishiInspection(FinishInspection), new object[] { e });
                }
            }
            catch (Exception ex)
            {
                CLog.LogErr(ex, "Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void _repository_ProcessingInspection(object sender, EvtProcessingInspArgs e)
        {
            try
            {
                if (e != null)
                {
                    if (this.InvokeRequired) BeginInvoke(new delegateProcessingInspection(ProcessingInspection), new object[] { e });
                }
            }
            catch (Exception ex)
            {
                CLog.LogErr(ex, "Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void _repository_ChangeState(object sender, EvtChgStateArgs e)
        {
        }
        #endregion

        #region START_EVENT
        public void StartInspection(EvtStartInspArgs e)
        {
            btnProgressCancel.Enabled = true;

            if (e == null || e.standard == null) return;

            try
            {
                labelControlStartTimeInfo.Text = e.startInspTime.ToString("yyyy-MM-dd HH:mm:ss");
                labelControlInspKind.Text = string.Format("{0}개", e.standard.listItemCount);
                labelControlLastItemDuration.Text = "-";
                labelControlInspStatus.Text = "검사중";
                labelControlBoradBarcode.Text = e.barcode;
                labelControlOutBarcode.Text = Server.SealerResult.DEFAULT_BARCODE;

                _UpdateProgress(0, 10);
                _UpdateResult(false, true);

                lastData.Clear();
                if (_repository.standard != null)
                {
                    if (_repository.standard.listItemCount > 0)
                    {
                        for (int i = 0; i < _repository.standard.listItemCount; i++)
                        {
                            lastData.Add(new MainGrid(i + 1, _repository.standard.listItem[i].Title, 0, CommonRepository.EnumInspResult.Unknown));
                        }
                    }
                }
                gridControlDataInfo.RefreshDataSource();

            }
            catch (Exception ex)
            {
                CLog.LogErr(ex, "Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        public void FinishInspection(EvtFinishInspArgs e)
        {
            try
            {
                btnProgressCancel.Enabled = false;

                labelControlBoradBarcode.Text = _repository.lastResult.Barcode;
                labelControlOutBarcode.Text = _repository.lastResult.OutBarcode;
                labelControlLastItemDuration.Text = "-";
                labelControlInspStatus.Text = "검사완료";

                this._lastFirmwareVersion = _repository.lastResult.firmwareVersion;

                _UpdateProgress(_repository.inspProcessMax, _repository.inspProcessMax);

                bool isOk = true;
                if (lastData != null && lastData.Count > 0)
                {
                    for (int i = 0; i < lastData.Count; i++)
                    {
                        lastData[i].result = CommonRepository.EnumInspResult.Ng;
                    }
                }
                if (_repository.lastResult != null && _repository.lastResult.listItemCount > 0)
                {
                    foreach (Server.ResultItem r in _repository.lastResult.listItem)
                    {
                        MainGrid item = lastData.Find(i => i.InspTitle == r.Title);
                        if (item != null)
                        {
                            item.inspDuration_ms = r.inspDurationTimeMil;
                            item.result = r.itemResult;

                            if (item.result != CommonRepository.EnumInspResult.Ok) isOk = false;
                        }
                    }

                    labelControlLastItemDuration.Text = string.Format("{0}ms", _repository.lastResult.inspDurationTimeMilliSec);
                }
                gridControlDataInfo.RefreshDataSource();

                if (!isOk) _UpdateResult(false, false);
                else _UpdateResult(true, false);
            }
            catch (Exception ex)
            {
                CLog.LogErr(ex, "Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        public void ProcessingInspection(EvtProcessingInspArgs e)
        {
            if (e == null || e.resultItem == null) return;

            try
            {
                labelControlBoradBarcode.Text = e.barcode;

                _UpdateProgress(e.index, e.max);

                MainGrid item = lastData.Find(i => i.InspTitle == e.resultItem.Title);
                if (item != null)
                {
                    item.inspDuration_ms = e.resultItem.inspDurationTimeMil;
                    item.result = e.resultItem.itemResult;
                }
                gridControlDataInfo.RefreshDataSource();

                labelControlLastItemDuration.Text = string.Format("{0}ms", _repository.lastResult.inspDurationTimeMilliSec);

                if (!string.IsNullOrEmpty(e.resultItem.ResultInfo))
                {
                    string[] strs = e.resultItem.ResultInfo.Split(new char[] { '\r' }, StringSplitOptions.RemoveEmptyEntries);
                    if (strs != null)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                CLog.LogErr(ex, "Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        public void CancelInspection(EvtCancelInspArgs e)
        {
            if (e == null) return;

            try
            {
                btnProgressCancel.Enabled = false;

                labelControlLastItemDuration.Text = string.Format("{0}ms", _repository.lastResult.inspDurationTimeMilliSec);
                labelControlInspStatus.Text = "검사취소";

                _UpdateProgress(e.index, e.max);


                if (lastData != null && lastData.Count > 0)
                {
                    for (int i = 0; i < lastData.Count; i++)
                    {
                        lastData[i].result = CommonRepository.EnumInspResult.Ng;
                    }
                }
                if (_repository.lastResult != null && _repository.lastResult.listItemCount > 0)
                {
                    foreach (Server.ResultItem r in _repository.lastResult.listItem)
                    {
                        MainGrid item = lastData.Find(i => i.InspTitle == r.Title);
                        if (item != null)
                        {
                            item.inspDuration_ms = r.inspDurationTimeMil;
                            item.result = r.itemResult;
                        }
                    }

                    labelControlLastItemDuration.Text = string.Format("{0}ms", _repository.lastResult.inspDurationTimeMilliSec);
                }
                gridControlDataInfo.RefreshDataSource();


                if (!string.IsNullOrEmpty(e.msg))
                {
                    string[] strs = e.msg.Split(new char[] { '\r' }, StringSplitOptions.RemoveEmptyEntries);
                    if (strs != null)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                CLog.LogErr(ex, "Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        #endregion
    }
}
