

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;
using UDS_XML_Editor.Models;
using UDS_XML_Editor.Views;

namespace UDS_XML_Editor.ViewModels
{
    public class MainElementViewModel: ObservableObject
    {
		#region Properties

		public ObservableCollection<BaseXmlSection> DataContext { get; set; }

		public string SearchName { get; set; }
		public string SearchID { get; set; }

		#endregion Properties

		#region Constructor

		public MainElementViewModel()
        {
			ExpandAllCommand = new RelayCommand(ExpandAll);
			CollapseAllCommand = new RelayCommand(CollapseAll);
			SearchCommand = new RelayCommand(Search);
			DeleteSearchCommand = new RelayCommand(DeleteSearch);
			AddSubSectionCommand = new RelayCommand<BaseXmlSection>(AddSubSection);
			AddSectionCommand = new RelayCommand(AddSection);
		}

		#endregion Constructor

		#region Methods

		#region Expand/Collapse

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
				else if (section is NamedSection namedSection && namedSection.Sections != null && namedSection.Sections.Count > 0)
				{
					ExpandAll(namedSection.Sections);
				}
				else if (section is SubFunc subFunc && subFunc.Sections != null && subFunc.Sections.Count > 0)
				{
					ExpandAll(subFunc.Sections);
				}
				else if (section is DataID dataID && dataID.Sections != null && dataID.Sections.Count > 0)
				{
					ExpandAll(dataID.Sections);
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
				else if (section is NamedSection namedSection && namedSection.Sections != null && namedSection.Sections.Count > 0)
				{
					CollapseAll(namedSection.Sections);
				}
				else if (section is SubFunc subFunc && subFunc.Sections != null && subFunc.Sections.Count > 0)
				{
					CollapseAll(subFunc.Sections);
				}
				else if (section is DataID dataID && dataID.Sections != null && dataID.Sections.Count > 0)
				{
					CollapseAll(dataID.Sections);
				}
			}
		}

		#endregion Expand/Collapse

		private void Search()
		{
			SetAllVisible(DataContext);
			Search(DataContext);
		}

		private Visibility Search(ObservableCollection<BaseXmlSection> itemsList)
		{


			Visibility visibilityTop = Visibility.Collapsed;
			foreach (BaseXmlSection item in itemsList)
			{
				Visibility visibility = Visibility.Collapsed;

				bool isMathcing = false;
				if (string.IsNullOrEmpty(SearchName) == false)
				{
					isMathcing |= (SearchName.ToLower() == item.Name.ToLower());					
				}

				if (string.IsNullOrEmpty(SearchID) == false && string.IsNullOrEmpty(item.ID) == false)
				{
					isMathcing |= (SearchID.ToLower() == item.ID.ToLower());
				}

				if (isMathcing)
				{
					visibility = Visibility.Visible;
				}

				if (visibility == Visibility.Visible)
				{
					item.Visibility = visibility;
					if (visibility == Visibility.Visible)
						visibilityTop = visibility;
					continue;
				}

				Visibility vis = Visibility.Collapsed;
				if (item is Customer customer)
				{
					vis = Search(customer.FWStepsList);
					if (vis == Visibility.Visible)
						visibility = vis;
				}
				else
				{
					if (item.Sections != null)
					{
						vis = Search(item.Sections);
						if (vis == Visibility.Visible)
							visibility = vis;
					}
				}

				item.Visibility = visibility;

				if (visibility == Visibility.Visible)
					visibilityTop = visibility;
			}

			return visibilityTop;
		}

		private void SetAllVisible(ObservableCollection<BaseXmlSection> itemsList)
		{
			if (itemsList == null)
				return;


			foreach (BaseXmlSection item in itemsList)
			{
				item.Visibility = Visibility.Visible;

				if (item is Customer customer)
				{
					SetAllVisible(customer.FWStepsList);
				}
				else
				{
					SetAllVisible(item.Sections);
				}
			}
		}

		private void DeleteSearch()
		{
			SetAllVisible(DataContext);
		}

		private void AddSubSection(BaseXmlSection baseItem)
		{
			AddSectionViewModel addSectionViewModel = new AddSectionViewModel()
			{
				BaseItem = baseItem,
				ItemsList = null,
			};

			AddSectionView addSectionView = new AddSectionView()
			{
				DataContext = addSectionViewModel,
			};
			addSectionView.Show();
		}

		private void AddSection()
		{
			AddSectionViewModel addSectionViewModel = new AddSectionViewModel()
			{
				BaseItem = null,
				ItemsList = DataContext,
			};

			AddSectionView addSectionView = new AddSectionView()
			{
				DataContext = addSectionViewModel,
			};
			addSectionView.Show();
		}

		#endregion Methods

		#region Commands

		public RelayCommand ExpandAllCommand { get; private set; }
		public RelayCommand CollapseAllCommand { get; private set; }
		public RelayCommand SearchCommand { get; private set; }
		public RelayCommand DeleteSearchCommand { get; private set; }
		public RelayCommand<BaseXmlSection> AddSubSectionCommand { get; private set; }
		public RelayCommand AddSectionCommand { get; private set; }

		#endregion Commands
	}
}
