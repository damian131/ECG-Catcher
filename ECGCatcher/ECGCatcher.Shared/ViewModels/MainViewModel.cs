using Caliburn.Micro;
using ECGCatcher.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Foundation;

namespace ECGCatcher.ViewModels
{
    public class MainViewModel : Screen
    {
        public MainViewModel(IGraphSpace graphSpace) {
            GraphSpace = graphSpace;
        }

        private IGraphSpace _GraphSpace;

        public IGraphSpace GraphSpace
        {
            get { return _GraphSpace; }
            set
            {
                _GraphSpace = value;
                NotifyOfPropertyChange(() => GraphSpace);
            }
        }

        private void PlayButton_Clicked()
        {
            var gd = new GraphDrawer(GraphSpace);
            gd.DrawData( new Point(150.3,240.2) );
        }
        
    }
}
