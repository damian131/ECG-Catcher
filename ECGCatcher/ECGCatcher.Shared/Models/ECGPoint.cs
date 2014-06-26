using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace ECGCatcher.Models
{
    public class ECGPoint
    {

        private Color _StrokeColor = Colors.Red;
        public Color StrokeColor
        {
            get { return _StrokeColor; }
            set
            {
                _StrokeColor = value;
            }
        }


        private double _X1;
        public double X1
        {
            get { return _X1; }
            set
            {
                _X1 = value;
            }
        }

        private double _X2;
        public double X2
        {
            get { return _X2; }
            set
            {
                _X2 = value;
            }
        }

        private double _Y1;
        public double Y1
        {
            get { return _Y1; }
            set
            {
                _Y1 = value;
            }
        }

        private double _Y2;
        public double Y2
        {
            get { return _Y2; }
            set
            {
                _Y2 = value;
            }
        }

        private double _StrokeThickness;
        public double StrokeThickness
        {
            get { return _StrokeThickness; }
            set
            {
                _StrokeThickness = value;
            }
        }
        

    }
}
