using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wamc.Tools.Util
{
    /*
    상위 3byte : 구분 Code
        * 1자리 : Success or Fail
        * 2자리 : MajorType
        - 0 : 기본 type
        - 1 : 알고리듬
        - 2 :
        - 3 :
        - 4 :
        - 5 :
        - 6 :
        - 7 :
        - 8 :
        - 9 :
        - A :
        - B :
        - C :
        - D : interface (Camera, PLC 등)
        - E : connect & disconnect
        - F : Comport or serial
     
    하위 3byte : MinorType
        * 구분 숫자 (순차적으로 입력)
    */

    public enum ErrorCode
    {
        BLINK = 0x000000,
        SUCCESS = 0x000001,
        Return_True = 0x000002,
        Return_False = 0x000003,

        //DATABASE
        S_DB_NODATA_DO_DEFAULT = 0x0C0003,


        // 기본 에러
        F_UNKNOWN       = 0x800000,
        F_INVALID_VALUE = 0x800001,
        F_BAD_ARGUMENT  = 0x800002,
        F_INSP_TIMEOUT  = 0x800003,
        F_SAVE_TIMEOUT  = 0x800004,
        F_OUTOFMEMORY   = 0x800005,
        F_SYSTEMPAUSE   = 0x800006,
        F_CANCEL        = 0x800007,
        F_USER1         = 0x800008,
        F_USER2         = 0x800009,
        F_NOT_INIT      = 0x80000A,


        // 알고리듬
        F_ALGORITHM_UNKNOWN         = 0x810000,
        F_ALGORITHM_INVALID_VALUE   = 0x810001,
        F_ALGORITHM_BAD_ARGUMENT    = 0x810002,
        F_ALGORITHM_TIMEOUT         = 0x810003,
        F_ALGORITHM_OUTOFMEMORY     = 0x810004,
        F_ALGORITHM_EMPTY_DATA      = 0x810005,
        F_ALGORITHM_NOT_INIT        = 0x810006,

        // 파일
        F_FILE_UNKNOWN              = 0x820000,
        F_FILE_INVALID_VALUE        = 0x820001,
        F_FILE_NOT_FOUND            = 0x820002,
        F_FILE_USING_FILE           = 0x820003,

        // Database
        F_DB_UNKNOWN                = 0x8C0000,
        F_DB_INVALID_VALUE          = 0x8C0001,
        F_DB_NODATA                 = 0x8C0002,
        F_DB_TIMEOUT                = 0x8C0003,
        F_DB_CANT_FIND_DATA         = 0x8C0004,
        F_DB_ADD_DATA               = 0x8C0005,
        F_DB_ADD_DATA_DUPLICATE     = 0x8C0006,

        // interface
        F_INTERFACE_UNKNOWN         = 0x8D0000,
        F_INTERFACE_FAIL            = 0x8D0001,
        F_INTERFACE_BUSY            = 0x8D0002,
        F_INTERFACE_CANT_CONNECT    = 0x8D0003,
        F_INTERFACE_TIMEOUT         = 0x8D0004,
        F_INTERFACE_NOT_FOUND       = 0x8D0005,
        F_INTERFACE_ERROR           = 0x8D0006,

        // connect 에러
        F_CONNECT_UNKNOWN               = 0x8E0000,
        F_CONNECT_FAIL                  = 0x8E0001,
        F_CONNECT_THREAD_BUSY           = 0x8E0002,
        F_CONNECT_ERROR                 = 0x8E0003,
        F_CONNECT_BAD_ARG               = 0x8E0004,
        F_CONNECT_BAD_IPADDRESS         = 0x8E0005,
        F_CONNECT_SDK_ERROR             = 0x8E0006,
        F_CONNECT_CANT_FIND_INTERFACE   = 0x8E0007,
        F_CONNECT_NOT_READY             = 0x8E0008,
        F_CONNECT_ALREADY_CONNECT       = 0x8E0009,

        // disconnect 에러
        F_DISCONNECT_UNKNOWN        = 0x8E1000,
        F_DISCONNECT_TIMEOUT        = 0x8E1001,
        F_DISCONNECT_ERROR          = 0x8E1002,
        F_DISCONNECT_BUSY           = 0x8E1003,
        F_DISCONNECT_NOT_CONNECT    = 0x8E1004,
        F_CLOSE_THREAD_TIMEOUT      = 0x8E1005,
        F_CLOSE_THREAD_ERROR        = 0x8E1006,

        // Upload 에러
        F_UPLOAD_UNKNOWN            = 0x8E2000,
        F_UPLOAD_TIMEOUT            = 0x8E2001,
        F_UPLOAD_DISCONNECT         = 0x8E2002,
        F_UPLOAD_BAD_DATA            = 0x8E2003,

        // 외부 장치 통신
        F_SERIAL_NOT_CONNECTED              = 0x8F0001,
        F_SERIAL_THREAD_NOT_RUNNING         = 0x8F0002,
        F_SERIAL_THREAD_NOT_READY           = 0x8F0003,
        F_SERIAL_READ_ACK_TIMEOUT           = 0x8F0004,
        F_SERIAL_WRITE_ACK_TIMEOUT          = 0x8F0005,
        F_SERIAL_READ_FAIL                  = 0x8F0006,
        F_SERIAL_READ_ERROR                 = 0x8F0007,
        F_SERIAL_WRITE_ERROR                = 0x8F0008,
        F_SERIAL_RECEIVE_READ_PACKET_ERROR  = 0x8F0009,
        F_SERIAL_RECEIVE_WRITE_PACKET_ERROR = 0x8F000A,
        F_SERIAL_READ_SIGNAL_ERROR          = 0x8F000B,
        F_SERIAL_WRITE_SIGNAL_ERROR         = 0x8F000C,
        F_SERIAL_MAKE_PACKET_ERROR          = 0x8F000D,
        F_SERIAL_RESET_SIGNAL_ERROR         = 0x8F000E,
        F_SERIAL_SET_SIGNAL_TIMEOUT         = 0x8F000F,
        F_SERIAL_SET_SIGNAL_ERROR           = 0x8F0010,
        F_SERIAL_GET_SIGNAL_ERROR           = 0x8F0011,
        
        F_SOCKET_SEND_FAIL              = 0x8F1001,
        F_SOCKET_SEND_ERROR             = 0x8F1002,
        F_SOCKET_RECEIVE_FAIL           = 0x8F1003,
        F_SOCKET_RECEIVE_ERROR          = 0x8F1004,

        F_PLC_NOT_CONNECTED             = 0x8F2001,
        F_PLC_THREAD_NOT_RUNNING        = 0x8F0002,
        F_PLC_THREAD_NOT_READY          = 0x8F0003,
        F_PLC_READ_ERROR                = 0x8F2004,
        F_PLC_WRITE_ERROR               = 0x8F2005,
        F_PLC_RESET_SIGNAL_ERROR        = 0x8F000E,
        F_PLC_GET_SIGNAL_TIMEOUT        = 0x8F2006,
        F_PLC_GET_SIGNAL_ERROR          = 0x8F2007,
        F_PLC_SET_SIGNAL_TIMEOUT        = 0x8F2008,
        F_PLC_SET_SIGNAL_ERROR          = 0x8F2009,
        F_PLC_NOT_MATCH_HEADER          = 0x8F2010,
        F_PLC_NOT_MATCH_PACKET_LENGTH   = 0x8F2011,
        F_NGBUFFER_NOT_READY            = 0x8F2013,
        F_PLC_DATA_SIZE_TOO_LONG        = 0x8F2014,

        F_CAMERA_NOT_CONNECTED              = 0x8F3001,
        F_CAMERA_NOT_INIT                   = 0x8F3002,
        F_CAMERA_BUSY                       = 0x8F3003,
        F_CAMERA_CANT_FOUND                 = 0x8F3004,
        F_CAMERA_FAILED_CAPTURE             = 0x8F3005,

        F_LIGHT_NOT_CONNECTED               = 0x8F4001,
        F_LIGHT_NOT_INIT                    = 0x8F4002,
        F_LIGHT_BUSY                        = 0x8F4003,
        F_LIGHT_CANT_FOUND                  = 0x8F4004,
        F_LIGHT_FAILED_SETUP                = 0x8F4005,

        // SELTJ
        F_UNIT_IS_NULL                      = 0x8F4006,
    }

    public class WamcError
    {
        public static bool Success(ErrorCode code)
        {
            return (code == ErrorCode.SUCCESS || code == ErrorCode.S_DB_NODATA_DO_DEFAULT);
        }

        public static bool Failed(ErrorCode code)
        {
            return !Success(code);
        }


        public static string GetErroString(ErrorCode type)//다국어
        {
            string msg = string.Empty;

            switch (type)
            {
                case ErrorCode.BLINK:
                    msg = "BLINK";
                    break;

                case ErrorCode.SUCCESS:
                    //msg = "성공"; 
                    msg = "Success";
                    break;
                case ErrorCode.S_DB_NODATA_DO_DEFAULT:
                    //msg = "성공: 데이터베이스에 데이터가 존재 하지않아 기본값으로 실행하였습니다.";
                    msg = "SUCCESS: The default was run because no data exists in the database";
                    break;

                case ErrorCode.F_UNKNOWN:
                    //msg = "실패: 알 수 없는 이유로 실패했습니다.";
                    msg = "Failed: Failed for unknown reason.";
                    break;
                case ErrorCode.F_INVALID_VALUE:
                    //msg = "실패: 알 수 없는 값이 존재합니다.";
                    msg = "Failed: An unknown value exists.";
                    break;
                case ErrorCode.F_BAD_ARGUMENT:
                    //msg = "실패: 잘못 입력된 인자값이 있습니다.";
                    msg = "Failed: There is an invalid argument value.";
                    break;
                case ErrorCode.F_INSP_TIMEOUT:
                    //msg = "대기 시간을 초과하였습니다. (inspection)";
                    msg = "The waiting time has been exceeded. (inspection)";
                    break;
                case ErrorCode.F_SAVE_TIMEOUT:
                    //msg = "대기 시간을 초과하였습니다. (save)";
                    msg = "The waiting time has been exceeded. (save)";
                    break;
                case ErrorCode.F_OUTOFMEMORY:
                    //msg = "실패: 메모리가 부족합니다.";
                    msg = "Failed: Out of Memory";
                    break;
                case ErrorCode.F_SYSTEMPAUSE:
                    //msg = "장비가 비상정지하였습니다.";
                    msg = "The equipment has been emergency stopped.";
                    break;
                case ErrorCode.F_CANCEL:
                    //msg = "명령이 취소되었습니다.";
                    msg = "The command was canceled.";
                    break;
                case ErrorCode.F_USER1:
                    //msg = "실패: 초기화가 되지 않았습니다.";
                    msg = "Failed: Not initialized.";
                    break;
                case ErrorCode.F_USER2:
                    //msg = "사용자 정의 에러";
                    msg = "User-defined error";
                    break;
                case ErrorCode.F_NOT_INIT:
                    //msg = "실패: 초기화가 되지 않았습니다.";
                    msg = "Failed: Not initialized.";
                    break;


                case ErrorCode.F_INTERFACE_UNKNOWN:
                    //msg = "실패: 알 수 없는 오류 장치(센서)연결에 실패하였습니다.";
                    msg = "Failed: Unknown error The device (sensor) connection failed.";
                    break;
                case ErrorCode.F_INTERFACE_FAIL:
                    //msg = "실패: 장치(센서) 제어에 실패하였습니다.";
                    msg = "Failed: Failed to control device (sensor).";
                    break;
                case ErrorCode.F_INTERFACE_BUSY:
                    //msg = "실패: 장치(센서)가 동작 중에는 제어를 하실 수 없습니다.";
                    msg = "Failed: Control is not available while the device (sensor) is operating.";
                    break;
                case ErrorCode.F_INTERFACE_CANT_CONNECT:
                    //msg = "실패: 장치(센서)에 연결할 수 없습니다. 연결 커넥터 또는 장치(센서) 전원을 확인해주세요.";
                    msg = "Failed: Unable to connect to device (sensor). Check the connector or sensor (sensor) power supply.";
                    break;
                case ErrorCode.F_INTERFACE_TIMEOUT:
                    //msg = "실패: 장치(센서)가 응답 시간을 초과하였습니다.";
                    msg = "Failed: The device (sensor) has exceeded the response time.";
                    break;
                case ErrorCode.F_INTERFACE_NOT_FOUND:
                    //msg = "실패: 장치(센서)를 찾을 수가 없습니다. 연결 커넥터 상태 또는 장치(센서) 전원을 확인해주세요.";
                    msg = "Failed: Device (Sensor) not found Check the connection connector status or the device (sensor) power.";
                    break;
                case ErrorCode.F_INTERFACE_ERROR:
                    //msg = "실패: 장치(센서)에 오류가 발생하였습니다. 장치 상태를 확인해주세요.";
                    msg = "Failed: An error has occurred in the device (sensor). Please check the device status.";
                    break;



                case ErrorCode.F_CONNECT_UNKNOWN:
                    //msg = "실패: 알 수 없는 이유로 연결에 실패했습니다.";
                    msg = "Failed: The connection failed for an unknown reason.";
                    break;
                case ErrorCode.F_CONNECT_FAIL:
                    //msg = "실패: 연결에 실패했습니다.";
                    msg = "Failed: The connection failed.";
                    break;
                case ErrorCode.F_CONNECT_THREAD_BUSY:
                    //msg = "실패: 쓰레드가 작업 중이라 연결할 수 없습니다.";
                    msg = "Failed: The thread is busy and cannot connect.";
                    break;
                case ErrorCode.F_CONNECT_ERROR:
                    //msg = "실패: 예기치 않은 에러가 발생하여 연결에 실패했습니다.";
                    msg = "Failed: The connection failed because an unexpected error occurred.";
                    break;
                case ErrorCode.F_CONNECT_BAD_ARG:
                    //msg = "실패: 입력된 데이터로 연결 시도를 할 수 없습니다.";
                    msg = "Failed: A connection attempt cannot be made with the data entered.";
                    break;
                case ErrorCode.F_CONNECT_BAD_IPADDRESS:
                    //msg = "실패: 장치(센서) 접속을 위한 IP 주소가 잘못 설정되어있습니다.";
                    msg = "Failed: The IP address for accessing the device (sensor) is incorrectly set.";
                    break;
                case ErrorCode.F_CONNECT_SDK_ERROR:
                    //msg = "실패: 장치(센서) 제어에 실패하였습니다. (SDK 오류)";
                    msg = "Failed: Failed to control device (sensor). (SDK error)";
                    break;
                case ErrorCode.F_CONNECT_CANT_FIND_INTERFACE:
                    //msg = "실패: 장치(센서)를 찾을 수 없습니다. 연결 커넥터 또는 장치(센서) 전원을 확인해주세요.";
                    msg = "Failed: Device (Sensor) not found Check the connector or sensor (sensor) power supply.";
                    break;
                case ErrorCode.F_CONNECT_NOT_READY:
                    //msg = "실패: 연결된 장치(센서)가 사용할 준비가 안 되었습니다. 연결상태 및 초기화 상태를 확인해주세요.";
                    msg = "Failed: The connected device (sensor) is not ready for use. Please check the connection and initialization.";
                    break;
                case ErrorCode.F_CONNECT_ALREADY_CONNECT:
                    //msg = "실패: 이미 연결된 장치(센서) 입니다.";
                    msg = "Failed: Device (sensor) already connected.";
                    break;



                case ErrorCode.F_DISCONNECT_BUSY:
                    //msg = "실패: 장치(센서) 사용 중에는 연결 해지를 할 수 없습니다.";
                    msg = "Failed: You cannot disconnect while the device (sensor) is in use.";
                    break;
                case ErrorCode.F_DISCONNECT_NOT_CONNECT:
                    //msg = "실패: 연결된 장치(센서)를 찾을 수 없습니다.";
                    msg = "Failed: No connected device (sensor) found";
                    break;
                case ErrorCode.F_DISCONNECT_UNKNOWN:
                    //msg = "실패: 알 수 없는 이유로 연결 해제에 실패했습니다.";
                    msg = "Failed: The connection failed for an unknown reason.";
                    break;
                case ErrorCode.F_DISCONNECT_TIMEOUT:
                    //msg = "실패: 연결 해제 시간이 초과되었습니다.";
                    msg = "Failed: The disconnect timed out.";
                    break;
                case ErrorCode.F_DISCONNECT_ERROR:
                    //msg = "실패: 연결 해제 도중에 예기치 않은 에러가 발생했습니다.";
                    msg = "Failed: An unexpected error occurred while disconnecting.";
                    break;
                case ErrorCode.F_CLOSE_THREAD_TIMEOUT:
                    //msg = "실패: 스레드 종료 시간이 초과되었습니다.";
                    msg = "Failed: The thread termination timed out.";
                    break;
                case ErrorCode.F_CLOSE_THREAD_ERROR:
                    //msg = "실패: 예기치 않은 에러가 발생하여 쓰레드가 종료되었습니다.";
                    msg = "Failed: The thread terminated due to an unexpected error.";
                    break;

                case ErrorCode.F_UPLOAD_UNKNOWN:
                    msg = "Failed: 파일 업로드에 실패하였습니다.(Unknown)";
                    break;
                case ErrorCode.F_UPLOAD_TIMEOUT:
                    msg = "Failed: 파일 업로드에 실패하였습니다.(timeout)";
                    break;
                case ErrorCode.F_UPLOAD_DISCONNECT:
                    msg = "Failed: 파일 업로드에 실패하였습니다.(서버 연결 끊김)";
                    break;
                case ErrorCode.F_UPLOAD_BAD_DATA:
                    msg = "Failed: 파일 업로드에 실패하였습니다.(bad data)";
                    break;

                case ErrorCode.F_SERIAL_NOT_CONNECTED:
                    //msg = "실패: 통신이 연결되지 않았습니다.";
                    msg = "Failed: There is no communication.";
                    break;
                case ErrorCode.F_SERIAL_THREAD_NOT_RUNNING:
                    //msg = "실패: 쓰레드가 동작 중이 아닙니다.";
                    msg = "Failed: The thread is not running.";
                    break;
                case ErrorCode.F_SERIAL_THREAD_NOT_READY:
                    //msg = "실패: 쓰레드가 송수신 가능 상태가 아닙니다.";
                    msg = "Failed: The thread is not ready to send or receive.";
                    break;
                case ErrorCode.F_SERIAL_READ_ACK_TIMEOUT:
                    //msg = "실패: 읽기 명령에 대한 응답 시간이 초과되었습니다.";
                    msg = "Failed: The response timed out for the read command.";
                    break;
                case ErrorCode.F_SERIAL_WRITE_ACK_TIMEOUT:
                    //msg = "실패: 쓰기 명령에 대한 응답 시간이 초과되었습니다.";
                    msg = "Failed: The response timed out for the write command.";
                    break;
                case ErrorCode.F_SERIAL_READ_FAIL:
                    //msg = "실패: 패킷 수신에 실패했습니다.";
                    msg = "Failed: Failed to receive a packet.";
                    break;
                case ErrorCode.F_SERIAL_READ_ERROR:
                    //msg = "실패: 패킷 수신 중 예기치 않은 에러가 발생했습니다.";
                    msg = "Failed: An unexpected error occurred while receiving a packet.";
                    break;
                case ErrorCode.F_SERIAL_WRITE_ERROR:
                    //msg = "실패: 패킷 전송 중 예기치 않은 에러가 발생했습니다.";
                    msg = "Failed: An unexpected error occurred while sending a packet.";
                    break;
                case ErrorCode.F_SERIAL_RECEIVE_READ_PACKET_ERROR:
                    //msg = "실패: 에러 패킷이 수신되었습니다.";
                    msg = "Failed: An error packet was received.";
                    break;
                case ErrorCode.F_SERIAL_RECEIVE_WRITE_PACKET_ERROR:
                    //msg = "실패: 에러 패킷이 수신되었습니다.";
                    msg = "Failed: An error packet was received.";
                    break;
                case ErrorCode.F_SERIAL_READ_SIGNAL_ERROR:
                    //msg = "실패: 신호 수신 중 예기치 않은 에러가 발생했습니다.";
                    msg = "Failed: An unexpected error occurred while receiving a signal.";
                    break;
                case ErrorCode.F_SERIAL_WRITE_SIGNAL_ERROR:
                    //msg = "실패: 신호 전송 중 예기치 않은 에러가 발생했습니다.";
                    msg = "Failed: An unexpected error occurred during signal transmission.";
                    break;
                case ErrorCode.F_SERIAL_MAKE_PACKET_ERROR:
                    //msg = "실패: 신호 전송 중 예기치 않은 에러가 발생했습니다.";
                    msg = "Failed: An unexpected error occurred during signal transmission.";
                    break;
                case ErrorCode.F_SERIAL_RESET_SIGNAL_ERROR:
                    //msg = "실패: 신호를 초기화 하는 중 예기치 않은 에러가 발생했습니다.";
                    msg = "Failed: An unexpected error occurred while initializing the signal.";
                    break;
                case ErrorCode.F_SERIAL_SET_SIGNAL_TIMEOUT:
                    //msg = "실패: 신호 설정 시간이 초과되었습니다.";
                    msg = "Failed: The signal set timed out.";
                    break;
                case ErrorCode.F_SERIAL_SET_SIGNAL_ERROR:
                    //msg = "실패: 신호 설정 중 예기치 않은 에러가 발생했습니다.";
                    msg = "Failed: An unexpected error occurred during signal setup.";
                    break;
                case ErrorCode.F_SERIAL_GET_SIGNAL_ERROR:
                    //msg = "실패: 신호 확인 중 예기치 않은 에러가 발생했습니다.";
                    msg = "Failed: An unexpected error occurred while checking the signal.";
                    break;

                case ErrorCode.F_SOCKET_SEND_FAIL:
                    //msg = "실패: Socket 통신에서 데이터 전송을 실패했습니다.";
                    msg = "Failed: Data transfer failed on Socket communication.";
                    break;
                case ErrorCode.F_SOCKET_SEND_ERROR:
                    //msg = "실패: Socket 통신에서 데이터 전송 중 예기치 않은 에러가 발생했습니다.";
                    msg = "Failed: An unexpected error occurred during data transfer in Socket communication.";
                    break;
                case ErrorCode.F_SOCKET_RECEIVE_FAIL:
                    //msg = "실패: Socket 통신에서 데이터 수신을 실패했습니다.";
                    msg = "Failed: Failed to receive data from Socket communication.";
                    break;
                case ErrorCode.F_SOCKET_RECEIVE_ERROR:
                    //msg = "실패: 소켓 통신에서 데이터 수신 중 예기치 않은 에러가 발생했습니다.";
                    msg = "Failed: An unexpected error occurred while receiving data on socket communication.";
                    break;
                case ErrorCode.F_PLC_NOT_CONNECTED:
                    //msg = "실패: PLC가 연결되지 않았습니다.";
                    msg = "Failed: The PLC is not connected.";
                    break;
                case ErrorCode.F_PLC_READ_ERROR:
                    //msg = "실패: PLC의 데이터를 읽어오는 중 예기치 않은 에러가 발생했습니다.";
                    msg = "Failed: An unexpected error occurred while reading data from the PLC.";
                    break;
                case ErrorCode.F_PLC_WRITE_ERROR:
                    //msg = "실패: PLC에 데이터를 기록하는 중 예기치 않은 에러가 발생했습니다.";
                    msg = "Failed: An unexpected error occurred while writing data to the PLC.";
                    break;
                case ErrorCode.F_PLC_GET_SIGNAL_TIMEOUT:
                    //msg = "실패: PLC 신호 대기 시간이 초과되었습니다.";
                    msg = "Failed: The PLC signal wait timed out.";
                    break;
                case ErrorCode.F_PLC_GET_SIGNAL_ERROR:
                    //msg = "실패: PLC의 신호를 가져오는 중 예기치 않은 에러가 발생했습니다.";
                    msg = "Failed: An unexpected error occurred while getting the signal from the PLC.";
                    break;
                case ErrorCode.F_PLC_SET_SIGNAL_TIMEOUT:
                    //msg = "실패: PLC 데이터 기록 시간이 초과되었습니다.";
                    msg = "Failed: The PLC data recording timed out.";
                    break;
                case ErrorCode.F_PLC_SET_SIGNAL_ERROR:
                    //msg = "실패: PLC에 신호를 설정하는 중 예기치 않은 에러가 발생했습니다.";
                    msg = "Failed: An unexpected error occurred while setting the signal to the PLC.";
                    break;
                case ErrorCode.F_PLC_NOT_MATCH_HEADER:
                    //msg = "실패: PLC로부터 수신된 패킷의 헤더가 비정상입니다.";
                    msg = "Failed: The header of the packet received from the PLC is abnormal.";
                    break;
                case ErrorCode.F_PLC_NOT_MATCH_PACKET_LENGTH:
                    //msg = "실패: PLC로부터 수신된 패킷의 길이가 비정상입니다.";
                    msg = "Failed: The length of the packet received from the PLC is abnormal.";
                    break;
                case ErrorCode.F_NGBUFFER_NOT_READY:
                    //msg = "실패: NG버퍼에 제품이 진입되지 않았습니다.";
                    msg = "Failed: No products have entered the NG buffer.";
                    break;
                case ErrorCode.F_PLC_DATA_SIZE_TOO_LONG:
                    msg = "Failed: PLC data size is too long";
                    break;

                case ErrorCode.F_CAMERA_NOT_CONNECTED:
                    //msg = "실패: 카메라 연결이 안되었습니다.";
                    msg = "Failed: Camera not connected";
                    break;

                case ErrorCode.F_CAMERA_NOT_INIT:
                    //msg = "실패: 카메라 사용을 위한 준비가 되지 않았습니다.";
                    msg = "Failed: The camera is not ready for use.";
                    break;

                case ErrorCode.F_CAMERA_BUSY:
                    //msg = "실패: 카메라를 다른 장치에서 사용중입니다.";
                    msg = "Failed: Camera is in use by another device.";
                    break;

                case ErrorCode.F_CAMERA_CANT_FOUND:
                    //msg = "실패: 연결된 카메라를 찾을 수 없습니다. PC 연결선 또는 카메라 전원선을 확인해주세요.";
                    msg = "Failed: No connected camera found Check the PC connection line or camera power line.";
                    break;

                case ErrorCode.F_CAMERA_FAILED_CAPTURE:
                    //msg = "실패: 영상 촬영에 실패하였습니다.";
                    msg = "Failed: Image recording failed.";
                    break;

                case ErrorCode.F_LIGHT_NOT_CONNECTED:
                    //msg = "실패: 조명 장치 연결이 안되었습니다.";
                    msg = "Failed: The lighting device is not connected.";
                    break;

                case ErrorCode.F_LIGHT_NOT_INIT:
                    //msg = "실패: 조명 장치 사용을 위한 준비가 되지 않았습니다.";
                    msg = "Failed: The lighting device is not ready for use.";
                    break;

                case ErrorCode.F_LIGHT_BUSY:
                    //msg = "실패: 조명 장치를 다른 장치에서 사용중입니다.";
                    msg = "Failed: The lighting device is in use by another device.";
                    break;

                case ErrorCode.F_LIGHT_CANT_FOUND:
                    //msg = "실패: 연결된 조명장치를 찾을 수 없습니다. PC 연결선 또는 조명장치 전원선을 확인해주세요.";
                    msg = "Failed: No connected fixtures found. Check the PC connection line or lighting device power line.";
                    break;

                case ErrorCode.F_LIGHT_FAILED_SETUP:
                    //msg = "실패: 조명 장치 제어에 실패하였습니다.";
                    msg = "Failed: Failed to control the lighting device.";
                    break;

                case ErrorCode.F_UNIT_IS_NULL:
                    msg = "Failed: The unit data is null";
                    break;

                case ErrorCode.F_ALGORITHM_UNKNOWN:
                    //msg = "실패: 알 수 없는 이유로 알고리듬 처리 중 에러가 발생하였습니다.";
                    msg = "Failed: An error occurred during algorithm processing for an unknown reason.";
                    break;
                case ErrorCode.F_ALGORITHM_INVALID_VALUE:
                    //msg = "실패: 잘못된 입력(invalid) 값으로 인해 알고리듬 에러가 발생하였습니다.";
                    msg = "Failed: An algorithm error occurred due to an invalid value.";
                    break;
                case ErrorCode.F_ALGORITHM_BAD_ARGUMENT:
                    //msg = "실패: 잘못된 입력(bad argument) 값으로 인해 알고리듬 에러가 발생하였습니다.";
                    msg = "Failed: An algorithm error occurred due to a bad argument value.";
                    break;
                case ErrorCode.F_ALGORITHM_TIMEOUT:
                    //msg = "실패: 알고리듬 수행시간을 초과하였습니다.";
                    msg = "Failed: Algorithm execution timed out.";
                    break;
                case ErrorCode.F_ALGORITHM_OUTOFMEMORY:
                    //msg = "실패: 메모리가 부족하여 알고리듬 수행에 실패하였습니다.";
                    msg = "Failed: Algorithm failed due to insufficient memory.";
                    break;
                case ErrorCode.F_ALGORITHM_EMPTY_DATA:
                    //msg = "실패: 알고리듬 수행에 필요한 데이터가 존재하지 않습니다.";
                    msg = "Failed: The data needed to run the algorithm does not exist.";
                    break;
                case ErrorCode.F_ALGORITHM_NOT_INIT:
                    //msg = "실패: 알고리듬 초기화가 되지 않았습니다.";
                    msg = "Failed: Algorithm not initialized.";
                    break;


                case ErrorCode.F_DB_UNKNOWN:
                    //msg = "실패: 데이터베이스 처리 중 알 수 없는 에러가 발생하였습니다.";
                    msg = "Failed: An unknown error occurred during database processing.";
                    break;
                case ErrorCode.F_DB_INVALID_VALUE:
                    //msg = "실패: 잘못된 입력 값으로 인해 데이터베이스 처리에 실패하였습니다.";
                    msg = "Failed: Database processing failed due to invalid input value.";
                    break;
                case ErrorCode.F_DB_NODATA:
                    //msg = "실패: 데이터베이스에 데이터가 존재 하지 않습니다.";
                    msg = "Failed: Data does not exist in the database.";
                    break;
                case ErrorCode.F_DB_TIMEOUT:
                    msg = "Failed: 데이터베이스 응답 대기시간 초과(timeout)";
                    break;
                case ErrorCode.F_DB_CANT_FIND_DATA:
                    msg = "Failed: 데이터베이스 응답 실패(Can't find data)";
                    break;
                case ErrorCode.F_DB_ADD_DATA:
                    msg = "Failed: 데이터베이스 데이터 저장 실패";
                    break;
                case ErrorCode.F_DB_ADD_DATA_DUPLICATE:
                    msg = "Failed: 데이터베이스 데이터 저장 실패(중복 데이터)";
                    break;

                default:
                    break;
            }

            return msg;
        }
    }
}
