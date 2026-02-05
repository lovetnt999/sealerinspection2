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
using AvisSealer.datas;
using Tools.Util.Log;

namespace AvisSealer
{
    public partial class DxItemDetailForm : DevExpress.XtraEditors.XtraForm
    {
        private CommonRepository _repository = CommonRepository.Instance();

        private MainForm _parent = null;

        private SealerResult _result = null;

        public List<MainGrid> _listGridResult = new List<MainGrid>();


        public DxItemDetailForm(System.Windows.Forms.Form parent, SealerResult result)
        {
            InitializeComponent();

            this.Owner = parent;
            this._parent = parent as MainForm;

            if (result != null)
            {
                if (_result == null) _result = new SealerResult();
                this._result.Set(result);
            }
        }

        private void DxItemDetailForm_Load(object sender, EventArgs e)
        {
            try
            {
                _listGridResult.Add(new MainGrid());
                gridControlDataInfo.DataSource = _listGridResult;
                gridControlDataInfo.RefreshDataSource();
                InitGrid();
                _listGridResult.Clear();
                gridControlDataInfo.RefreshDataSource();


                if (_result != null)
                {
                    labelControlInfoStartTime.Text = _result.startTime.ToString("yyyy-MM-dd HH:mm:ss");
                    labelControlInfoEndTime.Text = _result.inspEndTime.ToString("yyyy-MM-dd HH:mm:ss");
                    labelControlInfoDuration.Text = _result.InspDurationTimeMilliSec + "ms";
                    labelControlInfoItemCount.Text = _result.listItemCount + "개";
                    labelControlInfoBarcode.Text = string.Format("{0} ({1})", _result.Barcode, _result.OutBarcode);
                    if (_result.Barcode == Server.SealerResult.DEFAULT_BARCODE) labelControlInfoBarcode.Appearance.Options.UseImage = false;
                    else labelControlInfoBarcode.Appearance.Options.UseImage = true;


                    labelControlInfoResult.Appearance.Image = _result.totalResultImg;
                    if (_result.totalResult == CommonRepository.EnumInspResult.Ok) labelControlInfoResult.Text = "      OK";
                    else labelControlInfoResult.Text = "      NG";

                    if (_result.listItem != null && _result.listItem.Count > 0)
                    {
                        StandardToGridData(_result, ref _listGridResult);
                        gridControlDataInfo.DataSource = _listGridResult;
                        gridControlDataInfo.RefreshDataSource();
                    }
                }
            }
            catch (Exception) { }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            _repository.FormPaint(e.Graphics, this.ClientSize, this.BackColor, "상세보기", false, true);
        }

        protected override void WndProc(ref Message m)
        {
            /*
            IntPtr r = _repository.FormEvent(this, m, true);
            if (r != IntPtr.Zero) m.Result = r;
            else base.WndProc(ref m);
            */
            base.WndProc(ref m);
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

        private void gridViewDataInfo_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            listBoxControlInspDetail.Items.Clear();

            if (pictureEditLive.Tag != null && (bool)pictureEditLive.Tag && pictureEditLive.Image != null)
            {
                pictureEditLive.Image.Dispose();
                pictureEditLive.Image = null;
            }
            pictureEditLive.Image = Properties.Resources.autoit_ci;
            pictureEditLive.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Clip;
            pictureEditLive.Tag = false;

            if (pictureEditDetect.Tag != null && (bool)pictureEditDetect.Tag && pictureEditDetect.Image != null)
            {
                pictureEditDetect.Image.Dispose();
                pictureEditDetect.Image = null;
            }
            pictureEditDetect.Image = Properties.Resources.autoit_ci;
            pictureEditDetect.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Clip;
            pictureEditDetect.Tag = false;


            MainGrid gridResult = gridViewDataInfo.GetRow(e.FocusedRowHandle) as MainGrid;
            if (gridResult != null && gridResult.resultItem != null)
            {
                ResultItem r = gridResult.resultItem;
                if (r != null)
                {
                    if (!string.IsNullOrEmpty(r.ResultInfo))
                    {
                        r.ResultInfo = r.ResultInfo.Replace("\n", "");
                        string[] strs = r.ResultInfo.Split(new char[] { '\r' }, StringSplitOptions.RemoveEmptyEntries);
                        if (strs != null)
                        {
                            foreach (string s in strs)
                            {
                                if (!string.IsNullOrEmpty(s)) listBoxControlInspDetail.Items.Add(s);
                            }
                        }

                        /*
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
                                    foreach (string s in strs)
                                    {
                                        if (!string.IsNullOrEmpty(s)) listBoxControlInspDetail.Items.Add(s);
                                    }
                                }
                            }
                        }
                        */
                    }

                    if (System.IO.File.Exists(r.LiveCapturePath))
                    {
                        try
                        {
                            pictureEditLive.Image = Image.FromFile(r.LiveCapturePath);
                            pictureEditLive.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
                            pictureEditLive.Tag = true;
                        }
                        catch (Exception) { }
                    }

                    if (System.IO.File.Exists(r.DetectCapturePath))
                    {
                        try
                        {
                            pictureEditDetect.Image = Image.FromFile(r.DetectCapturePath);
                            pictureEditDetect.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
                            pictureEditDetect.Tag = true;
                        }
                        catch (Exception) { }
                    }
                }
            }
        }

        private void InitGrid()
        {
            try
            {
                gridViewDataInfo.Columns["Index"].Caption = "순번";
                gridViewDataInfo.Columns["InspTitle"].Caption = "검사항목";
                gridViewDataInfo.Columns["InspDuration"].Caption = "검사시간";
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
                gridViewDataInfo.Columns["InspTitle"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                gridViewDataInfo.Appearance.FocusedRow.Font = gridViewDataInfo.Columns[0].AppearanceCell.Font;

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

        private void StandardToGridData(SealerResult src, ref List<MainGrid> result)
        {
            try
            {
                if (src != null && src.listItem != null && src.listItem.Count > 0)
                {
                    result.Clear();
                    int index = 1;
                    foreach (ResultItem item in src.listItem)
                    {
                        MainGrid add = new MainGrid(index, item.Title, (double)item.inspDurationTimeMil, item.itemResult);
                        add.resultItem = item;
                        result.Add(add);
                        index++;
                    }
                }
            }
            catch (Exception ex)
            {
                CLog.LogErr(ex, "Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void labelControlInfoBarcode_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(labelControlInfoBarcode.Text);
            CommonRepository.PopupMsg(this, "Clipboard에 바코드 정보를 복사하였습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}