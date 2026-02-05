using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Tools.Util.Log;

namespace AvisSealer
{
    public class ProgramConfig
    {
        [XmlIgnore]
        private string configFileName = "programConfig.conf";

        [XmlIgnore]
        public string configDirectory = string.Empty;

        [XmlIgnore]
        private static volatile ProgramConfig _instance;

        [XmlIgnore]
        private static object _syncRoot = new object();

        public static ProgramConfig Instance()
        {
            lock (_syncRoot)
            {
                if (_instance == null)
                {
                    _instance = new ProgramConfig();
                    if (!_instance.ReadXml())
                    {
                        _instance.Clear();
                        _instance.SaveXml();
                    }
                }
            }
            return _instance;
        }


        [XmlElement]
        public string pucPort;


        [XmlIgnore]
        public string deviceIp
        {
            get
            {
                return string.Format("{0}.{1}.{2}.{3}", deviceIp1, deviceIp2, deviceIp3, deviceIp4);
            }
        }

        [XmlElement]
        public byte deviceIp1;

        [XmlElement]
        public byte deviceIp2;

        [XmlElement]
        public byte deviceIp3;

        [XmlElement]
        public byte deviceIp4;

        [XmlElement]
        public int devicePort;



        [XmlIgnore]
        public string virtualPoliceServerIp
        {
            get
            {
                return string.Format("{0}.{1}.{2}.{3}", pcsFtpIp1, pcsFtpIp2, pcsFtpIp3, pcsFtpIp4);
            }
        }

        [XmlElement]
        public byte pcsFtpIp1;

        [XmlElement]
        public byte pcsFtpIp2;

        [XmlElement]
        public byte pcsFtpIp3;

        [XmlElement]
        public byte pcsFtpIp4;

        [XmlElement]
        public int pcsFtpPort;

        [XmlElement]
        public string pcsFtpId;

        [XmlElement]
        public string pcsFtpPw;



        [XmlElement]
        public int pucInputStart;

        [XmlElement]
        public int pucInputCylinder1;

        [XmlElement]
        public int pucInputCylinder2;

        [XmlElement]
        public int pucOutputCylinder1;

        [XmlElement]
        public int pucOutputCylinder2;



        [XmlElement]
        public string serverIp;

        [XmlElement]
        public int serverPort;

        [XmlElement]
        public int inspType;


        [XmlElement]
        public int logPeriodDay;

        [XmlElement]
        public int dataPeriodDay;

        [XmlElement]
        public string spreadSheetId;

        [XmlElement]
        public bool bypass;

        [XmlElement]
        public int imgDispIdx;



        private ProgramConfig()
            : this(string.Empty)
        {
        }

        private ProgramConfig(string serverAdress)
        {
            this.configDirectory = string.Format("{0}\\WAMC\\{1}\\", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Program.PROCESS_NAME);
            if (System.IO.Directory.Exists(this.configDirectory) == false)
            {
                System.IO.Directory.CreateDirectory(this.configDirectory);
            }

            this.configDirectory = string.Format("{0}\\WAMC\\{1}\\config\\", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Program.PROCESS_NAME);
            if (System.IO.Directory.Exists(this.configDirectory) == false)
            {
                System.IO.Directory.CreateDirectory(this.configDirectory);
            }

            string configPath = SealerFilePath.GetConfigPath();
            string logPath = SealerFilePath.GetLogPath();

            if (System.IO.Directory.Exists(configPath) == false) System.IO.Directory.CreateDirectory(configPath);
            if (System.IO.Directory.Exists(logPath) == false) System.IO.Directory.CreateDirectory(logPath);
        }

        public void CopyFrom(ProgramConfig src)
        {
            if (src != null)
            {
                this.pucPort = src.pucPort;

                this.deviceIp1 = src.deviceIp1;
                this.deviceIp2 = src.deviceIp2;
                this.deviceIp3 = src.deviceIp3;
                this.deviceIp4 = src.deviceIp4;
                this.devicePort = src.devicePort;

                this.pcsFtpIp1 = src.pcsFtpIp1;
                this.pcsFtpIp2 = src.pcsFtpIp2;
                this.pcsFtpIp3 = src.pcsFtpIp3;
                this.pcsFtpIp4 = src.pcsFtpIp4;
                this.pcsFtpPort = src.pcsFtpPort;
                this.pcsFtpId = src.pcsFtpId;
                this.pcsFtpPw = src.pcsFtpPw;

                this.pucInputStart = src.pucInputStart;
                this.pucInputCylinder1 = src.pucInputCylinder1;
                this.pucInputCylinder2 = src.pucInputCylinder2;
                this.pucOutputCylinder1 = src.pucOutputCylinder1;
                this.pucOutputCylinder2 = src.pucOutputCylinder2;

                this.serverIp = src.serverIp;
                this.serverPort = src.serverPort;
                this.inspType = src.inspType;

                this.logPeriodDay = src.logPeriodDay;
                this.dataPeriodDay = src.dataPeriodDay;

                this.spreadSheetId = src.spreadSheetId;

                this.bypass = src.bypass;

                this.imgDispIdx = src.imgDispIdx;
            }
        }

        public void Clear()
        {
            this.pucPort = "COM1";

            this.deviceIp1 = 192;
            this.deviceIp2 = 168;
            this.deviceIp3 = 40;
            this.deviceIp4 = 200;
            this.devicePort = 0;

            this.pcsFtpIp1 = 192;
            this.pcsFtpIp2 = 168;
            this.pcsFtpIp3 = 1;
            this.pcsFtpIp4 = 143;
            this.pcsFtpPort = 2221;

            this.pcsFtpId = "admin";
            this.pcsFtpPw = "123123";


            this.pucInputStart = 1;
            this.pucInputCylinder1 = 2;
            this.pucInputCylinder2 = 3;
            this.pucOutputCylinder1 = 1;
            this.pucOutputCylinder2 = 2;

            this.serverIp = "sbc.auto-it.co.kr";
            this.serverPort = 9051;
            this.inspType = 1;

            this.logPeriodDay = 180;
            this.dataPeriodDay = 180;

            this.spreadSheetId = "133DqF7_5IS5VlKhfM66RJNRdOs9SOiczlD66-ZYOac0";

            this.bypass = false;

            this.imgDispIdx = 2;
        }

        public bool Equals(ProgramConfig gc)
        {
            if (Object.ReferenceEquals(gc, null))
            {
                return false;
            }
            if (Object.ReferenceEquals(this, gc))
            {
                return true;
            }
            if (this.GetType() != gc.GetType())
            {
                return false;
            }

            return (this.pucPort == gc.pucPort ||
                this.deviceIp1 == gc.deviceIp1 ||
                this.deviceIp2 == gc.deviceIp2 ||
                this.deviceIp3 == gc.deviceIp3 ||
                this.deviceIp4 == gc.deviceIp4 ||
                this.devicePort == gc.devicePort ||
                this.pcsFtpIp1 == gc.pcsFtpIp1 ||
                this.pcsFtpIp2 == gc.pcsFtpIp2 ||
                this.pcsFtpIp3 == gc.pcsFtpIp3 ||
                this.pcsFtpIp4 == gc.pcsFtpIp4 ||
                this.pcsFtpPort == gc.pcsFtpPort ||
                this.pcsFtpId == gc.pcsFtpId ||
                this.pcsFtpPw == gc.pcsFtpPw ||
                this.pucInputStart == gc.pucInputStart ||
                this.pucInputCylinder1 == gc.pucInputCylinder1 ||
                this.pucInputCylinder2 == gc.pucInputCylinder2 ||
                this.pucOutputCylinder1 == gc.pucOutputCylinder1 ||
                this.pucOutputCylinder2 == gc.pucOutputCylinder2 ||
                this.serverIp == gc.serverIp ||
                this.serverPort == gc.serverPort ||
                this.inspType == gc.inspType ||
                this.logPeriodDay == gc.logPeriodDay ||
                this.dataPeriodDay == gc.dataPeriodDay ||
                this.spreadSheetId == gc.spreadSheetId ||
                this.bypass == gc.bypass ||
                this.imgDispIdx == gc.imgDispIdx);
        }

        public bool ReadXml()
        {
            return ReadXml(string.Empty);
        }

        public bool ReadXml(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                path = this.configDirectory + configFileName;
            }

            System.IO.FileInfo fi = new System.IO.FileInfo(path);
            if (fi.Exists == false)
            {
                return false;
            }

            try
            {
                ProgramConfig xml = null;
                System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(ProgramConfig));
                using (System.IO.StreamReader sr = new System.IO.StreamReader(path))
                {
                    xml = (ProgramConfig)reader.Deserialize(sr);
                    sr.Close();
                }

                if (xml != null)
                {
                    this.CopyFrom(xml);

                    if (this.pcsFtpIp1 == 0) this.pcsFtpIp1 = 192;
                    if (this.pcsFtpIp2 == 0) this.pcsFtpIp2 = 9;
                    if (this.pcsFtpIp3 == 0) this.pcsFtpIp3 = 200;
                    if (this.pcsFtpIp4 == 0) this.pcsFtpIp4 = 150;
                    if (this.pcsFtpPort == 0) this.pcsFtpPort = 2221;
                    if (string.IsNullOrEmpty(this.pcsFtpId)) this.pcsFtpId = "admin";
                    if (string.IsNullOrEmpty(this.pcsFtpPw)) this.pcsFtpPw = "123123";
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return false;
        }

        public bool SaveXml()
        {
            return SaveXml(string.Empty);
        }

        public bool SaveXml(string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                {
                    path = this.configDirectory + configFileName;
                }

                System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(GetType());
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(path))
                {
                    writer.Serialize(sw, this);
                    sw.Close();
                }

                return true;
            }
            catch (System.Exception ex)
            {
                CLog.LogErr(ex);
            }

            return false;
        }
    }
}
