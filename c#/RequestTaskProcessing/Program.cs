using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RequestTaskProcessing;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.Threading;

namespace RequestTaskProcessing
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestWorkResourceMethod();
            //TestTaskMessageMethod();
            //TestTaskMessageAndConsumer();
            TestTaskManager();
        }

        static void TestWorkResourceMethod()
        {
            TestWorkResouceClass test = new TestWorkResouceClass();
            string id = "test_Resource_";
            string classPath = test.WriteClassfication(id, "subcategory");
            string boundPath = test.WriteBoundbox(id, "boundbox");


            MainCategoryContainer container = new MainCategoryContainer();
            container.classficationContainer.SetJObject(test.ReadJsonFile(classPath));
            container.boundboxContainer.SetJObject(test.ReadJsonFile(boundPath));

            Console.WriteLine(container.GetJObject().ToString());
            File.WriteAllText(@"C:/sub_category/maincategory.json", container.GetJObject().ToString());
        }
        static void TestTaskMessageAndConsumer()
        {
            TestTaskMessageClass.TestConsumer consumer = new TestTaskMessageClass.TestConsumer();
            IMessageProductAble pdt = consumer.GetProductor();
            int iterNum = 10;
            for(int i=0; i<iterNum; i++)
            {
                TaskMessage m = new TaskMessage("ip_" + i.ToString(),null,MessageType.MessageTypeNum, new ConfidenceContainer(i * 0.15f));
                pdt.Product(m);
            }
            while (!consumer.IsEmpty())
            {
                TaskMessage m = consumer.Consume();
                m.Print();
            }

        }
        static void TestTaskMessageMethod()
        {
            TestTaskMessageClass.TestConsumer consumer = new TestTaskMessageClass.TestConsumer();
            TaskMessage message1 = new TaskMessage("Worker1",
                consumer.GetProductor(),
                MessageType.MessageTypeNum,
                new ClassficationContainer("Maincategory"));
            Thread work1 = new Thread(() => TestTaskMessageClass.Work(message1));

            TaskMessage message2 = new TaskMessage("Worker2",
                consumer.GetProductor(),
                MessageType.Response_Fail,
                new ConfidenceContainer(0.9f));
            Thread work2 = new Thread(() => TestTaskMessageClass.Work(message2));

            TaskMessage message3 = new TaskMessage("Worker3",
                consumer.GetProductor(),
                MessageType.MessageTypeNum,
                new ClassficationContainer("Top"));
            Thread work3 = new Thread(() => TestTaskMessageClass.Work(message3));

            Console.WriteLine("Start Workers");
            work1.Start();
            work2.Start();
            work3.Start();

            Console.WriteLine("Sleep Main Process");
            Thread.Sleep(1);
            Console.WriteLine("Start Consume");
            while (!consumer.IsEmpty())
            {
                TaskMessage m = consumer.Consume();
                m.Print();
            }
        }
        static void TestTaskManager()
        {
            const int TASK_NUM = 10;
            TestTaskManager.TestSenderManager sender = new TestTaskManager.TestSenderManager();
            TaskManager taskManager = TaskManager.GetInstance();
            sender.Start();
            taskManager.Start();

            IMessageProductAble p = taskManager.GetProductor();
            for(int i=0; i<TASK_NUM; i++)
            {
                TaskMessage m = new TaskMessage(
                    "Task" + i.ToString(),
                    sender.GetProductor(),
                    MessageType.Request_TestTask_container);
                p.Product(m);
            }

        }
    }

    public class TestTaskManager
    {
        public class TestSenderManager : QTheading
        {
            const int SLEEP_TIME = 10;
            public TestSenderManager()
            {
                q = new ConcurrentQueue<TaskMessage>();
                productor.SetQueue(q);
            }

            public override void Join()
            {
                thread.Join();
            }

            public override void Start()
            {
                thread = new Thread(() => Run());
                thread.Start();
            }

            protected override void Run()
            {
                if (q == null)
                    throw new NullReferenceException();
                while (thread.IsAlive)
                {
                    Thread.Sleep(SLEEP_TIME);
                    //Get message
                    TaskMessage m = Consume();
                    if (!qTF) continue;//fali consume
                    if (m == null) continue;
                    //Success consume

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
                    Console.WriteLine(thread.ToString());
                    m.Print();
                }
            }
            Thread thread = null;
        }

        public class TestTask_ResourceContainer_NoHelper : IStrategyOperateAble
        {
            public TestTask_ResourceContainer_NoHelper()
            {
                WORKER_NUM++;
                workerNumber = WORKER_NUM;
            }
            public void ClearResource()
            {
                return;
            }

            public TaskMessage GetMessage()
            {
                TaskMessage m = new TaskMessage(ip, p, MessageType.Response_TestTask_container, r);
                return m;
            }

            public void SetResource(TaskMessage m)
            {
                //Worng
                if (m.type != MessageType.Request_TestTask_container)
                    throw new NullReferenceException();

                ip = m.ip.Value;
                //r = (StringContainer)m.resource;
                p = m.productor;
            }

            public void Work()
            {
                r = new StringContainer("resource", "Worker" + workerNumber.ToString());
            }
            static int WORKER_NUM = 0;
            private int workerNumber;
            protected string ip = null;
            protected IMessageProductAble p = null;
            protected StringContainer r = null;
        }
    }


    class TestTaskMessageClass
    {
        public class TestConsumer : IMessageConsumeAble
        {
            public TestConsumer()
            {
                productor.SetQueue(q);
            }
            public TaskMessage Consume()
            {
                TaskMessage message = new TaskMessage();
                if (q.IsEmpty) return message;

                q.TryDequeue(out message);
                return message;
            }

            public IMessageProductAble GetProductor()
            {
                return productor;
            }

            public bool IsEmpty()
            {
                return q.IsEmpty;
            }

            protected ConcurrentQueue<TaskMessage> q=new ConcurrentQueue<TaskMessage>();
            protected SimpleMessageProductor productor = new SimpleMessageProductor();
        }
        static public void Work(TaskMessage message, int iterNum=1000)
        {
            IMessageProductAble productor = message.productor;
            for (int i=0; i<iterNum; i++)
            {
                TaskMessage respenceMessage = new TaskMessage();
                StringContainer container = new StringContainer();
                container.Value = message.ip.Value + "_resource_" + i.ToString();
                respenceMessage.resource = message.resource;
                respenceMessage.ip = container;
                respenceMessage.type = message.type;
                respenceMessage.productor = null;

                productor.Product(respenceMessage);
            }
        }
    }
    class TestWorkResouceClass
    {
        public string WriteClassfication(string id, string resource)
        {
            //Set file Path
            string workDirectory = @"C:/sub_category/gpuworker1/" + id;
            string filename = workDirectory + "/classfication.json";

            //Work
            ClassficationContainer container = new ClassficationContainer();
            container.SetClassfication("Top");


            //get JObject
            JObject json = container.GetJObject();

            //create dircetory&json file
            CreateDirectory(workDirectory);
            File.WriteAllText(filename, json.ToString());
            return filename;
        }
        public string WriteBoundbox(string id, string resource)
        {
            //Set file Path
            string workDirectory = @"C:/sub_category/gpuworker1/" + id;
            string filename = workDirectory + "/boundbox.json";

            //Work
            BoundBoxContainer container = new BoundBoxContainer();
            container.SetBoundBox(10, 20, 15, 25);


            //get JObject
            JObject json = container.GetJObject();

            //create dircetory&json file
            CreateDirectory(workDirectory);
            File.WriteAllText(filename, json.ToString());
            return filename;
        }

        public void CreateDirectory(string dirPath)
        {
            if (Directory.Exists(dirPath) == false)
            {
                Directory.CreateDirectory(dirPath);
            }
        }

        public JObject ReadJsonFile(string filename)
        {
            string str = File.ReadAllText(filename);
            if (str == "")
                throw new Exception();
            JObject json = JObject.Parse(str);
            return json;
        }
    }
}