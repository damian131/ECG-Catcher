using ECGCatcher.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace ECGCatcher
{
    public interface IGraphSpace
    {
        double Width { get; set; }
        double Height { get; set; }

        double ActualWidth { get; set; }
        double ActualHeight { get; set; }

        UIElementCollection Children { get; set; }

        Point PreviousPoint { get; set; }
        //int AmountOfDrawnData { get; set; }
        double ZeroLevelCoordinate{get; set;}
        double DataScaleFactor { get; set; }
        Int64 GraphShiftFactor { get; set; }

        /// <summary>
        /// Occurs when [graphspace size changed].
        /// </summary>
        event EventHandler<SizeChangedEventArgs> GraphSpaceSizeChanged;
        /// <summary>
        /// Occurs when [actual size changed].
        /// </summary>
        event EventHandler<SizeChangedEventArgs> ActualSizeChanged;

        void CalculateInitialFactors();
    }
}
