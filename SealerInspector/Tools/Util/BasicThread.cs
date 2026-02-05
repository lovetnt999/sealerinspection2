using System;
using System.Drawing;
using System.Threading;
using System.Collections;
using System.Collections.Generic;


namespace Wamc.Tools.Util
{
    /// <summary>
    /// 스레드 클래스
    /// </summary>
    public class BasicThread : IDisposable
    {
        public delegate void WorkMethod(Object data);

        /// <summary>
        /// 스레드 타입
        /// </summary>
        public enum ThreadType
        {
            Polling, //폴링 (설정한 주기로 계속 실행)
            Event    //이벤트 발생시만 실행
        }

        /// <summary>
        /// 스레드 객체
        /// </summary>
        public Thread PollingThread { get; set; }

        /// <summary>
        /// 스레드 상태
        /// </summary>
        public ThreadState ThreadState { get; set; }

        /// <summary>
        /// 처리 메소드(함수) 리스트
        /// </summary>
        public List<WorkMethod> WorkMethodList { get; set; }

        /// <summary>
        /// 처리 메소드(함수) 실행 시간 간격
        /// </summary>
        public int Interval { get; set; }

        /// <summary>
        /// 이벤트 큐
        /// </summary>
        public Queue<Object> EventQueue { get; set; }

        /// <summary>
        /// 최대 처리 이벤트 갯수
        /// </summary>
        public int MaxEventCount { get; set; }

        /// <summary>
        /// 스레드 타입
        /// </summary>
        public ThreadType Type { get; set; }

        /// <summary>
        /// 스레드 리스트 (여러개의 스레드 사용할 경우)
        /// </summary>
        public List<Thread> ThreadList { get; set; }

        /// <summary>
        /// 멀티 코어 번호
        /// </summary>
        public int MultiCoreNo { get; set; }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="threadName">스레드 이름</param>
        /// <param name="type">스레드 타입</param>
        /// <param name="interval">스레드 메쏘드 실행 간격</param>
        /// <param name="maxEventCount">이벤트 처리 최대 갯수</param>
        public BasicThread(string threadName, ThreadType type, int interval = 1, int maxEventCount = 1000)
        {
            try
            {
                ThreadState = ThreadState.Suspended;

                Interval = interval;

                EventQueue = new Queue<Object>();
                WorkMethodList = new List<WorkMethod>();
                Type = type;
                MaxEventCount = maxEventCount;

                PollingThread = new Thread(Work);
                PollingThread.Name = threadName;

                ThreadList = new List<Thread>();
                ThreadList.Add(PollingThread);

                //ThreadUtil.ThreadStart(PollingThread, null, MultiCoreNo);
                PollingThread.Start();

                ThreadState = ThreadState.Running;
            }
            catch (Exception ex)
            {
                AjinUtil.Logger.Error(ex, "Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        /// <summary>
        /// 스레드 메소드
        /// </summary>
        public virtual void Work()
        {
            try
            {
                while (true)
                {
                    try
                    {
                        if (ThreadState == ThreadState.Stopped)
                            break;

                        if (ThreadState == ThreadState.Suspended)
                        {
                            Thread.Sleep(Interval);
                            continue;
                        }

                        switch (Type)
                        {
                            case ThreadType.Polling: //Case of polling mode
                                if (WorkMethodList != null && WorkMethodList.Count > 0)
                                {
                                    foreach (var workMethod in WorkMethodList)
                                        workMethod(null);
                                }
                                break;

                            case ThreadType.Event: //Case of work processor mode
                                if (EventQueue.Count <= 0)
                                {
                                    Thread.Sleep(Interval);
                                    continue;
                                }

                                Object deqTemp = EventQueue.Dequeue();
                                //workMethod(deqTemp);

                                /*
                                foreach (var workMethod in WorkMethodList)
                                {
                                    if (deqTemp is WorkData)
                                    {
                                        WorkData workData = deqTemp as WorkData;
                                        workMethod(workData.Clone() as WorkData);
                                    }
                                    else
                                    {
                                        workMethod(deqTemp);
                                    }
                                }
                                */
                                break;

                            default:
                                break;
                        }

                        Thread.Sleep(Interval);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 이벤트 넣기
        /// </summary>  
        /// <param name="data">이벤트 파라매터</param>
        public void PushEvent(Object data)
        {
            try
            {
                if (data == null)
                    return;

                lock (EventQueue)
                {
                    if (EventQueue.Count < MaxEventCount)
                    {
                        EventQueue.Enqueue(data);
                    }
                }
            }
            catch (Exception ex)
            {
                AjinUtil.Logger.Error(ex, "Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        /// <summary>
        /// 처리 메소드 추가
        /// </summary>
        /// <param name="method"></param>
        public void AddWorkMethod(WorkMethod method)
        {
            try
            {
                if (method == null)
                    return;

                WorkMethodList.Add(method);
            }
            catch (Exception ex)
            {
                AjinUtil.Logger.Error(ex, "Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        /// <summary>
        /// 처리 메소드 제거
        /// </summary>
        /// <param name="method"></param>
        public void RemoveWorkMethod(WorkMethod method)
        {
            try
            {
                if (method == null)
                    return;

                WorkMethodList.Remove(method);
            }
            catch (Exception ex)
            {
                AjinUtil.Logger.Error(ex, "Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        /// <summary>
        /// 처리 메소드 전체 제거
        /// </summary>
        public void RemoveAllWorkMethod()
        {
            try
            {
                WorkMethodList.Clear();
            }
            catch (Exception ex)
            {
                AjinUtil.Logger.Error(ex, "Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        #region Implement for method of IDisposable interface
        /// <summary>
        /// 소멸자
        /// </summary>
        public virtual void Dispose()
        {
            try
            {
                if (WorkMethodList.Count > 0)
                {
                    RemoveAllWorkMethod();

                    ThreadState = ThreadState.Stopped;

                    foreach (Thread thread in ThreadList)
                    {
                        thread.Join(2000);
                        //thread.Interrupt();
                    }
                }
            }
            catch (Exception)
            {
                try
                {
                    foreach (Thread thread in ThreadList)
                    {
                        thread.Abort();
                    }
                }
                catch (Exception) { }
            }
        }
        #endregion
    }
}