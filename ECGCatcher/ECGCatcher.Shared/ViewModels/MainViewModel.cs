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
            _GraphSpace.SetBasePreviousPoint(0);

            var gd = new GraphDrawer(GraphSpace);
            var rand = new Random();
            for (int i = 0; i < 1000; ++i)
                gd.DrawData(rand.Next(-200, 200));
        }
        
    }
}
