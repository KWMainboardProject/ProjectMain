using System;
using System.IO;
using System.Net;
using System.Collections.Concurrent;
using System.Threading;
using RequestTaskProcessing.StrategyOperator;
using Newtonsoft.Json.Linq;

namespace RequestTaskProcessing
{
    public class SenderManager : QTheading
    {
        const int SLEEP_TIME = 10;
        string dir = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;

        public SenderManager()
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
                TaskMessage m = Consume();
                if (!qTF || m == null)//fali consume
                {
                    //Console.WriteLine("Wait Messamge - TestSender");
                    continue;
                }

                //Success consume

                string[] name_buffer = m.ip.Value.Split('.');
                string file_name = "";
                for (int i = 0; i < name_buffer.Length - 1; i++)
                {
                    file_name += name_buffer[i];
                    if (i < name_buffer.Length - 2)
                        file_name += ".";
                }

                FashionObjectsContainer fc = new FashionObjectsContainer();
                fc.SetJObject(m.resource.GetJObject());
                int lll = 0;
                //Console.WriteLine(fc.GetJObject().ToString());
                foreach (CompoundContainer c in fc.GetList())
                {
                    FashionObjectContainer f = c as FashionObjectContainer;
                    if (f != null && !f.IsEmpty)
                    {
                        string end = "";
                        switch (f.GetKey())
                        {
                            case "Top": end = "_t.json"; break;
                            case "Bottom": end = "_b.json"; break;
                            case "Outer": end = "_o.json"; break;
                            case "Overall": end = "_a.json"; break;
                            default: throw new NullReferenceException();
                        }

                        MainCategoryContainer mc = new MainCategoryContainer(f.GetKey());
                        mc.SetBoundbox((JArray)f.boundbox.GetValue());
                        mc.SetAtribute(f.subcategory);
                        if(f.color.main.IsEmpty) f.color.main.SetDumi();
                        mc.SetAtribute(f.color.main);
                        if(f.color.sub.IsEmpty) f.color.sub.SetDumi();
                        mc.SetAtribute(f.color.sub);
                        mc.SetAtribute(f.pattern);
                        //mc.SetAtribute(f.style);
                        Console.WriteLine(mc.GetJObject().ToString());
                        File.WriteAllText(ShareWorkPath.GetInstance().RESULT_RESOURCE_PATH + @"\" + file_name + end, mc.GetJObject().ToString());
                        Myftp.Upload(ShareWorkPath.GetInstance().RESULT_RESOURCE_PATH + @"\" + file_name + end, file_name + end);
                        File.Delete(ShareWorkPath.GetInstance().RESULT_RESOURCE_PATH + @"\" + file_name + end);
                        lll++;
                    }
                }
                File.Delete(ShareWorkPath.GetInstance().IMAGE_RESOURCE_PATH + @"\" + file_name + ".jpg");
                if (lll == 0)
                {
                    string end = "_t.json";
                    Console.WriteLine(fc.top.GetJObject().ToString());
                    File.WriteAllText(ShareWorkPath.GetInstance().RESULT_RESOURCE_PATH + @"\" + file_name + end, fc.top.GetJObject().ToString());
                    Myftp.Upload(ShareWorkPath.GetInstance().RESULT_RESOURCE_PATH + @"\" + file_name + end, file_name + end);
                    File.Delete(ShareWorkPath.GetInstance().RESULT_RESOURCE_PATH + @"\" + file_name + end);
                }
                Console.WriteLine("Wait Next File...");
            }
        }
        public override void SetTimeOutThreshold(int time = 5000)
        {
            timeout.SetThesholdTime(time);
        }

        Thread thread = null;
    }

    static class Myftp
    {
        private static JToken json;

        public static void GetKey()
        {
            string str = null;
            using (StreamReader sr = new StreamReader(ShareWorkPath.GetInstance().KEY_PATH +@"\key.json"))
            {
                str = sr.ReadToEnd();
            }
            JObject keyfile = JObject.Parse(str);
            json = keyfile["KEY"];
        }

        public static string[] GetFileList()
        {
            string[] file_list;
            try
            {
                string uri = string.Format("ftp://{0}/imgwork/", json["IP"]);
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.Credentials = new NetworkCredential((string)json["ID"], (string)json["PW"]);

                using (WebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string s = reader.ReadToEnd();
                        file_list = s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    }
                }
                return file_list;
            }
            catch
            {
                Console.WriteLine("Read Error");
                return new string[] { ".", ".." };
            }
        }

        public static void Upload(string filePath, string filename)
        {
            //FTP다운로드관련 URL, Method설정(UploadFile)
            string uri = string.Format("ftp://{0}/json/{1}", json["IP"], filename);
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential((string)json["ID"], (string)json["PW"]);

            //파일정보를 Byte로열기
            byte[] fileContents = null;
            using (BinaryReader br = new BinaryReader(File.Open(filePath, FileMode.Open)))
            {
                long dataLength = br.BaseStream.Length;
                fileContents = new byte[br.BaseStream.Length];
                fileContents = br.ReadBytes((int)br.BaseStream.Length);
            }

            //FTP서버에 파일전송처리
            request.ContentLength = fileContents.LongLength;
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(fileContents, 0, fileContents.Length);
            }

            //FTP전송결과확인
            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                Console.WriteLine($"Upload " + filename + " Complete, status {response.StatusDescription}");
            }
        }

        public static void Download(string downloadFile, string saveFilePath)
        {
            //FTP다운로드관련 URL, Method설정(DownloadFile)
            string uri = string.Format("ftp://{0}/imgwork/{1}", json["IP"], downloadFile);
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential((string)json["ID"], (string)json["PW"]);

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (FileStream fs = new FileStream(saveFilePath, FileMode.Create))
                    {
                        byte[] buffer = new byte[750000];
                        int read = 0;
                        do
                        {
                            read = responseStream.Read(buffer, 0, buffer.Length);
                            fs.Write(buffer, 0, read);
                            fs.Flush();
                        } while (!(read == 0));

                        fs.Flush();
                    }
                }
            }
        }
        public static void Remove(string filename)
        {
            try
            {
                string uri = string.Format("ftp://{0}/imgwork/{1}", json["IP"], filename);
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential((string)json["ID"], (string)json["PW"]);

                request.Method = WebRequestMethods.Ftp.DeleteFile;
                FtpWebResponse ftpWebResponse = (FtpWebResponse)request.GetResponse();
            }
            catch
            {
                Console.WriteLine("FTP Remove Error");
            }
        }

        public static void Run_server()
        {
            GetKey();

            string dir = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string[] file_list;

            SenderManager sender = new SenderManager();
            TaskManager taskManager = TaskManager.GetInstance();
            GPUWorkManager gpuManager = GPUWorkManager.GetInstance();

            Console.WriteLine("Consume Message");
            sender.Start();
            taskManager.Start();

            IMessageProductAble p = taskManager.GetProductor();

            Console.WriteLine("Start Observe...");
            while (true)
            {
                Thread.Sleep(100);
                file_list = (string[])GetFileList().Clone();
                if (file_list.Length > 2)
                {
                    foreach (string file in file_list)
                    {
                        if (!file.Equals(".") && !file.Equals(".."))
                        {
                            string[] name_buffer = file.Split('.');
                            string file_name = "";
                            for (int i = 0; i < name_buffer.Length - 1; i++)
                            {
                                file_name += name_buffer[i];
                                if (i < name_buffer.Length - 2)
                                    file_name += ".";
                            }
                            Download(file, ShareWorkPath.GetInstance().IMAGE_RESOURCE_PATH + @"\" + file_name + ".jpg");
                            Console.WriteLine("download " + file);
                            Remove(file);
                            Console.WriteLine("remove " + file);

                            TaskMessage m = new TaskMessage(
                                file_name + ".jpg",
                                sender.GetProductor(),
                                MessageType.Request_ImageAnalysis_ImagePath,
                                new StringContainer(file_name + ".jpg", ShareWorkPath.GetInstance().IMAGE_RESOURCE_PATH + @"\" + file_name + ".jpg"));
                            p.Product(m);
                        }
                    }
                }
            }
        }
    }
}
