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
    public partial class DXItemVirtualController : DevExpress.XtraEditors.XtraForm
    {
        private CommonRepository _repository = CommonRepository.Instance();

        private Timer _reset = new Timer();

        private Timer _continueInsp = new Timer();

        public DXItemVirtualController()
        {
            InitializeComponent();
        }

        private void DXItemVirtualController_Load(object sender, EventArgs e)
        {
            _reset.Interval = 500;
            _reset.Tick += _reset_Tick;

            _continueInsp.Interval = 30 * 1000;
            _continueInsp.Tick += _continueInsp_Tick;
        }

        private void _continueInsp_Tick(object sender, EventArgs e)
        {
            simpleButtonInspStart_Click(this, new EventArgs());
        }

        private void _reset_Tick(object sender, EventArgs e)
        {
            _reset.Stop();

            _repository.IsVirtualStart = false;
            simpleButtonInspStart.Enabled = true;
        }

        private void simpleButtonInspStart_Click(object sender, EventArgs e)
        {
            _repository.IsVirtualStart = true;
            simpleButtonInspStart.Enabled = false;

            _reset.Start();
        }

        private void toggleSwitchCylinder_Toggled(object sender, EventArgs e)
        {
            if (toggleSwitchCylinder.IsOn) _repository.IsVirtualCylinder = CommonRepository.BoolType.True;
            else _repository.IsVirtualCylinder = CommonRepository.BoolType.False;
        }

        private void simpleButtonClose_Click(object sender, EventArgs e)
        {
            _reset.Stop();

            if(_continueInsp.Enabled) _continueInsp.Stop();

            _repository.IsVirtualStart = false;
            _repository.IsVirtualCylinder = CommonRepository.BoolType.Unknown;

            this.Hide();
        }

        private void DXItemVirtualController_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                simpleButtonInspStart.Enabled = true;
                toggleSwitchCylinder.IsOn = false;

                _repository.IsVirtualStart = false;
                _repository.IsVirtualCylinder = CommonRepository.BoolType.False;
                textEditBarcode.Text = string.Format("ADM{0}", DateTime.Now.ToString("MMddfffff"));//Server.LprResult.DEFAULT_BARCODE;

                toggleSwitchContinueInsp.IsOn = false;
                toggleSwitchLebelPrint.IsOn = true;
            }
        }

        private void textEditBarcode_EditValueChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textEditBarcode.Text)) _repository.virtualBarcode = textEditBarcode.Text;
        }

        private void toggleSwitchContinueInsp_Toggled(object sender, EventArgs e)
        {
            if (toggleSwitchContinueInsp.IsOn)
            {
                _continueInsp.Start();
                simpleButtonInspStart_Click(this, new EventArgs());
            }
            else _continueInsp.Stop();
        }

        private void toggleSwitchLebelPrint_Toggled(object sender, EventArgs e)
        {
            _repository.IsVirtualLabelPrint = !toggleSwitchLebelPrint.IsOn;
        }
    }   
}