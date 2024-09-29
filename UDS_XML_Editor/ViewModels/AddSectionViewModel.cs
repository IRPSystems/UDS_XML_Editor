
using CommunityToolkit.Mvvm.ComponentModel;
using UDS_XML_Editor.Models;

namespace UDS_XML_Editor.ViewModels
{
	public class AddSectionViewModel: ObservableObject
	{
		public BaseXmlSection BaseItem { get; set; }
	}
}
