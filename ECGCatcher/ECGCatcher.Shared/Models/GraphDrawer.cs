using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace ECGCatcher.Models
{
    public class GraphDrawer
    {
        private IGraphSpace _GraphSpace;

        public GraphDrawer(IGraphSpace graphSpace){
            _GraphSpace = graphSpace;
        }

        public void DrawData(Point p)
        {
            Line line = new Line()
            {
                X1 = _GraphSpace.PreviousPoint.X,
                Y1 = _GraphSpace.PreviousPoint.Y,
                X2 = p.X,
                Y2 = p.Y,
                StrokeThickness = 10,
                Stroke = new SolidColorBrush( Colors.Blue )
            };

            _GraphSpace.Children.Add(line);

            _GraphSpace.PreviousPoint = new Point(p.X, p.Y);
        }
        
    }
}
