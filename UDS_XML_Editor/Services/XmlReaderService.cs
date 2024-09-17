

using System.Collections.ObjectModel;
using System.IO;
using System.Net.NetworkInformation;
using System.Xml;
using UDS_XML_Editor.Models;

namespace UDS_XML_Editor.Services
{
    public class XmlReaderService
    {
		public XmlData ReadXml(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
				return null;

			if (File.Exists(path) == false)
				return null;

			string fileData = File.ReadAllText(path);

			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(fileData);
			XmlElement root = xmlDocument.DocumentElement;


			XmlData xmlData = new XmlData();

			HandleSystemElement(
				xmlData,
				root);

			HandleCustomersElement(
				xmlData,
				root);

			HandleServicesElement(
				xmlData,
				root,
				"Requests");

			HandleServicesElement(
				xmlData,
				root,
				"Responses");

			return xmlData;
		}

		private void HandleSystemElement(
			XmlData xmlData,
			XmlElement root)
		{
			XmlNodeList nodes = root.SelectNodes("//System");
			if (nodes.Count == 0)
				return;

			XmlNode system = nodes[0];
			if (system == null)
				return;

			if (system.Attributes.Count == 0)
				return;

			xmlData.System = new Models.System();
			XmlAttribute attribute = system.Attributes[0];
			xmlData.System.LogDir = attribute.Value;
		}

		private void HandleCustomersElement(
			XmlData xmlData,
			XmlElement root)
		{
			XmlNodeList nodesList = root.SelectNodes("//CustomerList/Cursomer");
			if (nodesList.Count == 0)
				return;

			xmlData.CustomersList = new ObservableCollection<Customer>();
			foreach (XmlNode node in nodesList)
			{
				Customer customer = new Customer();
				if (node.Attributes.Count == 0)
					continue;

				foreach (XmlAttribute attribute in node.Attributes)
				{
					switch (attribute.Name)
					{
						case "Name":
							customer.Name = attribute.Value;
							break;
						case "Rate":
							int rate;
							int.TryParse(attribute.Value, out rate);
							customer.Rate = rate;
							break;
						case "TxID":
							int txID;
							int.TryParse(attribute.Value, out txID);
							customer.TxID = txID;
							break;
						case "RxID":
							int rxID;
							int.TryParse(attribute.Value, out rxID);
							customer.RxID = rxID;
							break;
						case "BCID":
							int bcID;
							int.TryParse(attribute.Value, out bcID);
							customer.BCID = bcID;
							break;
					}
				}

				customer.FWStepsList = new ObservableCollection<FWStep>();
				foreach (XmlNode customerNode in node.ChildNodes)
				{
					if (customerNode.Name != "FWStep")
						continue;

					if (customerNode.Attributes.Count == 0)
						continue;

					if (customerNode.Attributes[0].Name != "Name")
						continue;

					FWStep fWStep = new FWStep()
					{
						Name = customerNode.Attributes[0].Value,
					};

					customer.FWStepsList.Add(fWStep);
				}

				xmlData.CustomersList.Add(customer);
			}


		}

		#region Services

		private void HandleServicesElement(
			XmlData xmlData,
			XmlElement root,
			string topElement)
		{
			XmlNodeList nodesList = root.SelectNodes($"//{topElement}/Service");
			if (nodesList.Count == 0)
				return;

			ObservableCollection<Service> servicesList = new ObservableCollection<Service>();

			foreach (XmlNode node in nodesList)
			{
				if (node.Name != "Service")
					continue;

				GetService(
					servicesList,
					node);
			}

			if (topElement == "Requests")
				xmlData.RequestsList = servicesList;
			else if (topElement == "Responses")
				xmlData.ResponsesList = servicesList;
		}

		private void GetService(
			ObservableCollection<Service> servicesList,
			XmlNode nodeService)
		{
			Service service = new Service();
			servicesList.Add(service);

			GetServiceAttributes(
				nodeService,
				service);

			if (nodeService.ChildNodes.Count == 0)
				return;

			NamedSection subFuncs = new NamedSection() { Name = "SubFuncs", Items = new ObservableCollection<object>() };
			NamedSection fields = new NamedSection() { Name = "Fields", Items = new ObservableCollection<object>() };
			NamedSection dataIDs = new NamedSection() { Name = "DataIDs", Items = new ObservableCollection<object>() };
			

			foreach (XmlNode node in nodeService.ChildNodes)
			{
				switch (node.Name)
				{
					case "SubFunc":
						GetSubFunc(
							node,
							subFuncs);
						break;

					case "Field":
						Field field = GetField(node);
						fields.Items.Add(field);
						break;

					case "DataID":
						GetDataID(
							node,
							dataIDs);
						break;
				}
			}

			service.Sections = new ObservableCollection<NamedSection>();

			if (subFuncs.Items.Count > 0) 
				service.Sections.Add(subFuncs);
			if (fields.Items.Count > 0)
				service.Sections.Add(fields);
			if (dataIDs.Items.Count > 0)
				service.Sections.Add(dataIDs);
		}

		private void GetServiceAttributes(
			XmlNode nodeService,
			Service service)
		{
			if (nodeService.Attributes.Count == 0)
				return;

			foreach (XmlAttribute attribute in nodeService.Attributes)
			{
				switch (attribute.Name)
				{
					case "Name":
						service.Name = attribute.Value;
						break;
					case "ID":
						int id;
						int.TryParse(attribute.Value, out id);
						service.ID = id;
						break;
					case "DataType":
						service.DataType = attribute.Value;
						break;
				}
			}
		}

		#region Sub func

		private void GetSubFunc(
			XmlNode nodeSubFunc,
			NamedSection namedSection)
		{
			SubFunc subFunc = new SubFunc();
			namedSection.Items.Add(subFunc);

			GetSubFuncAttributes(
				nodeSubFunc,
				subFunc);

			subFunc.FieldsList = new ObservableCollection<Field>();
			foreach (XmlNode node in nodeSubFunc.ChildNodes)
			{
				switch (node.Name)
				{
					case "Field":
						Field field = GetField(node);
						subFunc.FieldsList.Add(field);
						break;
				}
			}

		}

		private void GetSubFuncAttributes(
			XmlNode nodeSubFunc,
			SubFunc subFunc)
		{
			if (nodeSubFunc.Attributes.Count == 0)
				return;

			foreach (XmlAttribute attribute in nodeSubFunc.Attributes)
			{
				switch (attribute.Name)
				{
					case "Name":
						subFunc.Name = attribute.Value;
						break;
					case "ID":
						int id;
						int.TryParse(attribute.Value, out id);
						subFunc.ID = id;
						break;
					case "DataType":
						subFunc.DataType = attribute.Value;
						break;
				}
			}
		}

		#endregion Sub func

		#region DataID

		private void GetDataID(
			XmlNode nodeDataID,
			NamedSection namedSection)
		{
			DataID dataID = new DataID();
			namedSection.Items.Add(dataID);

			GetDataIDAttributes(
				nodeDataID,
				dataID);

			dataID.FieldsList = new ObservableCollection<Field>();
			foreach (XmlNode node in nodeDataID.ChildNodes)
			{
				switch (node.Name)
				{
					case "Field":
						Field field = GetField(node);
						dataID.FieldsList.Add(field);
						break;
				}
			}

		}

		private void GetDataIDAttributes(
			XmlNode nodeDataID,
			DataID dataID)
		{
			if (nodeDataID.Attributes.Count == 0)
				return;

			foreach (XmlAttribute attribute in nodeDataID.Attributes)
			{
				switch (attribute.Name)
				{
					case "Name":
						dataID.Name = attribute.Value;
						break;
					case "ID":
						int id;
						int.TryParse(attribute.Value, out id);
						dataID.ID = id;
						break;
				}
			}
		}

		#endregion Sub func

		private Field GetField(
			XmlNode nodeField)
		{
			Field field = new Field();
			if (nodeField.Attributes.Count == 0)
				return field;

			foreach (XmlAttribute attribute in nodeField.Attributes)
			{
				switch (attribute.Name)
				{
					case "Name":
						field.Name = attribute.Value;
						break;
					case "DataTyp":
						field.DataType = attribute.Value;
						break;
					case "PyDataType":
						field.PyDataType = attribute.Value;
						break;
					case "DefVal":
						double defVal;
						double.TryParse(attribute.Value, out defVal);
						field.DefVal = defVal;
						break;
					case "MinVal":
						double minVal;
						double.TryParse(attribute.Value, out minVal);
						field.MinVal = minVal;
						break;
					case "MaxVal":
						double maxVal;
						double.TryParse(attribute.Value, out maxVal);
						field.MaxVal = maxVal;
						break;
					case "Unit":
						field.Unit = attribute.Value;
						break;
					case "Offset":
						double offset;
						double.TryParse(attribute.Value, out offset);
						field.Offset = offset;
						break;
					case "Res":
						double res;
						double.TryParse(attribute.Value, out res);
						field.Res = res;
						break;
				}
			}

			return field;
		}

		#endregion Services
	}
}
