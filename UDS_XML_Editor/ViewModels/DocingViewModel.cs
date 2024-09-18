using Controls.ViewModels;
using Syncfusion.Windows.Tools.Controls;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using UDS_XML_Editor.Models;
using UDS_XML_Editor.Views;
using static System.Net.Mime.MediaTypeNames;

namespace UDS_XML_Editor.ViewModels
{
	public class DocingViewModel : DocingBaseViewModel
	{
		#region Fields

		//private ContentControl _appSystem;
		private ContentControl _appCustormers;
		private ContentControl _appRequests;
		private ContentControl _appResponses;

		#endregion Fields

		#region Constructor

		public DocingViewModel() :
			base("DockingMain")
		{
			DockFill = true;
		}

		#endregion Constructor

		#region Methods

		public void CreateWindows(
			Models.System system,
			MainElementViewModel customersList,
			MainElementViewModel requestsList,
			MainElementViewModel responsesList)
		{
			Children.Clear();

			SystemView systemView = new SystemView() { DataContext = system };
			CreateTabbedWindow(systemView, "System", string.Empty, out _appResponses);

			MainElementView customersListView = new MainElementView() { DataContext = customersList };
			CreateTabbedWindow(customersListView, "Customers", "System", out _appCustormers);

			MainElementView requestsListView = new MainElementView() { DataContext = requestsList };
			CreateTabbedWindow(requestsListView, "Requests", "System", out _appRequests);

			MainElementView responsesListView = new MainElementView() { DataContext = responsesList };
			CreateTabbedWindow(responsesListView, "Responses", "System", out _appResponses);

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
