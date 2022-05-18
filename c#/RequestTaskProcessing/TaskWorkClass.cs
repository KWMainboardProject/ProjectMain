using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;


namespace RequestTaskProcessing
{
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
            //Console.WriteLine("ADD time : " + time.ToString() + 's');
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
            TimeSpan timeSpan =currnetTime - startTime;

            return (int)(timeSpan.Milliseconds);
        }


        
    }

    public abstract class QTheading : IMessageConsumeAble
    {
        // 참고했음 Queue&Thread 구조 -> https://programerstory.tistory.com/8

        abstract public void SetTimeOutThreshold(int time = TimeOut.DEFAULT_TIME);

        public TaskMessage Consume()
        {
            if (q == null) throw new NullReferenceException();

            int elpse = timeout.EndTime();
            bool timeoutTF = timeout.CheckTimeOut(elpse);
            if (timeoutTF) StopAndClear();//time out 시 멈춤
            timeout.StartTime();

            if (q.IsEmpty) return null;
            TaskMessage message = new TaskMessage();
            qTF = q.TryDequeue(out message);
            if (qTF) timeout.ResetTimeOut();//성공 시 timeout reset
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

        public void SetOperatorFactory(IOperatorFactory factory)
        {
            this.factory = factory;
        }

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
                    Console.WriteLine("Wait Messamge - Worker");
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
            Thread scheduleThread = new Thread(() => SchedulingTaskProcess());
            scheduleThread.Start();
        }
        public override void Join()
        {
            foreach (var worker in workers)
            {
                worker.Join();
            }
            
        }
        public override void SetTimeOutThreshold(int time = 5000)
        {
            foreach(Worker worker in workers)
            {
                worker.SetTimeOutThreshold(time);
            }
            
        }
        protected List<Worker> workers = new List<Worker>(); 
    }
    public class TaskWorker : Worker
    {
        public TaskWorker(ConcurrentQueue<TaskMessage> Q) : base(TaskOperatorFactory.GetInstance())
        {
            this.q = Q;
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
}