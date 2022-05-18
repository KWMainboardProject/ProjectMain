using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;

namespace RequestTaskProcessing
{
    public interface IMessageProductAble
    {
        void Product(TaskMessage massage);
    }
    public interface IMessageConsumeAble
    {
        IMessageProductAble GetProductor();
        TaskMessage Consume();
        bool IsEmpty();
    }
    public class TaskMessage
    {
        public TaskMessage(string ip=null, IMessageProductAble p=null, MessageType t=MessageType.EmptyMessage, IJObjectUseAbleContainer r = null)
        {
            if (ip != null) this.ip.Value = ip;
            if (p != null) this.productor = p;
            if (t != MessageType.EmptyMessage) this.type = t;
            if (r != null) this.resource = r;
        }

        public void Print()
        {
            Console.WriteLine("===========Message===========");
            Console.WriteLine("ip   : " + ip.Value);
            Console.WriteLine("type : " + type.ToString());
            Console.WriteLine("resource");
            Console.WriteLine(resource.GetJObject().ToString());
            Console.WriteLine("=============================");
        }

        public StringContainer ip = new StringContainer();
        public IMessageProductAble productor = null;
        public MessageType type = MessageType.EmptyMessage;
        public IJObjectUseAbleContainer resource = null;
    }
    public enum MessageType
    {
        EmptyMessage=0,
        Request_ImageAnalysis_ImagePath,
        Request_ImageAnalysis_JsonPath,


        Request_TestTask_container,
        Response_TestTask_container,

        Response_Fail,
        MessageTypeNum,
    }
    public class SimpleMessageProductor : IMessageProductAble
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
