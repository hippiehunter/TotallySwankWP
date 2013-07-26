using System;
using System.Windows;
using System.Windows.Data;

namespace TotallySwankWP.Converters
{
  public sealed class VisibilityConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo language)
    {
      return ((bool)value) ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo language)
    {
      return value.Equals(Visibility.Visible);
    }
  }
}
