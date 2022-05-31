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
        protected RemoveBGContainer container;
        protected string workingPath = null;

        protected RemoveBackgroundOperatorU2Net()
        {
            string nlpName = @"\u2net_human_seg.onnx";
            Console.WriteLine("Loading : " + ShareWorkPath.GetInstance().WEIGHT_PATH + nlpName);
            rmbg = new RembgU2Net(ShareWorkPath.GetInstance().WEIGHT_PATH + nlpName);
            Console.WriteLine("Complete load " + nlpName);
            ClearResource();
            ShareWorkPath.CreateDirectory(workingPath);
        }
        public void SetResource(TaskMessage message)
        {
            Console.WriteLine("\tplz Set resource");
            requestMessage = new TaskMessage(message);
            try
            {
                workingPath = workingPath + @"\" + System.IO.Path.GetFileNameWithoutExtension(message.ip.Value);
            }
            catch
            {
                workingPath = workingPath + @"\" + message.ip.Value;
            }
            ShareWorkPath.CreateDirectory(workingPath);
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

                    var result = rmbg.objectSegmentation(img);
                    //save mask img
                    SaveMaskImg(result[0], img);
                }
                Console.WriteLine("End rembg work");
                return;
            }
        }

        protected void SaveMaskImg(Mat mask, Mat img)
        {
            const int threshold = 10;
            //get save mask path
            container.maskPath.Value = workingPath + @"\mask.jpg";
            container.imgPath.Value = workingPath + @"\img.jpg";

            //Save mask
            Cv2.ImWrite(container.maskPath.Value,mask);

            //merge img + mask
            Mat mergeImg = img.Clone();
            //Cv2.ImShow("before mergeimg", mergeImg); //test???????????
            for(int row = 0; row <mask.Rows; row++)
            {
                for(int col=0; col < mask.Cols; col++)
                {
                    if(mask.At<byte>(row, col) < threshold)
                    {
                        mergeImg.Set<Vec3b>(row, col, new Vec3b(255, 255, 255));
                    }
                }
            }
            //Save merge img
            Cv2.ImWrite(container.imgPath.Value, mergeImg);

            //Cv2.ImShow("after mergeimg", mergeImg);//test???????????
            //Cv2.WaitKey();
        }

        public TaskMessage GetMessage()
        {
            lock (Holder.instance)
            {
                TaskMessage taskMessage = new TaskMessage(requestMessage);
                taskMessage.type = MessageType.Receive_ImagePath_RemoveBG;        //set
                taskMessage.productor = null;                                   //set
                taskMessage.resource = container;
                return taskMessage;
            }
        }

        public void ClearResource()
        {
            rmbg.InitResource();
            img = null;
            container = new RemoveBGContainer();
            workingPath = ShareWorkPath.GetInstance().WORKER_PATH + @"\REMOVE_BG\";
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
