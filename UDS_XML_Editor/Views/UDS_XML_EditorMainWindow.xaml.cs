using MahApps.Metro.Controls;
using UDS_XML_Editor.ViewModels;

namespace UDS_XML_Editor
{
	/// <summary>
	/// Interaction logic for UDS_XML_EditorMainWindow.xaml
	/// </summary>
	public partial class UDS_XML_EditorMainWindow : MetroWindow
	{
		public UDS_XML_EditorMainWindow()
		{
			InitializeComponent();

			DataContext = new UDS_XML_EditorMainViewModel();
		}
	}
}