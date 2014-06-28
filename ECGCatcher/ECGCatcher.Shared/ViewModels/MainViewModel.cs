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


// TODO:
// 1. Finger manipulation
// 2. Performance improvement
// 3. Bluetooth connection test
// 4. Remove IoC.Get induction , maybe Ninject integration?
// 5. Run-time points displayed on the Canvas => performance
// 6. Dynamic graph scaling with orientation changing - bug
// 7. DataReader.LoadAsync test

namespace ECGCatcher.ViewModels
{
    public class MainViewModel : Screen
    {
        public GraphDrawer Drawer { get; private set; }

        public MainViewModel(IGraphSpace graphSpace) {
            GraphSpace = graphSpace;
            Drawer = new GraphDrawer(GraphSpace);

#if WINDOWS_PHONE_APP
            BluetoothPanel = new BluetoothPanelViewModel();
#endif
        }

        #region BINDED PROPERTIES

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

        #endregion //BINDED PROPERTIES

        #region EVENT HANDLERS

        private void PlayButton_Clicked()
        {

            if (Drawer.CurrentStatus == GraphDrawerStatus.Stopped)
            {
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
