using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using AvisSealer.Server;
using Tools.Util.Log;
using System.Threading;

namespace AvisSealer
{
    public partial class DxItemSearchForm : DevExpress.XtraEditors.XtraForm
    {
        private CommonRepository _repository = CommonRepository.Instance();

        private MainForm _parent = null;

        private SealerResults _results = new SealerResults();

        private readonly int _rowCount = 100;
        private readonly int _SEARCH_TIMEOUT = 30000;

        private bool _isInit = false;


        public DxItemSearchForm(System.Windows.Forms.Form parent)
        {
            InitializeComponent();

            this.Owner = parent;
            this._parent = parent as MainForm;
        }

        private void DxItemDetailForm_Load(object sender, EventArgs e)
        {
            try
            {
                InitMenuBar();

                _results.listResult.Add(new SealerResult());
                gridControlDataInfo.DataSource = _results.listResult;
                gridControlDataInfo.RefreshDataSource();
                InitGrid();
                _results.listResult.Clear();
                gridControlDataInfo.RefreshDataSource();

                System.Windows.Forms.Timer firstSerachTimer = new System.Windows.Forms.Timer();
                firstSerachTimer.Tick += delegate (object o, EventArgs a)
                {
                    firstSerachTimer.Stop();
                    _DoSearch();
                };
                firstSerachTimer.Interval = 100;
                firstSerachTimer.Start();

                _isInit = true;
            }
            catch (Exception) { }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            _repository.FormPaint(e.Graphics, this.ClientSize, this.BackColor, "이력보기", false, true);
        }

        protected override void WndProc(ref Message m)
        {
            IntPtr r = _repository.FormEvent(this, m, true);
            if (r != IntPtr.Zero) m.Result = r;
            else base.WndProc(ref m);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gridViewDataInfo_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            /*
            ElementObject obj = gridViewDataInfo.GetRow(e.RowHandle) as ElementObject;
            if (obj != null)
            {
                if (obj.name != obj.groundTruthName)
                {
                    if(e.RowHandle == gridViewDataInfo.FocusedRowHandle) e.Appearance.BackColor = Color.FromArgb(240, 0, 0);
                    else e.Appearance.BackColor = Color.FromArgb(192, 0, 0);
                    e.Appearance.ForeColor = Color.White;
                }
            }
            */
        }

        private void gridViewDataInfo_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
                if (_IsHitDataArea(view, e))
                {
                    SealerResult r = view.GetFocusedRow() as SealerResult;
                    _PopupDetailInfo(r);
                }
            }
            catch (Exception ex)
            {
                CLog.LogErr(ex, "Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void DxItemSearchForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }




        private void InitMenuBar()
        {
            DateTime now = DateTime.Now;
            dateEditStartDate.EditValue = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            dateEditEndDate.EditValue = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);

            spinEditStartTime.Value = 0;
            spinEditEndDate.Value = now.Hour;

            comboBoxEditResult.Properties.Items.Clear();
            comboBoxEditResult.Properties.Items.Add("전체");
            comboBoxEditResult.Properties.Items.Add("양품");
            comboBoxEditResult.Properties.Items.Add("불량");
            comboBoxEditResult.SelectedIndex = 0;
        }

        private void InitGrid()
        {
            try
            {
                gridViewDataInfo.Columns["ResultSeq"].Visible = false;
                gridViewDataInfo.Columns["TotalResult"].Visible = false;
                gridViewDataInfo.Columns["StdSeq"].Visible = false;
                gridViewDataInfo.Columns["ResultInfo"].Visible = false;

                gridViewDataInfo.Columns["Index"].Caption = "순번";
                gridViewDataInfo.Columns["StartTime"].Caption = "시작시간";
                gridViewDataInfo.Columns["InspEndTime"].Caption = "종료시간";
                gridViewDataInfo.Columns["InspDurationTimeMilliSec"].Caption = "검사시간(msec)";
                gridViewDataInfo.Columns["listItemCount"].Caption = "검사항목";
                gridViewDataInfo.Columns["Model"].Caption = "모델";
                gridViewDataInfo.Columns["Barcode"].Caption = "바코드";
                gridViewDataInfo.Columns["OutBarcode"].Caption = "납품코드";
                gridViewDataInfo.Columns["totalResultImg"].Caption = "결과";


                for (int i = 0; i < gridViewDataInfo.Columns.Count; i++)
                {
                    gridViewDataInfo.Columns[i].OptionsColumn.AllowEdit = false;
                    gridViewDataInfo.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    gridViewDataInfo.Columns[i].AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                    gridViewDataInfo.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    gridViewDataInfo.Columns[i].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                    gridViewDataInfo.Columns[i].AppearanceHeader.Font = new Font(
                        gridViewDataInfo.Columns[i].AppearanceHeader.Font.FontFamily,
                        11,
                        FontStyle.Bold);
                    gridViewDataInfo.Columns[i].AppearanceCell.Font = new Font(
                        gridViewDataInfo.Columns[i].AppearanceHeader.Font.FontFamily,
                        10,
                        FontStyle.Regular);
                    gridViewDataInfo.Columns[i].BestFit();
                    gridViewDataInfo.Columns[i].AppearanceCell.Font = new Font(
                        gridViewDataInfo.Columns[i].AppearanceHeader.Font.FontFamily,
                        10,
                        FontStyle.Regular);
                    gridViewDataInfo.Columns[i].BestFit();
                }
                gridViewDataInfo.Appearance.FocusedRow.Font = gridViewDataInfo.Columns[0].AppearanceCell.Font;

                /*
                int gridWidth = gridControl_Main.Width;
                gridView_Main.Columns["SelectItem"].Width = (int)(gridWidth * 5 / 100);
                gridView_Main.Columns["Index"].Width = (int)(gridWidth * 5 / 100);
                gridView_Main.Columns["InspStartTimeLocal"].Width = (int)(gridWidth * 16 / 100);
                gridView_Main.Columns["InspEndTimeLocal"].Width = (int)(gridWidth * 16 / 100);
                gridView_Main.Columns["InspDurationTimeMilliSec"].Width = (int)(gridWidth * 12 / 100);
                gridView_Main.Columns["GridInspType"].Width = (int)(gridWidth * 7 / 100);
                gridView_Main.Columns["Barcode"].Width = (int)(gridWidth * 11 / 100);
                gridView_Main.Columns["Barcode2"].Width = (int)(gridWidth * 11 / 100);
                //gridView_Main.Columns["itemResultCount"].Width = (int)(gridWidth * 7 / 100);
                gridView_Main.Columns["totalLhResult"].Width = (int)(gridWidth * 5 / 100);
                gridView_Main.Columns["totalRhResult"].Width = (int)(gridWidth * 5 / 100);
                */
            }
            catch (Exception ex)
            {
                CLog.LogErr(ex, "Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            _DoSearch();
        }

        private void _DoSearch()
        {
            ProgressWaitForm.ShowForm(this, "Searching...", "Loading");

            CancellationTokenSource timeout = new CancellationTokenSource();
            timeout.CancelAfter(_SEARCH_TIMEOUT);

            try
            {
                // about Searching
                _results.Clear();


                bool result = false;
                Task.Run(() =>
                {
                    int idx = 0;
                    while (!result)
                    {
                        if (idx % 3 == 0) ProgressWaitForm.SetDescript(string.Format("검색 중."));
                        else if (idx % 3 == 1) ProgressWaitForm.SetDescript(string.Format("검색 중.."));
                        else if (idx % 3 == 2) ProgressWaitForm.SetDescript(string.Format("검색 중..."));
                        idx++;

                        //상단검색조건
                        DateTime startTime = _GetStartTime();
                        DateTime endTime = _GetEndTime();
                        CommonRepository.EnumInspResult resultType = _GetResultType();

                        string resultValue = string.Empty;
                        if (comboBoxEditResult.EditValue != null) resultValue = comboBoxEditResult.Text;
                        string barcode = string.Empty;
                        if (textEditBarcode.EditValue != null) barcode = textEditBarcode.Text;
                        string outBarcode = string.Empty;
                        if (textEditOutBarcode.EditValue != null) outBarcode = textEditOutBarcode.Text;

                        _results = ServerManager.GetResults(int.MinValue, startTime, endTime, resultType, barcode, outBarcode);

                        int rIdx = 0;
                        foreach (SealerResult r in _results.listResult)
                        {
                            r.Index = rIdx + 1;
                            rIdx++;
                        }

                        result = true;

                        System.Threading.Thread.Sleep(100);
                    }
                }, timeout.Token).Wait(timeout.Token);

                ProgressWaitForm.HideForm();
            }
            catch (Exception)
            {
                ProgressWaitForm.HideForm();

                CLog.LogErr("[SearchForm] 서버 연결 확인에 실패하였습니다.");

                CommonRepository.ErrorMsg(this, "데이터 조회에 실패하였습니다. 인터넷 연결상태 및 서버주소를 확인해주세요.");
            }

            gridControlDataInfo.DataSource = _results.listResult;
            gridControlDataInfo.RefreshDataSource();
        }

        private bool _IsHitDataArea(object sender, EventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            DevExpress.Utils.DXMouseEventArgs dxMouseEvtArgs = e as DevExpress.Utils.DXMouseEventArgs;
            if (view != null && dxMouseEvtArgs != null)
            {
                DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hitInfo = view.CalcHitInfo(dxMouseEvtArgs.Location);
                if (hitInfo.InDataRow)
                {
                    return true;
                }
            }

            return false;
        }

        private DateTime _GetStartTime()
        {
            if (dateEditStartDate.EditValue == null) return DateTime.MinValue;

            int startHour = 0;
            if (spinEditStartTime.EditValue.GetType() == typeof(int))
            {
                startHour = (int)spinEditStartTime.EditValue;
            }
            else
            {
                startHour = Decimal.ToInt32((Decimal)spinEditStartTime.EditValue);
            }
            DateTime temps = (DateTime)dateEditStartDate.EditValue;
            return new DateTime(temps.Year, temps.Month, temps.Day, startHour, 0, 0);
        }

        private DateTime _GetEndTime()
        {
            if (dateEditEndDate.EditValue == null) return DateTime.MinValue;

            int endHour = 0;
            if (spinEditEndDate.EditValue.GetType() == typeof(int))
            {
                endHour = (int)spinEditEndDate.EditValue;
            }
            else
            {
                endHour = Decimal.ToInt32((Decimal)spinEditEndDate.EditValue);
            }

            DateTime tempF = (DateTime)dateEditEndDate.EditValue;
            return new DateTime(tempF.Year, tempF.Month, tempF.Day, endHour, 59, 59);
        }

        private CommonRepository.EnumInspResult _GetResultType()
        {
            if (comboBoxEditResult.SelectedIndex == 0) return CommonRepository.EnumInspResult.Unknown;
            else if (comboBoxEditResult.SelectedIndex == 1) return CommonRepository.EnumInspResult.Ok;
            else if (comboBoxEditResult.SelectedIndex == 2) return CommonRepository.EnumInspResult.Ng;

            return CommonRepository.EnumInspResult.Unknown;
        }

        private void _PopupDetailInfo(SealerResult r)
        {
            if (r != null)
            {
                if (r.ResultSeq < 0)
                {
                    CommonRepository.ErrorMsg(this, "검사 세부정보를 불러올 수 없습니다. (시퀀스 정보 없음)");
                }
                else
                {
                    SealerResult detailResult = ServerManager.GetResultDetail(r.ResultSeq);
                    if (detailResult == null)
                    {
                        CommonRepository.ErrorMsg(this, "검사 세부정보를 불러올 수 없습니다. (세부정보 불러오기 실패)");
                    }
                    else
                    {
                        using (DxItemDetailForm form = new DxItemDetailForm(this, detailResult))
                        {
                            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                            }
                        }
                    }
                }
            }
        }

        private void dateEditStartDate_EditValueChanged(object sender, EventArgs e)
        {
            if (!_isInit) return;

            DateTime startTime = _GetStartTime();
            DateTime endTime = _GetEndTime();

            if (startTime > endTime)
            {
                if (startTime.Year == endTime.Year && startTime.Month == endTime.Month && startTime.Day == endTime.Day)
                {
                    spinEditEndDate.EditValue = startTime.Hour;
                }
                else
                {
                    dateEditEndDate.EditValue = new DateTime(startTime.Year, startTime.Month, startTime.Day);
                    spinEditEndDate.EditValue = 23;
                }
            }
        }

        private void spinEditStartTime_EditValueChanged(object sender, EventArgs e)
        {
            if (!_isInit) return;

            DateTime startTime = _GetStartTime();
            DateTime endTime = _GetEndTime();

            if (startTime > endTime)
            {
                spinEditEndDate.EditValue = startTime.Hour;
            }
        }

        private void dateEditEndDate_EditValueChanged(object sender, EventArgs e)
        {
            if (!_isInit) return;

            DateTime startTime = _GetStartTime();
            DateTime endTime = _GetEndTime();

            if (startTime > endTime)
            {
                if (startTime.Year == endTime.Year && startTime.Month == endTime.Month && startTime.Day == endTime.Day)
                {
                    spinEditStartTime.EditValue = endTime.Hour;
                }
                else
                {
                    dateEditStartDate.EditValue = new DateTime(endTime.Year, endTime.Month, endTime.Day);
                    spinEditStartTime.EditValue = 0;
                }
            }
        }

        private void spinEditEndDate_EditValueChanged(object sender, EventArgs e)
        {
            if (!_isInit) return;

            DateTime startTime = _GetStartTime();
            DateTime endTime = _GetEndTime();

            if (startTime > endTime)
            {
                spinEditStartTime.EditValue = endTime.Hour;
            }
        }

        private void labelControlOpenSpreadSheet_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://docs.google.com/spreadsheets/d/" + ProgramConfig.Instance().spreadSheetId);
        }

        private void labelControlBarcode_Click(object sender, EventArgs e)
        {
            if (_results != null && _results.listResult != null && _results.listInspResultCount > 0)
            {
                if (CommonRepository.PopupMsg(this, "이력 조회 전체의 바코드를 출력 하시겠습니까?\r(양품만 출력 가능)", "Notice", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    try
                    {
                        string printName = "TEC B-452-R";
                        string printPaper = "LABEL (100x20)";
                        ProgramConfig config = ProgramConfig.Instance();


                        if (_results != null && _results.listResult != null)
                        {
                            foreach (SealerResult r in _results.listResult)
                            {
                                if (!string.IsNullOrEmpty(r.Barcode) && !string.IsNullOrEmpty(r.OutBarcode) && r.OutBarcode != Server.SealerResult.DEFAULT_BARCODE && r.totalResult == CommonRepository.EnumInspResult.Ok)
                                {
                                    string defaultBarcode = "MOTREX\rMTX-SR100\r@@@@@\r#####\rADS-P1202-P3G";
                                    string barcodeDesc = defaultBarcode.Replace("@@@@@", r.Barcode);
                                    barcodeDesc = barcodeDesc.Replace("#####", r.OutBarcode);
                                }
                            }
                        }
                    }
                    catch (Exception) { }
                }
            }
        }

        private void labelControlExport_Click(object sender, EventArgs e)
        {
            if (_results != null && _results.listResult != null && _results.listInspResultCount > 0)
            {
                try
                {
                    using (DXItemExcel excel = new DXItemExcel(_results))
                    {
                        if (System.Windows.Forms.DialogResult.OK == excel.ShowDialog())
                        {
                        }
                    }
                }
                catch (Exception) { }
            }
        }
    }
}