using DevExpress.XtraReports.UI;
using AvisSealer.datas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tools.Util.Log;

namespace AvisSealer
{
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {
        public static int MAX_LOG_COUNT = 200;

        private ProgramConfig _config = ProgramConfig.Instance();
        private CommonRepository _repository = CommonRepository.Instance();
        private DXItemVirtualController _virtualForm = new DXItemVirtualController();

        private Timer _initTimner = new Timer();

        public List<MainGrid> lastData = new List<MainGrid>();

        public Font fontTitle = null;
        public Font fontTitleVersion = null;
        public Brush bruTitle = null;
        public Brush bruTitleVersion = null;
        public Pen penOutLine = null;

        private ServerManager _server = null;
        
        private DetectCarInfo _detectInfo = null;

        private bool isVirtualDevice = false;

        private bool _inspProcessing = false;

        private bool _inspStart_fromBarcode = false;
        private string _barcodePcb_fromScanner = "";

        private string _lastFirmwareVersion = "";


        delegate void delegateStartInspection(EvtStartInspArgs e);
        delegate void delegateFinishiInspection(EvtFinishInspArgs e);
        delegate void delegateProcessingInspection(EvtProcessingInspArgs e);
        delegate void delegateCancelInspection(EvtCancelInspArgs e);
        delegate void delegateChangeTdImg(CommonRepository.TestDataIndex idx);
        delegate void delegateChangeClient(bool status);


        private object lockObject = new object();



        private enum SUMMARY_GRAPH_IDX
        {
            PASS = 0,
            FAIL = 1,
            CHECK = 2
        }


        public MainForm()
        {
            InitializeComponent();

            _repository.IsGuiReady = false;

            CommonRepository.FORM_CAPTIONBAR_SIZE = Properties.Resources.title_bg.Height;

            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);

            fontTitle = new Font("현대하모니 M", 15, FontStyle.Regular);
            fontTitleVersion = new Font("현대하모니 L", 8, FontStyle.Regular);
            bruTitle = new SolidBrush(Color.White);
            bruTitleVersion = new SolidBrush(Color.FromArgb(217, 217, 217));
            penOutLine = new Pen(Color.FromArgb(242, 164, 10), 2);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                /*
                using (ReportLabel labelReportPrint = new ReportLabel(_printName, _printPaper))
                {
                    labelReportPrint.Print(CommonRepository.BARCODE_MODEL, CommonRepository.BARCODE_INPUT, "P122CDBBCJ001", CommonRepository.BARCODE_REGNUM, "ADM531450005", _config.printOffsetX, _config.printOffsetY);
                }
                */

                _repository.lastResult = null;

                _server = new ServerManager(100);
                _server.DoStart();

                // PUC
                if (_repository.puc == null)
                {
                    _repository.puc = new Tools.Device.PUC.PlcPuc();
                    _repository.puc.Connect(_config.pucPort, isVirtualDevice);
                }
                //puc.GetInput_Start(); //ex

                //detectInfo
                _detectInfo = new DetectCarInfo();


                //event 연결
                _repository.ChangeState += _repository_ChangeState;
                _repository.StartInspection += _repository_StartInspection;
                _repository.FinishInspection += _repository_FinishInspection;
                _repository.ProcessingInspection += _repository_ProcessingInspection;

                while (_repository.lastResult == null)
                {
                    System.Threading.Thread.Sleep(100);
                }

                _ClearUi();

                _initTimner.Tick += _initTimner_Tick;
                _initTimner.Interval = 100;
                _initTimner.Start();

                System.Threading.Tasks.Task.Run(() => _MainManager());

                _repository.IsGuiReady = true;
            }
            catch (Exception)
            { }
        }

        private void _initTimner_Tick(object sender, EventArgs e)
        {
            _initTimner.Stop();

            // last result
            if (_repository.lastResult != null && _repository.lastResult.listItemCount > 0)
            {
                try
                {
                    labelControlStartTimeInfo.Text = _repository.lastResult.startTime.ToString("yyyy-MM-dd HH:mm:ss");
                    labelControlInspKind.Text = string.Format("{0}개", _repository.lastResult.listItemCount);
                    labelControlLastItemDuration.Text = string.Format("{0}ms", _repository.lastResult.inspDurationTimeMilliSec);
                    labelControlInspStatus.Text = "검사완료";
                    labelControlBoradBarcode.Text = _repository.lastResult.Barcode;
                    labelControlOutBarcode.Text = _repository.lastResult.OutBarcode;
                    _UpdateProgress(_repository.inspProcessMax, _repository.inspProcessMax);


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

                    bool isOk = true;
                    foreach (Server.ResultItem r in _repository.lastResult.listItem)
                    {
                        if (!string.IsNullOrEmpty(r.ResultInfo))
                        {
                            string path = r.ResultInfo;
                            if (System.IO.File.Exists(path))
                            {
                                string data = System.IO.File.ReadAllText(path);
                                if (!string.IsNullOrEmpty(data))
                                {
                                    data = data.Replace("\n", "");
                                    string[] strs = data.Split(new char[] { '\r' }, StringSplitOptions.RemoveEmptyEntries);
                                    if (strs != null)
                                    {
                                    }
                                }
                            }
                        }

                        MainGrid item = lastData.Find(i => i.InspTitle == r.Title);
                        if (item != null)
                        {
                            item.inspDuration_ms = r.inspDurationTimeMil;
                            item.result = r.itemResult;

                            if (item.result != CommonRepository.EnumInspResult.Ok) isOk = false;
                        }
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
            else
            {
                _ClearUi();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _repository.IsCloseProgram = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            _repository.FormPaint(e.Graphics, this.ClientSize, this.BackColor, "AVIS-SEALER (SealerInspector)", true, false);

            // CI
            e.Graphics.DrawImage(Properties.Resources.autoit_ci, new Rectangle(this.ClientSize.Width / 2 - Properties.Resources.autoit_ci.Width / 2, 3, Properties.Resources.autoit_ci.Width - 4, Properties.Resources.autoit_ci.Height - 2), new Rectangle(0, 0, Properties.Resources.autoit_ci.Width, Properties.Resources.autoit_ci.Height), GraphicsUnit.Pixel);

            // VERSION
            e.Graphics.DrawString("v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(), fontTitleVersion, bruTitleVersion, new Point(305, 18));
        }

        protected override void WndProc(ref Message m)
        {
            IntPtr r = _repository.FormEvent(this, m, true);
            if (r != IntPtr.Zero) m.Result = r;
            else base.WndProc(ref m);
        }

        private void BarcodeProcess(string code)
        {
            _repository.lastBarcode.Set(code, DateTime.Now);

            /*
            this.Invoke(new MethodInvoker(delegate ()
            {
                MessageBox.Show(code);
            }));
            */
        }

        private void _ClearUi()
        {
            labelControlStartTimeInfo.Text = "-";
            labelControlInspKind.Text = "-";
            labelControlLastItemDuration.Text = "-";
            labelControlInspStatus.Text = "-";
            labelControlBoradBarcode.Text = "-";
            labelControlOutBarcode.Text = "-";
            //btnCheckItemOption.Enabled = false;
            btnProgressCancel.Enabled = false;
            //gridControlDataInfo.Enabled = false;
            this._lastFirmwareVersion = string.Empty;

            _UpdateProgress(0, 1);
            _UpdateGrid();
            _UpdateResult(false, true);
        }

        private void _UpdateResult(bool isOk, bool isReady)
        {
            if (labelControlResult.Font != null)
            {
                labelControlResult.Font.Dispose();
                labelControlResult.Font = null;
            }

            if (isReady)
            {
                labelControlResult.Text = "-";
                labelControlResult.Font = new Font("HDharmony M", 50);
                labelControlResult.BackColor = Color.DarkGray;
            }
            else
            {
                if (isOk)
                {
                    labelControlResult.Text = "OK";
                    labelControlResult.Font = new Font("HDharmony M", 100);
                    labelControlResult.BackColor = Color.FromArgb(0, 221, 68);
                }
                else
                {
                    labelControlResult.Text = "NG";
                    labelControlResult.Font = new Font("HDharmony M", 100);
                    labelControlResult.BackColor = Color.FromArgb(242, 63, 85);
                }
            }
        }

        private void _UpdateProgress(int cur, int max)
        {
            int maxProgressW = labelControlProgressBack.Width - 2;
            int maxProgressH = labelControlProgressBack.Height - 2;
            double curPer = cur / (double)max * 100;

            if (_repository.inspProcessMax == 0) _repository.inspProcessMax = 1;

            labelControlProgress.Size = new Size((int)(maxProgressW * curPer / 100), maxProgressH);

            labelControlProgressInfo.Text = string.Format("{0}%", curPer.ToString("0.0"));
        }

        private void _DrawProductInfoLabel(DevExpress.XtraEditors.LabelControl label, int value)
        {
            if (label != null)
            {
                if (value > 99999) value = 99999;
                if (value < 0) value = 0;

                if (value >= 10000) label.Text = string.Format("{0}ea", value);
                else if (value >= 1000) label.Text = string.Format("<color=49, 42, 35>0</color>{0}ea", value);
                else if (value >= 100) label.Text = string.Format("<color=49, 42, 35>00</color>{0}ea", value);
                else if (value >= 10) label.Text = string.Format("<color=49, 42, 35>000</color>{0}ea", value);
                else label.Text = string.Format("<color=49, 42, 35>0000</color>{0}ea", value);
            }
        }

        private void _UpdateGrid()
        {
            try
            {
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



                gridControlDataInfo.DataSource = lastData;
                gridControlDataInfo.RefreshDataSource();


                gridViewDataInfo.Columns["Index"].Caption = "순번";
                gridViewDataInfo.Columns["InspTitle"].Caption = "검사항목";
                gridViewDataInfo.Columns["InspDuration"].Caption = "처리시간";
                gridViewDataInfo.Columns["Result"].Caption = "결과";



                for (int i = 0; i < gridViewDataInfo.Columns.Count; i++)
                {
                    gridViewDataInfo.Columns[i].OptionsColumn.AllowEdit = false;

                    gridViewDataInfo.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    gridViewDataInfo.Columns[i].AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                    gridViewDataInfo.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    gridViewDataInfo.Columns[i].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;

                    gridViewDataInfo.Columns[i].AppearanceHeader.Font = new Font(
                        gridViewDataInfo.Columns[i].AppearanceHeader.Font.FontFamily,
                        12,
                        FontStyle.Bold);
                    gridViewDataInfo.Columns[i].AppearanceCell.Font = new Font(
                        gridViewDataInfo.Columns[i].AppearanceHeader.Font.FontFamily,
                        12,
                        FontStyle.Regular);

                    gridViewDataInfo.Columns[i].BestFit();
                }
                gridViewDataInfo.Columns[1].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;


                int gridWidth = gridControlDataInfo.Width;
                gridViewDataInfo.Columns["Index"].Width = (int)(gridWidth * 10 / 100);
                gridViewDataInfo.Columns["InspTitle"].Width = (int)(gridWidth * 50 / 100);
                gridViewDataInfo.Columns["InspDuration"].Width = (int)(gridWidth * 25 / 100);
                gridViewDataInfo.Columns["Result"].Width = (int)(gridWidth * 15 / 100);
            }
            catch (Exception ex)
            {
                CLog.LogErr(ex, "Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void _PopupDetailForm()
        {
            /*
            DataAnnotation rowData = gridViewDataInfo.GetFocusedRow() as DataAnnotation;
            if (rowData != null)
            {
                using (DxItemDetailForm form = new DxItemDetailForm(this, rowData.idx))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        
                    }
                    gridControlDataInfo.RefreshDataSource();
                }
            }
            */
        }

        private void btnDetail_Click(object sender, EventArgs e)
        {
            _PopupDetailForm();
        }

        private void gridViewDataInfo_DoubleClick(object sender, EventArgs e)
        {
            Point pt = gridControlDataInfo.PointToClient(Control.MousePosition);
            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo info = gridViewDataInfo.CalcHitInfo(pt);
            if (info.InRow || info.InRowCell)
            {
                _PopupDetailForm();
            }
        }

        private void btnVirtualPlc_Click(object sender, EventArgs e)
        {
            if (!_virtualForm.Visible) _virtualForm.Show();

            _virtualForm.Location = new Point(this.Location.X + 1, this.Location.Y);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            using (DxItemSearchForm form = new DxItemSearchForm(this))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                }
            }
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            using (XtraFormSetting form = new XtraFormSetting(this))
            {
                int dispIdx = _config.imgDispIdx;
                if (form.ShowDialog() == DialogResult.OK)
                {
                }
            }
        }
        

        private void labelClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void CheckDeleteData(string logPath, int logDay, List<string> tmpPaths, int tmpDay)
        {
            // 익명메소드로 처리
            System.Threading.Thread t = new System.Threading.Thread
            (
                delegate ()
                {
                    #region CHECK_LOG
                    try
                    {
                        DateTime createMinTime2 = DateTime.Now.Subtract(new TimeSpan(logDay, 0, 0, 0));
                        var files = from fs in System.IO.Directory.GetFiles(logPath, "*.*", SearchOption.AllDirectories)
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

                        var dirs = from ds in System.IO.Directory.GetDirectories(logPath, "*.*", SearchOption.AllDirectories)
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

                    #region CHECK_TMP
                    try
                    {
                        DateTime createMinTime2 = DateTime.Now.Subtract(new TimeSpan(tmpDay, 0, 0, 0));
                        foreach (string p in tmpPaths)
                        {
                            var files = from fs in System.IO.Directory.GetFiles(p, "*.*", SearchOption.AllDirectories)
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

                            var dirs = from ds in System.IO.Directory.GetDirectories(p, "*.*", SearchOption.AllDirectories)
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
                    }
                    catch (Exception)
                    {
                    }
                    #endregion
                }
            );
            t.Start();
        }

        private void btnProgressCancel_Click(object sender, EventArgs e)
        {
            if (_repository.lastResult != null && _repository.lastResult.listItemCount > 0)
            {
                for (int i = _repository.inspProcessIndex; i < _repository.lastResult.listItemCount; i++)
                {
                    _repository.lastResult.listItem[i].itemResult = CommonRepository.EnumInspResult.Ng;
                }
            }

            CLog.Log("검사 취소를 눌렀습니다");
            _repository.IsEmegStop = true;
        }

        private void labelControlBarcodeReprint_Click(object sender, EventArgs e)
        {
            if (CommonRepository.PopupMsg(this, "바코드 출력을 하시겠습니까?", "Notice", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
              
            }
        }

        private void labelControlGroupSummary_DoubleClick(object sender, EventArgs e)
        {
            btnVirtualPlc.Visible = !btnVirtualPlc.Visible;
        }

        private void labelControl4_Click(object sender, EventArgs e)
        {
            /*
            int y = 2024;
            int m = 6;
            int d = 13;
            int idx = 1;

            string barcode = _repository.GetBarcode("C10", y.ToString().Substring(2, 2).ToString() + m.ToString("00") + d.ToString("00"), idx);
            Console.WriteLine("바코드 생성 : " + barcode);

            //CLog.Log("[MainManager] 바코드 생성 : " + barcode);
            CommonRepository.NoticeMsg(this, "바코드 생성 : " + barcode);
            */
        }
    }
}