
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using UDS_XML_Editor.Models;
using UDS_XML_Editor.Services;

namespace UDS_XML_Editor.ViewModels
{
	public class UDS_XML_EditorMainViewModel: ObservableObject
	{
		#region Properties

		public string Version { get; set; }
		public string Path { get; set; }

		public XmlData XmlData { get; set; }
		public DocingViewModel Docking { get; set; }

		#endregion Properties

		#region Fields

		private UDS_XML_EditorSettings _uds_XML_EditorSettings;
		private XmlReaderService _xmlReader;
		private XmlWriterService _xmlWriter;


		#endregion Fields

		#region Constructor

		public UDS_XML_EditorMainViewModel()
		{
			Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

			LoadFileCommand = new RelayCommand(LoadFile);
			SaveFileCommand = new RelayCommand(SaveFile);
			CleanFileCommand = new RelayCommand(CleanFile);
			ClosingCommand = new RelayCommand<CancelEventArgs>(Closing);

			_uds_XML_EditorSettings = UDS_XML_EditorSettings.LoadUDS_XML_EditorUserData("UDS_XML_Editor");

			_xmlReader = new XmlReaderService();
			_xmlWriter = new XmlWriterService();

			Docking = new DocingViewModel();
		}

		#endregion Constructor

		#region Methods

		private void Closing(CancelEventArgs e)
		{
			UDS_XML_EditorSettings.SaveEvvaUserData(
				"UDS_XML_Editor",
				_uds_XML_EditorSettings);

			if(Docking != null)
				Docking.Close();
		}

		private void LoadFile()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			if (string.IsNullOrEmpty(_uds_XML_EditorSettings.LoadXmlDir) &&
				Directory.Exists(_uds_XML_EditorSettings.LoadXmlDir))
			{
				openFileDialog.InitialDirectory = _uds_XML_EditorSettings.LoadXmlDir;
			}

			bool? result = openFileDialog.ShowDialog();
			if (result != true)
				return;

			Path = openFileDialog.FileName;
			_uds_XML_EditorSettings.LoadXmlDir = System.IO.Path.GetDirectoryName(Path);

			XmlData = _xmlReader.ReadXml(Path);

			Docking.CreateWindows(
				XmlData.System,
				new MainElementViewModel() { DataContext = XmlData.CustomersList },
				new MainElementViewModel() { DataContext = XmlData.RequestsList },
				new MainElementViewModel() { DataContext = XmlData.ResponsesList });
		}

		private void SaveFile()
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			if (string.IsNullOrEmpty(_uds_XML_EditorSettings.SaveXmlDir) &&
				Directory.Exists(_uds_XML_EditorSettings.SaveXmlDir))
			{
				saveFileDialog.InitialDirectory = _uds_XML_EditorSettings.LoadXmlDir;
			}

			bool? result = saveFileDialog.ShowDialog();
			if (result != true)
				return;

			_uds_XML_EditorSettings.LoadXmlDir = System.IO.Path.GetDirectoryName(saveFileDialog.FileName);

			_xmlWriter.WriteXml(saveFileDialog.FileName, XmlData);
		}

		private void CleanFile()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			if (string.IsNullOrEmpty(_uds_XML_EditorSettings.LoadXmlDir) &&
				Directory.Exists(_uds_XML_EditorSettings.LoadXmlDir))
			{
				openFileDialog.InitialDirectory = _uds_XML_EditorSettings.LoadXmlDir;
			}

			bool? result = openFileDialog.ShowDialog();
			if (result != true)
				return;

			string fileData = null;
			using(StreamReader reader = new StreamReader(openFileDialog.FileName))
			{
				fileData = reader.ReadToEnd();
			}

			int index = 0;
			while (index < fileData.Length)
			{
				int startFixIndex = fileData.IndexOf("> </", index);
				if (startFixIndex < 0)
					break;

				index = startFixIndex + 1;

				int closeBracket = fileData.IndexOf(">", index);
				if (closeBracket < 0)
					break;

				string temp = fileData.Substring(startFixIndex, closeBracket - (startFixIndex - 1));
				fileData = fileData.Replace(temp, "/>");
			}

			index = 0;
			while (index < fileData.Length)
			{
				int startFixIndex = fileData.IndexOf("<!--", index);
				if (startFixIndex < 0)
					break;

				index = startFixIndex + 1;

				int closeBracket = fileData.IndexOf("-->", index);
				if (closeBracket < 0)
					break;

				
				index = closeBracket + 1;

				closeBracket = fileData.IndexOf("\r\n", index);
				if (closeBracket < 0)
					break;

				closeBracket += "\r\n".Length;

				fileData = fileData.Remove(startFixIndex, closeBracket - startFixIndex);
			}





			string fixedFileName = System.IO.Path.GetFileName(openFileDialog.FileName);
			string extension = System.IO.Path.GetExtension(fixedFileName);
			string directory = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
			fixedFileName = fixedFileName.Replace(extension, string.Empty);
			fixedFileName += "_AutoFixed";

			string path = System.IO.Path.Combine(directory, fixedFileName + extension);

			using (StreamWriter streamWriter = new StreamWriter(path))
			{
				streamWriter.Write(fileData);
			}
		}

		#endregion Methods

		#region Commands

		public RelayCommand LoadFileCommand { get; private set; }
		public RelayCommand SaveFileCommand { get; private set; }

		public RelayCommand CleanFileCommand { get; private set; }

		public RelayCommand<CancelEventArgs> ClosingCommand { get; private set; }

		#endregion Commands
	}
}
