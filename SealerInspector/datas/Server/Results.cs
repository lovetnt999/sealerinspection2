using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AvisSealer.Server
{
    public class SealerResults
    {
        public List<SealerResult> listResult;

        [XmlIgnore]
        public int listInspResultCount
        {
            get
            {
                if (listResult == null || listResult.Count <= 0)
                {
                    return 0;
                }

                return listResult.Count;
            }
        }


        public SealerResults()
        {
            Clear();
        }

        public SealerResults(List<SealerResult> listResult)
        {
            Set(listResult);
        }

        public void Clear()
        {
            Set(new List<SealerResult>());
        }

        public void Set(List<SealerResult> listResult)
        {
            if (this.listResult == null) this.listResult = new List<SealerResult>();

            this.listResult.Clear();
            if (listResult != null)
            {
                foreach (SealerResult param in listResult)
                {
                    SealerResult newData = new SealerResult();
                    newData.Set(param);
                    this.listResult.Add(newData);
                }
            }
        }

        public void Set(SealerResults src)
        {
            Clear();
            if (src != null)
            {
                Set(src.listResult);
            }
        }
    }

    public class SealerResult
    {
        public static string DEFAULT_BARCODE = "0000000000";

        public static string NONE_BARCODE = "-";

        public int Index { get; set; }

        public int ResultSeq { get; set; }

        public string StartTime
        {
            get
            {
                return startTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            set
            {
                string myFormat = "yyyyMMddHHmmssfff";
                if (false == DateTime.TryParseExact(value, myFormat, null, System.Globalization.DateTimeStyles.AssumeLocal, out startTime))
                {
                    myFormat = "yyyy-MM-dd HH:mm:ss.fff";
                    if (false == DateTime.TryParseExact(value, myFormat, null, System.Globalization.DateTimeStyles.AssumeLocal, out startTime))
                    {
                        startTime = DateTime.MinValue;
                    }
                }
            }
        }
        public DateTime startTime;

        public string InspEndTime
        {
            get
            {
                return inspEndTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            set
            {
                string myFormat = "yyyyMMddHHmmssfff";
                if (false == DateTime.TryParseExact(value, myFormat, null, System.Globalization.DateTimeStyles.AssumeLocal, out inspEndTime))
                {
                    myFormat = "yyyy-MM-dd HH:mm:ss.fff";
                    if (false == DateTime.TryParseExact(value, myFormat, null, System.Globalization.DateTimeStyles.AssumeLocal, out inspEndTime))
                    {
                        inspEndTime = DateTime.MinValue;
                    }
                }
            }
        }
        public DateTime inspEndTime;

        public int inspDurationTimeMilliSec;

        public string InspDurationTimeMilliSec
        {
            get
            {
                return inspDurationTimeMilliSec.ToString("#,###");
            }
        }

        public int TotalResult
        {
            get
            {
                return (int)totalResult;
            }

            set
            {
                int cnt = (int)CommonRepository.EnumInspResult.Count;
                if (0 <= value && value < cnt)
                {
                    totalResult = (CommonRepository.EnumInspResult)value;
                }
                else
                {
                    totalResult = CommonRepository.EnumInspResult.Unknown;
                }
            }
        }
        [XmlIgnore]
        public CommonRepository.EnumInspResult totalResult;

        public int listItemCount
        {
            get
            {
                if (listItem == null) return 0;

                return listItem.Count;
            }
        }

        public int Model { get; set; }

        public string Barcode { get; set; }

        public string OutBarcode { get; set; }

        public int StdSeq { get; set; }

        public string ResultInfo { get; set; }

        [XmlIgnore]
        public string firmwareVersion;

        public SealerStandard std;

        public System.Drawing.Image totalResultImg
        {
            get
            {
                if (totalResult == CommonRepository.EnumInspResult.Ok) return Properties.Resources.icon_pass;
                else if (totalResult == CommonRepository.EnumInspResult.Ng) return Properties.Resources.icon_fail;
                else if (totalResult == CommonRepository.EnumInspResult.Warning) return Properties.Resources.icon_check;

                return Properties.Resources.icon_check ;
            }
        }

        public List<ResultItem> listItem;

        public bool IsSaveData = false;
        public bool IsSaveImg = false;



        public SealerResult()
        {
            Clear();
        }

        public SealerResult(int Index, int ResultSeq, DateTime startTime, DateTime inspEndTime, CommonRepository.EnumInspResult totalResult, string Barcode, string OutBarcode, int Model, int StdSeq, string ResultInfo, int inspDurationTimeMilliSec, string firmwareVersion, List<ResultItem> listItem)
        {
            Set(Index, ResultSeq, startTime, inspEndTime, totalResult, Barcode, OutBarcode, Model, StdSeq, ResultInfo, inspDurationTimeMilliSec, firmwareVersion, listItem);
        }

        public SealerResult(int Index, int ResultSeq, DateTime startTime, DateTime inspEndTime, int totalResult, string Barcode, string OutBarcode, int Model, int StdSeq, string ResultInfo, int inspDurationTimeMilliSec, string firmwareVersion, List<ResultItem> listItem)
        {
            Set(Index, ResultSeq, startTime, inspEndTime, totalResult, Barcode, OutBarcode, Model, StdSeq, ResultInfo, inspDurationTimeMilliSec, firmwareVersion, listItem);
        }

        public void Clear()
        {
            Set(0, 0, DateTime.MinValue, DateTime.MinValue, CommonRepository.EnumInspResult.Unknown, DEFAULT_BARCODE, DEFAULT_BARCODE, 0, 0, string.Empty, 0, string.Empty, null);
        }

        public void Set(int Index, int ResultSeq, DateTime startTime, DateTime inspEndTime, CommonRepository.EnumInspResult totalResult, string Barcode, string OutBarcode, int Model, int StdSeq, string ResultInfo, int inspDurationTimeMilliSec, string firmwareVersion, List<ResultItem> listItem)
        {
            if (this.listItem == null) this.listItem = new List<ResultItem>();

            if (Index >= 0) this.Index = Index;
            if (ResultSeq >= 0) this.ResultSeq = ResultSeq;
            this.startTime = startTime;
            this.inspEndTime = inspEndTime;
            this.totalResult = totalResult;
            this.Barcode = Barcode;
            this.OutBarcode = OutBarcode;
            this.Model = Model;
            this.StdSeq = StdSeq;
            this.ResultInfo = ResultInfo;
            this.inspDurationTimeMilliSec = inspDurationTimeMilliSec;

            this.listItem.Clear();
            if (listItem != null)
            {
                foreach (ResultItem param in listItem)
                {
                    ResultItem newData = new ResultItem();
                    newData.Set(param);
                    this.listItem.Add(newData);
                }
            }

            this.IsSaveData = false;
            this.IsSaveImg = false;

            if (this.std == null) this.std = new SealerStandard();
        }

        public void Set(int Index, int ResultSeq, DateTime startTime, DateTime inspEndTime, int totalResult, string Barcode, string OutBarcode, int Model, int StdSeq, string ResultInfo, int inspDurationTimeMilliSec, string firmwareVersion, List<ResultItem> listItem)
        {
            Set(Index, ResultSeq, startTime, inspEndTime, CommonRepository.EnumInspResult.Unknown, Barcode, OutBarcode, Model, StdSeq, ResultInfo, inspDurationTimeMilliSec, firmwareVersion, listItem);
            this.TotalResult = totalResult;
        }

        public void Set(SealerResult src)
        {
            Clear();
            if (src != null)
            {
                Set(src.Index, src.ResultSeq, src.startTime, src.inspEndTime, src.totalResult, src.Barcode, src.OutBarcode, src.Model, src.StdSeq, src.ResultInfo, src.inspDurationTimeMilliSec, src.firmwareVersion, src.listItem);

                if (this.std == null) this.std = new SealerStandard();
                this.std.Set(src.std);
            }
        }

        public string GetResult()
        {
            if (this.totalResult == CommonRepository.EnumInspResult.Ok) return "OK";
            else if (this.totalResult == CommonRepository.EnumInspResult.Warning) return "WARN";
            else if (this.totalResult == CommonRepository.EnumInspResult.Ng) return "NG";
            else return "";
        }
    }

    public class ResultItem
    {
        public int ItemSeq { get; set; }

        public string Title
        {
            get
            {
                if (stdItem != null) return stdItem.Title;

                return "unknown title";
            }
        }

        public string InspStartTime
        {
            get
            {
                return inspStartTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            set
            {
                string myFormat = "yyyyMMddHHmmssfff";
                if (false == DateTime.TryParseExact(value, myFormat, null, System.Globalization.DateTimeStyles.AssumeLocal, out inspStartTime))
                {
                    myFormat = "yyyy-MM-dd HH:mm:ss.fff";
                    if (false == DateTime.TryParseExact(value, myFormat, null, System.Globalization.DateTimeStyles.AssumeLocal, out inspStartTime))
                    {
                        inspStartTime = DateTime.MinValue;
                    }
                }
            }
        }
        public DateTime inspStartTime;
       

        public string InspEndTime
        {
            get
            {
                return inspEndTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            set
            {
                string myFormat = "yyyyMMddHHmmssfff";
                if (false == DateTime.TryParseExact(value, myFormat, null, System.Globalization.DateTimeStyles.AssumeLocal, out inspEndTime))
                {
                    myFormat = "yyyy-MM-dd HH:mm:ss.fff";
                    if (false == DateTime.TryParseExact(value, myFormat, null, System.Globalization.DateTimeStyles.AssumeLocal, out inspEndTime))
                    {
                        inspEndTime = DateTime.MinValue;
                    }
                }
            }
        }
        public DateTime inspEndTime;

        public int inspDurationTimeMil;

        public int ItemResult
        {
            get
            {
                return (int)itemResult;
            }

            set
            {
                int cnt = (int)CommonRepository.EnumInspResult.Count;
                if (0 <= value && value < cnt)
                {
                    itemResult = (CommonRepository.EnumInspResult)value;
                }
                else
                {
                    itemResult = CommonRepository.EnumInspResult.Unknown;
                }
            }
        }
        public CommonRepository.EnumInspResult itemResult;

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

        public string VehicleNumber { get; set; }

        public string DetectType { get; set; }

        public double LightSensor1 { get; set; }

        public double LightSensor2 { get; set; }

        public double LightSensor3 { get; set; }

        public double LightSensor4 { get; set; }

        public double VoltageSensor1 { get; set; }

        public double VoltageSensor2 { get; set; }

        public string LiveCapturePath { get; set; }

        public string DetectCapturePath { get; set; }

        public int StdItemSeq { get; set; }

        public string ResultInfo { get; set; }

        public SealerStandardItem stdItem;



        public ResultItem()
        {
            Clear();
        }

        public ResultItem(int ResultSeq, DateTime inspStartTime, DateTime inspEndTime, CommonRepository.EnumInspResult itemResult, CommonRepository.BoolType virtualServerConnect, CommonRepository.BoolType isCameraDetect, string VehicleNumber, string DetectInfo,
            double LightSensor1, double LightSensor2, double LightSensor3, double LightSensor4, double VoltageSensor1, double VoltageSensor2, string LiveCapturePath, string DetectCapturePath, int ItemSeq, string ResultInfo, int inspDurationTimeMil)
        {
            Set(ItemSeq, inspStartTime, inspEndTime, itemResult, virtualServerConnect, isCameraDetect, VehicleNumber, DetectInfo, LightSensor1, LightSensor2, LightSensor3, LightSensor4,
                    VoltageSensor1, VoltageSensor2, LiveCapturePath, DetectCapturePath, StdItemSeq, ResultInfo, inspDurationTimeMil);
        }

        public ResultItem(int ItemSeq, DateTime inspStartTime, DateTime inspEndTime, int itemResult, int virtualServerConnect, int isCameraDetect, string VehicleNumber, string DetectInfo,
            double LightSensor1, double LightSensor2, double LightSensor3, double LightSensor4, double VoltageSensor1, double VoltageSensor2, string LiveCapturePath, string DetectCapturePath, int StdItemSeq, string ResultInfo, int inspDurationTimeMil)
        {
            Set(ItemSeq, inspStartTime, inspEndTime, itemResult, virtualServerConnect, isCameraDetect, VehicleNumber, DetectInfo, LightSensor1, LightSensor2, LightSensor3, LightSensor4,
                    VoltageSensor1, VoltageSensor2, LiveCapturePath, DetectCapturePath, StdItemSeq, ResultInfo, inspDurationTimeMil);
        }

        public void Clear()
        {
            Set(0, DateTime.MinValue, DateTime.MinValue, CommonRepository.EnumInspResult.Unknown, CommonRepository.BoolType.Unknown, CommonRepository.BoolType.Unknown, string.Empty, string.Empty, 0, 0, 0, 0, 0, 0, string.Empty, string.Empty, 0, string.Empty, 0);
        }

        public void Set(int ItemSeq, DateTime inspStartTime, DateTime inspEndTime, CommonRepository.EnumInspResult itemResult, CommonRepository.BoolType virtualServerConnect, CommonRepository.BoolType isCameraDetect, string VehicleNumber, string DetectInfo,
            double LightSensor1, double LightSensor2, double LightSensor3, double LightSensor4, double VoltageSensor1, double VoltageSensor2, string LiveCapturePath, string DetectCapturePath, int StdItemSeq, string ResultInfo, int inspDurationTimeMil)
        {
            if (stdItem == null) stdItem = new SealerStandardItem();

            this.ItemSeq = ItemSeq;
            this.inspStartTime = inspStartTime;
            this.inspEndTime = inspEndTime;
            this.itemResult = itemResult;
            this.virtualServerConnect = virtualServerConnect;
            this.isCameraDetect = isCameraDetect;
            this.VehicleNumber = VehicleNumber;
            this.DetectType = DetectInfo;
            this.LightSensor1 = LightSensor1;
            this.LightSensor2 = LightSensor2;
            this.LightSensor3 = LightSensor3;
            this.LightSensor4 = LightSensor4;
            this.VoltageSensor1 = VoltageSensor1;
            this.VoltageSensor2 = VoltageSensor2;
            this.LiveCapturePath = LiveCapturePath;
            this.DetectCapturePath = DetectCapturePath;
            this.StdItemSeq = StdItemSeq;
            this.ResultInfo = ResultInfo;
            this.inspDurationTimeMil = inspDurationTimeMil;
        }

        public void Set(int ItemSeq, DateTime inspStartTime, DateTime inspEndTime, int itemResult, int virtualServerConnect, int isCameraDetect, string VehicleNumber, string DetectInfo,
            double LightSensor1, double LightSensor2, double LightSensor3, double LightSensor4, double VoltageSensor1, double VoltageSensor2, string LiveCapturePath, string DetectCapturePath, int StdItemSeq, string ResultInfo, int inspDurationTimeMil)
        {
            Set(ItemSeq, inspStartTime, inspEndTime, CommonRepository.EnumInspResult.Unknown, CommonRepository.BoolType.Unknown, CommonRepository.BoolType.Unknown, VehicleNumber, DetectInfo,
                LightSensor1, LightSensor2, LightSensor3, LightSensor4, VoltageSensor1, VoltageSensor2, LiveCapturePath, DetectCapturePath, StdItemSeq, ResultInfo, inspDurationTimeMil);
            this.ItemResult = itemResult;
            this.VirtualServerConnect = virtualServerConnect;
            this.IsCameraDetect = isCameraDetect;
        }

        public void Set(ResultItem src)
        {
            Clear();
            if (src != null)
            {
                Set(src.ItemSeq, src.inspStartTime, src.inspEndTime, src.itemResult, src.virtualServerConnect, src.isCameraDetect, src.VehicleNumber, src.DetectType, src.LightSensor1, src.LightSensor2, src.LightSensor3, src.LightSensor4,
                    src.VoltageSensor1, src.VoltageSensor2, src.LiveCapturePath, src.DetectCapturePath, src.StdItemSeq, src.ResultInfo, src.inspDurationTimeMil);

                this.stdItem.Set(src.stdItem);
            }
        }

        public string GetResult()
        {
            if (itemResult == CommonRepository.EnumInspResult.Ok) return "OK";
            else if (itemResult == CommonRepository.EnumInspResult.Ng) return "NG";

            return "unknown";
        }
    }
}
