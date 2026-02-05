using AvisSealer.datas;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvisSealer
{
    public class CommonRepository
    {
        #region Singleton
        private static volatile CommonRepository _instance;
        private static object _syncRoot = new CommonRepository();

        public static CommonRepository Instance()
        {
            lock (_syncRoot)
            {
                if (_instance == null)
                {
                    _instance = new CommonRepository();
                }
            }
            return _instance;
        }
        #endregion

        public enum EnumInspResult
        {
            Unknown = 0,
            Ok,
            Ng,
            Warning,
            Count
        }

        public enum TestDataIndex
        {
            Init = 0,
            TD1,     //일반
            TD2,     //수배
            TD3,  //미납
            Count
        }

        public enum BoolType
        {
            Unknown = 0,
            True,
            False,
            Count
        }

        public enum BtnType
        {
            Unknown = 0,
            Push,
            UnPush,
            Count
        }

        public enum MainState
        {
            Init = 0,
            StandBy,
            Inspection,
            Save,
            Error,
            Destroy
        }

        public static string GRID_NULL_TEXT = "[EMPTY]";

        public static int FORM_GRIP_SIZE = 16;      // Grip size
        public static int FORM_CAPTIONBAR_SIZE = 32 - 5;   // Caption bar height;

        public static string DEFAULT_BOARD_BARCODE = "ADM000000000";

        public static string BARCODE_MODELCODE = "P122";
        public static string BARCODE_MODEL = "MTX-SR100";
        public static string BARCODE_INPUT = "DC 12/24V, 2A";
        public static string BARCODE_REGNUM = "R-R-MOr-MTX-SR100";
        

        //result
        public Server.SealerStandard standard = new Server.SealerStandard();
        public Server.SealerResult lastResult = new Server.SealerResult();

        public Font fontTitle = null;
        public Font fontTitleVersion = null;
        public Brush bruTitle = null;
        public Brush bruTitleVersion = null;
        public Pen penOutLine = null;

        public int inspProcessIndex = 0;
        public int inspProcessMax = 0;

        private Pen _outlinePen = null;

        public MainState mainState = MainState.Init;

        public bool IsConnectServer = false;
        public bool IsCloseProgram = false;
        public bool IsGuiReady = false;
        public int processDelay_ms = 50;
        public bool IsInspBypass = false;
        public bool IsEmegStop = false;

        // virtual
        public bool IsVirtualStart = false;
        public BoolType IsVirtualCylinder = BoolType.Unknown;
        public string virtualBarcode = string.Empty;
        public bool IsVirtualLabelPrint = true;

        // error
        public string errorMessage = string.Empty;

        // device
        public BarcodeInfo lastBarcode = new BarcodeInfo();
        public ImageDownInfo lastLiveImg = new ImageDownInfo();
        public ImageDownInfo lastDetectImg = new ImageDownInfo();

        public AvisSealer.Tools.Device.PUC.PlcPuc puc = null;

        public static int DEFAULT_MODEL_CODE = 9999;


        #region EVENT
        public event EventHandler<EvtChgStateArgs> ChangeState = null;
        public event EventHandler<EvtConnectInfoArgs> ChgConnectServer = null;
        public event EventHandler<EventMsgArgs> EvtPopupMsg = null;
        public event EventHandler<EvtStartInspArgs> StartInspection = null;
        public event EventHandler<EvtFinishInspArgs> FinishInspection = null;
        public event EventHandler<EvtProcessingInspArgs> ProcessingInspection = null;



        public void OnEvtChgStateArgs(object sender, EvtChgStateArgs e)
        {
            EventHandler<EvtChgStateArgs> handler = ChangeState;
            if (handler != null && !IsCloseProgram)
            {
                handler(sender, e);
            }
        }

        public void OnChgConnectServer(object sender, EvtConnectInfoArgs e)
        {
            EventHandler<EvtConnectInfoArgs> handler = ChgConnectServer;
            if (handler != null && !IsCloseProgram)
            {
                handler(sender, e);
            }
        }

        public void OnEventPopupMsg(object sender, EventMsgArgs e)
        {
            EventHandler<EventMsgArgs> handler = EvtPopupMsg;
            if (handler != null && !IsCloseProgram)
            {
                handler(sender, e);
            }
        }

        public void OnStartInspection(object sender, EvtStartInspArgs e)
        {
            EventHandler<EvtStartInspArgs> handler = StartInspection;
            if (handler != null && !IsCloseProgram)
            {
                handler(sender, e);
            }
        }

        public void OnFinishInspection(object sender, EvtFinishInspArgs e)
        {
            EventHandler<EvtFinishInspArgs> handler = FinishInspection;
            if (handler != null && !IsCloseProgram)
            {
                handler(sender, e);
            }
        }

        public void OnProcessingInspection(object sender, EvtProcessingInspArgs e)
        {
            EventHandler<EvtProcessingInspArgs> handler = ProcessingInspection;
            if (handler != null && !IsCloseProgram)
            {
                handler(sender, e);
            }
        }
        #endregion


        private CommonRepository()
        {
            fontTitle = new Font("현대하모니 M", 15, FontStyle.Regular);
            fontTitleVersion = new Font("현대하모니 L", 8, FontStyle.Regular);
            bruTitle = new SolidBrush(Color.White);
            bruTitleVersion = new SolidBrush(Color.FromArgb(217, 217, 217));
            penOutLine = new Pen(Color.FromArgb(242, 164, 10), 2);

            _outlinePen = new Pen(Color.FromArgb(234, 147, 10), 3);
        }


        #region Popup_message
        public static System.Windows.Forms.DialogResult NoticeMsg(System.Windows.Forms.Form parent, string message)
        {
            //return PopupMsg(parent, message, Program.GetResourceSet().GetString("NOUN.NOTIFICATION"), System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);//알림
            return PopupMsg(parent, message, "Notice", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);//알림
        }

        public static System.Windows.Forms.DialogResult WarningMsg(System.Windows.Forms.Form parent, string message)
        {
            //return PopupMsg(parent, message, Program.GetResourceSet().GetString("NOUN.WARNING"), System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);//경고
            return PopupMsg(parent, message, "Warning", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);//경고
        }

        public static System.Windows.Forms.DialogResult ErrorMsg(System.Windows.Forms.Form parent, string message)
        {
            //return PopupMsg(parent, message, Program.GetResourceSet().GetString("NOUN.MISTAKE"), System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);//오류
            return PopupMsg(parent, message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);//경고
        }

        public static System.Windows.Forms.DialogResult PopupMsg(System.Windows.Forms.Form parent, string message, string caption,
            System.Windows.Forms.MessageBoxButtons button, System.Windows.Forms.MessageBoxIcon icon)
        {
            if (parent == null && System.Windows.Forms.Application.OpenForms != null && System.Windows.Forms.Application.OpenForms.Count > 0)
            {
                //parent = System.Windows.Forms.Application.OpenForms[0];
            }

            return DevExpress.XtraEditors.XtraMessageBox.Show(parent, message, caption, button, icon);
        }
        #endregion

        public void FormPaint(Graphics g, Size clientSize, Color backColor, string formTitle, bool showGrip, bool showOutLine)
        {
            if (g != null)
            {
                int width = clientSize.Width;
                int height = clientSize.Height;

                //title
                {
                    //bg
                    int bgW = Properties.Resources.title_bg.Width;
                    for (int i = 0; i <= width / bgW; i++)
                    {
                        g.DrawImage(Properties.Resources.title_bg, new Rectangle(i * bgW, 0, Properties.Resources.title_bg.Width, Properties.Resources.title_bg.Height), new Rectangle(0, 0, Properties.Resources.title_bg.Width, Properties.Resources.title_bg.Height), GraphicsUnit.Pixel);
                    }

                    //string
                    g.DrawString(formTitle, fontTitle, bruTitle, new Point(3, 9));
                }

                //OutLine 그립
                if (showOutLine)
                {
                    g.DrawRectangle(_outlinePen, 1, 1, clientSize.Width - _outlinePen.Width, clientSize.Height - _outlinePen.Width);
                }

                //우측하단 그립
                if (showGrip)
                {
                    int grip = CommonRepository.FORM_GRIP_SIZE;
                    Rectangle rc = new Rectangle(width - grip, height - grip, grip, grip);
                    System.Windows.Forms.ControlPaint.DrawSizeGrip(g, backColor, rc);
                }
            }
        }

        public IntPtr FormEvent(DevExpress.XtraEditors.XtraForm form, System.Windows.Forms.Message m, bool isFormGrip)
        {
            if (form != null && m != null)
            {
                if (m.Msg == 0x84)
                {
                    // Trap WM_NCHITTEST
                    Point pos = new Point(m.LParam.ToInt32());
                    pos = form.PointToClient(pos);
                    if (pos.Y < CommonRepository.FORM_CAPTIONBAR_SIZE)
                    {
                        return (IntPtr)2;  // HTCAPTION
                    }
                    if (isFormGrip && (pos.X >= form.ClientSize.Width - CommonRepository.FORM_GRIP_SIZE && pos.Y >= form.ClientSize.Height - CommonRepository.FORM_GRIP_SIZE))
                    {
                        return (IntPtr)17; // HTBOTTOMRIGHT
                    }
                }
            }

            return IntPtr.Zero;
        }

        public void ClearResult()
        {
            this.lastBarcode.Set(Server.SealerResult.DEFAULT_BARCODE, DateTime.MinValue);
            this.lastLiveImg.Set(string.Empty, DateTime.MinValue);
            this.lastDetectImg.Set(string.Empty, DateTime.MinValue);

            if (lastResult == null) lastResult = new Server.SealerResult();

            lastResult.Clear();
        }

        public string GetBarcode(string modelCode, string yyMMdd, int productIndex)
        {
            string serialDate = string.Empty;
            for (int i = 0; i < yyMMdd.Length; i++)
            {
                string s = yyMMdd.Substring(i, 1);
                if (s == "0") serialDate += "A";
                else if (s == "1") serialDate += "B";
                else if (s == "2") serialDate += "C";
                else if (s == "3") serialDate += "D";
                else if (s == "4") serialDate += "E";
                else if (s == "5") serialDate += "F";
                else if (s == "6") serialDate += "G";
                else if (s == "7") serialDate += "H";
                else if (s == "8") serialDate += "I";
                else if (s == "9") serialDate += "J";
            }

            if (productIndex > 999) productIndex = 999;
            if (productIndex < 0) productIndex = 0;

            return string.Format("{0}{1}{2}", modelCode, serialDate, productIndex.ToString("000"));
        }

        public string GetModelName(int modelCode)
        {
            if (modelCode == CommonRepository.DEFAULT_MODEL_CODE) return "SealerItem";

            return string.Format("Unknown Model ({0})", modelCode);
        }
    }


    #region EVENT_CLASS
    public class EvtChgStateArgs : EventArgs
    {
        public object oldState;
        public object newState;

        public EvtChgStateArgs()
            : this(CommonRepository.MainState.Init, CommonRepository.MainState.Init)
        {
        }

        public EvtChgStateArgs(object oldState, object newState)
        {
            this.oldState = oldState;
            this.newState = newState;
        }
    }
    
    public class EvtConnectInfoArgs : EventArgs
    {
        public bool IsConnect;

        public EvtConnectInfoArgs()
            : this(false)
        {
        }

        public EvtConnectInfoArgs(bool IsConnect)
        {
            this.IsConnect = IsConnect;
        }
    }

    public class EventMsgArgs : EventArgs
    {
        public System.Windows.Forms.MessageBoxIcon icon = System.Windows.Forms.MessageBoxIcon.Information;
        public string msg = string.Empty;

        public EventMsgArgs()
            : this(System.Windows.Forms.MessageBoxIcon.Information, string.Empty)
        {
        }

        public EventMsgArgs(System.Windows.Forms.MessageBoxIcon icon, string msg)
        {
            this.icon = icon;
            this.msg = msg;
        }
    }

    public class EvtStartInspArgs : EventArgs
    {
        public DateTime startInspTime = DateTime.MinValue;
        public Server.SealerStandard standard = null;
        public string barcode = string.Empty;

        public EvtStartInspArgs()
            : this(DateTime.MinValue, null, string.Empty)
        {
        }

        public EvtStartInspArgs(DateTime startInspTime, Server.SealerStandard standard, string barcode)
        {
            this.startInspTime = startInspTime;
            this.standard = standard;
            this.barcode = barcode;
        }
    }

    public class EvtFinishInspArgs : EventArgs
    {
        public DateTime finishInspTime;
        public string barcode;
        public CommonRepository.EnumInspResult result;

        public EvtFinishInspArgs()
            : this(DateTime.MinValue, string.Empty, CommonRepository.EnumInspResult.Unknown)
        {
        }

        public EvtFinishInspArgs(DateTime finishInspTime, string barcode, CommonRepository.EnumInspResult result)
        {
            this.finishInspTime = finishInspTime;
            this.barcode = barcode;
            this.result = result;
        }
    }

    public class EvtProcessingInspArgs : EventArgs
    {
        public int index;
        public int max;
        public Server.ResultItem resultItem;
        public string barcode;
        public string firmwareVer;

        public EvtProcessingInspArgs()
            : this(-1, -1, new Server.ResultItem(), string.Empty, string.Empty)
        {
        }

        public EvtProcessingInspArgs(int index, int max, Server.ResultItem resultItem, string barcode, string firmwareVer)
        {
            this.index = index;
            this.max = max;
            this.resultItem = resultItem;
            this.barcode = barcode;
            this.firmwareVer = firmwareVer;
        }
    }

    public class EvtCancelInspArgs : EventArgs
    {
        public int index;
        public int max;
        public string msg;

        public EvtCancelInspArgs()
            : this(-1, -1, string.Empty)
        {
        }

        public EvtCancelInspArgs(int index, int max, string msg)
        {
            this.index = index;
            this.max = max;
            this.msg = msg;
        }
    }

    public class BarcodeInfo
    {
        public string barcode = string.Empty;
        public DateTime scanTime = DateTime.MinValue;

        public BarcodeInfo() : this(string.Empty, DateTime.MinValue)
        {
        }

        public BarcodeInfo(string barcode, DateTime scanTime)
        {
            Set(barcode, scanTime);
        }

        public void Set(string barcode, DateTime scanTime)
        {
            this.barcode = barcode;
            this.scanTime = scanTime;
        }
    }

    public class ImageDownInfo
    {
        public string imgPath = string.Empty;
        public DateTime downTime = DateTime.MinValue;

        public ImageDownInfo() : this(string.Empty, DateTime.MinValue)
        {
        }

        public ImageDownInfo(string imgPath, DateTime downTime)
        {
            Set(imgPath, downTime);
        }

        public void Set(string imgPath, DateTime downTime)
        {
            this.imgPath = imgPath;
            this.downTime = downTime;
        }
    }

    public class DetectCarInfo
    {
        public string detectTime = string.Empty;
        public string carNumber = string.Empty;
        public string detectType = string.Empty;
        public string detectPath = string.Empty;
        public DateTime eventTime = DateTime.MinValue;

        public DetectCarInfo() : this(string.Empty, string.Empty, string.Empty, string.Empty, DateTime.MinValue)
        {
        }

        public DetectCarInfo(string detectTime, string carNumber, string eventType, string detectPath, DateTime eventTime)
        {
            Set(detectTime, carNumber, eventType, detectPath, eventTime);
        }

        public void Set(string detectTime, string carNumber, string eventType, string detectPath, DateTime eventTime)
        {
            this.detectTime = detectTime;
            this.carNumber = carNumber;
            this.detectType = eventType;
            this.detectPath = detectPath;
            this.eventTime = eventTime;
        }
    }
    #endregion
}
