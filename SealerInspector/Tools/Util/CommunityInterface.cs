using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Wamc.Tools.Util
{
    public class CommunityInterface
    {
        public enum CommunityType
        {
            Unknown = 0,
            Ethernet,
            Serial
        };

        public bool isConnect;
        public string deviceName;
        public CommunityType communityType;

        public CommunityInterface()
        {
            CopyFrom(false, string.Empty);
        }

        public CommunityInterface(bool isConnect, string deviceName)
        {
            CopyFrom(isConnect, deviceName);
        }

        public void CopyFrom(CommunityInterface src)
        {
            if (src != null)
            {
                CopyFrom(src.isConnect, src.deviceName);
            }
        }

        public void CopyFrom(bool isConnect, string deviceName)
        {
            this.communityType = CommunityType.Unknown;
            this.isConnect = isConnect;
            this.deviceName = deviceName;
        }
    }

    public class CommunityEthernet : CommunityInterface
    {
        [XmlAttribute]
        public bool IsConnect
        {
            get
            {
                return isConnect;
            }
            set
            {
                this.isConnect = value;
            }
        }

        [XmlAttribute]
        public string DeviceName
        {
            get
            {
                return deviceName;
            }
            set
            {
                this.deviceName = value;
            }
        }

        [XmlAttribute]
        public CommunityType MyCommunityType
        {
            get
            {

                return communityType;
            }
        }

        [XmlAttribute]
        public string IpAddress
        {
            get;
            set;
        }

        [XmlAttribute]
        public int Port
        {
            get;
            set;
        }

        public CommunityEthernet()
        {
            CopyFrom(string.Empty, 0);
        }

        public CommunityEthernet(string IpAddress, int Port)
        {
            CopyFrom(IpAddress, Port);
        }

        public void CopyFrom(CommunityEthernet src)
        {
            if (src != null)
            {
                CopyFrom(src.IpAddress, src.Port);
            }
        }

        public void CopyFrom(string IpAddress, int Port)
        {
            this.communityType = CommunityType.Ethernet;
            this.IpAddress = IpAddress;
            this.Port = Port;
        }
    }

    public class CommunitySerial : CommunityInterface
    {
        [XmlAttribute]
        public bool IsConnect
        {
            get
            {
                return isConnect;
            }
            set
            {
                this.isConnect = value;
            }
        }

        [XmlAttribute]
        public string DeviceName
        {
            get
            {
                return deviceName;
            }
            set
            {
                this.deviceName = value;
            }
        }

        [XmlAttribute]
        public CommunityType MyCommunityType
        {
            get
            {

                return communityType;
            }
        }

        [XmlAttribute]
        public string PortName
        {
            get;
            set;
        }

        [XmlAttribute]
        public int BaudRate
        {
            get;
            set;
        }

        [XmlAttribute]
        public int DataBits
        {
            get;
            set;
        }

        [XmlAttribute]
        public System.IO.Ports.Parity Parity
        {
            get;
            set;
        }

        [XmlAttribute]
        public System.IO.Ports.StopBits StopBits
        {
            get;
            set;
        }

        public CommunitySerial()
        {
            CopyFrom(string.Empty, 9600, 8, System.IO.Ports.Parity.None, System.IO.Ports.StopBits.None);
        }

        public CommunitySerial(string PortName, int BaudRate)
        {
            CopyFrom(PortName, BaudRate, 8, System.IO.Ports.Parity.None, System.IO.Ports.StopBits.None);
        }

        public CommunitySerial(string PortName, int BaudRate, int DataBits, System.IO.Ports.Parity Parity, System.IO.Ports.StopBits StopBits)
        {
            CopyFrom(PortName, BaudRate, DataBits, Parity, StopBits);
        }

        public void CopyFrom(CommunitySerial src)
        {
            if (src != null)
            {
                CopyFrom(src.PortName, src.BaudRate, src.DataBits, src.Parity, src.StopBits);
            }
        }

        public void CopyFrom(string PortName, int BaudRate, int DataBits, System.IO.Ports.Parity Parity, System.IO.Ports.StopBits StopBits)
        {
            this.communityType = CommunityType.Serial;
            this.PortName = PortName;
            this.BaudRate = BaudRate;
            this.DataBits = DataBits;
            this.Parity = Parity;
            this.StopBits = StopBits;
        }
    }
}
