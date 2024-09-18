
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace UDS_XML_Editor.Converters
{
    public class HideEmptyStringsConverter : IValueConverter
	{
		object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if(value == null)
				return Visibility.Collapsed;

			if(!(value is string str))
				return Visibility.Visible;

			if(string.IsNullOrEmpty(str)) 
				return Visibility.Collapsed;

			return Visibility.Visible;
		}

		object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return string.Empty;
		}
	}
}
