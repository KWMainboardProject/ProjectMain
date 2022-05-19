using System;
using System.Collections.Generic;
using System.Text;
using RequestTaskProcessing;
using System.Threading;
using System.Collections.Concurrent;

namespace RequestTaskProcessing.StrategyOperator
{
    public class ImageAnalysisOperator : QTheading, IStrategyOperateAble
    {
        const int SLEEP_TIME = 100;
        public ImageAnalysisOperator()
        {
            this.q = new ConcurrentQueue<TaskMessage>();
            productor.SetQueue(q);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message">
        /// ip : 요청자 / p : 보낼곳 
        /// / t : Request_ImageAnalysis_ImagePath 
        /// / resource : img_path
        /// </param>
        public void SetResource(TaskMessage message)
        {
            requestMessage = new TaskMessage(message);
        }
        protected TaskMessage requestMessage = null;

        public void Work()
        {
            if (requestMessage == null) throw new NullReferenceException();
            IMessageProductAble requester = GPUWorkManager.GetInstance().GetProductor();

            //request remove bg
            TaskMessage rmbgM = new TaskMessage(requestMessage);
            rmbgM.type = MessageType.Request_Removebg_ImagePath;
            rmbgM.productor = GetProductor();
            // 받아야할 메세지 추가하기
            requester.Product(rmbgM);

            //wait returned remove bg

            //request yolo v5

            //wait returnd Detected objects

            // Yolo postprocessing

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

            //origin img delete
        }

        public TaskMessage GetMessage()
        {
            throw new NotImplementedException();
        }

        public void ClearResource()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Thread는 한번씩 밖에 start를 못하니
        /// 여러번 해주기 위해서 이전 thread를 버리는 
        /// 함수
        /// </summary>
        public void init()
        {
            if (thread.IsAlive)
            {
                StopAndClear();
                Join();
            }
            thread = null;
            SetTimeOutThreshold(0);
        }

        public override void SetTimeOutThreshold(int time = TimeOut.DEFAULT_TIME)
        {
            timeout.SetThesholdTime(time);
        }
        /// <summary>
        /// Q에서 내용을 기다리다가
        /// return Message를 보고 container를 채우는 methods 
        /// </summary>
        protected override void Run()
        {
            if (q == null)
                throw new NullReferenceException();
            if (factory == null)
                throw new NullReferenceException();
            while (thread.IsAlive)
            {
                Thread.Sleep(SLEEP_TIME);
                //stop thread and claear Q
                if (stopAndClearTF)
                {
                    lock (q)
                    {
                        do
                        {
                            Consume();
                            //clear Q
                        } while (!qTF);

                        stopAndClearTF = false;

                        //need thread stop
                        Console.WriteLine("plz thread stop at QThread.Run");
                        break;
                    }
                }

                //Get message
                TaskMessage m = Consume();
                if (!qTF || m == null)//fali consume
                {
                    continue;
                }
                //Success consume



                //Get Operator
                IStrategyOperateAble strategy = factory.GetOperator(m.type);

                if (strategy == null)
                    throw new NullReferenceException();

                //Run operator
                strategy.SetResource(m);
                strategy.Work();
                IMessageProductAble sender = m.productor;
                sender.Product(strategy.GetMessage());
                strategy.ClearResource();
            }
        }
        /// <summary>
        /// Run를 multi Thread를 한다.
        /// </summary>
        public override void Start()
        {
            throw new NotImplementedException();
        }

        public override void Join()
        {
            throw new NotImplementedException();
        }

        protected Thread thread = null;
    }
}
