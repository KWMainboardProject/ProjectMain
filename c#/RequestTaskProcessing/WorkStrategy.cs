using System;
using System.Collections.Generic;
using System.Text;

namespace RequestTaskProcessing
{
    /// <summary>
    /// plz singleton pattern
    /// </summary>
    public interface IStrategyOperateAble
    {
        public void SetResource(TaskMessage message);
        public void Work();
        public TaskMessage GetMessage();
        public void ClearResource();
    }

    abstract public class HaveHelperOperator : IStrategyOperateAble
    {
        public abstract void ClearResource();
        public abstract TaskMessage GetMessage();
        public abstract void SetResource(TaskMessage message);
        public abstract void Work();

        public void SetHelpProductor(IMessageProductAble productor)
        {
            this.requestProductor = productor;
        }
        protected IMessageProductAble requestProductor= null;
    }

    public class ImageAnalysisOperator : HaveHelperOperator
    {
        public override void ClearResource()
        {
            throw new NotImplementedException();
        }

        public override TaskMessage GetMessage()
        {
            throw new NotImplementedException();
        }

        public override void SetResource(TaskMessage message)
        {
            throw new NotImplementedException();
        }

        public override void Work()
        {
            throw new NotImplementedException();
        }

        public static ImageAnalysisOperator GetInstance()
        {
            return Holder.instance;
        }
        /// <summary>
        /// Lazy Initialization + holder
        /// </summary>
        private static class Holder
        {
            public static ImageAnalysisOperator instance = new ImageAnalysisOperator();
        }
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
