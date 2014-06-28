using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Media;

namespace ECGCatcher.Models
{
    public class GraphShifter : PropertyChangedBase
    {
        private IGraphSpace _GraphSpace;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphShifter"/> class.
        /// </summary>
        /// <param name="graphSpace">The graph space.</param>
        public GraphShifter( IGraphSpace graphSpace){
            this._GraphSpace = graphSpace;
            this.TranslateX = 0;
        }

        private double _TranslateX;
        /// <summary>
        /// Gets or sets the translate x. Specified graph shift from the base point.
        /// </summary>
        /// <value>
        /// The translate x.
        /// </value>
        public double TranslateX
        {
            get { return _TranslateX; }
            set
            {
                _TranslateX = value;
                NotifyOfPropertyChange(() => TranslateX);
            }
        }

        /// <summary>
        /// Shifts the graph by specified data offset.
        /// </summary>
        /// <param name="DataOffset">The data offset.</param>
        public void ShiftGraph(int DataOffset)
        {
            TranslateX -= DataOffset;
        }

        /// <summary>
        /// Restores the starting shift. Backs to the zero level.
        /// </summary>
        public void RestoreStartingShift()
        {
            TranslateX = 0;
        }
    }
}
