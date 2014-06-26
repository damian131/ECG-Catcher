using Caliburn.Micro;
using RGraphics.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECGCatcher.ViewModels
{

    public class BluetoothViewModel : Screen, IDialog
    {
        public BluetoothViewModel()
        {
            BluetoothPanel = new BluetoothPanelViewModel();
        }

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
        

        private Boolean _IsOpen;

        /// <summary>
        /// Determines whether the bluetooth dialog is opened
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is open; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsOpen
        {
            get { return _IsOpen; }
            set
            {
                _IsOpen = value;
                NotifyOfPropertyChange(() => IsOpen);
            }
        }
    }
}
