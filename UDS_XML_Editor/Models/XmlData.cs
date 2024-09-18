
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
		public int Rate { get; set; }
		public int TxID { get; set; }
		public int RxID { get; set; }
		public int BCID { get; set; }

		public ObservableCollection<BaseXmlSection> FWStepsList { get; set; }
	}

	public class FWStep: BaseXmlSection
	{
		public string Name { get; set; }
	}

	public class Service: BaseXmlSection
	{
		public string Name { get; set; }
		public int ID { get; set; }
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
		public int ID { get; set; }
		public string DataType { get; set; }

		public ObservableCollection<BaseXmlSection> FieldsList { get; set; }
	}

	public class Field: BaseXmlSection
	{
		public string Name { get; set; }
		public string DataType { get; set; }
		public string PyDataType { get; set; }
		public double DefVal { get; set; }
		public double MinVal { get; set; }
		public double MaxVal { get; set; }
		public string Unit { get; set; }
		public double Offset { get; set; }
		public double Res { get; set; }
	}

	public class DataID: BaseXmlSection
	{
		public string Name { get; set; }
		public int ID { get; set; }

		public ObservableCollection<BaseXmlSection> FieldsList { get; set; }
	}
}
