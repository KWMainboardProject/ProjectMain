using System;
using System.Collections.Generic;
using System.Text;
using RequestTaskProcessing;
using RequestTaskProcessing.StrategyOperator.SubCategory;


namespace RequestTaskProcessing.StrategyOperator
{
    /// <summary>
    /// plz singleton pattern
    /// </summary>
    public interface IOperatorFactory
    {
        public IStrategyOperateAble GetOperator(MessageType type);
    }
    public class GPUWorkerOperatorFactory : IOperatorFactory
    {
        protected GPUWorkerOperatorFactory()
        {
            //init path
        }
        public IStrategyOperateAble GetOperator(MessageType type)
        {
            lock (Holder.instance)
            {
                IStrategyOperateAble strategyOperator = null;
                switch (type)
                {
                    case MessageType.Request_Removebg_ImagePath:
                        strategyOperator = RemovebgOperator.GetInstance();
                        break;
                    case MessageType.Request_FindMainCategory_ImagePath:
                        strategyOperator = ClothesObjectDetectOperatorYolov5.GetInstance();
                        break;
                    case MessageType.Request_FindSubCategory_Top_ImagePath:
                        strategyOperator = TopClassification.GetInstance();
                        break;
                    case MessageType.Request_FindSubCategory_Bottom_ImagePath:
                        strategyOperator = BottomClassification.GetInstance();
                        break;
                    case MessageType.Request_FindSubCategory_Outer_ImagePath:
                        strategyOperator = OuterClassification.GetInstance();
                        break;
                    case MessageType.Request_FindSubCategory_Overall_ImagePath:
                        strategyOperator = OverallClassification.GetInstance();
                        break;
                    case MessageType.Request_FindPattern_Top_ImagePath:
                        strategyOperator = Pattern.PatternClassification.GetInstance();
                        break;
                    case MessageType.Request_FindPattern_Bottom_ImagePath:
                        strategyOperator = Pattern.PatternClassification.GetInstance();
                        break;
                    case MessageType.Request_FindPattern_Outer_ImagePath:
                        strategyOperator = Pattern.PatternClassification.GetInstance();
                        break;
                    case MessageType.Request_FindPattern_Overall_ImagePath:
                        strategyOperator = Pattern.PatternClassification.GetInstance();
                        break;
                    case MessageType.Request_FindStyle_ImagePath:
                        strategyOperator = Style.StyleClassification.GetInstance();
                        break;
                }
                return strategyOperator;
            }
        }
        public string workerName = null;
        protected string rootpath = null;
        public string RootPath
        {
            get { return rootpath; }
        }


        /// <summary>
        /// singleton pattern
        /// </summary>
        /// <returns></returns>
        public static GPUWorkerOperatorFactory GetInstance()
        {
            return Holder.instance;
        }
        /// <summary>
        /// Lazy Initialization + holder
        /// </summary>
        private static class Holder
        {
            public static GPUWorkerOperatorFactory instance = new GPUWorkerOperatorFactory();
        }
    }


    public class TaskOperatorFactory : IOperatorFactory
    {
        public IStrategyOperateAble GetOperator(MessageType type)
        {
            lock (Holder.instance)
            {
                IStrategyOperateAble strategyOperator = null;
                switch (type)
                {
                    case MessageType.Request_ImageAnalysis_ImagePath:
                        strategyOperator = new ImageAnalysisOperator();
                        break;
                    case MessageType.Request_TestTask_container:
                        //throw new NotImplementedException();
                        strategyOperator = new TestTaskManager.TestTask_ResourceContainer_NoHelper();
                        break;
                }
                return strategyOperator;
            }
        }
        
        /// <summary>
        /// singleton pattern
        /// </summary>
        /// <returns></returns>
        public static TaskOperatorFactory GetInstance()
        {
            return Holder.instance;
        }
        /// <summary>
        /// Lazy Initialization + holder
        /// </summary>
        private static class Holder
        {
            public static TaskOperatorFactory instance = new TaskOperatorFactory();
        }
    }
}
