using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Tools.Util.Log;

namespace AvisSealer.Server
{
    public class SealerStandards
    {
        public List<SealerStandard> listStd { get; set; }

        [XmlIgnore]
        public int listStdCount
        {
            get
            {
                if (listStd == null || listStd.Count <= 0)
                {
                    return 0;
                }

                return listStd.Count;
            }
        }


        public SealerStandards()
        {
            Clear();
        }

        public SealerStandards(List<SealerStandard> listStd)
        {
            Set(listStd);
        }

        public void Clear()
        {
            Set(null);
        }

        public void Set(List<SealerStandard> listStd)
        {
            if (this.listStd == null) this.listStd = new List<SealerStandard>();
            this.listStd.Clear();

            if (listStd != null)
            {
                foreach (SealerStandard param in listStd)
                {
                    SealerStandard newData = new SealerStandard();
                    newData.Set(param.SeqStandard, param.Model, param.listItem);
                    this.listStd.Add(newData);
                }
            }
        }
    }

    public class SealerStandard
    {
        public int SeqStandard;

        public int Model { get; set; }

        public List<SealerStandardItem> listItem { get; set; }

        [XmlIgnore]
        public int listItemCount
        {
            get
            {
                if (listItem == null || listItem.Count <= 0)
                {
                    return 0;
                }

                return listItem.Count;
            }
        }


        public SealerStandard()
        {
            Clear();
        }

        public SealerStandard(int SeqStandard, int Model, List<SealerStandardItem> listItem)
        {
            Set(SeqStandard, Model, listItem);
        }

        public void Clear()
        {
            Set(0, 0, null);
        }

        public void Set(int SeqStandard, int Model, List<SealerStandardItem> listItem)
        {
            if (this.listItem == null) this.listItem = new List<SealerStandardItem>();
            this.listItem.Clear();

            this.SeqStandard = SeqStandard;
            this.Model = Model;

            if (listItem != null)
            {
                foreach (SealerStandardItem param in listItem)
                {
                    SealerStandardItem newData = new SealerStandardItem();
                    newData.Set(param);
                    this.listItem.Add(newData);
                }
            }
        }

        public void Set(SealerStandard src)
        {
            Clear();
            if (src != null)
            {
                Set(src.SeqStandard, src.Model, src.listItem);
            }
        }
    }

    public class SealerStandardItem
    {
        public int SeqItem { get; set; }

        public string Title { get; set; }

        public int ControlPushBtn
        {
            get
            {
                return (int)controlPushBtn;
            }

            set
            {
                int cnt = (int)CommonRepository.BtnType.Count;
                if (0 <= value && value < cnt)
                {
                    controlPushBtn = (CommonRepository.BtnType)value;
                }
                else
                {
                    controlPushBtn = CommonRepository.BtnType.Unknown;
                }
            }
        }
        public CommonRepository.BtnType controlPushBtn;


        public int VirtualServerConnect
        {
            get
            {
                return (int)virtualServerConnect;
            }

            set
            {
                int cnt = (int)CommonRepository.BoolType.Count;
                if (0 <= value && value < cnt)
                {
                    virtualServerConnect = (CommonRepository.BoolType)value;
                }
                else
                {
                    virtualServerConnect = CommonRepository.BoolType.Unknown;
                }
            }
        }
        public CommonRepository.BoolType virtualServerConnect;

        public int IsCameraDetect
        {
            get
            {
                return (int)isCameraDetect;
            }

            set
            {
                int cnt = (int)CommonRepository.BoolType.Count;
                if (0 <= value && value < cnt)
                {
                    isCameraDetect = (CommonRepository.BoolType)value;
                }
                else
                {
                    isCameraDetect = CommonRepository.BoolType.Unknown;
                }
            }
        }
        public CommonRepository.BoolType isCameraDetect;

        public int IsBarcodeScan
        {
            get
            {
                return (int)isBarcodeScan;
            }

            set
            {
                int cnt = (int)CommonRepository.BoolType.Count;
                if (0 <= value && value < cnt)
                {
                    isBarcodeScan = (CommonRepository.BoolType)value;
                }
                else
                {
                    isBarcodeScan = CommonRepository.BoolType.Unknown;
                }
            }
        }
        public CommonRepository.BoolType isBarcodeScan;

        public double LightSensor1;

        public double LightSensor2;

        public double LightSensor3;

        public double LightSensor4;

        public double VoltageSensor1;

        public double VoltageSensor2;

        public int OutVehicleIndex
        {
            get
            {
                return (int)outVehicleIndex;
            }

            set
            {
                int cnt = (int)CommonRepository.TestDataIndex.Count;
                if (0 <= value && value < cnt)
                {
                    outVehicleIndex = (CommonRepository.TestDataIndex)value;
                }
                else
                {
                    outVehicleIndex = CommonRepository.TestDataIndex.Init;
                }
            }
        }
        public CommonRepository.TestDataIndex outVehicleIndex;


        public string VehicleNumber;

        public string DetectType;


        public uint InspBeforeDelay;

        public uint InspAfterDelay;


        public int InspectDelayMs { get; set; }


        public SealerStandardItem()
        {
            Clear();
        }

        public SealerStandardItem(int SeqItem, string Title, CommonRepository.BtnType controlPushBtn, CommonRepository.BoolType virtualServerConnect, CommonRepository.BoolType isCameraDetect, CommonRepository.BoolType isBarcodeScan,
            double LightSensor1, double LightSensor2, double LightSensor3, double LightSensor4, double VoltageSensor1, double VoltageSensor2, CommonRepository.TestDataIndex outVehicleIndex, string VehicleNumber, string VehicleType, uint InspBeforeDelay, uint InspAfterDelay)
        {
            Set(SeqItem, Title, controlPushBtn, virtualServerConnect, isCameraDetect, isBarcodeScan, LightSensor1, LightSensor2, LightSensor3, LightSensor4, VoltageSensor1, VoltageSensor2, outVehicleIndex, VehicleNumber, VehicleType, InspBeforeDelay, InspAfterDelay);
        }

        public SealerStandardItem(int SeqItem, string Title, int controlPushBtn, int virtualServerConnect, int isCameraDetect, int isBarcodeScan, double LightSensor1, double LightSensor2, double LightSensor3, double LightSensor4,
            double VoltageSensor1, double VoltageSensor2, int outVehicleIndex, string VehicleNumber, string VehicleType, uint InspBeforeDelay, uint InspAfterDelay)
        {
            Set(SeqItem, Title, controlPushBtn, virtualServerConnect, isCameraDetect, isBarcodeScan, LightSensor1, LightSensor2, LightSensor3, LightSensor4, VoltageSensor1, VoltageSensor2, outVehicleIndex, VehicleNumber, VehicleType, InspBeforeDelay, InspAfterDelay);
        }

        public void Clear()
        {
            Set(-1, string.Empty, CommonRepository.BtnType.Unknown, CommonRepository.BoolType.Unknown, CommonRepository.BoolType.Unknown, CommonRepository.BoolType.Unknown, -1, -1, -1, -1, -1, -1, CommonRepository.TestDataIndex.Init, string.Empty, string.Empty, 0, 0);
        }

        public void Set(int SeqItem, string Title, CommonRepository.BtnType controlPushBtn, CommonRepository.BoolType virtualServerConnect, CommonRepository.BoolType isCameraDetect, CommonRepository.BoolType isBarcodeScan,
            double LightSensor1, double LightSensor2, double LightSensor3, double LightSensor4, double VoltageSensor1, double VoltageSensor2, CommonRepository.TestDataIndex outVehicleIndex, string VehicleNumber, string VehicleType, uint InspBeforeDelay, uint InspAfterDelay)
        {
            if (SeqItem >= 0) this.SeqItem = SeqItem;
            this.Title = Title;
            this.controlPushBtn = controlPushBtn;
            this.virtualServerConnect = virtualServerConnect;
            this.isCameraDetect = isCameraDetect;
            this.isBarcodeScan = isBarcodeScan;
            this.LightSensor1 = LightSensor1;
            this.LightSensor2 = LightSensor2;
            this.LightSensor3 = LightSensor3;
            this.LightSensor4 = LightSensor4;
            this.VoltageSensor1 = VoltageSensor1;
            this.VoltageSensor2 = VoltageSensor2;
            this.outVehicleIndex = outVehicleIndex;
            this.VehicleNumber = VehicleNumber;
            this.DetectType = VehicleType;
            this.InspBeforeDelay = InspBeforeDelay;
            this.InspAfterDelay = InspAfterDelay;

            if (string.IsNullOrEmpty(this.VehicleNumber)) this.VehicleNumber = "-";
            if (string.IsNullOrEmpty(this.DetectType)) this.DetectType = "-";
        }

        public void Set(int SeqItem, string Title, int controlPushBtn, int virtualServerConnect, int isCameraDetect, int isBarcodeScan, double LightSensor1, double LightSensor2, double LightSensor3, double LightSensor4,
            double VoltageSensor1, double VoltageSensor2, int outVehicleIndex, string VehicleNumber, string VehicleType, uint InspBeforeDelay, uint InspAfterDelay)
        {
            Set(SeqItem, Title, CommonRepository.BtnType.Unknown, CommonRepository.BoolType.Unknown, CommonRepository.BoolType.Unknown, CommonRepository.BoolType.Unknown, LightSensor1, LightSensor2, LightSensor3, LightSensor4, VoltageSensor1, VoltageSensor2, CommonRepository.TestDataIndex.Init, VehicleNumber, VehicleType, InspBeforeDelay, InspAfterDelay);
            this.ControlPushBtn = controlPushBtn;
            this.VirtualServerConnect = virtualServerConnect;
            this.OutVehicleIndex = outVehicleIndex;
            this.IsCameraDetect = isCameraDetect;
            this.IsBarcodeScan = isBarcodeScan;
        }

        public void Set(SealerStandardItem src)
        {
            if (src != null)
            {
                Set(src.SeqItem, src.Title, src.controlPushBtn, src.virtualServerConnect, src.isCameraDetect, src.isBarcodeScan, src.LightSensor1, src.LightSensor2, src.LightSensor3, src.LightSensor4,
                    src.VoltageSensor1, src.VoltageSensor2, src.outVehicleIndex, src.VehicleNumber, src.DetectType, src.InspBeforeDelay, src.InspAfterDelay);
            }
        }

        public string GetDbString()
        {
            return "";// return string.Format("_addStdVehicleSignal({0}, {1}, {2}, {3}, {4}, {5}, {6});", CanConnect ? 1 : 0, IGN1 ? 1 : 0, IGN2 ? 1 : 0, GearD ? 1 : 0, GearP ? 1 : 0, GearR ? 1 : 0, DoorOpen ? 1 : 0);
        }
    }
}
