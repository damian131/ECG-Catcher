using Caliburn.Micro;
using ECGCatcher.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using ECGCatcher.Common;

namespace ECGCatcher.ViewModels
{
    public class MainViewModel : Screen
    {
        public MainViewModel(IGraphSpace graphSpace) {
            GraphSpace = graphSpace;
            Drawer = new GraphDrawer(GraphSpace);

#if WINDOWS_PHONE_APP
            BluetoothPanel = new BluetoothPanelViewModel();
#endif
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

#if WINDOWS_PHONE_APP

        private BluetoothPanelViewModel _BluetoothPanel;
        public BluetoothPanelViewModel BluetoothPanel
        {
            get { return _BluetoothPanel; }
            set
            {
                _BluetoothPanel = value;
                NotifyOfPropertyChange(() => BluetoothPanel);
            }
        }
        
#endif

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

        private void ButtonConnect_Clicked()
        {

        }

        private void ButtonDisconnect_Clicked()
        {

        }

        private void ButtonListPairedDevices_Clicked()
        {

        }

#if WINDOWS_APP
        async private void BluetoothButton_Clicked()
        {
            var bluetoothDialog = IoC.Get<BluetoothViewModel>();
            await bluetoothDialog.ShowDialogAsync();
        }
#endif

        #endregion //EVENTS

    }
}
