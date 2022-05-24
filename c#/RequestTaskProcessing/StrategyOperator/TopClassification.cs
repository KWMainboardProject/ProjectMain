using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace RequestTaskProcessing.StrategyOperator
{
    class TopClassification : IStrategyOperateAble
    {
        protected TopClassification()
        {
            //plz set gpu device
            Console.WriteLine("\tplz Top category weight upload");
            ClearResource();

            //파이썬 코드 연동
            engine = IronPython.Hosting.Python.CreateEngine();
            scope = engine.CreateScope();
            source = engine.CreateScriptSourceFromFile("./ProjectMain/python/SubCategory/Top/Top.py");
            source.Execute(scope);
            string top_model_path = "./ProjectMain/python/SubCategory/Top/Top_classification.pt";
            var getPythonClass = scope.GetVariable("Top_classification")(top_model_path);
            Console.WriteLine("\t Pt Loading...");
            getPythonClass.NN_load();
            Console.WriteLine("\t Pt Done!");
        }

        public void ClearResource()
        {
            container = new SubCategoryContainer();
            Console.WriteLine("\tplz clear resource img");
        }


        public TaskMessage GetMessage()
        {
            //보내는 메세지
            lock (Holder.instance)
            {
                TaskMessage taskMessage = new TaskMessage(requestMessage);
                taskMessage.type = MessageType.Receive_Container_SubCategory_Top;        //set
                taskMessage.productor = null;                                   //set
                taskMessage.resource = container;
                return taskMessage;
            }
        }

        public void SetResource(TaskMessage message)
        {
            //받는 메세지
            Console.WriteLine("\tplz Set resource");
            requestMessage = new TaskMessage(message);  //메세지 열어서 내용 확인
        }
        protected TaskMessage requestMessage = null;
        protected SubCategoryContainer container;
        protected Microsoft.Scripting.Hosting.ScriptEngine engine = null;
        protected Microsoft.Scripting.Hosting.ScriptScope scope = null;
        protected Microsoft.Scripting.Hosting.ScriptSource source = null;

        public void Work()
        {
            lock (Holder.instance)
            {
                //절대 경로
                string img_abs_path = requestMessage.resource.GetValue().ToString();
                var getPythonClass = scope.GetVariable("Top_classification")(0, img_abs_path);
                getPythonClass.set_device();        //디바이스 설정
                getPythonClass.img_load();          //이미지 로드
                getPythonClass.img_transform();     //이미지 전처리, 정규화
                getPythonClass.classification();    //분류

                //결과 (conf, class)
                container.confidenceContainer.SetConfidence(getPythonClass.getConf());
                container.classficationContainer.SetClassfication(getPythonClass.getResult());
               
                Thread.Sleep(1000);

                //Set container
                return;
                throw new NotImplementedException();
            }
        }
        /// <summary>
        /// singleton pattern
        /// </summary>
        /// <returns></returns>
        public static TopClassification GetInstance()
        {
            return Holder.instance;
        }
        /// <summary>
        /// Lazy Initialization + holder
        /// </summary>
        private static class Holder
        {
            public static TopClassification instance = new TopClassification();
        }
    }
}
