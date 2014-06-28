using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace ECGCatcher.Models
{
    /// <summary>
    /// Specifies the ECG point with binded properties.
    /// </summary>
    public class ECGPoint
    {

        private Color _StrokeColor = Colors.Red;
        /// <summary>
        /// Gets or sets the color of the stroke.
        /// </summary>
        /// <value>
        /// The color of the stroke.
        /// </value>
        public Color StrokeColor
        {
            get { return _StrokeColor; }
            set
            {
                _StrokeColor = value;
            }
        }


        private double _X1;
        /// <summary>
        /// Gets or sets the x1.
        /// </summary>
        /// <value>
        /// The x1.
        /// </value>
        public double X1
        {
            get { return _X1; }
            set
            {
                _X1 = value;
            }
        }

        private double _X2;
        /// <summary>
        /// Gets or sets the x2.
        /// </summary>
        /// <value>
        /// The x2.
        /// </value>
        public double X2
        {
            get { return _X2; }
            set
            {
                _X2 = value;
            }
        }

        private double _Y1;
        /// <summary>
        /// Gets or sets the y1.
        /// </summary>
        /// <value>
        /// The y1.
        /// </value>
        public double Y1
        {
            get { return _Y1; }
            set
            {
                _Y1 = value;
            }
        }

        private double _Y2;
        /// <summary>
        /// Gets or sets the y2.
        /// </summary>
        /// <value>
        /// The y2.
        /// </value>
        public double Y2
        {
            get { return _Y2; }
            set
            {
                _Y2 = value;
            }
        }

        private double _StrokeThickness;
        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        /// <value>
        /// The stroke thickness.
        /// </value>
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
