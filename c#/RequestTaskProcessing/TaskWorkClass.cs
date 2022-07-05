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
        /// 
        /// Time out 이 설정 되었을 경우 time out 될 경우
        /// TimeoutException을 반환한다.
        /// </summary>
        /// <returns>반환이 성공하면 message를 실패하는 null을 반환한다.</returns>
        public TaskMessage Consume()
        {
            if (q == null) throw new NullReferenceException();

            int elpse = timeout.EndTime();//측정시간
            //타임아웃여부
            bool timeoutTF = timeout.CheckTimeOut(elpse);
            
            //Consume Message
            TaskMessage message = null;
            if (!q.IsEmpty){
                lock (q){
                    if (!q.IsEmpty){
                        message = new TaskMessage();
                        qTF = q.TryDequeue(out message);
                    }
                }
            }

            //time out check and Run
            if (message != null && qTF){
                //성공 시 timeout reset
                timeout.ResetTimeOut();}
            else if (timeoutTF)
                //message가 계속 없을 때, time out으로 멈춤
                throw new TimeoutException();

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
            Console.WriteLine("\tplz Add code at StopAndClear");
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
                            try
                            {
                                Consume();
                            }
                            catch { Console.WriteLine(this.ToString()+" - end"); return; }
                            //clear Q
                        } while (!qTF) ;

                        stopAndClearTF = false;

                        //need thread stop
                        Console.WriteLine("\tplz thread stop at QThread.Run");
                        break;
                    }
                }

                //Get message
                TaskMessage m = null;
                try{m = Consume();}
                catch(TimeoutException e)
                {
                    Console.WriteLine("Stop and Clear (Worker)");
                    StopAndClear();
                }

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
                lock (strategy)
                {
                    IMessageProductAble sender = m.productor;


                    //Console.WriteLine(this.ToString() + "-> SetResource");
                    strategy.SetResource(m);
                    //Console.WriteLine(this.ToString() + "-> Work");
                    strategy.Work();
                                                                           // Console.WriteLine(this.ToString() + "-> GetMessage");
                                                                           // strategy.GetMessage().Print();
                    sender.Product(strategy.GetMessage());
                    //Console.WriteLine(this.ToString() + "-> ClearResource");
                    strategy.ClearResource();
                }
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

        public override void SetTimeOutThreshold(int time = TimeOut.DEFAULT_TIME)
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

        protected bool isStart = false;
        public override void Start()
        {
            if (!isStart)
            {
                lock (this)
                {
                    if (!isStart)
                    {
                        Run();

                        if (workers.Count == 0)
                            throw new NullReferenceException();

                        foreach (var worker in workers)
                        {
                            worker.Start();
                        }
                        Thread.Sleep(SLEEP_TIME);
                        scheduleThread = new Thread(() => SchedulingTaskProcess());
                        scheduleThread.Start();
                        isStart = true;
                    }
                }
            }
        }
        public override void Join()
        {
            foreach (var worker in workers)
            {
                worker.Join();
            }
            scheduleThread.Join();
        }
        public override void SetTimeOutThreshold(int time = TimeOut.DEFAULT_TIME)
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
        /// Lazy Initialization + holder\t
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

        int messageCount = 0;
        public override void SchedulingTaskProcess()
        {
            if (q == null)
                throw new NullReferenceException();
            while (scheduleThread.IsAlive)
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
                        Console.WriteLine("\tplz thread stop at QThread.Run");
                        break;
                    }
                }

                //Get message
                TaskMessage m = null;
                try { m = Consume(); /*Console.WriteLine("Try Counsume in gpu worker");*/ }
                catch (TimeoutException e) { Console.WriteLine("Stop and Clear");  StopAndClear(); }

                if (!qTF || m == null)//fali consume
                {
                    continue;
                }
                //Console.WriteLine("GPU Worker Catch Message");
                //m.Print();

                //Success consume
                messageCount++;
                IMessageProductAble p = workers[messageCount % THREAD_COUNT].GetProductor();
                //plz change scheduling methods
                p.Product(m);
            }
        }

        protected override void Run()
        {
            Console.WriteLine("GpuWorkManaer - Run");
            for (int i = 0; i < THREAD_COUNT; i++)
            {
                workers.Add(new GPUWorker());
            }
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
        /// <summary>
        /// Time out을 판별할 수 있는 Threashold 값 설정 함수
        /// </summary>
        /// <param name="time">
        /// 음수 : 없음 / 
        /// 0 : time out 설정 안함 / 
        /// 1 이상 : 해당 시간 이 지나며 time out을 띄움
        /// </param>
        public void SetThesholdTime(int time = DEFAULT_TIME)
        {
            thresholdTime = time;
        }
        /// <summary>
        /// time 만큼 시간이 흐를때 time out 인지를 판별해주는 함수
        /// </summary>
        /// <param name="time">
        /// time 만큼 시간이 흘렀을 때
        /// </param>
        /// <returns>
        /// time out 인지(TF)를 반환한다
        /// ThresholdTime 이 0으로 설정되면 항상 F반환
        /// </returns>
        public bool CheckTimeOut(int time)
        {
            if (thresholdTime == 0) return false;
            waitTime += time;
            if (waitTime >= thresholdTime) return true;
            else return false;
        }
        /// <summary>
        /// Time out을 위해 측정하던 시간 초기화
        /// </summary>
        public void ResetTimeOut()
        {
            waitTime = 0;
        }

        //count time
        DateTime startTime;
        /// <summary>
        /// 측정하기 원하는 시작시간
        /// </summary>
        public void StartTime()
        {
            startTime = DateTime.Now;
        }
        /// <summary>
        /// 측정이 완료되는 시점
        /// </summary>
        /// <returns>
        /// StartTime 이후부터 흐른시간 반환
        /// 단위(=ms)
        /// </returns>
        public int EndTime()
        {
            if (thresholdTime == 0) return 0;
            DateTime currnetTime = DateTime.Now;
            TimeSpan timeSpan = currnetTime - startTime;

            return (int)(timeSpan.Milliseconds);
        }
    }
}