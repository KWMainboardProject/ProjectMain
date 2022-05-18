using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;

namespace RequestTaskProcessing
{

    abstract class WorkManager : IMessageConsumeAble
    {


        public WorkManager()
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

        abstract protected void Run();

        protected ConcurrentQueue<TaskMessage> q = new ConcurrentQueue<TaskMessage>();
        protected SimpleMessageProductor productor = new SimpleMessageProductor();
    }
}
