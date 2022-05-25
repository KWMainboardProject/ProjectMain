
using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace RequestTaskProcessing
{
	/// <summary>
	/// work resouce의 내용을 담을 수 있는 container
	/// </summary>
	public interface IJObjectUseAbleContainer
	{
		JObject GetJObject();
		void SetJObject(JObject obj);
		string GetKey();
		JToken GetValue();

	}

	public class StringContainer : IJObjectUseAbleContainer
	{
		public StringContainer(string key = "ip", string str = null)
		{
			if (str != null) Value = str;
			this.key = key;
		}
		public JObject GetJObject()
		{
			JObject json = new JObject();
			json.Add(GetKey(), Value);
			return json;
		}

		public string GetKey()
		{
			return key;
		}

		public JToken GetValue()
		{
			return GetJObject()[GetKey()];
		}

		public void SetJObject(JObject obj)
		{
            try
            {
				Value = (string)obj[GetKey()];
			}
			catch(NullReferenceException ex)
            {
				Value = null;
            }
		}
		protected string key = null;
		protected string str = null;
		public string Value
		{
			get { return str; }
			set { str = value; }
		}
	}
	public abstract class CompoundContainer : IJObjectUseAbleContainer
	{
		abstract public void SetJObject(JObject obj);
		abstract public string GetKey();
		public JObject GetJObject()
		{
			JObject json = new JObject();
			json.Add(GetKey(), GetValue());
			return json;
		}
		public void SetAtribute(IJObjectUseAbleContainer container)
		{
			containers.Add(container);
		}

		public JToken GetValue()
		{
			JObject value = new JObject();
			if(containers.Count == 0)
            {
				value = null;
            }
            else
            {
				foreach (IJObjectUseAbleContainer container in containers)
				{
					value.Add(container.GetKey(), container.GetValue());
				}
			}
			
			return value;
		}

		public List<IJObjectUseAbleContainer> GetList()
		{
			return containers;
		}
		protected List<IJObjectUseAbleContainer> containers = new List<IJObjectUseAbleContainer>();
	}

	public class ClassficationContainer : IJObjectUseAbleContainer
	{
		public ClassficationContainer(string classfication = "None")
		{
			this.classfication = classfication;
		}
		public string GetKey() { return "classfication"; }
		public JObject GetJObject()
		{
			//if (classfication == null)
			JObject json = new JObject();
			json.Add(GetKey(), classfication);
			return json;
		}
		public void SetJObject(JObject obj)
		{
			if (obj == null) return;
			classfication = (string)obj[GetKey()];
		}
		public void SetClassfication(string classfication)
		{
			this.classfication = classfication;
		}

		public JToken GetValue()
		{
			return classfication;
			//return GetJObject()[GetKey()];
		}

		protected string classfication;
	}

	//public class ReturnedResourceContainer : IJObjectUseAbleContainer
	//{
	//	public ReturnedResourceContainer(MessageType type = MessageType.EmptyMessage, IJObjectUseAbleContainer container = null)
	//	{
	//		SetKey(type);
	//		if (container != null) SetContainer(container);
	//	}
	//	public JObject GetJObject()
	//	{
	//		JObject json = new JObject();
	//		json.Add(GetKey(), container);
	//		return json;
	//	}

	//	public string GetKey()
	//	{
	//		return type.ToString();
	//	}

	//	protected MessageType type = MessageType.EmptyMessage;
	//	protected JObject container = null;
	//	public void SetKey(MessageType type)
	//	{
	//		this.type = type;
	//	}
	//	public void SetContainer(IJObjectUseAbleContainer container)
	//	{
	//		if (container == null) throw new NullReferenceException();
	//		this.container = container.GetJObject();
	//	}
	//	public void SetContainer(JObject container)
	//	{
	//		this.container = container;
	//	}

	//	public JToken GetValue()
	//	{
	//		return container;
	//	}

	//	public void SetJObject(JObject obj)
	//	{
	//		if (obj == null) return;
	//		string key = obj.Properties().ToString();
	//		SetKey((MessageType)Enum.Parse(typeof(MessageType), key));

	//		SetContainer(obj.Value<JObject>());
	//	}
	//}

	public class ConfidenceContainer : IJObjectUseAbleContainer
	{
		private float threashold = 0.55f;
		public ConfidenceContainer(float confidence = 0)
		{
			this.confidence = (threashold <= confidence);
		}

		public JObject GetJObject()
		{
			JObject json = new JObject();
			json.Add(GetKey(), GetValue());
			return json;
		}

		public string GetKey()
		{
			return "confidence";
		}

		public JToken GetValue()
		{
			return confidence;
		}

		public void SetJObject(JObject obj)
		{
			if (obj == null) return;
			confidence = (bool)obj[GetKey()];
		}
		public void SetConfidence(float confidence)
		{
			this.confidence = (threashold <= confidence);
		}
		bool confidence;
	}

    public class IntigerContainer : IJObjectUseAbleContainer
    {
		public IntigerContainer(string key = "proportion", int num = -1)
		{
			Value = num;
			this.key = key;
		}
		public JObject GetJObject()
		{
			JObject json = new JObject() ;
			json.Add(GetKey(), GetValue());
			return json;
		}

		public string GetKey()
		{
			return key;
		}

		public JToken GetValue()
		{
			return Value;
		}

		public void SetJObject(JObject obj)
		{
			try
			{
				Value = (int)obj[GetKey()];
			}
			catch (NullReferenceException ex)
			{
				Value = -1;
			}
		}
		protected string key = null;
		protected int num = -1;
		public int Value
		{
			get { return num; }
			set { if (value  >= 0) num = value; }
		}
	}
    public class MainSubColorContainer : CompoundContainer
    {
		public MainSubColorContainer()
        {
			SetAtribute(main);
			SetAtribute(sub);
        }
        public override string GetKey()
        {
			return "color";
        }

        public override void SetJObject(JObject obj)
        {
			if (obj == null) return;
			JObject value = (JObject)obj[GetKey()];
			foreach (IJObjectUseAbleContainer container in containers)
			{
				container.SetJObject(value);
			}
		}
		public void SetDumi()
        {
			Random r = new Random();
			main.rgbContainer.SetDumi();
			main.SetProportion(r.Next(30, 100));
			sub.rgbContainer.SetDumi();
			sub.SetProportion(r.Next(0, 100 - main.proportionContainer.Value));
		}
		public ProportionRgbContainer main = new ProportionRgbContainer("maincolor");
		public ProportionRgbContainer sub = new ProportionRgbContainer("subcolor");
	}

    public class ProportionRgbContainer : CompoundContainer
    {
		public ProportionRgbContainer(string key="maincolor")
        {
			this.key = key;
			SetAtribute(rgbContainer);
			SetAtribute(proportionContainer);
		}
		public override string GetKey()
        {
			return key;
        }

        public override void SetJObject(JObject obj)
        {
			if (obj == null) return;
			JObject value = (JObject)obj[GetKey()];
			foreach (IJObjectUseAbleContainer container in containers)
			{
				container.SetJObject(value);
			}
		}

		public void SetRGB(char r, char g, char b)
		{
			rgbContainer.SetRGB(r, g, b);
		}
		public void SetProportion(int proportion)
        {
			proportionContainer.Value = proportion;
        }

		public void SetDumi()
        {
			rgbContainer.SetDumi();
		}

		protected string key = null;
		public IntigerContainer proportionContainer = new IntigerContainer();
		public RgbContainer rgbContainer = new RgbContainer();

    }
    public class RgbContainer : IJObjectUseAbleContainer
    {
		public RgbContainer(int r = -1, int g = -1, int b = -1)
		{ 
			if(r+g+b >= 0)
				SetRGB((char)r, (char)g, (char)b);
        }
		public JObject GetJObject()
		{
			//if (boundbox[0]+ boundbox[1]+ boundbox[2]+ boundbox[3] =< 0) throw NullReferenceException;
			JObject json = new JObject();
			json.Add(GetKey(), GetValue());
			return json;
		}
		public void SetJObject(JObject obj)
		{
			JArray value = (JArray)obj[GetKey()];
			this.rgb = value;
		}
		public void SetRGB(char r, char g, char b)
		{
			rgb = new JArray();
			this.rgb.Add(r);
			this.rgb.Add(g);
			this.rgb.Add(b);
		}
		public void SetDumi()
        {
			Random r = new Random();
			SetRGB((char)r.Next(0,255),
				(char)r.Next(0, 255),
				(char)r.Next(0, 255));
        }
		public string GetKey() { return "rgb"; }

		public JToken GetValue()
		{
			return rgb;
		}
		/// <summary>
		/// 0:x_min / 1:x_max / 2:y_min / 3:y_max
		/// </summary>
		protected JArray rgb=null;
	}
    public class BoundBoxContainer : IJObjectUseAbleContainer
	{
		private int threashold = 5;
		public BoundBoxContainer(int x_min = 0, int x_max = 0, int y_min = 0, int y_max = 0)
		{
			if (x_min + x_max + y_max + y_min != 0) SetBoundBox(x_min, x_max, y_min, y_max);
		}
		public JObject GetJObject()
		{
			//if (boundbox[0]+ boundbox[1]+ boundbox[2]+ boundbox[3] =< 0) throw NullReferenceException;
			JObject json = new JObject();
			json.Add(GetKey(), GetValue());
			return json;
		}
		public void SetJObject(JObject obj)
		{
			try
			{
				JArray value = (JArray)obj[GetKey()];
				this.boundbox = value;
			}
			catch { }
		}
		public void SetBoundBox(int x_min, int x_max, int y_min, int y_max)
		{
			boundbox = new JArray();
			this.boundbox.Add(x_min);
			this.boundbox.Add(x_max);
			this.boundbox.Add(y_min);
			this.boundbox.Add(y_max);
		}
		public void SetDumi()
		{
			Random r = new Random();
			SetBoundBox(r.Next(10, 200),
				r.Next(10, 200),
				r.Next(10, 200),
				r.Next(10, 200));
		}
		/// <summary>
		/// 객체가 비었는지 아닌지 판별해줌
		/// 비어있으면 false return
		/// </summary>
		public bool IsEmpty
		{
			get
			{
				int sum = 0;
                if (boundbox != null)
                {
					sum = (int)boundbox[0]
					+ (int)boundbox[1]
					+ (int)boundbox[2]
					+ (int)boundbox[3];
				}
				return (sum < threashold);
			}
		}

		public string GetKey() { return "boundbox"; }

		public JToken GetValue()
		{
			return boundbox;
		}
		/// <summary>
		/// 0:x_min / 1:x_max / 2:y_min / 3:y_max
		/// </summary>
		protected JArray boundbox;
	}
	public class FashionObjectsContainer : CompoundContainer
	{
		public FashionObjectsContainer()
		{
			SetAtribute(top);
			SetAtribute(bottom);
			SetAtribute(overall);
			SetAtribute(outer);
		}
		public override string GetKey()
		{
			return "Fashion";
		}

		public override void SetJObject(JObject obj)
		{
			if (obj == null) return;
			JObject value = (JObject)obj[GetKey()];
			foreach (IJObjectUseAbleContainer container in containers)
			{
				container.SetJObject(value);
			}
		}
		public FashionObjectContainer top = new FashionObjectContainer("Top");
		public FashionObjectContainer bottom = new FashionObjectContainer("Bottom");
		public FashionObjectContainer overall = new FashionObjectContainer("Overall");
		public FashionObjectContainer outer = new FashionObjectContainer("Outer");
	}

	public class EmptyDetectedObjectsContainer : CompoundContainer
    {
		public override string GetKey()
		{
			return "Fashion";
		}

        public override void SetJObject(JObject obj)
        {
            throw new NotImplementedException();
        }
    }

	public class DetectedObjectsContainer : CompoundContainer
    {
		public DetectedObjectsContainer()
        {
			SetAtribute(top);
			SetAtribute(bottom);
			SetAtribute(overall);
			SetAtribute(outer);
		}
        public override string GetKey()
        {
			return "Fashion";
		}

        public override void SetJObject(JObject obj)
        {
			if (obj == null) return;
			try
			{
				JObject value = (JObject)obj[GetKey()];
				foreach (IJObjectUseAbleContainer container in containers)
				{
					container.SetJObject(value);
				}
			}
			catch { }
		}

		public MainCategoryContainer top = new MainCategoryContainer("Top");
		public MainCategoryContainer bottom = new MainCategoryContainer("Bottom");
		public MainCategoryContainer overall = new MainCategoryContainer("Overall");
		public MainCategoryContainer outer = new MainCategoryContainer("Outer");
	}
	public class FashionObjectContainer : CompoundContainer
	{
		public FashionObjectContainer(string key="Top")
		{
			this.key = key;
			SetAtribute(boundbox);
			SetAtribute(color);
			SetAtribute(subcategory);
			SetAtribute(pattern);
			SetAtribute(style);
		}
		public override string GetKey()
		{
			return key;
		}

		public override void SetJObject(JObject obj)
		{
			if (obj == null) return;
			JObject value = (JObject)obj[GetKey()];
			foreach (IJObjectUseAbleContainer container in containers)
			{
				container.SetJObject(value);
			}
		}
		protected string key = null;
		public bool IsEmpty
		{
			get { return boundbox.IsEmpty; }
		}

		public BoundBoxContainer boundbox = new BoundBoxContainer();
		public MainSubColorContainer color = new MainSubColorContainer();
		public SubCategoryContainer subcategory = new SubCategoryContainer();
		public PatternContainer pattern = new PatternContainer();
		public StyleContainer style = new StyleContainer();
	}
	public class MainCategoryContainer : CompoundContainer
    {
		public MainCategoryContainer(string classfication= "maincategory")
        {
			classficationContainer = new ClassficationContainer();
			classficationContainer.SetClassfication(classfication);
			boundboxContainer = new BoundBoxContainer();
            //SetAtribute(classficationContainer);
            SetAtribute(boundboxContainer);
        }
        public override string GetKey()
        {
			return classficationContainer.GetValue().ToString();
        }
		public override void SetJObject(JObject obj)
        {
			JObject value = null;

            try
            {
				value = (JObject)obj[GetKey()];
			}
			catch (NullReferenceException ex){}

			if (value != null)
			{
				foreach (IJObjectUseAbleContainer container in containers)
				{
					container.SetJObject(value);
				}
			}
        }
		public void SetClassfication(string classfication)
        {
			this.classficationContainer.SetClassfication(classfication);
		}
		public void SetBoundbox(int x_min, int x_max, int y_min, int y_max)
        {
			boundboxContainer.SetBoundBox(x_min, x_max, y_min, y_max);

        }
		public void SetBoundbox(JArray box)
		{
			if (box == null) return;
			boundboxContainer.SetBoundBox((int)box[0], (int)box[1], (int)box[2], (int)box[3]);
		}

		public bool IsEmpty
        {
            get { return boundboxContainer.IsEmpty; }
        }

		public void CropImgAndSave(string imgPath, string rootPath)
        {

        }
		public StringContainer cropimgPath = new StringContainer("img_path");
		public ClassficationContainer classficationContainer;
		public BoundBoxContainer boundboxContainer;
    }

    public class SubCategoryContainer : CompoundContainer
    {
		public SubCategoryContainer(float confidence_threshold=0.55f)
        {
			confidenceContainer = new ConfidenceContainer(confidence_threshold);
			SetAtribute(classficationContainer);
			SetAtribute(confidenceContainer);
        }
        public override string GetKey()
        {
			return "subcategory";
        }

        public override void SetJObject(JObject obj)
        {
			JObject value = (JObject)obj[GetKey()];
			foreach (IJObjectUseAbleContainer container in containers)
			{
				container.SetJObject(value);
			}
		}
		//string key;
		public ClassficationContainer classficationContainer = new ClassficationContainer();
		public ConfidenceContainer confidenceContainer;// = new ConfidenceContainer();
	}
	public class PatternContainer : CompoundContainer
	{
		public PatternContainer(float confidence_threshold = 0.55f)
		{
			confidenceContainer = new ConfidenceContainer(confidence_threshold);
			SetAtribute(classficationContainer);
			SetAtribute(confidenceContainer);
		}
		public override string GetKey()
		{
			return "pattern";
		}

		public override void SetJObject(JObject obj)
		{
			JObject value = (JObject)obj[GetKey()];
			foreach (IJObjectUseAbleContainer container in containers)
			{
				container.SetJObject(value);
			}
		}
		//string key;
		public ClassficationContainer classficationContainer = new ClassficationContainer();
		public ConfidenceContainer confidenceContainer;// = new ConfidenceContainer();
	}
	public class StyleContainer : CompoundContainer
	{
		public StyleContainer(float confidence_threshold = 0.55f)
		{
			confidenceContainer = new ConfidenceContainer(confidence_threshold);
			SetAtribute(classficationContainer);
			SetAtribute(confidenceContainer);
		}
		public override string GetKey()
		{
			return "style";
		}

		public override void SetJObject(JObject obj)
		{
			JObject value = (JObject)obj[GetKey()];
			foreach (IJObjectUseAbleContainer container in containers)
			{
				container.SetJObject(value);
			}
		}
		//string key;
		public ClassficationContainer classficationContainer = new ClassficationContainer();
		public ConfidenceContainer confidenceContainer;// = new ConfidenceContainer();
	}
	
	public class ShareWorkPath
	{
		protected ShareWorkPath()
		{
			currentPath.Value = Environment.CurrentDirectory;
			rootPath.Value = System.IO.Directory.GetParent(currentPath.Value).ToString();
			for (int i = 0; i < 4; i++)
			{
				rootPath.Value = System.IO.Directory.GetParent(rootPath.Value).ToString();
			}
			weightPath.Value = rootPath.Value + @"\weight";
			pythonPath.Value = rootPath.Value + @"\python";
			cPath.Value = rootPath.Value + @"\c#";

			string outPath = System.IO.Directory.GetParent(rootPath.Value).ToString();

			workerPath.Value = outPath + @"\WORKER_PATH";

			envList.Add(currentPath);
			envList.Add(rootPath);
			envList.Add(weightPath);
			envList.Add(pythonPath);
			envList.Add(cPath);
			envList.Add(workerPath);
			foreach (var path in envList)
            {
				Console.WriteLine(path.GetKey() + "\t: " + path.Value);
				CreateDirectory(path.Value);
            }
		}

		static public void CreateDirectory(string dirPath)
		{
			if (System.IO.Directory.Exists(dirPath) == false)
			{
				System.IO.Directory.CreateDirectory(dirPath);
				Console.WriteLine("Create Directory : " + dirPath);
			}
		}
		private StringContainer currentPath = new StringContainer("CURRENT_PATH");
		private StringContainer rootPath = new StringContainer("ROOT_PATH");
		private StringContainer weightPath = new StringContainer("WEIGHT_PATH");
		private StringContainer pythonPath = new StringContainer("PYTHON_PATH");
		private StringContainer cPath = new StringContainer("C#_PATH");
		private StringContainer workerPath = new StringContainer("WORKER_PATH");



		private List<StringContainer> envList = new List<StringContainer>();
		public List<StringContainer> GetEnvList()
        {
			return new List<StringContainer>(envList);
        }
		public string WORKER_PATH
        {
            get { return workerPath.Value; }
        }
		public string CURRENT_PATH
        {
            get { return currentPath.Value; }
        }
		public string ROOT_PATH
		{
			get { return rootPath.Value; }
		}
		public string WEIGHT_PATH
		{
			get { return weightPath.Value; }
		}
		public string PYTHON_PATH
		{
			get { return pythonPath.Value; }
		}
		public string C____PATH
        {
			get { return cPath.Value; }
		}
		/// <summary>
		/// singleton pattern
		/// </summary>
		/// <returns></returns>
		public static ShareWorkPath GetInstance()
		{
			return Holder.instance;
		}
		/// <summary>
		/// Lazy Initialization + holder
		/// </summary>
		private static class Holder
		{
			public static ShareWorkPath instance = new ShareWorkPath();
		}
	}
}


