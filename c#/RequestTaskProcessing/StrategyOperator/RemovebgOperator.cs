using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace RequestTaskProcessing.StrategyOperator
{
    /// <summary>
    /// singleton pattern
    /// </summary>
    public class RemovebgOperator : IStrategyOperateAble
    {
        protected RemovebgOperator()
        {
            Console.WriteLine("\tplz removebg weight upload");
            //plz set gpu device
        }
        public void ClearResource()
        {
            Console.WriteLine("\tplz clear resource img");
        }

        public TaskMessage GetMessage()
        {
            lock (Holder.instance)
            {
                TaskMessage taskMessage = new TaskMessage(requestMessage);
                taskMessage.type = MessageType.Receive_ImagePath_RemoveBG;        //set
                taskMessage.productor = null;                                   //set
                taskMessage.resource =
                    new StringContainer("img_path",
                    requestMessage.resource.GetValue().ToString() + "/gpu_worker/removed_img.jpg");         //set
                return taskMessage;
            }
        }

        public void SetResource(TaskMessage message)
        {
            Console.WriteLine("\tplz Set resource");
            requestMessage = new TaskMessage(message);
        }
        protected TaskMessage requestMessage = null;

        public void Work()
        {
            lock (Holder.instance)
            {
                Console.WriteLine(requestMessage.ip.Value + "_removebg를 실행 중이예요.\n remove bg는 자요");
                Thread.Sleep(1000);
                Console.WriteLine(requestMessage.ip.Value + "_removebg는 작업을 완료했어요");
                return;
                throw new NotImplementedException();
            }
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
