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

        /// <summary>
        /// Converts the specified value. Color to Brush.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="language">The language.</param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var colorValue = (Color)value;
            return new SolidColorBrush(colorValue);
        }

        /// <summary>
        /// Converts the back. Brush to color.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="language">The language.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
