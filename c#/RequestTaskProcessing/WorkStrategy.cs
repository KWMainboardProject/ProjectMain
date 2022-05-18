using System;
using System.Collections.Generic;
using System.Text;

namespace RequestTaskProcessing
{
    
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
            return;
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
            IMessageProductAble requester = GPUWorkManager.GetInstance().GetProductor();

            //request remove bg

            //wait returned remove bg

            //request yolo v5

            //wait returnd Detected objects

            //crop img each object

            //save croped img

            //request subcategory * (0,4)

            //request pattern * (0,4)

            //request style

            //calc color

            //wait returned resources

            //merge resource

            //save resource

            //prepare resoure for GetMessage
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
