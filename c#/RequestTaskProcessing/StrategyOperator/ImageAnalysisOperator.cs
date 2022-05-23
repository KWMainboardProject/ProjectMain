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
            GPUWorkManager requester = GPUWorkManager.GetInstance();
            requester.Start();


            //request remove bg
            TaskMessage rmbgM = new TaskMessage(requestMessage);        //init
            rmbgM.type = MessageType.Request_Removebg_ImagePath;        //set
            rmbgM.productor = GetProductor();                           //set
            waitMessage.Add(MessageType.Receive_ImagePath_RemoveBG);    //받을 메세지 추가
            requester.GetProductor().Product(rmbgM);                                   //request

            //wait returned remove bg
            Start(); // Set rbimgPath
            Join();
            //Console.WriteLine("Pass Join");
            InitThread();
            Console.WriteLine("Clear working");
            //Console.WriteLine("Set Remove Backgruound Message");
            //Console.WriteLine(rbimgPath.GetJObject().ToString());
            return;

            //request yolo v5
            TaskMessage yoloM = new TaskMessage(requestMessage);
            yoloM.type = MessageType.Request_FindMainCategory_ImagePath;        //set
            yoloM.productor = GetProductor();                                   //set
            yoloM.resource = rbimgPath;                                         //set
            waitMessage.Add(MessageType.Receive_Container_DetectedObjects);     //받을 메세지 추가
            requester.GetProductor().Product(yoloM);                                          //request

            //wait returnd Detected objects
            Start(); // Set container
            Join();
            InitThread();
            Console.WriteLine("Set Detected Objects Message");
            Console.WriteLine(container.GetJObject().ToString());

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
        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// type : Receive_Container_Fashion
        /// </returns>
        public TaskMessage GetMessage()
        {
            TaskMessage taskMessage = new TaskMessage(requestMessage);
            taskMessage.type = MessageType.Receive_Container_Fashion;        //set
            taskMessage.productor = null;                                   //set
            taskMessage.resource = container;                               //set
            return taskMessage;
        }

        public void ClearResource()
        {
            return;
        }
        /// <summary>
        /// Thread는 한번씩 밖에 start를 못하니
        /// 여러번 해주기 위해서 이전 thread를 버리는 
        /// 함수
        /// </summary>
        public void InitThread()
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

        protected FashionObjectsContainer container = new FashionObjectsContainer();
        protected StringContainer rbimgPath = null;
        protected List<MessageType> waitMessage = new List<MessageType>();

        public void OpenMessage(TaskMessage message)
        {
            //delete wait message
            waitMessage.Remove(message.type);
            Console.WriteLine("Open Message");
            message.Print();
            //open & input to container
            switch (message.type)
            {
                case MessageType.Receive_ImagePath_RemoveBG:
                    rbimgPath = message.resource as StringContainer; //clone
                    break;
                case MessageType.Receive_Container_DetectedObjects:
                    container.SetJObject(message.resource.GetJObject()); //clone
                    break;
                case MessageType.Receive_Container_SubCategory_Top:
                case MessageType.Receive_Container_Pattern_Top:
                    container.top.SetAtribute(message.resource);        //attach
                    break;
                case MessageType.Receive_Container_SubCategory_Bottom:
                case MessageType.Receive_Container_Pattern_Bottom:
                    container.bottom.SetAtribute(message.resource);        //attach
                    break;
                case MessageType.Receive_Container_SubCategory_Outer:
                case MessageType.Receive_Container_Pattern_Outer:
                    container.outer.SetAtribute(message.resource);        //attach
                    break;
                case MessageType.Receive_Container_SubCategory_Overall:
                case MessageType.Receive_Container_Pattern_Overall:
                    container.overall.SetAtribute(message.resource);        //attach
                    break;
                case MessageType.Receive_Container_Style:
                    foreach(CompoundContainer obj in container.GetList())
                    {
                        obj.SetAtribute(message.resource);                  //모든 요소에 attach
                    }
                    break;
            }
            if(waitMessage.Count == 0)
            {
                StopAndClear();
            }
        }

        /// <summary>
        /// Q에서 내용을 기다리다가
        /// return Message를 보고 container를 채우는 methods 
        /// </summary>
        protected override void Run()
        {
            if (q == null)
                throw new NullReferenceException();
            //thread가 살아있고, 받을 메세지가 남았다면
            while (thread.IsAlive && waitMessage.Count>0)
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
                        Console.WriteLine("\tplz thread stop at QThread.Run");
                        break;
                    }
                }

                //Get message
                //Get message
                TaskMessage m = null;
                m = Consume();
                //try { m = Consume(); }
                //catch (TimeoutException e) { StopAndClear(); }
                if (!qTF || m == null)//fali consume
                {
                    continue;
                }
                //Success consume
                OpenMessage(m);
            }
        }
        /// <summary>
        /// Run를 multi Thread를 한다.
        /// </summary>
        public override void Start()
        {
            if (thread == null)
            {
                thread = new Thread(() => Run());
            }
            thread.Start();
        }

        public override void Join()
        {
            if (thread == null)
                throw new NullReferenceException();
            thread.Join();
        }

        protected Thread thread = null;
    }
}
