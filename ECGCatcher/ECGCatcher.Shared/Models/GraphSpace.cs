using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace ECGCatcher.Models
{
    public sealed class GraphSpace : PropertyChangedBase, IGraphSpace
    {
        public GraphSpace() {
            PreviousPoint = new Point(0, ActualHeight * 0.5);
        }

        public readonly double DataSpace = 2;
        public Point PreviousPoint { get; set; }

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
    }
}
