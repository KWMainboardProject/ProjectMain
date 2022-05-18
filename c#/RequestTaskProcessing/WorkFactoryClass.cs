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
            IStrategyOperateAble strategyOperator = null;
            switch (type)
            {
                case MessageType.Request_ImageAnalysis_ImagePath:
                    strategyOperator = ImageAnalysisOperator.GetInstance();
                    //plz set helper productor
                    throw new NotImplementedException();
                    break;
            }
            return strategyOperator;
        }
    }
}
