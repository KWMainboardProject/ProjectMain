using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;

namespace Mainboard
{
    interface IMessageProductAble
    {
        void Product(TaskMessage massage);
    }
    interface IMessageConsumeAble
    {
        IMessageProductAble GetProductor();
        TaskMessage Consume();
    }
    class TaskMessage
    {
        public IpContainer ip = new IpContainer();
        public IMessageProductAble productor = null;
        public MessageType type = MessageType.EmptyMessage;
        public IJObjectUseAbleContainer resource = null;
    }
    enum MessageType
    {
        EmptyMessage=0,


        ResponseFail,
        MessageTypeNum,
    }
    class SimpleMessageProductor : IMessageProductAble
    {
        public void Product(TaskMessage massage)
        {
            if (q == null) throw new NullReferenceException();
            q.Enqueue(massage);
        }
        public void SetQueue(ConcurrentQueue<TaskMessage> Q)
        {
            if (Q == null) throw new ArgumentNullException();
            q = Q;
        }
        protected ConcurrentQueue<TaskMessage> q=null;
    }
}
