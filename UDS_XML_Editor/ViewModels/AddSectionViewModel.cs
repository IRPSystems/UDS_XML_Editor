
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;
using UDS_XML_Editor.Models;

namespace UDS_XML_Editor.ViewModels
{
	public class AddSectionViewModel : ObservableObject
	{
		#region Properties

		public BaseXmlSection BaseItem { get; set; }
		public BaseXmlSection NewItem { get; set; }

		public ObservableCollection<string> ItemSubTypes { get; set; }
		public string SelecteSubType { get; set; }

		public Visibility FWStepVisibility { get; set; }
		public Visibility SubFuncVisibility { get; set; }
		public Visibility DataIDVisibility { get; set; }
		public Visibility FieldVisibility { get; set; }

		#endregion Properties

		#region Construction

		public AddSectionViewModel()
		{
			LoadedCommand = new RelayCommand(Loaded);
			ItemSubTypes_SelectionChangedCommand = new RelayCommand(ItemSubTypes_SelectionChanged);
			OKCommand = new RelayCommand(OK);

			ItemSubTypes = new ObservableCollection<string>();

			SetVisibilityToCollapse();
		}

		#endregion Construction

		#region Methods

		private void SetVisibilityToCollapse()
		{
			FWStepVisibility = Visibility.Collapsed;
			SubFuncVisibility = Visibility.Collapsed;
			DataIDVisibility = Visibility.Collapsed;
			FieldVisibility = Visibility.Collapsed;
		}

		private void Loaded()
		{
			if (BaseItem == null)
				return;

			ItemSubTypes.Clear();

			SelecteSubType = null;

			if (BaseItem is Customer)
			{
				ItemSubTypes.Add("FWStep");
				SelecteSubType = ItemSubTypes[0];
			}
			else if (BaseItem is Service)
			{
				ItemSubTypes.Add("SubFunc");
				ItemSubTypes.Add("Field");
				ItemSubTypes.Add("DataID");
				SelecteSubType = null;
			}
			else if (BaseItem is SubFunc)
			{
				ItemSubTypes.Add("Field");
				SelecteSubType = ItemSubTypes[0];
			}
			else if (BaseItem is DataID)
			{
				ItemSubTypes.Add("Field");
				SelecteSubType = ItemSubTypes[0];
			}

			ItemSubTypes_SelectionChanged();
		}

		private void ItemSubTypes_SelectionChanged()
		{
			SetVisibilityToCollapse();

			if (SelecteSubType == "FWStep")
			{
				NewItem = new FWStep();
				FWStepVisibility = Visibility.Visible;
			}

			if (SelecteSubType == "SubFunc")
			{
				NewItem = new SubFunc();
				SubFuncVisibility = Visibility.Visible;
			}

			if (SelecteSubType == "DataID")
			{
				NewItem = new DataID();
				DataIDVisibility = Visibility.Visible;
			}

			if (SelecteSubType == "Field")
			{
				NewItem = new Field();
				FieldVisibility = Visibility.Visible;
			}
		}

		private void OK()
		{
			if (NewItem == null)
				return;

			if (BaseItem is Customer customer)
			{
				if (!(NewItem is FWStep))
					return;

				customer.FWStepsList.Add(NewItem);
			}
			else if (BaseItem is Service service)
			{
				AddToNamedSection(
					service,
					$"{NewItem.GetType().Name}s");
			}
			else if (BaseItem is SubFunc subFunc)
			{
				if (!(NewItem is Field))
					return;

				AddToNamedSection(
					subFunc,
					"Fields");
			}
			else if (BaseItem is DataID dataID)
			{
				if (!(NewItem is Field))
					return;

				AddToNamedSection(
					dataID,
					"Fields");
			}
		}

		private void AddToNamedSection(
			BaseXmlSection parent,
			string fieldTypeName)
		{
			NamedSection namedSection = null;

			foreach(NamedSection section in parent.Sections)
			{
				if (section.Name == fieldTypeName)
				{
					namedSection = section;
					break;
				}
			}

			if (namedSection == null)
			{
				namedSection = new NamedSection()
				{
					Name = fieldTypeName,
					Sections = new ObservableCollection<BaseXmlSection>()
				};

				parent.Sections.Add(namedSection);
			}

			namedSection.Sections.Add(NewItem);
		}

		#endregion Methods

		#region Commands

		public RelayCommand LoadedCommand { get; private set; }
		public RelayCommand ItemSubTypes_SelectionChangedCommand { get; private set; }

		public RelayCommand OKCommand { get; private set; }

		#endregion Commands
	}
}
