using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mainboard;
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
            TestTaskMessageAndConsumer();
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
                MessageType.ResponseFail,
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
                IpContainer container = new IpContainer();
                container.IP = message.ip.IP + "_resource_" + i.ToString();
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