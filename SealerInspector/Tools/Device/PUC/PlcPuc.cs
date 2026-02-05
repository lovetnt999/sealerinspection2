using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.Util.Log;
using Wamc.Tools.Util;

namespace AvisSealer.Tools.Device.PUC
{
    public class PlcPuc
    {
        private SerialPort _serial = null;
        private int _recvDelay = 50;

        public bool isConnect = false;

        public bool isVirtual = false;

        public string comPort = string.Empty;

        public PlcPuc()
        {
            _serial = new SerialPort();
        }

        public ErrorCode Connect(string comPort, bool isVirtual = false)
        {
            try
            {
                this.isVirtual = isVirtual;
                this.comPort = comPort;

                if (_serial.IsOpen)
                {
                    Disconnect();
                }


                if (!isVirtual)
                {
                    if (!_serial.IsOpen && comPort.ToUpper().IndexOf("COM") >= 0)
                    {
                        _serial.PortName = comPort;
                        _serial.BaudRate = 115200;
                        _serial.DataBits = 8;
                        _serial.Parity = System.IO.Ports.Parity.None;
                        _serial.StopBits = System.IO.Ports.StopBits.One;

                        _serial.Open();
                        isConnect = _serial.IsOpen;

                        _InitComm();
                        return ErrorCode.SUCCESS;
                    }
                    else
                    {
                        return ErrorCode.F_CONNECT_BAD_IPADDRESS;
                    }
                }
                else
                {
                    isConnect = true;
                }
            }
            catch (Exception ex)
            {
                CLog.LogErr(ex, "Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

            return ErrorCode.F_CONNECT_CANT_FIND_INTERFACE;
        }

        public ErrorCode Disconnect()
        {
            if (isVirtual) return ErrorCode.SUCCESS;

            try
            {
                if (_serial.IsOpen)
                {
                    _serial.Close();
                }

                isConnect = _serial.IsOpen;

                return ErrorCode.SUCCESS;
            }
            catch (Exception ex)
            {
                CLog.LogErr(ex, "Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

            return ErrorCode.F_CONNECT_CANT_FIND_INTERFACE;
        }

        public ErrorCode ReceiveData(int startOffset, ref byte[] value)
        {
            if (isVirtual) return ErrorCode.SUCCESS;

            ErrorCode returnCode = ErrorCode.F_UNKNOWN;

            try
            {
                if (isConnect)
                {
                    _serial.ReadTimeout = 100;
                    _serial.Read(value, startOffset, value.Length);
                    returnCode = ErrorCode.SUCCESS;
                }
            }
            catch (Exception ex)
            {
                CLog.LogErr(ex, "Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name);
                returnCode = ErrorCode.F_PLC_READ_ERROR;
            }

            return returnCode;
        }

        public ErrorCode SendData(int startOffset, byte[] value)
        {
            if (isVirtual) return ErrorCode.SUCCESS;

            ErrorCode returnCode = ErrorCode.F_UNKNOWN;

            try
            {
                if (value != null && isConnect)
                {
                    _serial.Write(value, startOffset, value.Length);
                    returnCode = ErrorCode.SUCCESS;
                }
            }
            catch (Exception ex)
            {
                CLog.LogErr(ex, "Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name);
                returnCode = ErrorCode.F_PLC_READ_ERROR;
            }

            return returnCode;
        }

        private ErrorCode _RecvData(ref string value)
        {
            if (isVirtual) return ErrorCode.SUCCESS;

            _serial.ReadTimeout = 100;
            value = _serial.ReadExisting();
            value += _serial.ReadExisting();
            value += _serial.ReadExisting();

            value = value.Replace(" ", "");
            value = value.Replace("\n", "");
            value = value.Replace("\r", "");

            return ErrorCode.SUCCESS;
        }

        private ErrorCode _SendData(string value)
        {
            if (isVirtual) return ErrorCode.SUCCESS;

            _serial.Write(value);
            return ErrorCode.SUCCESS;
        }

        private string ToBinary(byte[] data)
        {
            return string.Join(" ", data.Select(byt => Convert.ToString(byt, 2).PadLeft(8, '0')));
        }

        private int[] ToBinary(string data)
        {
            if (string.IsNullOrEmpty(data)) return new int[] { 0, 0, 0, 0, 0, 0, 0, 0 };

            string res = string.Empty;
            try
            {
                if (data.Length == 1)
                {
                    res = Convert.ToString(Convert.ToInt32(data, 16), 2).PadLeft(8, '0');   //16진수 > 2진수
                    res = new string(res.ToCharArray().Reverse().ToArray());                //문자열 반전
                }
                else
                {
                    for (int i = 0; i < data.Length; i++)
                    {
                        string r = Convert.ToString(Convert.ToInt32(data[i].ToString(), 16), 2).PadLeft(8, '0');   //16진수 > 2진수
                        res += new string(r.ToCharArray().Reverse().ToArray());                //문자열 반전
                    }
                }

                return Array.ConvertAll(res.ToArray(), c => (int)(c - '0'));
            }
            catch (Exception) { }

            return new int[] { 0, 0, 0, 0, 0, 0, 0, 0 };
        }


        private ErrorCode _InitComm()
        {
            ErrorCode returnCode = ErrorCode.F_UNKNOWN;
            string recvData = string.Empty;

            // Poll 방식 변경
            if (WamcError.Failed(returnCode = _SendData("$SMIL\r\n")))
            {
                return returnCode;
            }
            System.Threading.Thread.Sleep(_recvDelay);
            if (WamcError.Failed(returnCode = _RecvData(ref recvData)))
            {
                return returnCode;
            }
            if (recvData.IndexOf("!MIL") < 0)
            {
                return ErrorCode.F_SERIAL_RECEIVE_READ_PACKET_ERROR;
            }

            // 출력값 초기화
            if (WamcError.Failed(returnCode = _SendData("$SYO0000\r\n")))
            {
                return returnCode;
            }
            System.Threading.Thread.Sleep(_recvDelay);
            if (WamcError.Failed(returnCode = _RecvData(ref recvData)))
            {
                return returnCode;
            }
            if (recvData.IndexOf("!O0000") < 0)
            {
                return ErrorCode.F_SERIAL_RECEIVE_READ_PACKET_ERROR;
            }

            return ErrorCode.SUCCESS;
        }

        public int[] GetInput()
        {
            if (_serial.IsOpen)
            {
                ErrorCode returnCode = ErrorCode.F_UNKNOWN;
                string recvData = string.Empty;

                if (WamcError.Failed(returnCode = _SendData("$GI\r\n")))
                {
                    return null;
                }
                System.Threading.Thread.Sleep(_recvDelay);
                if (WamcError.Failed(returnCode = _RecvData(ref recvData)))
                {
                    return null;
                }

                if (string.IsNullOrEmpty(recvData)) return null;

                string data = recvData.Substring(recvData.Length - 2, 2);
                data = new string(data.ToCharArray().Reverse().ToArray());
                return ToBinary(data);
            }

            return null;
        }

        public bool GetInput_buttonUnpush()
        {
            int[] inputData = GetInput();
            if (inputData == null) return false;

            return (inputData[0] == 1);
        }

        public bool GetInput_buttonPush()
        {
            int[] inputData = GetInput();
            if (inputData == null) return false;

            return (inputData[1] == 1);
        }

        public bool GetInput_Start()
        {
            int[] inputData = GetInput();
            if (inputData == null) return false;

            return (inputData[2] == 1);
        }

        public void SetOutput_push(bool isPush)
        {
            if (_serial.IsOpen)
            {
                ErrorCode returnCode = ErrorCode.F_UNKNOWN;
                string recvData = string.Empty;

                if (!isPush)
                {
                    _SendData(string.Format("$SIO0{0}\r\n", 0));    //0번 출력 0으로 변경
                    _SendData(string.Format("$SIO1{0}\r\n", 1));    //1번 출력 1로 변경
                }
                else
                {
                    _SendData(string.Format("$SIO1{0}\r\n", 0));
                    _SendData(string.Format("$SIO0{0}\r\n", 1));
                }

                System.Threading.Thread.Sleep(_recvDelay);
                if (WamcError.Failed(returnCode = _RecvData(ref recvData)))
                {
                    return;
                }
            }
        }

        public void SetOutput_pushTest(bool isPush)
        {
            if (_serial.IsOpen)
            {
                ErrorCode returnCode = ErrorCode.F_UNKNOWN;
                string recvData = string.Empty;

                if (isPush)
                {
                    _SendData(string.Format("$SIO5{0}\r\n", 0));
                    _SendData(string.Format("$SIO6{0}\r\n", 1));
                }
                else
                {
                    _SendData(string.Format("$SIO6{0}\r\n", 0));
                    _SendData(string.Format("$SIO5{0}\r\n", 1));
                }

                System.Threading.Thread.Sleep(_recvDelay);
                if (WamcError.Failed(returnCode = _RecvData(ref recvData)))
                {
                    return;
                }
            }
        }

        public void SetOutput_startLed(bool isOn)
        {
            if (_serial.IsOpen)
            {
                ErrorCode returnCode = ErrorCode.F_UNKNOWN;
                string recvData = string.Empty;

                if (isOn)
                {
                    _SendData(string.Format("$SIO2{0}\r\n", 1));    //0번 출력 0으로 변경
                }
                else
                {
                    _SendData(string.Format("$SIO2{0}\r\n", 0));
                }

                System.Threading.Thread.Sleep(_recvDelay);
                if (WamcError.Failed(returnCode = _RecvData(ref recvData)))
                {
                    return;
                }
            }
        }
    }
}
