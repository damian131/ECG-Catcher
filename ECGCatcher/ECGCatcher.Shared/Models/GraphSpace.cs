using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace ECGCatcher.Models
{
    public sealed class GraphSpace : PropertyChangedBase, IGraphSpace
    {
        public GraphSpace() {}

        private const int _MaxData = 5000;

        public Point PreviousPoint { get; set; }
        public double ZeroLevelCoordinate { get; set; }
        public double DataScaleFactor { get; set; }
        public Int64 GraphShiftFactor { get; set; } // used to shifting a graph
        //public int AmountOfDrawnData { get; set; }

        private double _Width;
        public double Width {
            get { return _Width; }
            set
            {
                _Width = value;
                NotifyOfPropertyChange(() => Width);
            }
        }

        private double _Height;
        public double Height
        {
            get { return _Height; }
            set
            {
                _Height = value;
                NotifyOfPropertyChange(() => Height);
            }
        }

        private double _ActualWidth;
        public double ActualWidth
        {
            get { return _ActualWidth; }
            set
            {
                _ActualWidth = value;
                NotifyOfPropertyChange(() => ActualWidth);
            }
        }

        private double _ActualHeight;
        public double ActualHeight
        {
            get { return _ActualHeight; }
            set
            {
                _ActualHeight = value;
                NotifyOfPropertyChange(() => ActualHeight);
            }
        }

        private UIElementCollection _Children;
        public UIElementCollection Children
        {
            get { return _Children; }
            set
            {
                _Children = value;
                NotifyOfPropertyChange(() => Children);
            }
        }

        private Transform _RenderTransform;
        public Transform RenderTransform
        {
            get { return _RenderTransform; }
            set
            {
                _RenderTransform = value;
                NotifyOfPropertyChange(() => RenderTransform);
            }
        }
        

        /// <summary>
        /// Occurs when [graphspace size changed].
        /// </summary>
        public event EventHandler<SizeChangedEventArgs> GraphSpaceSizeChanged;
        /// <summary>
        /// Raises the <see cr="E:WorkspaceSizeChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="SizeChangedEventArgs"/> instance containing the event data.</param>
        private void OnWorkspaceSizeChanged(SizeChangedEventArgs e)
        {
            if (GraphSpaceSizeChanged != null)
                GraphSpaceSizeChanged(this, e);
        }

        /// <summary>
        /// Occurs when [actual size changed].
        /// </summary>
        public event EventHandler<SizeChangedEventArgs> ActualSizeChanged;
        /// <summary>
        /// Raises the <see cref="E:ActualSizeChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="SizeChangedEventArgs"/> instance containing the event data.</param>
        private void OnActualSizeChanged(SizeChangedEventArgs e)
        {
            if (ActualSizeChanged != null)
                ActualSizeChanged(this, e);
        }

        public void CalculateInitialFactors()
        {
            ZeroLevelCoordinate = Height * 0.5;
            DataScaleFactor = CalculateScale(_MaxData);
            PreviousPoint = new Point(0, ZeroLevelCoordinate); // TODO: should be width
            GraphShiftFactor = 0;
        }

        private double CalculateScale(int limit)
        {
            double scale = 0.9;

            while (scale * limit > ZeroLevelCoordinate)
                scale -= 0.01;

            return scale;
        }
    }
}
