using System;
using System.Collections.Generic;
using System.Text;
using RequestTaskProcessing.StrategyOperator;

namespace RequestTaskProcessing.StrategyOperator.SubCategory
{
    class BottomClassification : IStrategyOperateAble
    {
        public void ClearResource()
        {
            throw new NotImplementedException();
        }

        public TaskMessage GetMessage()
        {
            throw new NotImplementedException();
        }

        public void SetResource(TaskMessage message)
        {
            throw new NotImplementedException();
        }

        public void Work()
        {
            throw new NotImplementedException();
        }
    }
}
