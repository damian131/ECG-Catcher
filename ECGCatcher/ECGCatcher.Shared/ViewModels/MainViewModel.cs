using Caliburn.Micro;
using ECGCatcher.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Media;

namespace ECGCatcher.ViewModels
{
    public class MainViewModel : Screen
    {
        public MainViewModel(IGraphSpace graphSpace) {
            GraphSpace = graphSpace;
            Drawer = new GraphDrawer(GraphSpace);
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

        public GraphDrawer Drawer { get; private set; }


        #region EVENTS

        private void PlayButton_Clicked()
        {
            List<int> testData = new List<int>();
            var rand = new Random();

            for (int i = 0; i < 1000; ++i)
                testData.Add(rand.Next(-5000, 5000));

            if (Drawer.CurrentStatus == GraphDrawerStatus.Stopped)
            {
                _GraphSpace.CalculateInitialFactors();
                Drawer.StartDrawingGraph(testData);
            }
            else if (Drawer.CurrentStatus ==  GraphDrawerStatus.Paused)
                Drawer.ContinueDrawingGraph();
        }

        private void RightShiftButton_Clicked()
        {
            if (Drawer.CurrentStatus != GraphDrawerStatus.Stopped)
                Drawer.Shifter.ShiftGraph(Drawer.DataOffset);
        }

        private void LeftShiftButton_Clicked()
        {
            if (Drawer.CurrentStatus != GraphDrawerStatus.Stopped)
                Drawer.Shifter.ShiftGraph(-Drawer.DataOffset);
        }

        private void PauseButton_Clicked()
        {
            if (Drawer.CurrentStatus == GraphDrawerStatus.Started)
                Drawer.PauseDrawingGraph();
        }

        private void ButtonConnect_Clicked() // TODO: Callisto dialog for win8 app
        {

        }

        private void ButtonDisconnect_Clicked()
        {

        }

        private void ButtonListPairedDevices_Clicked()
        {

        }

        #endregion //EVENTS

    }
}
