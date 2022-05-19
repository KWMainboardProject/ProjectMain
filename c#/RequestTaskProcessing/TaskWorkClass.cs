using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;
using RequestTaskProcessing.StrategyOperator;


namespace RequestTaskProcessing
{
    
    public abstract class QTheading : IMessageConsumeAble
    {
        // 참고했음 Queue&Thread 구조 -> https://programerstory.tistory.com/8

        /// <summary>
        /// Time out 설정하는 함수
        /// 기본적으로 0이 설정되어 있고 0은 Time out 이 없다는 뜻이다.
        /// 해당 ms가 지나면 time out 된다.
        /// </summary>
        /// <param name="time">범위[0,큰값)</param>
        abstract public void SetTimeOutThreshold(int time = TimeOut.DEFAULT_TIME);


        /// <summary>
        /// message Q에 쌓여있는 것중에 가장 앞에 있는 message를 꺼내서 반환함
        /// message Q를 반환에 성공하면 qTF는 true로 세팅된다.
        /// </summary>
        /// <returns>반환이 성공하면 message를 실패하는 null을 반환한다.</returns>
        public TaskMessage Consume()
        {
            if (q == null) throw new NullReferenceException();

            int elpse = timeout.EndTime();
            bool timeoutTF = timeout.CheckTimeOut(elpse);
            
            //Consume Message
            TaskMessage message = null;
            if (!q.IsEmpty)
            {
                message = new TaskMessage();
                qTF = q.TryDequeue(out message);
            }     

            //time out check and Run
            if (message != null && qTF) timeout.ResetTimeOut();//성공 시 timeout reset
            else if(timeoutTF) StopAndClear();//message가 계속 없을 때, time out으로 멈춤

            //time start
            timeout.StartTime();
            return message;
        }

        public IMessageProductAble GetProductor()
        {
            if (q == null) throw new NullReferenceException();
            return productor;
        }

        public bool IsEmpty()
        {
            if (q == null) throw new NullReferenceException();
            return q.IsEmpty;
        }

        /// <summary>
        /// 연산에 필요한 operator Factory를 설정하는 함수이다.
        /// </summary>
        /// <param name="factory"></param>
        public void SetOperatorFactory(IOperatorFactory factory)
        {
            this.factory = factory;
        }

        /// <summary>
        /// 해당 스레드를 멈추고 싶을 때 작동한다.
        /// </summary>
        public void StopAndClear()
        {
            stopAndClearTF = true;
            //plz add code
            Console.WriteLine("plz Add code at StopAndClear");
        }
        /// <summary>
        /// 실제로 manager에서 동작하게 될 동작
        /// </summary>
        abstract protected void Run();
        abstract public void Start();
        abstract public void Join();

        protected TimeOut timeout = new TimeOut();

        protected bool stopAndClearTF = false;
        protected bool qTF = false;

        protected IOperatorFactory factory = null;
        protected ConcurrentQueue<TaskMessage> q=null;// = new ConcurrentQueue<TaskMessage>();
        protected SimpleMessageProductor productor = new SimpleMessageProductor();
    }

    public class Worker : QTheading
    {
        const int SLEEP_TIME = 100;
        public Worker(IOperatorFactory factory=null)
        {
            SetOperatorFactory(factory);
        }

        protected override void Run()
        {
            if (q == null)
                throw new NullReferenceException();
            if (factory == null)
                throw new NullReferenceException();
            while (thread.IsAlive)
            {
                Thread.Sleep(SLEEP_TIME);
                //stop thread and claear Q
                if (stopAndClearTF)
                {
                    lock (q)
                    {
                        do
                        {
                            Consume();
                            //clear Q
                        } while (!qTF);

                        stopAndClearTF = false;

                        //need thread stop
                        Console.WriteLine("plz thread stop at QThread.Run");
                        break;
                    }
                }

                //Get message
                TaskMessage m = Consume();
                if (!qTF || m == null)//fali consume
                {
                    continue;
                }
                //Success consume

                
                
                //Get Operator
                IStrategyOperateAble strategy = factory.GetOperator(m.type);

                if(strategy == null)
                    throw new NullReferenceException();

                //Run operator
                strategy.SetResource(m);
                strategy.Work();
                IMessageProductAble sender = m.productor;
                sender.Product(strategy.GetMessage());
                strategy.ClearResource();
            }
        }
        public override void Start()
        {
            if(thread == null)
            {
                thread = new Thread(() => Run());
            }
            thread.Start();
        }
        public override void Join()
        {
            if (thread == null)
                throw new NullReferenceException();
            thread.Join();
        }

        public override void SetTimeOutThreshold(int time = 5000)
        {
            timeout.SetThesholdTime(time);
        }

        protected Thread thread = null;
    }
    /// <summary>
    /// Singleton pattern
    /// </summary>
    public abstract class WorkManager : QTheading
    {
        public const int SLEEP_TIME = 100;

        protected WorkManager() 
        {
            q = new ConcurrentQueue<TaskMessage>();
            productor.SetQueue(q);
        }

        /// <summary>
        /// 실제로 manager에서 동작하게 될 동작
        /// </summary>
        abstract override protected void Run(); 
        /// <summary>
        /// Task Q에 쌓인 task들을 어떻게 scheduling 할지 조절해 주는 함수
        /// 
        /// </summary>
        abstract public void SchedulingTaskProcess();
        public override void Start()
        {
            Run();

            if (workers.Count == 0)
                throw new NullReferenceException();

            foreach(var worker in workers)
            {
                worker.Start();
            }
            Thread.Sleep(SLEEP_TIME);
            scheduleThread = new Thread(() => SchedulingTaskProcess());
            scheduleThread.Start();
        }
        public override void Join()
        {
            foreach (var worker in workers)
            {
                worker.Join();
            }
            scheduleThread.Join();
            
        }
        public override void SetTimeOutThreshold(int time = 5000)
        {
            foreach(Worker worker in workers)
            {
                worker.SetTimeOutThreshold(time);
            }
            
        }
        protected Thread scheduleThread = null;
        protected List<Worker> workers = new List<Worker>(); 
    }
    public class TaskWorker : Worker
    {
        public TaskWorker(ConcurrentQueue<TaskMessage> Q) : base(TaskOperatorFactory.GetInstance())
        {
            //Task Manager의 Q를 consume함
            this.q = Q;
            productor.SetQueue(q);
        }
    }
    public class GPUWorker : Worker
    {
        public GPUWorker() : base(GPUWorkerOperatorFactory.GetInstance())
        {
            //개별적인 Q를 consume함
            this.q = new ConcurrentQueue<TaskMessage>();
            productor.SetQueue(q);
        }
    }
    /// <summary>
    /// singleton pattern
    /// </summary>
    public class TaskManager : WorkManager
    {
        const int THREAD_COUNT = 3;

        /// <summary>
        /// child process에서 counsume 해줌
        /// 그래서 아무것도 안함
        /// </summary>
        public override void SchedulingTaskProcess()
        {
            return;
        }

        protected override void Run()
        {
            for (int i=0; i<THREAD_COUNT; i++)
            {
                workers.Add(new TaskWorker(q));
            }
        }
        /// <summary>
        /// sington pattern
        /// </summary>
        /// <returns></returns>
        public static TaskManager GetInstance()
        {
            return Holder.instance;
        }
        /// <summary>
        /// Lazy Initialization + holder
        /// </summary>
        private static class Holder
        {
            public static TaskManager instance = new TaskManager();
        }
    }

    /// <summary>
    /// singleton pattern
    /// </summary>
    public class GPUWorkManager : WorkManager
    {
        const int THREAD_COUNT = 3;
        public override void SchedulingTaskProcess()
        {
            throw new NotImplementedException();
        }

        protected override void Run()
        {
            Console.WriteLine("GpuWorkManaer - Run");
            throw new NotImplementedException();
        }


        /// <summary>
        /// sington pattern
        /// </summary>
        /// <returns></returns>
        public static GPUWorkManager GetInstance()
        {
            return Holder.instance;
        }
        /// <summary>
        /// Lazy Initialization + holder
        /// </summary>
        private static class Holder
        {
            public static GPUWorkManager instance = new GPUWorkManager();
        }
    }
    public class TimeOut
    {
        //for time out
        public const int DEFAULT_TIME = 5000;
        int thresholdTime = 0;
        int waitTime = 0;
        public void SetThesholdTime(int time = DEFAULT_TIME)
        {
            thresholdTime = time;
        }
        public bool CheckTimeOut(int time)
        {
            if (thresholdTime == 0) return false;
            waitTime += time;
            if (waitTime >= thresholdTime) return true;
            else return false;
        }
        public void ResetTimeOut()
        {
            waitTime = 0;
        }

        //count time
        DateTime startTime;
        public void StartTime()
        {
            startTime = DateTime.Now;
        }
        public int EndTime()
        {
            if (thresholdTime == 0) return 0;
            DateTime currnetTime = DateTime.Now;
            TimeSpan timeSpan = currnetTime - startTime;

            return (int)(timeSpan.Milliseconds);
        }
    }
}