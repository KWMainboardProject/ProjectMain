using System;
using System.Collections.Generic;
using System.Text;
using System.Threading; ;

namespace RequestTaskProcessing.StrategyOperator
{
    /// <summary>
    /// singleton pattern
    /// </summary>
    public class RemovebgOperator : IStrategyOperateAble
    {
        protected RemovebgOperator()
        {
            Console.WriteLine("plz weight upload");
        }
        public void ClearResource()
        {
            Console.WriteLine("plz clear resource img");
        }

        public TaskMessage GetMessage()
        {
            TaskMessage taskMessage = new TaskMessage(requestMessage);
            taskMessage.type = MessageType.Receive_Container_Fashion;        //set
            taskMessage.productor = null;                                   //set
            taskMessage.resource = 
                new StringContainer("img_path", "./gpu_worker/removed_img.jpg");         //set
            return taskMessage;
        }

        public void SetResource(TaskMessage message)
        {
            Console.WriteLine("plz Set resource");
            requestMessage = new TaskMessage(message);
        }
        protected TaskMessage requestMessage = null;

        public void Work()
        {
            Console.WriteLine("remove bg는 자요");
            Thread.Sleep(1000);
            return;
            throw new NotImplementedException();
        }

        /// <summary>
        /// singleton pattern
        /// </summary>
        /// <returns></returns>
        public static RemovebgOperator GetInstance()
        {
            return Holder.instance;
        }
        /// <summary>
        /// Lazy Initialization + holder
        /// </summary>
        private static class Holder
        {
            public static RemovebgOperator instance = new RemovebgOperator();
        }
    }
}
