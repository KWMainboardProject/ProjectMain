using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;

namespace RequestTaskProcessing
{

    /// <summary>
    /// Singleton pattern
    /// </summary>
    public abstract class WorkManager : IMessageConsumeAble
    {
        public const int SLEEP_TIME = 100;

        protected WorkManager()
        {
            productor.SetQueue(q);
        }

        public TaskMessage Consume()
        {
            TaskMessage message = new TaskMessage();
            if (q.IsEmpty) return message;

            q.TryDequeue(out message);
            return message;
        }

        public IMessageProductAble GetProductor()
        {
            return productor;
        }

        public bool IsEmpty()
        {
            return q.IsEmpty;
        }
        /// <summary>
        /// 실제로 manager에서 동작하게 될 동작
        /// </summary>
        abstract protected void Run(); 
        /// <summary>
        /// Task Q에 쌓인 task들을 어떻게 scheduling 할지 조절해 주는 함수
        /// 
        /// </summary>
        abstract public void SchedulingTaskProcess();
        public void Start()
        {
            foreach(Thread thread in threads)
            {
                thread.Start();
            }
            Thread.Sleep(SLEEP_TIME);
            Thread scheduleThread = new Thread(() => SchedulingTaskProcess());
            scheduleThread.Start();
        }
        public void Join()
        {
            foreach (Thread thread in threads)
            {
                thread.Join();
            }
        }
      
        protected List<Thread> threads = new List<Thread>(); 
        protected ConcurrentQueue<TaskMessage> q = new ConcurrentQueue<TaskMessage>();
        protected SimpleMessageProductor productor = new SimpleMessageProductor();
    }

    /// <summary>
    /// singleton pattern
    /// </summary>
    public class TaskManager : WorkManager
    {
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
            throw new NotImplementedException();
        }


        protected IMessageProductAble gpuRequestor = null;


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
}
