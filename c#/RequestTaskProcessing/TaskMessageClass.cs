using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;

namespace RequestTaskProcessing
{
    public enum MessageType
    {
        //Empty
        EmptyMessage = 0,
        //Request
        Request_ImageAnalysis_ImagePath, //Request_ImageSave_JsonPath,
        Request_Removebg_ImagePath,
        Request_FindMainCategory_ImagePath,
        Request_FindSubCategory_Top_ImagePath,
        Request_FindSubCategory_Bottom_ImagePath,
        Request_FindSubCategory_Outer_ImagePath,
        Request_FindSubCategory_Overall_ImagePath,
        Request_FindPattern_Top_ImagePath,
        Request_FindPattern_Bottom_ImagePath,
        Request_FindPattern_Outer_ImagePath,
        Request_FindPattern_Overall_ImagePath,
        Request_FindStyle_ImagePath,
        //Receive
        Receive_ImagePath_RemoveBG,
        Receive_ImagePath_Original,
        Receive_JsonPath_ImageList,
        Receive_Container_DetectedObjects,//Receive_Container_MainCategory,
        Receive_Container_SubCategory_Top,
        Receive_Container_SubCategory_Bottom,
        Receive_Container_SubCategory_Outer,
        Receive_Container_SubCategory_Overall,
        Receive_Container_Pattern_Top,
        Receive_Container_Pattern_Bottom,
        Receive_Container_Pattern_Outer,
        Receive_Container_Pattern_Overall,
        Receive_Container_Style,
        Receive_Container_Fashion,
        //special
        Response_Fail,
        MessageTypeNum,
        //Test
        Request_TestTask_container,
        Response_TestTask_container,
    }

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
        public TaskMessage(TaskMessage message)
        {
            SetMessage(message);
        }

        public void SetMessage(TaskMessage message)
        {
            this.ip.Value = message.ip.Value;
            this.productor = message.productor;
            this.type = message.type;
            this.resource = message.resource;
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
    
    public class SimpleMessageProductor : IMessageProductAble
    {
        public void Product(TaskMessage massage)
        {
            try
            {
                q.Enqueue(massage);
            }
            catch(NullReferenceException ex)
            {
                Console.WriteLine("Deleted Q");
            }
        }
        public void SetQueue(ConcurrentQueue<TaskMessage> Q)
        {
            if (Q == null) throw new ArgumentNullException();
            q = Q;
        }
        protected ConcurrentQueue<TaskMessage> q=null;
    }
}
