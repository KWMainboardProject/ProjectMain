using System;
using System.Collections.Generic;
using System.Text;

namespace RequestTaskProcessing.StrategyOperator
{
    
    public interface IStrategyOperateAble
    {
        public void SetResource(TaskMessage message);
        public void Work();
        public TaskMessage GetMessage();
        public void ClearResource();
    }

       

    public class NullOperator : IStrategyOperateAble
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
        
        public static NullOperator GetInstance()
        {
            return Holder.instance;
        }
        /// <summary>
        /// Lazy Initialization + holder
        /// </summary>
        private static class Holder
        {
            public static NullOperator instance = new NullOperator();
        }
    }

}
