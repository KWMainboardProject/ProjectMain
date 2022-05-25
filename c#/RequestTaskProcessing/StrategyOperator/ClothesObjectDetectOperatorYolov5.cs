using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using RequestTaskProcessing.StrategyOperator.Yolov5;
using OpenCvSharp;
using OpenCvSharp.Dnn;

namespace RequestTaskProcessing.StrategyOperator
{
    class ClothesObjectDetectOperatorYolov5 : IStrategyOperateAble
    {
        protected YoloDetector yolo = null;
        protected Mat img = null;

        protected ClothesObjectDetectOperatorYolov5()
        {
            //plz set gpu device
            Console.WriteLine("Loading : " + ShareWorkPath.GetInstance().WEIGHT_PATH + @"\FashionDetector.onnx");
            yolo = new YoloDetector(ShareWorkPath.GetInstance().WEIGHT_PATH + @"\FashionDetector.onnx");
            Console.WriteLine("Complete load FashionDetector.onnx");
            ClearResource();
        }

        public void ClearResource()
        {
            yolo.InitResource();
            img = null;
            container = new EmptyDetectedObjectsContainer();
            //Console.WriteLine("\tplz clear resource img");
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

        public void SetResource(TaskMessage message)
        {
            Console.WriteLine("\tplz Set resource");
            requestMessage = new TaskMessage(message);

        }
        
        protected TaskMessage requestMessage = null;
        protected EmptyDetectedObjectsContainer container;
        public void Work()
        {
            lock (Holder.instance)
            {
                Console.WriteLine("Open Image in yolo : " + requestMessage.resource.GetValue().ToString());
                using (var img = Cv2.ImRead(requestMessage.resource.GetValue().ToString()))
                {
                    Console.WriteLine("Complete Open Image in yolo : " + requestMessage.resource.GetValue().ToString());

                    Console.WriteLine("Start yolo work");
                    var result = yolo.objectDetection(img);


                    //Set Container
                    container = new EmptyDetectedObjectsContainer();
                    foreach (var prediction in result)
                    {
                        MainCategoryContainer mc = new MainCategoryContainer();
                        //Set Category
                        mc.SetClassfication(prediction.Label);
                        //Set Boundbox
                        mc.SetBoundbox(
                            (int)prediction.Box.Ymin,
                            (int)prediction.Box.Ymax,
                            (int)prediction.Box.Xmin,
                            (int)prediction.Box.Xmax);
                        //Set confidence
                        ConfidenceContainer cdc = new ConfidenceContainer(0.45f);
                        mc.SetAtribute(cdc);

                        //SetAtribute
                        container.SetAtribute(mc);
                    }
                }
                Console.WriteLine("End yolo work");
                return;
            }
        }

        public void SetContainer()
        {
            SetTestContainer();
        }
        protected void SetTestContainer()
        {
            Random random = new Random();

            int i = 0;
            if(random.Next(2) == 0)
            {
                MainCategoryContainer mainCategory = new MainCategoryContainer(GetFashionNameToIndex(i));
                mainCategory.SetBoundbox(random.Next(10, 200), random.Next(10, 200), random.Next(10, 200), random.Next(10, 200));
                container.SetAtribute(mainCategory);
            }
            i++;
            if (random.Next(2) == 0)
            {
                MainCategoryContainer mainCategory = new MainCategoryContainer(GetFashionNameToIndex(i));
                mainCategory.SetBoundbox(random.Next(10, 200), random.Next(10, 200), random.Next(10, 200), random.Next(10, 200));
                container.SetAtribute(mainCategory);
            }
            i++;
            if (random.Next(2) == 0)
            {
                MainCategoryContainer mainCategory = new MainCategoryContainer(GetFashionNameToIndex(i));
                mainCategory.SetBoundbox(random.Next(10, 200), random.Next(10, 200), random.Next(10, 200), random.Next(10, 200));
                container.SetAtribute(mainCategory);
            }
            i++;
            if (random.Next(2) == 0)
            {
                MainCategoryContainer mainCategory = new MainCategoryContainer(GetFashionNameToIndex(i));
                mainCategory.SetBoundbox(random.Next(10, 200), random.Next(10, 200), random.Next(10, 200), random.Next(10, 200));
                container.SetAtribute(mainCategory);
            }
        }
        protected string GetFashionNameToIndex(int idx)
        {
            string name = "NULL";
            switch (idx)
            {
                case 0:
                    name = "Overall";

                    break;
                case 1:
                    name = "Bottom";
                    break;
                case 2:
                    name = "Top";

                    break;
                case 3:
                    name = "Outer";
                    break;
                case 4:
                    name = "Shoes";
                    break;
            }
            return name;
        }

        /// <summary>
        /// singleton pattern
        /// </summary>
        /// <returns></returns>
        public static ClothesObjectDetectOperatorYolov5 GetInstance()
        {
            return Holder.instance;
        }
        /// <summary>
        /// Lazy Initialization + holder
        /// </summary>
        private static class Holder
        {
            public static ClothesObjectDetectOperatorYolov5 instance = new ClothesObjectDetectOperatorYolov5();
        }
    }
}
