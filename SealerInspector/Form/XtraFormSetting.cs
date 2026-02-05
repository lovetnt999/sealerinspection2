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

namespace AvisSealer
{
    public partial class XtraFormSetting : DevExpress.XtraEditors.XtraForm
    {
        private CommonRepository _repository = CommonRepository.Instance();

        private MainForm _parent = null;

        private ProgramConfig _config = ProgramConfig.Instance();

        private string NO_DISPLAY = "[NO MONITOR]";



        public XtraFormSetting(System.Windows.Forms.Form parent)
        {
            InitializeComponent();

            this.Owner = parent;
            this._parent = parent as MainForm;
        }

        private void XtraFormSetting_Load(object sender, EventArgs e)
        {
            try
            {
                string[] comlist = System.IO.Ports.SerialPort.GetPortNames();
                if (comlist.Length > 0)
                {
                    comboBoxEditPucPort.Properties.Items.AddRange(comlist);
                    comboBoxEditPucPort.EditValue = _config.pucPort;
                }

                comboBoxEditDisplayIdx.Properties.Items.Clear();
                Screen[] scr = Screen.AllScreens;
                if (scr != null && scr.Length > 1)
                {
                    for (int i = 0; i < scr.Length; i++)
                    {
                        comboBoxEditDisplayIdx.Properties.Items.Add(string.Format("{0}번 모니터", i + 1));
                    }
                }
                else
                {
                    comboBoxEditDisplayIdx.Properties.Items.Add(NO_DISPLAY);
                    comboBoxEditDisplayIdx.ReadOnly = true;
                }

                spinEditDeviceIp1.Value = _config.deviceIp1;
                spinEditDeviceIp2.Value = _config.deviceIp2;
                spinEditDeviceIp3.Value = _config.deviceIp3;
                spinEditDeviceIp4.Value = _config.deviceIp4;
                spinEditDevicePort.Value = _config.devicePort;

                spinEditPcsFtpIp1.Value = _config.pcsFtpIp1;
                spinEditPcsFtpIp2.Value = _config.pcsFtpIp2;
                spinEditPcsFtpIp3.Value = _config.pcsFtpIp3;
                spinEditPcsFtpIp4.Value = _config.pcsFtpIp4;
                spinEditPcsFtpPort.Value = _config.pcsFtpPort;

                textEditPcsFtpID.Text = _config.pcsFtpId;
                textEditPcsFtpPW.Text = _config.pcsFtpPw;

                spinEditPucInputStart.Value = _config.pucInputStart;
                spinEditPucInputCylinder1.Value = _config.pucInputCylinder1;
                spinEditPucInputCylinder2.Value = _config.pucInputCylinder2;
                spinEditPucInputReserved.Value = 0;

                spinEditPucOutputCylinder1.Value = _config.pucOutputCylinder1;
                spinEditPucOutputCylinder2.Value = _config.pucOutputCylinder2;
                spinEditPucOutputReserved1.Value = 0;
                spinEditPucOutputReserved2.Value = 0;

                textEditServerIp.Text = _config.serverIp;
                spinEditServerPort.Value = _config.serverPort;
                spinEditInspType.Value = _config.inspType;

                comboBoxEditLogPeriod.SelectedIndex = _DayToCombobox(_config.logPeriodDay);
                comboBoxEditDataPeriod.SelectedIndex = _DayToCombobox(_config.dataPeriodDay);

                textEditSpreadSheetId.Text = _config.spreadSheetId;

                if (_config.bypass) comboBoxEditBypass.SelectedIndex = 1;
                else comboBoxEditBypass.SelectedIndex = 0;

                if (comboBoxEditDisplayIdx.Properties.Items.Count > _config.imgDispIdx) comboBoxEditDisplayIdx.SelectedIndex = _config.imgDispIdx;
                else comboBoxEditDisplayIdx.SelectedIndex = 0;
            }
            catch (Exception)
            {
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            _repository.FormPaint(e.Graphics, this.ClientSize, this.BackColor, "설정", false, true);
        }

        protected override void WndProc(ref Message m)
        {
            IntPtr r = _repository.FormEvent(this, m, false);
            if (r != IntPtr.Zero) m.Result = r;
            else base.WndProc(ref m);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            try
            {
                _config.pucPort = comboBoxEditPucPort.EditValue.ToString();

                _config.deviceIp1 = (byte)spinEditDeviceIp1.Value;
                _config.deviceIp2 = (byte)spinEditDeviceIp2.Value;
                _config.deviceIp3 = (byte)spinEditDeviceIp3.Value;
                _config.deviceIp4 = (byte)spinEditDeviceIp4.Value;
                _config.devicePort = (int)spinEditDevicePort.Value;

                _config.pcsFtpIp1 = (byte)spinEditPcsFtpIp1.Value;
                _config.pcsFtpIp2 = (byte)spinEditPcsFtpIp2.Value;
                _config.pcsFtpIp3 = (byte)spinEditPcsFtpIp3.Value;
                _config.pcsFtpIp4 = (byte)spinEditPcsFtpIp4.Value;
                _config.pcsFtpPort = (int)spinEditPcsFtpPort.Value;

                _config.pcsFtpId = textEditPcsFtpID.Text;
                _config.pcsFtpPw = textEditPcsFtpPW.Text;

                _config.pucInputStart = (int)spinEditPucInputStart.Value;
                _config.pucInputCylinder1 = (int)spinEditPucInputCylinder1.Value;
                _config.pucInputCylinder2 = (int)spinEditPucInputCylinder2.Value;

                _config.pucOutputCylinder1 = (int)spinEditPucOutputCylinder1.Value;
                _config.pucOutputCylinder2 = (int)spinEditPucOutputCylinder2.Value;

                _config.serverIp = textEditServerIp.Text;
                _config.serverPort = (int)spinEditServerPort.Value;
                _config.inspType = (int)spinEditInspType.Value;

                _config.logPeriodDay = _ComboToday(comboBoxEditLogPeriod.SelectedIndex);
                _config.dataPeriodDay = _ComboToday(comboBoxEditDataPeriod.SelectedIndex);

                _config.spreadSheetId = textEditSpreadSheetId.Text;

                _config.bypass = (comboBoxEditBypass.SelectedIndex == 1);

                _config.imgDispIdx = comboBoxEditDisplayIdx.SelectedIndex;

                _config.SaveXml();

                System.Threading.Thread.Sleep(500);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception)
            {
            }
        }

        private int _DayToCombobox(int day)
        {
            if (day == 30) return 0;
            else if (day == 60) return 1;
            else if (day == 90) return 2;
            else if (day == 180) return 3;
            else if (day == 365) return 4;

            return 3;
        }

        private int _ComboToday(int selectIndex)
        {
            if (selectIndex == 0) return 30;
            else if (selectIndex == 1) return 60;
            else if (selectIndex == 2) return 90;
            else if (selectIndex == 3) return 180;
            else if (selectIndex == 4) return 365;

            return 3;
        }

        private void labelControlOpenSpreadSheet_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://docs.google.com/spreadsheets/d/" + textEditSpreadSheetId.Text);
        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(_config.configDirectory);
        }
    }
}