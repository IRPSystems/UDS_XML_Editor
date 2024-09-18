
using System.Collections.ObjectModel;
using System.Xml;
using UDS_XML_Editor.Models;

namespace UDS_XML_Editor.Services
{
    public class XmlWriterService
    {
		public void WriteXml(string path, XmlData xmlData)
		{
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			settings.IndentChars = "    ";
			XmlWriter writer = XmlWriter.Create(path, settings);

			writer.WriteStartDocument();

			writer.WriteStartElement("UDS");

			WriteSystem(xmlData, writer);
			WriteCustomers(xmlData, writer);

			writer.WriteStartElement("Requests");
			WriteServices(xmlData.RequestsList, writer);
			writer.WriteEndElement();

			writer.WriteStartElement("Responses");
			WriteServices(xmlData.ResponsesList, writer);
			writer.WriteEndElement();

			writer.WriteEndElement();
			writer.WriteEndDocument();
			writer.Flush();
			writer.Close();
		}

		private void WriteSystem(
			XmlData xmlData,
			XmlWriter writer)
		{
			writer.WriteStartElement("System");
			writer.WriteAttributeString("LogDir", xmlData.System.LogDir);
			writer.WriteEndElement();
		}

		private void WriteCustomers(
			XmlData xmlData,
			XmlWriter writer)
		{
			writer.WriteStartElement("CustomerList");

			foreach (Customer customer in xmlData.CustomersList)
			{
				WriteSingleCustomers(
					customer,
					writer);
			}


			writer.WriteEndElement();
		}

		private void WriteSingleCustomers(
			Customer customer,
			XmlWriter writer)
		{
			writer.WriteStartElement("Cursomer");

			writer.WriteAttributeString("Name", customer.Name);
			writer.WriteAttributeString("Rate", customer.Rate);
			writer.WriteAttributeString("TxID", customer.TxID);
			writer.WriteAttributeString("RxID", customer.RxID);
			writer.WriteAttributeString("BCID", customer.BCID);

			foreach(FWStep fwStep in customer.FWStepsList)
			{
				writer.WriteStartElement("FWStep");
				writer.WriteAttributeString("Name", fwStep.Name);
				writer.WriteEndElement();
			}


			writer.WriteEndElement();
		}

		private void WriteServices(
			ObservableCollection<BaseXmlSection> servicesList,
			XmlWriter writer)
		{
			foreach (BaseXmlSection section in servicesList)
			{
				if(!(section is Service service))
					continue;

				WriteSingleServices(service, writer);
			}
		}

		private void WriteSingleServices(
			Service service,
			XmlWriter writer)
		{
			writer.WriteStartElement("Service");

			writer.WriteAttributeString("Name", service.Name);

			if (string.IsNullOrEmpty(service.ID) == false)
				writer.WriteAttributeString("ID", service.ID);


			if (string.IsNullOrEmpty(service.DataType) == false)
				writer.WriteAttributeString("DataType", service.DataType);

			if (service.Sections == null || service.Sections.Count == 0)
			{
				writer.WriteEndElement();
				return;
			}

			foreach (BaseXmlSection section in service.Sections)
			{
				if(!(section is NamedSection namedSection))
					continue;

				WriteSingleNamedSection(namedSection, writer);
			}

			writer.WriteEndElement();
		}

		private void WriteSingleNamedSection(
			NamedSection namedSection,
			XmlWriter writer)
		{
			foreach (BaseXmlSection item in namedSection.Items)
			{
				if(item is SubFunc subFunc)
					WriteSubFunc(subFunc, writer);
				else if (item is Field field)
					WriteField(field, writer);
				else if (item is DataID dataID)
					WriteDataID(dataID, writer);
			}
		}

		private void WriteSubFunc(
			SubFunc subFunc,
			XmlWriter writer)
		{
			writer.WriteStartElement("SubFunc");

			writer.WriteAttributeString("Name", subFunc.Name);

			if (string.IsNullOrEmpty(subFunc.ID) == false)
				writer.WriteAttributeString("ID", subFunc.ID);


			if (string.IsNullOrEmpty(subFunc.DataType) == false)
				writer.WriteAttributeString("DataType", subFunc.DataType);

			if (subFunc.Sections == null || subFunc.Sections.Count == 0)
			{
				writer.WriteEndElement();
				return;
			}

			foreach (BaseXmlSection section in subFunc.Sections)
			{
				if (!(section is NamedSection namedSection))
					continue;

				WriteSingleNamedSection(namedSection, writer);
			}


			writer.WriteEndElement();
		}

		private void WriteField(
			Field field,
			XmlWriter writer)
		{
			writer.WriteStartElement("Field");

			writer.WriteAttributeString("Name", field.Name);

			if (string.IsNullOrEmpty(field.DataType) == false)
				writer.WriteAttributeString("DataType", field.DataType);

			if (string.IsNullOrEmpty(field.PyDataType) == false)
				writer.WriteAttributeString("PyDataType", field.PyDataType);

			if (string.IsNullOrEmpty(field.DefVal) == false)
				writer.WriteAttributeString("DefVal", field.DefVal);

			if (string.IsNullOrEmpty(field.MinVal) == false)
				writer.WriteAttributeString("MinVal", field.MinVal);

			if (string.IsNullOrEmpty(field.MaxVal) == false)
				writer.WriteAttributeString("MaxVal", field.MaxVal);

			if (string.IsNullOrEmpty(field.Unit) == false)
				writer.WriteAttributeString("Unit", field.Unit);

			if (string.IsNullOrEmpty(field.Offset) == false)
				writer.WriteAttributeString("Offset", field.Offset);

			if (string.IsNullOrEmpty(field.Res) == false)
				writer.WriteAttributeString("Res", field.Res);

			writer.WriteEndElement();
		}

		private void WriteDataID(
			DataID dataID,
			XmlWriter writer)
		{
			writer.WriteStartElement("DataID");

			writer.WriteAttributeString("Name", dataID.Name);

			if (string.IsNullOrEmpty(dataID.ID) == false)
				writer.WriteAttributeString("ID", dataID.ID);

			if (dataID.FieldsList == null || dataID.FieldsList.Count == 0)
			{
				writer.WriteEndElement();
				return;
			}

			foreach (BaseXmlSection section in dataID.FieldsList)
			{
				if (!(section is Field field))
					continue;

				WriteField(field, writer);
			}


			writer.WriteEndElement();
		}
	}
}
