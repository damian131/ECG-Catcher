using System;
using System.Collections.Generic;
using System.Text;
using Caliburn.Micro;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI;

namespace ECGCatcher.Common
{
    public class ColorToBrushConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var colorValue = (Color)value;
            return new SolidColorBrush(colorValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
