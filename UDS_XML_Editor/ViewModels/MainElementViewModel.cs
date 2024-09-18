

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using UDS_XML_Editor.Models;

namespace UDS_XML_Editor.ViewModels
{
    public class MainElementViewModel: ObservableObject
    {
        public ObservableCollection<BaseXmlSection> DataContext { get; set; }

        public MainElementViewModel()
        {
			ExpandAllCommand = new RelayCommand(ExpandAll);
			CollapseAllCommand = new RelayCommand(CollapseAll);
		}

        private void ExpandAll()
        {
			ExpandAll(DataContext);
		}

		private void ExpandAll(ObservableCollection<BaseXmlSection> list)
        {
			foreach (BaseXmlSection section in list)
			{
				section.IsExpanded = true;

				if (section is Customer customer && customer.FWStepsList != null && customer.FWStepsList.Count > 0)
				{
					ExpandAll(customer.FWStepsList);					
				}
				else if (section is Service service && service.Sections != null && service.Sections.Count > 0)
				{
					ExpandAll(service.Sections);
				}
				else if (section is NamedSection namedSection && namedSection.Items != null && namedSection.Items.Count > 0)
				{
					ExpandAll(namedSection.Items);
				}
				else if (section is SubFunc subFunc && subFunc.FieldsList != null && subFunc.FieldsList.Count > 0)
				{
					ExpandAll(subFunc.FieldsList);
				}
				else if (section is DataID dataID && dataID.FieldsList != null && dataID.FieldsList.Count > 0)
				{
					ExpandAll(dataID.FieldsList);
				}
			}
		}


		private void CollapseAll()
        {
			CollapseAll(DataContext);

		}

		private void CollapseAll(ObservableCollection<BaseXmlSection> list)
		{
			foreach (BaseXmlSection section in list)
			{
				section.IsExpanded = false;

				if (section is Customer customer && customer.FWStepsList != null && customer.FWStepsList.Count > 0)
				{
					CollapseAll(customer.FWStepsList);
				}
				else if (section is Service service && service.Sections != null && service.Sections.Count > 0)
				{
					CollapseAll(service.Sections);
				}
				else if (section is NamedSection namedSection && namedSection.Items != null && namedSection.Items.Count > 0)
				{
					CollapseAll(namedSection.Items);
				}
				else if (section is SubFunc subFunc && subFunc.FieldsList != null && subFunc.FieldsList.Count > 0)
				{
					CollapseAll(subFunc.FieldsList);
				}
				else if (section is DataID dataID && dataID.FieldsList != null && dataID.FieldsList.Count > 0)
				{
					CollapseAll(dataID.FieldsList);
				}
			}
		}

		public RelayCommand ExpandAllCommand { get; set; }
		public RelayCommand CollapseAllCommand { get; set; }
	}
}
