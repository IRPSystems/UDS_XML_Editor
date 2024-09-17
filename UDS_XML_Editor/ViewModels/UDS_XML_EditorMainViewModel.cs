
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml;
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


		#endregion Fields

		#region Constructor

		public UDS_XML_EditorMainViewModel()
		{
			Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

			LoadFileCommand = new RelayCommand(LoadFile);
			SaveFileCommand = new RelayCommand(SaveFile);
			ClosingCommand = new RelayCommand<CancelEventArgs>(Closing);

			_uds_XML_EditorSettings = UDS_XML_EditorSettings.LoadUDS_XML_EditorUserData("UDS_XML_Editor");

			_xmlReader = new XmlReaderService();

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

		private void BrowseFilePath()
		{
			
			OpenFileDialog openFileDialog = new OpenFileDialog();
			if (string.IsNullOrEmpty(_uds_XML_EditorSettings.XMLDir) &&
				Directory.Exists(_uds_XML_EditorSettings.XMLDir))
			{
				openFileDialog.InitialDirectory = _uds_XML_EditorSettings.XMLDir;
			}
			
			bool? result = openFileDialog.ShowDialog();
			if (result != true)
				return;

			Path = openFileDialog.FileName;
			_uds_XML_EditorSettings.XMLDir = System.IO.Path.GetDirectoryName(Path);
		}

		private void LoadFile()
		{
			BrowseFilePath();

			XmlData = _xmlReader.ReadXml(Path);

			Docking.CreateWindows(
				XmlData.System,
				XmlData.CustomersList,
				XmlData.RequestsList,
				XmlData.ResponsesList);
		}

		private void SaveFile()
		{

		}

		#endregion Methods

		#region Commands

		public RelayCommand LoadFileCommand { get; private set; }
		public RelayCommand SaveFileCommand { get; private set; }

		public RelayCommand<CancelEventArgs> ClosingCommand { get; private set; }

		#endregion Commands
	}
}
