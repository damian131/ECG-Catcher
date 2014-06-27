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


        #region EVENT HANDLERS

        async private void PlayButton_Clicked()
        {
            //var rand = new Random();

            //for (int i = 0; i < 1000; ++i)
            //    Drawer.GraphData.Enqueue(rand.Next(-5000, 5000));

            if (Drawer.CurrentStatus == GraphDrawerStatus.Stopped)
            {
                #region SIMULATION
                var s = new Simulation("ecgca154.txt");
                String strData = await s.Run();

                foreach (var str in strData.Split(' '))
                {
                    if (str != String.Empty)
                    {
                        double parsed;
                        if (!double.TryParse(str, out parsed))
                        {
                            throw new InvalidCastException();
                        }
                        Drawer.GraphData.Enqueue(parsed);
                    }
                }
                #endregion // SIMULATION

                _GraphSpace.CalculateInitialFactors();
                Drawer.StartDrawingGraph();
            }
            else if (Drawer.CurrentStatus ==  GraphDrawerStatus.Paused)
                Drawer.ContinueDrawingGraph();
        }

        private void RightShiftButton_Clicked()
        {
            if (Drawer.CurrentStatus != GraphDrawerStatus.Stopped)
                Drawer.Shifter.ShiftGraph(Drawer.DataOffset * Drawer.ShiftOffsetMultiplier);
        }

        private void LeftShiftButton_Clicked()
        {
            if (Drawer.CurrentStatus != GraphDrawerStatus.Stopped)
                Drawer.Shifter.ShiftGraph(-Drawer.DataOffset * Drawer.ShiftOffsetMultiplier );
        }

        private void PauseButton_Clicked()
        {
            if (Drawer.CurrentStatus == GraphDrawerStatus.Started)
                Drawer.PauseDrawingGraph();
        }

#if WINDOWS_APP
        async private void BluetoothButton_Clicked()
        {
            var bluetoothDialog = IoC.Get<BluetoothViewModel>();
            await bluetoothDialog.ShowDialogAsync();
        }
#endif

        #endregion //EVENT HANDLERS

    }
}
