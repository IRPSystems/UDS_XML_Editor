using Controls.ViewModels;
using Syncfusion.Windows.Tools.Controls;
using System.IO;
using System.Windows.Controls;

namespace UDS_XML_Editor.ViewModels
{
	public class DocingViewModel: DocingBaseViewModel
	{
		#region Fields

		private ContentControl _appSystem;
		private ContentControl _appCustormers;
		private ContentControl _appRequests;
		private ContentControl _appResponses;

		#endregion Fields

		#region Constructor

		public DocingViewModel(
			) :
			base("DockingMain")
		{

			CreateWindows(
				);
		}

		#endregion Constructor

		#region Methods

		private void CreateWindows(
			)
		{
			DockFill = true;


			
		}



		

		public void RestorWindowsLayout()
		{
			string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			path = Path.Combine(path, "UDS_XML_Editor");
			path = Path.Combine(path, "Default.txt");
			if (System.IO.File.Exists(path))
				LoadDockState(path);
		}

#endregion Methods
	}
}
