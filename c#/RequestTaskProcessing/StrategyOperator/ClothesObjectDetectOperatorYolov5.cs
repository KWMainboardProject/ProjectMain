using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace RequestTaskProcessing.StrategyOperator
{
    class ClothesObjectDetectOperatorYolov5 : IStrategyOperateAble
    {
        protected ClothesObjectDetectOperatorYolov5()
        {
            //plz set gpu device
            Console.WriteLine("\tplz yolov5 weight upload");
            ClearResource();
        }

        public void ClearResource()
        {
            container = new DetectedObjectsContainer();
            Console.WriteLine("\tplz clear resource img");
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
        protected DetectedObjectsContainer container;
        public void Work()
        {
            lock (Holder.instance)
            {
                //Console.WriteLine(requestMessage.ip.Value + "_yolo를 실행 중이예요.\n remove bg는 자요");
                SetContainer();
                Thread.Sleep(1000);
                //Console.WriteLine(requestMessage.ip.Value + "_yolo는 작업을 완료했어요");

                //Set container
                return;
                throw new NotImplementedException();
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
                    name = "Top";
                    break;
                case 1:
                    name = "Bottom";
                    break;
                case 2:
                    name = "Overall";
                    break;
                case 3:
                    name = "Outer";
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
