
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace UDS_XML_Editor.Models
{
	public abstract class BaseXmlSection: ObservableObject
	{
		public bool IsExpanded { get; set; }
	}

	public class XmlData: BaseXmlSection
	{
		public System System { get; set; }

		public ObservableCollection<BaseXmlSection> CustomersList { get; set; }

		public ObservableCollection<BaseXmlSection> RequestsList { get; set; }

		public ObservableCollection<BaseXmlSection> ResponsesList { get; set; }
	}

	public class System: BaseXmlSection
	{
		public string LogDir { get; set; }
	}

	public class Customer : BaseXmlSection
	{
		public string Name { get; set; }
		public string Rate { get; set; }
		public string TxID { get; set; }
		public string RxID { get; set; }
		public string BCID { get; set; }

		public ObservableCollection<BaseXmlSection> FWStepsList { get; set; }
	}

	public class FWStep: BaseXmlSection
	{
		public string Name { get; set; }
	}

	public class Service: BaseXmlSection
	{
		public string Name { get; set; }
		public string ID { get; set; }
		public string DataType { get; set; }

		public ObservableCollection<BaseXmlSection> Sections { get; set; }
	}

	public class NamedSection: BaseXmlSection
	{
		public string Name { get; set; }
		public ObservableCollection<BaseXmlSection> Items { get; set; }
	}

	public class SubFunc: BaseXmlSection
	{
		public string Name { get; set; }
		public string ID { get; set; }
		public string DataType { get; set; }

		//public ObservableCollection<BaseXmlSection> FieldsList { get; set; }
		public ObservableCollection<BaseXmlSection> Sections { get; set; }
	}

	public class Field: BaseXmlSection
	{
		public string Name { get; set; }
		public string DataType { get; set; }
		public string PyDataType { get; set; }
		public string DefVal { get; set; }
		public string MinVal { get; set; }
		public string MaxVal { get; set; }
		public string Unit { get; set; }
		public string Offset { get; set; }
		public string Res { get; set; }
	}

	public class DataID: BaseXmlSection
	{
		public string Name { get; set; }
		public string ID { get; set; }

		public ObservableCollection<BaseXmlSection> FieldsList { get; set; }
	}
}
