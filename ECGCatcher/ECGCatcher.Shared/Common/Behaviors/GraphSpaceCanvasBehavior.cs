using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ECGCatcher.Common
{
    public class GraphSpaceCanvasBehavior : DependencyObject, IBehavior
    {
        public DependencyObject AssociatedObject { get; private set; }
        private Canvas GraphSpaceCanvas { get; set; }

        public IGraphSpace GraphSpace
        {
            get { return (IGraphSpace)GetValue(GraphSpaceProperty); }
            set { SetValue(GraphSpaceProperty, value); }
        }

        public static readonly DependencyProperty GraphSpaceProperty =
            DependencyProperty.Register("GraphSpace", typeof(object), typeof(GraphSpaceCanvasBehavior), new PropertyMetadata(null, GraphSpaceChanged));

        private static void GraphSpaceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GraphSpaceCanvasBehavior gcb = d as GraphSpaceCanvasBehavior;
            if (e.OldValue != null)
            {
                (e.OldValue as IGraphSpace).ActualSizeChanged -= gcb.ActualSizeChanged;
                (e.OldValue as IGraphSpace).GraphSpaceSizeChanged -= gcb.GraphSpaceSizeChanged;
            }

            if (e.NewValue != null)
            {
                (e.NewValue as IGraphSpace).ActualSizeChanged += gcb.ActualSizeChanged; 
                (e.NewValue as IGraphSpace).GraphSpaceSizeChanged += gcb.GraphSpaceSizeChanged;
                (e.NewValue as IGraphSpace).Children = gcb.GraphSpaceCanvas.Children;
                //(e.NewValue as IGraphSpace).RenderTransform = gcb.GraphSpaceCanvas.RenderTransform;

                
            }
        }

        /// <summary>
        /// Refreshes UI - called when Image's size has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Models.SizeChangedEventArgs"/> instance containing the event data.</param>
        private void ActualSizeChanged(object sender, Models.SizeChangedEventArgs e)
        {
            GraphSpaceCanvas.Width = e.NewWidth;
            GraphSpaceCanvas.Height = e.NewHeight;

            GraphSpaceCanvas.InvalidateMeasure();
        }

        /// <summary>
        /// Updates UI when size of the graphspace has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Models.SizeChangedEventArgs"/> instance containing the event data.</param>
        private void GraphSpaceSizeChanged(object sender, Models.SizeChangedEventArgs e)
        {
            GraphSpaceCanvas.Width = e.NewWidth;
            GraphSpaceCanvas.Height = e.NewHeight;

            GraphSpaceCanvas.InvalidateMeasure();
        }

        public void Attach(DependencyObject associatedObject)
        {
            AssociatedObject = associatedObject;
            GraphSpaceCanvas = AssociatedObject as Canvas;
            GraphSpaceCanvas.SizeChanged += GraphSpaceCanvas_SizeChanged;
        }

        void GraphSpaceCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if(GraphSpace != null)
            {
                GraphSpace.Width = e.NewSize.Width;
                GraphSpace.Height = e.NewSize.Height;
            }
        }

        public void Detach()
        {
            AssociatedObject = null;
            GraphSpaceCanvas = null;
        }
    }
}
