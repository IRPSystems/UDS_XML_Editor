
using Newtonsoft.Json;
using System.IO;

namespace UDS_XML_Editor.Models
{
	public class UDS_XML_EditorSettings
	{
		public bool IsLightTheme { get; set; }
		public string XMLDir { get; set; }

		public UDS_XML_EditorSettings()
		{
			IsLightTheme = false;
		}

		private static UDS_XML_EditorSettings GetDefaultSettings()
		{
			UDS_XML_EditorSettings UDS_XML_EditorSettings = new UDS_XML_EditorSettings();
			UDS_XML_EditorSettings.IsLightTheme = false;


			return UDS_XML_EditorSettings;
		}

		public static UDS_XML_EditorSettings LoadUDS_XML_EditorUserData(string dirName)
		{
			UDS_XML_EditorSettings UDS_XML_EditorSettings = null;

			string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			path = Path.Combine(path, dirName);
			if (Directory.Exists(path) == false)
			{
				UDS_XML_EditorSettings = GetDefaultSettings();
				return UDS_XML_EditorSettings;
			}
			path = Path.Combine(path, "UDS_XML_EditorSettings.json");
			if (File.Exists(path) == false)
			{
				UDS_XML_EditorSettings = GetDefaultSettings();
				return UDS_XML_EditorSettings;
			}


			string jsonString = File.ReadAllText(path);
			JsonSerializerSettings settings = new JsonSerializerSettings();
			settings.Formatting = Formatting.Indented;
			settings.TypeNameHandling = TypeNameHandling.All;
			UDS_XML_EditorSettings = JsonConvert.DeserializeObject(jsonString, settings) as UDS_XML_EditorSettings;

			

			return UDS_XML_EditorSettings;
		}



		public static void SaveEvvaUserData(
			string dirName,
			UDS_XML_EditorSettings UDS_XML_EditorSettings)
		{
			string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			path = Path.Combine(path, dirName);
			if (Directory.Exists(path) == false)
				return;
			path = Path.Combine(path, "UDS_XML_EditorSettings.json");

			UDS_XML_EditorSettings.IsLightTheme = !UDS_XML_EditorSettings.IsLightTheme;

			JsonSerializerSettings settings = new JsonSerializerSettings();
			settings.Formatting = Formatting.Indented;
			settings.TypeNameHandling = TypeNameHandling.All;
			var sz = JsonConvert.SerializeObject(UDS_XML_EditorSettings, settings);
			File.WriteAllText(path, sz);
		}
	}
}
