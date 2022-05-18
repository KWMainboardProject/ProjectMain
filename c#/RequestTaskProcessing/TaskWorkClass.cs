using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;

namespace RequestTaskProcessing
{
    public abstract class QTheading : IMessageConsumeAble
    {
        // 참고했음 Queue&Thread 구조 -> https://programerstory.tistory.com/8

        public TaskMessage Consume()
        {
            if (q == null) throw new NullReferenceException();
            if (q.IsEmpty) return null;
            TaskMessage message = new TaskMessage();
            qTF = q.TryDequeue(out message);
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
                //Get message
                TaskMessage m = Consume();
                if (!qTF) continue;//fali consume
                if (m == null) continue;
                //Success consume

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
        const int THREAD_COUNT = 1;

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