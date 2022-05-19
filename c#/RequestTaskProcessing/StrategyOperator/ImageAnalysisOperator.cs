using System;
using System.Collections.Generic;
using System.Text;

namespace RequestTaskProcessing.StrategyOperator
{
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
}
