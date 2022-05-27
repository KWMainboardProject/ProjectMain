using System;
using System.Collections.Generic;
using System.Text;
using RequestTaskProcessing.StrategyOperator.U2Net;
using OpenCvSharp;

namespace RequestTaskProcessing.StrategyOperator
{
    class RemoveBackgroundOperatorU2Net : IStrategyOperateAble
    {
        protected RembgU2Net rmbg = null;
        protected Mat img = null;
        protected TaskMessage requestMessage = null;
        protected StringContainer container;
        protected string workingPath = null;

        protected RemoveBackgroundOperatorU2Net()
        {
            string nlpName = @"\u2net.onnx";
            Console.WriteLine("Loading : " + ShareWorkPath.GetInstance().WEIGHT_PATH + nlpName);
            rmbg = new RembgU2Net(ShareWorkPath.GetInstance().WEIGHT_PATH + nlpName);
            Console.WriteLine("Complete load " + nlpName);
            ClearResource();

            workingPath = ShareWorkPath.GetInstance().WORKER_PATH + @"\REMOVE_BG\";
            ShareWorkPath.CreateDirectory(workingPath);
        }
        public void SetResource(TaskMessage message)
        {
            Console.WriteLine("\tplz Set resource");
            requestMessage = new TaskMessage(message);
            try
            {
                workingPath = workingPath + @"\" + System.IO.Path.GetFileNameWithoutExtension(message.ip.Value) + ".jpg";
            }
            catch
            {
                workingPath = workingPath + @"\" + message.ip.Value + ".jpg";
            }
        }

        public void Work()
        {
            lock (Holder.instance)
            {
                Console.WriteLine("Open Image in rembg : " + requestMessage.resource.GetValue().ToString());
                using (var img = Cv2.ImRead(requestMessage.resource.GetValue().ToString()))
                {
                    Console.WriteLine("Complete Open Image in rembg : " + requestMessage.resource.GetValue().ToString());
                    Console.WriteLine("Start rembg work");
                    //Get mask Mat(1, imgSize)
                    //Mat result = rmbg.

                    //save mask img

                    //get save mask path
                    container.Value = workingPath;
                }
                Console.WriteLine("End rembg work");
                return;
            }
        }

        public TaskMessage GetMessage()
        {
            lock (Holder.instance)
            {
                TaskMessage taskMessage = new TaskMessage(requestMessage);
                taskMessage.type = MessageType.Receive_Container_DetectedObjects;        //set
                taskMessage.productor = null;                                   //set
                taskMessage.resource = container;
                return taskMessage;
            }
        }

        public void ClearResource()
        {
            rmbg.InitResource();
            img = null;
            container = new StringContainer();
        }

        /// <summary>
        /// singleton pattern
        /// </summary>
        /// <returns></returns>
        public static RemoveBackgroundOperatorU2Net GetInstance()
        {
            return Holder.instance;
        }
        /// <summary>
        /// Lazy Initialization + holder
        /// </summary>
        private static class Holder
        {
            public static RemoveBackgroundOperatorU2Net instance = new RemoveBackgroundOperatorU2Net();
        }
    }
}
