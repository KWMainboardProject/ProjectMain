using System;
using System.Collections.Generic;
using System.Text;

namespace RequestTaskProcessing
{
    /// <summary>
    /// plz singleton pattern
    /// </summary>
    public interface IOperatorFactory
    {
        public IStrategyOperateAble GetOperator(MessageType type);
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
                        TestTaskManager.TestTask_ResourceContainer_NoHelper worker = new TestTaskManager.TestTask_ResourceContainer_NoHelper();
                        strategyOperator = worker;
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
