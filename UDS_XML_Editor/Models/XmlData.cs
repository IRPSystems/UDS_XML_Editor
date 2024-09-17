
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace UDS_XML_Editor.Models
{
	public class XmlData: ObservableObject
	{
		public System System { get; set; }

		public ObservableCollection<Customer> CustomersList { get; set; }

		public ObservableCollection<Service> RequestsList { get; set; }

		public ObservableCollection<Service> ResponsesList { get; set; }
	}

	public class System
	{
		public string LogDir { get; set; }
	}

	public class Customer : ObservableObject
	{
		public string Name { get; set; }
		public int Rate { get; set; }
		public int TxID { get; set; }
		public int RxID { get; set; }
		public int BCID { get; set; }

		public ObservableCollection<FWStep> FWStepsList { get; set; }
	}

	public class FWStep
	{
		public string Name { get; set; }
	}

	public class Service
	{
		public string Name { get; set; }
		public int ID { get; set; }
		public string DataType { get; set; }

		public ObservableCollection<NamedSection> Sections { get; set; }
	}

	public class NamedSection
	{
		public string Name { get; set; }
		public ObservableCollection<object> Items { get; set; }
	}

	public class SubFunc
	{
		public string Name { get; set; }
		public int ID { get; set; }
		public string DataType { get; set; }

		public ObservableCollection<Field> FieldsList { get; set; }
	}

	public class Field
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

	public class DataID
	{
		public string Name { get; set; }
		public int ID { get; set; }

		public ObservableCollection<Field> FieldsList { get; set; }
	}
}
