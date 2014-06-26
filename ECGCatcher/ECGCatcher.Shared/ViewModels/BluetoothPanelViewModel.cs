using System;
using System.Collections.Generic;
using System.Text;
using Caliburn.Micro;
using System.Collections.ObjectModel;
using BluetoothRfcomm;
using BluetoothRfcommClientWP8;
using Windows.Networking.Proximity;
using System.Threading.Tasks;

namespace ECGCatcher.ViewModels
{
    public enum BluetoothStatus
    {
        NotInitialized,
        Searching,
        Connected,
        Disconnected,
        NoDeviceFound,
        DeviceFound,
        BluetoothOff
    }

    public class BluetoothPanelViewModel : Screen
    {
        private readonly String[] StatusTable = { "Not initialized", "Searching...", "Connected", "Disconnected", "No Device Found", "Device found", "Bluetooth is turned off"};

        public BluetoothPanelViewModel() {
            UpdateStatus(BluetoothStatus.NotInitialized);
            SelectedIndex = -1;

            Devices = new ObservableCollection<BluetoothDevice>();

            //Devices.Add(new BluetoothDevice("dupa", "salata", "tescior"));
            //Devices.Add(new BluetoothDevice("hahha", "jsdijs", "test2"));

            _Client = new ECGBluetoothRfcommClient(BluetoothRfcommGlobal.BluetoothServiceUuid, BluetoothRfcommGlobal.BluetoothServiceDisplayName);
        }

        private IBluetoothClient _Client;

        private String _Status;
        public String Status
        {
            get { return _Status; }
            set
            {
                _Status = value;
                NotifyOfPropertyChange(() => Status);
            }
        }

        private int _SelectedIndex;
        public int SelectedIndex
        {
            get { return _SelectedIndex; }
            set
            {
                _SelectedIndex = value;
                NotifyOfPropertyChange(() => SelectedIndex);
            }
        }

        private void UpdateStatus(BluetoothStatus status)
        {
            Status = StatusTable[(int)status];
        }
        

        /// <summary>
        /// Collection of available devices.
        /// </summary>
        private ObservableCollection<BluetoothDevice> _Devices;
        /// <summary>
        /// Gets or sets the devices collection.
        /// </summary>
        /// <value>
        /// The shapes.
        /// </value>
        public ObservableCollection<BluetoothDevice> Devices
        {
            get { return _Devices; }
            set
            {
                _Devices = value;
                NotifyOfPropertyChange(() => Devices);
            }
        }


        #region EVENTS

        private void GetPairedDeviceButton_Clicked()
        {
            //PeerFinder.Start();
            //var currentBluetoothStatus = await _Client.Initialization();

            //if (currentBluetoothStatus == BluetoothClientReturnCode.Success)
            //{
            //    UpdateStatus(BluetoothStatus.Searching);

            //FillListPairedDevices();

            ////if (await FillListPairedDevices())
            ////    UpdateStatus(BluetoothStatus.DeviceFound);

            //}
            //else if (currentBluetoothStatus == BluetoothClientReturnCode.InitBluetoothOff)
            //    UpdateStatus(BluetoothStatus.BluetoothOff);
        }

        async void FillListPairedDevices()
        {
            List<BluetoothDevice> List = null;
            PeerFinder.Start();

            try
            {

                List = await _Client.GetListPairedDevices();

            }catch( Exception ex)
            {

            }
            if (List != null)
            {
                Devices.Clear();

                if (List.Count == 0)
                {
                    UpdateStatus(BluetoothStatus.NoDeviceFound);
                }
                else
                {
                    // Add peers to list
                    foreach (var device in List)
                    {
                        Devices.Add(device);
                    }
                    //return true;
                }
            }
            //return false;
        }

        private void ConnectButton_Clicked()
        {

        }

        private void DisconnectButton_Clicked()
        {

        }

        #endregion
    }
}
