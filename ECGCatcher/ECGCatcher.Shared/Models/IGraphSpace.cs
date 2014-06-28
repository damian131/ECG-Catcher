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
        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        double Width { get; set; }
        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        double Height { get; set; }

        /// <summary>
        /// Gets or sets the actual width.
        /// </summary>
        /// <value>
        /// The actual width.
        /// </value>
        double ActualWidth { get; set; }
        /// <summary>
        /// Gets or sets the actual height.
        /// </summary>
        /// <value>
        /// The actual height.
        /// </value>
        double ActualHeight { get; set; }

        /// <summary>
        /// Gets or sets the children of the graph space.
        /// </summary>
        /// <value>
        /// The children of the graph space.
        /// </value>
        UIElementCollection Children { get; set; }

        /// <summary>
        /// Gets or sets the previous point. Specifies the last displayed point on the graph space.
        /// </summary>
        /// <value>
        /// The previous point.
        /// </value>
        Point PreviousPoint { get; set; }
        /// <summary>
        /// Gets or sets the zero level coordinate. Specifies middle of the graph space.
        /// </summary>
        /// <value>
        /// The zero level coordinate.
        /// </value>
        double ZeroLevelCoordinate{get; set;}
        /// <summary>
        /// Gets or sets the data scale factor. Used to specified scale data level.
        /// </summary>
        /// <value>
        /// The data scale factor.
        /// </value>
        double DataScaleFactor { get; set; }
        /// <summary>
        /// Gets or sets the graph shift factor. Used to specified shift according to base point.
        /// </summary>
        /// <value>
        /// The graph shift factor.
        /// </value>
        Int64 GraphShiftFactor { get; set; }

        /// <summary>
        /// Occurs when [graphspace size changed].
        /// </summary>
        event EventHandler<SizeChangedEventArgs> GraphSpaceSizeChanged;
        /// <summary>
        /// Occurs when [actual size changed].
        /// </summary>
        event EventHandler<SizeChangedEventArgs> ActualSizeChanged;

        /// <summary>
        /// Calculates the initial factors before drawing graph.
        /// </summary>
        void CalculateInitialFactors();
    }
}
