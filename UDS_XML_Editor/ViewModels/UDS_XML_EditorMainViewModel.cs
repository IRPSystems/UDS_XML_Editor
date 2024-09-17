﻿
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.NetworkInformation;
using System.Reflection;
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

		#endregion Properties

		#region Fields

		private UDS_XML_EditorSettings _uds_XML_EditorSettings;
		private XmlReaderService _xmlReader;

		#endregion Fields

		#region Constructor

		public UDS_XML_EditorMainViewModel()
		{
			Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

			BrowseFilePathCommand = new RelayCommand(BrowseFilePath);
			LoadFileCommand = new RelayCommand(LoadFile);

			_uds_XML_EditorSettings = UDS_XML_EditorSettings.LoadUDS_XML_EditorUserData("UDS_XML_Editor");

			_xmlReader = new XmlReaderService();
		}

		#endregion Constructor

		#region Methods

		

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
			XmlData = _xmlReader.ReadXml(Path);
		}

		#endregion Methods

		#region Commands

		public RelayCommand BrowseFilePathCommand { get; private set; }
		public RelayCommand LoadFileCommand { get; private set; }

		#endregion Commands
	}
}
