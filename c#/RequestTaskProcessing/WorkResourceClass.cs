
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
		public StringContainer(string key="ip",string str=null)
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
			Value = (string)obj[GetKey()];
        }
		protected string key = null;
		protected string str = null;
		public string Value
        {
            get { return str; }
            set { str = value; }
        }
    }
    public abstract class CompoundContainer :IJObjectUseAbleContainer
    {
		abstract public void SetJObject(JObject obj);
		abstract public string GetKey();
		public JObject GetJObject()
        {
			JObject value = new JObject();
			foreach(IJObjectUseAbleContainer container in containers)
			{
				value.Add(container.GetKey(), container.GetJObject()[container.GetKey()]);
            }
			JObject json = new JObject();
			json.Add(GetKey(), value);
			return json;
        }
		public void SetAtribute(IJObjectUseAbleContainer container)
		{
			containers.Add(container);
		}

        public JToken GetValue()
        {
			JObject value = new JObject();
			foreach (IJObjectUseAbleContainer container in containers)
			{
				value.Add(container.GetKey(), container.GetJObject()[container.GetKey()]);
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
		public ClassficationContainer(string classfication="None")
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
			classfication = (string)obj[GetKey()];
		}
		public void SetClassfication(string classfication)
        {
			this.classfication = classfication;
        }

        public JToken GetValue()
        {
			return GetJObject()[GetKey()];
        }

        protected string classfication;
	}

    public class ReturnedResourceContainer : IJObjectUseAbleContainer
    {
		public ReturnedResourceContainer(MessageType type=MessageType.EmptyMessage, IJObjectUseAbleContainer container=null)
        {
			SetKey(type);
			if(container != null) SetContainer(container);
        }
        public JObject GetJObject()
        {
			JObject json = new JObject();
			json.Add(GetKey(), container);
			return json;
        }

        public string GetKey()
        {
			return type.ToString();
        }

		protected MessageType type = MessageType.EmptyMessage;
		protected JObject container = null;
		public void SetKey(MessageType type)
        {
			this.type = type;
        }
		public void SetContainer(IJObjectUseAbleContainer container)
        {
			if (container == null) throw new NullReferenceException();
			this.container = container.GetJObject();
        }
		public void SetContainer(JObject container)
        {
			this.container = container;
        }

        public JToken GetValue()
        {
			return container;
        }

        public void SetJObject(JObject obj)
        {
			string key = obj.Properties().ToString();
			SetKey( (MessageType)Enum.Parse(typeof(MessageType), key) );

			SetContainer(obj.Value<JObject>());
		}
	}

    public class ConfidenceContainer : IJObjectUseAbleContainer
    {
		private float threashold = 0.55f;
		public ConfidenceContainer(float confidence=0)
        {
			this.confidence = (threashold <= confidence);
        }

        public JObject GetJObject()
        {
			JObject json = new JObject();
			json.Add(GetKey(), confidence);
			return json;
		}

        public string GetKey()
        {
			return "confidence";
        }

        public JToken GetValue()
        {
			return GetJObject()[GetKey()];
		}

        public void SetJObject(JObject obj)
        {
			confidence = (bool)obj[GetKey()];
		}
		public void SetConfidence(float confidence)
        {
			this.confidence = (threashold <= confidence);
        }
		bool confidence;
    }

	public class BoundBoxContainer : IJObjectUseAbleContainer
    {
		private int threashold = 5;
		public BoundBoxContainer(int x_min=0, int x_max=0, int y_min=0, int y_max=0)
        {
			if(x_min+x_max+y_max+y_min != 0) SetBoundBox(x_min, x_max, y_min, y_max);
		}
		public JObject GetJObject()
		{
			//if (boundbox[0]+ boundbox[1]+ boundbox[2]+ boundbox[3] =< 0) throw NullReferenceException;
			JObject json = new JObject();
			json.Add(GetKey(), boundbox);
			return json;
		}
		public void SetJObject(JObject obj)
		{
			 JArray value = (JArray)obj[GetKey()];
			this.boundbox = value;
		}
		public void SetBoundBox(int x_min, int x_max, int y_min, int y_max)
        {
			boundbox = new JArray();
			this.boundbox.Add(x_min);
			this.boundbox.Add(x_max);
			this.boundbox.Add(y_min);
			this.boundbox.Add(y_max);
		}

		/// <summary>
		/// 객체가 비었는지 아닌지 판별해줌
		/// 비어있으면 false return
		/// </summary>
		public bool IsEmpty
        {
            get 
			{
				int sum = (int)boundbox[0]
					+ (int)boundbox[1]
					+ (int)boundbox[2]
					+ (int)boundbox[3];
				return (sum > threashold);
			}
        }

		public string GetKey() { return "boundbox"; }

        public JToken GetValue()
        {
			return GetJObject()[GetKey()];
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
			JObject value = (JObject)obj[GetKey()];
			foreach (IJObjectUseAbleContainer container in containers)
			{
				container.SetJObject(value);
			}
		}
		public MainCategoryContainer top = new MainCategoryContainer("Top");
		public MainCategoryContainer bottom = new MainCategoryContainer("Bottom");
		public MainCategoryContainer overall = new MainCategoryContainer("Overall");
		public MainCategoryContainer outer = new MainCategoryContainer("Outer");
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
			JObject value = (JObject)obj[GetKey()];
			foreach(IJObjectUseAbleContainer container in containers)
            {
				container.SetJObject(value);
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

		ClassficationContainer GetClassficationContainer()
        {
			return classficationContainer;
        }
		BoundBoxContainer GetBoundBoxContainer()
        {
			return boundboxContainer;
        }

		public ClassficationContainer classficationContainer;
		public BoundBoxContainer boundboxContainer;
    }

    public class SubCategoryContainer : CompoundContainer
    {
		public SubCategoryContainer()
        {
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
		public ConfidenceContainer confidenceContainer = new ConfidenceContainer();
	}
    public class WorkResourceClass
	{
		public WorkResourceClass()
		{
		}
	}
}


