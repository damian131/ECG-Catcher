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

        public GraphShifter( IGraphSpace graphSpace){
            this._GraphSpace = graphSpace;
            this.TranslateX = 0;
        }

        private double _TranslateX;
        public double TranslateX
        {
            get { return _TranslateX; }
            set
            {
                _TranslateX = value;
                NotifyOfPropertyChange(() => TranslateX);
            }
        }

        public void ShiftGraph(int DataOffset)
        {
            TranslateX -= DataOffset;
        }

        public void RestoreStartingShift()
        {
            TranslateX = 0;
        }
    }
}
