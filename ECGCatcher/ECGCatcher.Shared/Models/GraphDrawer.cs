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

        #region CONSTANT FACTORS
        private readonly SolidColorBrush _StrokeColor = new SolidColorBrush(Colors.Red);
        private readonly double _StrokeThickness = 3;
        private const int _DataOffset = 2;
        #endregion // CONSTANT FACTORS

        public GraphDrawer(IGraphSpace graphSpace){
            _GraphSpace = graphSpace;
        }

        public void DrawData(double coordinate)
        {
            double y = _GraphSpace.ZeroLevelCoordinate - coordinate*_GraphSpace.DataScaleFactor; // scaled coordinate
            Point p = new Point(_GraphSpace.PreviousPoint.X + _DataOffset, y);

            Line line = new Line()
            {
                #region LINE PROPERTIES
                X1 = _GraphSpace.PreviousPoint.X,
                Y1 = _GraphSpace.PreviousPoint.Y,
                X2 = p.X,
                Y2 = p.Y,
                StrokeThickness = _StrokeThickness,
                Stroke = _StrokeColor
                #endregion //LINE PROPERTIES
            };

            //var graphShifter = new GraphShifter( _GraphSpace ); // TODO: fix graph shifting
            //graphShifter.ShiftGraph( _DataOffset);

            _GraphSpace.Children.Add(line);

            _GraphSpace.PreviousPoint = new Point(p.X, p.Y);
            //_GraphSpace.AmountOfDrawnData++;
        }
        
    }
}
