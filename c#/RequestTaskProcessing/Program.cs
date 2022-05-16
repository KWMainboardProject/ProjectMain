using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mainboard;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;

namespace RequestTaskProcessing
{
    class Program
    {
        static void Main(string[] args)
        {
            TestWorkResourceMethod();
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