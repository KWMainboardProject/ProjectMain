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
using RequestTaskProcessing.StrategyOperator;
using RequestTaskProcessing.StrategyOperator.SubCategory;
using System.Diagnostics;
using OpenCvSharp;

namespace RequestTaskProcessing
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestWorkResourceMethod();
            //TestTaskMessageMethod();
            //TestTaskMessageAndConsumer();
            //TestTaskManager();
            //TestReturnedResourceContiner();
            //TestTaskMessage();
            //TestJsonFile();
            //TestSubCategory();
            Myftp.Run_server();
            //TestSharePath();
            //TestYolo();
            //TestClassification();
            //TestYoloBoundbox();
            //TestMeanshift();
            //TestColor();
            //TestHsi2XYI();
        }

        static void TestHsi2XYI()
        {
            Vec3b hsi = new Vec3b(10, 200, 150);

            Vec3d xyi = new Vec3d();

            xyi = MainSubColorOperator.hsi2normalxyi(hsi);
            Console.Write("xyi : ");
            for(int i=0; i<3; i++)
            {
                Console.Write(xyi[i]+" ");
            }
            Console.WriteLine();


            hsi = MainSubColorOperator.normalxyi2hsi(xyi);
            Console.Write("hsi : ");
            for (int i = 0; i < 3; i++)
            {
                Console.Write(hsi[i] + " ");
            }
            Console.WriteLine();

            xyi = MainSubColorOperator.hsi2normalxyi(hsi);
            Console.Write("xyi : ");
            for (int i = 0; i < 3; i++)
            {
                Console.Write(xyi[i] + " ");
            }
            Console.WriteLine();


            hsi = MainSubColorOperator.normalxyi2hsi(xyi);
            Console.Write("hsi : ");
            for (int i = 0; i < 3; i++)
            {
                Console.Write(hsi[i] + " ");
            }
            Console.WriteLine();
            xyi = MainSubColorOperator.hsi2normalxyi(hsi);
            Console.Write("xyi : ");
            for (int i = 0; i < 3; i++)
            {
                Console.Write(xyi[i] + " ");
            }
            Console.WriteLine();


            hsi = MainSubColorOperator.normalxyi2hsi(xyi);
            Console.Write("hsi : ");
            for (int i = 0; i < 3; i++)
            {
                Console.Write(hsi[i] + " ");
            }
            Console.WriteLine();
            xyi = MainSubColorOperator.hsi2normalxyi(hsi);
            Console.Write("xyi : ");
            for (int i = 0; i < 3; i++)
            {
                Console.Write(xyi[i] + " ");
            }
            Console.WriteLine();


            hsi = MainSubColorOperator.normalxyi2hsi(xyi);
            Console.Write("hsi : ");
            for (int i = 0; i < 3; i++)
            {
                Console.Write(hsi[i] + " ");
            }
            Console.WriteLine();
            xyi = MainSubColorOperator.hsi2normalxyi(hsi);
            Console.Write("xyi : ");
            for (int i = 0; i < 3; i++)
            {
                Console.Write(xyi[i] + " ");
            }
            Console.WriteLine();


            hsi = MainSubColorOperator.normalxyi2hsi(xyi);
            Console.Write("hsi : ");
            for (int i = 0; i < 3; i++)
            {
                Console.Write(hsi[i] + " ");
            }
            Console.WriteLine();
        }

        static void TestColor()
        {
            string workPath = ShareWorkPath.GetInstance().WORKER_PATH + @"\imageAnalysis\shirt5";
            MainSubColorOperator msop = new MainSubColorOperator();
            MainCategoryContainer mc = new MainCategoryContainer("Top");
            mc.cropimgPath.Value = workPath + @"\croped_Top.jpg";
            mc.cropmaskPath.Value = workPath + @"\mask_Top.jpg";

            TaskMessage m = new TaskMessage("6298b267b3eed", null, MessageType.EmptyMessage, mc);

            msop.SetResource(m);
            msop.Work();
            TaskMessage rm = msop.GetMessage();

            rm.Print();
        }
        static void TestMeanshift()
        {
            string imgPath = ShareWorkPath.GetFileList(ShareWorkPath.GetInstance().IMAGE_RESOURCE_PATH)[0];

            Console.WriteLine("target img path : " + imgPath);

            TaskMessage m = new TaskMessage(imgPath, null, MessageType.Request_ImageAnalysis_ImagePath, new StringContainer("img_path", imgPath));

            // analysis Work
            ImageAnalysisOperator analysis = new ImageAnalysisOperator();
            analysis.SetResource(m);
            analysis.Work();
            TaskMessage rm = analysis.GetMessage();
            analysis.ClearResource();
            rm.Print();

            //Get path
            string rootPath = System.IO.Directory.GetParent(imgPath).ToString();
            string resultName = System.IO.Path.GetFileNameWithoutExtension(imgPath) + "_result.jpg";

            using (Mat img = Cv2.ImRead(imgPath))
            {
                int level = 2;
                double space_radius = 30.0;
                double color_radius = 30.0;
                // mean shift : ouput = nomal xyi
                Console.WriteLine("Start MeanShift");
                using(Mat seg = img.PyrMeanShiftFiltering(space_radius, color_radius, level,
                    new TermCriteria(CriteriaType.Eps | CriteriaType.Count, 1000, 0.001)))
                {
                    Console.WriteLine("End MeanShift");

                    FashionObjectsContainer fobject = new FashionObjectsContainer();
                    fobject.SetJObject(rm.resource.GetJObject());
                    int i = 0;
                    foreach (var obj in fobject.GetList())
                    {
                        FashionObjectContainer f = obj as FashionObjectContainer;
                        if (f != null && !f.IsEmpty)
                        {
                            string Label = f.GetKey() + "." + f.subcategory.classficationContainer.GetValue().ToString();
                            Cv2.Rectangle(seg,
                                new Point(f.boundbox.Ymin, f.boundbox.Xmin),
                                new Point(f.boundbox.Ymax, f.boundbox.Xmax),
                                new Scalar(((i % 2) * 173) % 255, (((i + 1) % 3) * 173) % 255, (((i + 2) % 4) * 173) % 255), 2);
                            Cv2.PutText(seg, Label, new Point(f.boundbox.Ymin, f.boundbox.Xmin - 5),
                                HersheyFonts.HersheySimplex, 1,
                                new Scalar(((i % 2) * 173) % 255, (((i + 1) % 3) * 173) % 255, (((i + 2) % 4) * 173) % 255), 2);
                            //Console.WriteLine(i.ToString()+":"+(((i % 7) * 173) % 255).ToString());
                            i++;
                        }
                    }
                    Cv2.ImWrite(rootPath + @"\" + resultName, seg);
                    using (var smallMat = new Mat())
                    {
                        Cv2.Resize(seg, smallMat, new Size(1000, 1000), 1, 1);
                        Cv2.ImShow(imgPath, smallMat);
                        Cv2.WaitKey();
                    }
                }
            }
        }
        static void TestYoloBoundbox()
        {
            Console.Write("target img path : ");
            string imgPath = Console.ReadLine();

            //Set prepare
            ConcurrentQueue<TaskMessage> q = new ConcurrentQueue<TaskMessage>();
            SimpleMessageProductor mp = new SimpleMessageProductor();
            mp.SetQueue(q);

            TaskMessage m = new TaskMessage(imgPath, mp, MessageType.Request_ImageAnalysis_ImagePath, new StringContainer("img_path", imgPath));

            // analysis Work
            ImageAnalysisOperator analysis = new ImageAnalysisOperator();
            analysis.SetResource(m);
            analysis.Work();
            TaskMessage rm = analysis.GetMessage();
            analysis.ClearResource();
            //rm.Print();

            //Get path
            string rootPath = System.IO.Directory.GetParent(imgPath).ToString();
            string resultName = System.IO.Path.GetFileNameWithoutExtension(imgPath) + "_result.jpg";

            using (Mat img = Cv2.ImRead(imgPath))
            {
                FashionObjectsContainer fobject = new FashionObjectsContainer();
                fobject.SetJObject(rm.resource.GetJObject());
                int i = 0;
                foreach (var obj in fobject.GetList())
                {
                    FashionObjectContainer f = obj as FashionObjectContainer;
                    if (f != null && !f.IsEmpty)
                    {
                        string Label = f.GetKey() + "." + f.subcategory.classficationContainer.GetValue().ToString();
                        Cv2.Rectangle(img, 
                            new Point(f.boundbox.Ymin, f.boundbox.Xmin), 
                            new Point(f.boundbox.Ymax, f.boundbox.Xmax),
                            new Scalar(((i % 2) * 173)%255, (((i+1) % 3) * 173) % 255, (((i+2) % 4) * 173) % 255), 2);
                        Cv2.PutText(img, Label, new Point(f.boundbox.Ymin, f.boundbox.Xmin - 5), 
                            HersheyFonts.HersheySimplex, 1,
                            new Scalar(((i % 2) * 173) % 255, (((i + 1) % 3) * 173) % 255, (((i + 2) % 4) * 173) % 255), 2);
                        //Console.WriteLine(i.ToString()+":"+(((i % 7) * 173) % 255).ToString());
                        i++;
                    }
                }
                Cv2.ImWrite(rootPath+@"\"+resultName, img);
                using(var smallMat = new Mat())
                {
                    Cv2.Resize(img, smallMat,new Size(1000,1000),1,1);
                    Cv2.ImShow(imgPath, smallMat);
                    Cv2.WaitKey();
                }
            }
        }
        static void TestClassification()
        {
            TestTaskManager.TestSenderManager sender = new TestTaskManager.TestSenderManager();
            GPUWorker worker = new GPUWorker();
            worker.Start();
            sender.Start();

            IMessageProductAble p = worker.GetProductor();
            foreach (var file in ShareWorkPath.GetFileList(ShareWorkPath.GetInstance().IMAGE_RESOURCE_PATH))
            {
                var fname = Path.GetFileName(file);
                TaskMessage m = new TaskMessage(
                    fname,
                    sender.GetProductor(),
                    MessageType.Request_FindSubCategory_Top_ImagePath,
                    new StringContainer("img_path", file));
                p.Product(m);
            }

            worker.SetTimeOutThreshold();
            Console.WriteLine("Join taskManager");
            worker.Join();

            sender.SetTimeOutThreshold();
            Console.WriteLine("Set Time out task manager");
            sender.Join();

            Console.WriteLine("complete##################################################");

        }
        static void TestYolo()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();
            TestTaskManager.TestSenderManager sender = new TestTaskManager.TestSenderManager();
            TaskManager taskManager = TaskManager.GetInstance();
            GPUWorkManager gpuManager = GPUWorkManager.GetInstance();

            Console.WriteLine("Consume Message");
            sender.Start();
            taskManager.Start();


            IMessageProductAble p = taskManager.GetProductor();
            Console.WriteLine("Strat Create Message");
            Console.WriteLine(ShareWorkPath.GetInstance().IMAGE_RESOURCE_PATH);
            foreach (var file in ShareWorkPath.GetFileList(ShareWorkPath.GetInstance().IMAGE_RESOURCE_PATH))
            {
                var fname = Path.GetFileName(file);
                TaskMessage m = new TaskMessage(
                    fname,
                    sender.GetProductor(),
                    MessageType.Request_ImageAnalysis_ImagePath,
                    new StringContainer("img_path", file));
                p.Product(m);
            }
            Console.WriteLine("Complete Create Message");

            sender.SetTimeOutThreshold(15000);
            Console.WriteLine("Set Time out task manager");
            sender.Join();

            Console.WriteLine("complete##################################################");
            stopwatch.Stop();
            System.Console.WriteLine("run time : " + stopwatch.Elapsed);

            Console.WriteLine("Set Time out task manager & gpu manager");
            gpuManager.SetTimeOutThreshold(5000);
            taskManager.SetTimeOutThreshold(5000);

            taskManager.Join();
            gpuManager.Join();
        }

        static void TestSharePath()
        {
            ShareWorkPath swp = ShareWorkPath.GetInstance();
        }
        static void TestSubCategory()
        {
            TopClassification tc = TopClassification.GetInstance();
            TestTaskManager.TestSenderManager sender = new TestTaskManager.TestSenderManager();
            
            const int iterNum = 10;
            for(int i=0; i<iterNum; i++)
            {
                TaskMessage message = new TaskMessage();
                message.ip.Value = "top" + i.ToString();
                message.productor = sender.GetProductor();
                message.resource = new StringContainer("img_path", @"C:\Users\vbmrk\Downloads/hud.jpg");//file name
                message.type = MessageType.Request_FindSubCategory_Top_ImagePath;

                tc.SetResource(message);
                tc.Work();
                TaskMessage m = tc.GetMessage();
                m.Print();
                tc.ClearResource();
            }
        }
        static void TestJsonFile()
        {
            DetectedObjectsContainer fashion = new DetectedObjectsContainer();
            Random random = new Random();
            if (random.Next(2) == 0)
            {
                MainCategoryContainer container = fashion.top;
                container.boundboxContainer.SetDumi();

                {
                    SubCategoryContainer c = new SubCategoryContainer();
                    c.classficationContainer.SetClassfication("T-shirt");
                    c.confidenceContainer.SetConfidence(0.7f);
                    container.SetAtribute(c);
                }
                {
                    MainSubColorContainer c = new MainSubColorContainer();
                    c.main.rgbContainer.SetDumi();
                    c.main.SetProportion(random.Next(30, 100));
                    c.sub.rgbContainer.SetDumi();
                    c.sub.SetProportion(random.Next(0, 100 - c.main.proportionContainer.Value));
                    container.SetAtribute(c);
                }

                {
                    PatternContainer c = new PatternContainer();
                    c.classficationContainer.SetClassfication("solid");
                    c.confidenceContainer.SetConfidence(0.7f);
                    container.SetAtribute(c);
                }

                {
                    StyleContainer c = new StyleContainer();
                    c.classficationContainer.SetClassfication("casual");
                    c.confidenceContainer.SetConfidence(0.7f);
                    container.SetAtribute(c);
                }

            }
            if (random.Next(2) == 0)
            {
                MainCategoryContainer container = fashion.bottom;
                container.boundboxContainer.SetDumi();

                {
                    SubCategoryContainer c = new SubCategoryContainer();
                    c.classficationContainer.SetClassfication("Jean");
                    c.confidenceContainer.SetConfidence(0.7f);
                    container.SetAtribute(c);
                }
                {
                    MainSubColorContainer c = new MainSubColorContainer();
                    c.main.rgbContainer.SetDumi();
                    c.main.SetProportion(random.Next(30, 100));
                    c.sub.rgbContainer.SetDumi();
                    c.sub.SetProportion(random.Next(0, 100 - c.main.proportionContainer.Value));
                    container.SetAtribute(c);
                }

                {
                    PatternContainer c = new PatternContainer();
                    c.classficationContainer.SetClassfication("solid");
                    c.confidenceContainer.SetConfidence(0.7f);
                    container.SetAtribute(c);
                }

                {
                    StyleContainer c = new StyleContainer();
                    c.classficationContainer.SetClassfication("casual");
                    c.confidenceContainer.SetConfidence(0.7f);
                    container.SetAtribute(c);
                }
            }
            if (random.Next(2) == 0)
            {
                MainCategoryContainer container = fashion.overall;
                container.boundboxContainer.SetDumi();

                {
                    SubCategoryContainer c = new SubCategoryContainer();
                    c.classficationContainer.SetClassfication("Jump_suit");
                    c.confidenceContainer.SetConfidence(0.7f);
                    container.SetAtribute(c);
                }
                {
                    MainSubColorContainer c = new MainSubColorContainer();
                    c.main.rgbContainer.SetDumi();
                    c.main.SetProportion(random.Next(30, 100));
                    c.sub.rgbContainer.SetDumi();
                    c.sub.SetProportion(random.Next(0, 100 - c.main.proportionContainer.Value));
                    container.SetAtribute(c);
                }

                {
                    PatternContainer c = new PatternContainer();
                    c.classficationContainer.SetClassfication("solid");
                    c.confidenceContainer.SetConfidence(0.7f);
                    container.SetAtribute(c);
                }

                {
                    StyleContainer c = new StyleContainer();
                    c.classficationContainer.SetClassfication("casual");
                    c.confidenceContainer.SetConfidence(0.7f);
                    container.SetAtribute(c);
                }
            }
            if (random.Next(2) == 0)
            {
                MainCategoryContainer container = fashion.outer;
                container.boundboxContainer.SetDumi();

                {
                    SubCategoryContainer c = new SubCategoryContainer();
                    c.classficationContainer.SetClassfication("Jaket");
                    c.confidenceContainer.SetConfidence(0.7f);
                    container.SetAtribute(c);
                }
                {
                    MainSubColorContainer c = new MainSubColorContainer();
                    c.main.rgbContainer.SetDumi();
                    c.main.SetProportion(random.Next(30, 100));
                    c.sub.rgbContainer.SetDumi();
                    c.sub.SetProportion(random.Next(0, 100 - c.main.proportionContainer.Value));
                    container.SetAtribute(c);
                }

                {
                    PatternContainer c = new PatternContainer();
                    c.classficationContainer.SetClassfication("solid");
                    c.confidenceContainer.SetConfidence(0.7f);
                    container.SetAtribute(c);
                }

                {
                    StyleContainer c = new StyleContainer();
                    c.classficationContainer.SetClassfication("casual");
                    c.confidenceContainer.SetConfidence(0.7f);
                    container.SetAtribute(c);
                }
            }
            string filename = @"C:/sub_category/gpuworker1/fashion.json";
            File.WriteAllText(filename, fashion.GetJObject().ToString());
        }

        static void TestTaskMessage()
        {
            TestTaskManager.TestSenderManager sender = new TestTaskManager.TestSenderManager();
            TaskMessage m = new TaskMessage(
                    "Task" + 123123.ToString(),
                    sender.GetProductor(),
                    MessageType.Request_ImageAnalysis_ImagePath,
                    new StringContainer("img_path", "./task/img_" + 123123.ToString()));

            m.Print();

            TaskMessage mCopy = new TaskMessage(m);
            mCopy.Print();
        }

        //static void Test
        static void TestReturnedResourceContiner()
        {
            const int TASK_NUM = 5;
            TestTaskManager.TestSenderManager sender = new TestTaskManager.TestSenderManager();
            TaskManager taskManager = TaskManager.GetInstance();
            GPUWorkManager gpuManager = GPUWorkManager.GetInstance();

            Console.WriteLine("Consume Message");
            sender.Start();
            taskManager.Start();


            IMessageProductAble p = taskManager.GetProductor();
            Console.WriteLine("Strat Create Message");
            for (int i = 0; i < TASK_NUM; i++)
            {
                TaskMessage m = new TaskMessage(
                    "Task" + i.ToString(),
                    sender.GetProductor(),
                    MessageType.Request_ImageAnalysis_ImagePath,
                    new StringContainer("img_path", "./task/img_"+i.ToString()));
                p.Product(m);
            }
            Console.WriteLine("Complete Create Message");

            
            taskManager.SetTimeOutThreshold();
            Console.WriteLine("Set Time out task manager");
            taskManager.Join();

            Console.WriteLine("Join taskManager");

            sender.SetTimeOutThreshold();
            Console.WriteLine("Set Time out task manager");
            sender.Join();

            gpuManager.SetTimeOutThreshold();
            gpuManager.Join();
            Console.WriteLine("complete##################################################");
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
            for (int i = 0; i < TASK_NUM; i++)
            {
                TaskMessage m = new TaskMessage(
                    "Task" + i.ToString(),
                    sender.GetProductor(),
                    MessageType.Request_TestTask_container);
                p.Product(m);
            }

            taskManager.SetTimeOutThreshold();
            taskManager.Join();
            sender.SetTimeOutThreshold();
            sender.Join();
            Console.WriteLine("complete##################################################");
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
                    //stop thread and claear Q
                    if (stopAndClearTF)
                    {
                        lock (q)
                        {
                            do
                            {
                                try
                                {
                                    Consume();
                                }
                                catch { Console.WriteLine("Clear Q"); }
                                //clear Q
                            } while (!qTF);

                            stopAndClearTF = false;

                            //need thread stop
                            Console.WriteLine("\tplz thread stop at QThread.Run");
                            break;
                        }
                    }

                    //Get message

                    TaskMessage m=null;
                    try
                    {
                        m = Consume();
                    }
                    catch (TimeoutException ex)
                    {
                        StopAndClear();
                        Console.WriteLine("stop sender");
                    }
                    if (!qTF || m == null)//fali consume
                    {
                        //Console.WriteLine("Wait Messamge - TestSender");
                        continue;
                    }

                    //Success consume
                    //string filename = ShareWorkPath.GetInstance().RESULT_RESOURCE_PATH + @"\" + m.ip.Value+".json";
                    //File.WriteAllText(filename, m.resource.GetJObject().ToString());

                    Console.WriteLine(thread.ToString()+" : resource");
                    m.Print();

                    //SubCategoryContainer fc = new SubCategoryContainer();
                    //fc.SetJObject(m.resource.GetJObject());
                    //Console.WriteLine(fc.GetJObject().ToString());
                }
            }
            public override void SetTimeOutThreshold(int time = 5000)
            {
                timeout.SetThesholdTime(time);
            }

            Thread thread = null;
        }

        public class TestTask_ResourceContainer_NoHelper : IStrategyOperateAble
        {
            const int SLEEP_TIME = 500;
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
                Thread.Sleep(SLEEP_TIME);
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