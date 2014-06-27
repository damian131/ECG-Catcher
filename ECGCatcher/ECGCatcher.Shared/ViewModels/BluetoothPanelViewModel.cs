using System;
using System.Collections.Generic;
using System.Text;
using Caliburn.Micro;
using System.Collections.ObjectModel;
using Windows.Networking.Proximity;
using System.Threading.Tasks;
using ECGCatcher.Models.Bluetooth;

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
        BluetoothOff,
        //    "Access to the device is denied because the application was not granted access",
        //    NotifyType.StatusMessage);
        NoAccess,
        //    "The Chat service is not advertising the Service Name attribute (attribute id=0x100). " +
        //    "Please verify that you are running the BluetoothRfcommChat server.",
        WrongService,
        //    "The Chat service is using an unexpected format for the Service Name attribute. " +
        //    "Please verify that you are running the BluetoothRfcommChat server.",
        UnexpectedDataFormat,
        UnexpectedConnectionError
    }

    public class BluetoothPanelViewModel : Screen
    {
        private readonly String[] StatusTable = { 
                                                    #region STATUS STATEMENTS
                                                    "Not initialized", 
                                                    "Searching...", 
                                                    "Connected", 
                                                    "Disconnected", 
                                                    "No Device Found", 
                                                    "Device found", 
                                                    "Bluetooth is turned off", 
                                                    "No application access", 
                                                    "Wrong service", 
                                                    "Unexpected data format", 
                                                    "Unexpected connection error"
                                                    #endregion
                                                };

        private IBluetoothService _Client;

        public BluetoothPanelViewModel() {
            UpdateStatus(BluetoothStatus.NotInitialized);
            SelectedIndex = -1;

            Devices = new ObservableCollection<BluetoothDevice>();

            //Devices.Add(new BluetoothDevice("dupa", "salata", "tescior"));
            //Devices.Add(new BluetoothDevice("hahha", "jsdijs", "test2"));

            _Client = new ECGBluetoothService(BluetoothSpecification.RfcommServiceUuid);

            ConnectEnabled = false;
            DisconnectEnabled = false;

            _Client.Disconnect();
        }

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

        private bool _ConnectEnabled;
        public bool ConnectEnabled
        {
            get { return _ConnectEnabled; }
            set
            {
                _ConnectEnabled = value;
                NotifyOfPropertyChange(() => ConnectEnabled);
            }
        }

        private bool _DisconnectEnabled;
        public bool DisconnectEnabled
        {
            get { return _DisconnectEnabled; }
            set
            {
                _DisconnectEnabled = value;

                if (_DisconnectEnabled)
                    IsConnected = true;
                else
                    IsConnected = false;

                NotifyOfPropertyChange(() => DisconnectEnabled);
            }
        }

        public Boolean IsConnected { get; private set; }

        #region EVENT HANDLERS

        async private void GetPairedDeviceButton_Clicked()
        {
                UpdateStatus(BluetoothStatus.Searching);

                bool ifFoundedAnyService = await FillListPairedDevices();

                if (ifFoundedAnyService)
                {
                    UpdateStatus(BluetoothStatus.DeviceFound);
                    ConnectEnabled = true;
                }
                else
                {
                    UpdateStatus(BluetoothStatus.NoDeviceFound);
                    ConnectEnabled = false;
                }
                DisconnectEnabled = false;
        }

        async private void ConnectButton_Clicked()
        {
            var currentStatus = await _Client.Connect(SelectedIndex);
            UpdateStatus( currentStatus );

            if (currentStatus == BluetoothStatus.Connected)
            {
                ConnectEnabled = false;
                DisconnectEnabled = true;
            }

        }

        private void DisconnectButton_Clicked()
        {
            _Client.Disconnect();
            UpdateStatus(BluetoothStatus.Disconnected);

            ConnectEnabled = true;
            DisconnectEnabled = false;
        }

        #endregion // EVENT HANDLERS

        #region METHODS

        private void UpdateStatus(BluetoothStatus status)
        {
            Status = StatusTable[(int)status];
        }

        async Task<bool> FillListPairedDevices()
        {
            List<string> List = null;

            try
            {
                List = await _Client.GetListPairedDevices();

            }
            catch (Exception ex)
            {

            }
            if (List != null)
            {
                Devices.Clear();

                if (List.Count == 0)
                {
                    return false;
                }
                else
                {
                    // Add peers to list
                    foreach (var deviceName in List)
                    {
                        Devices.Add(new BluetoothDevice("", deviceName, deviceName));
                    }
                    return true;
                }
            }
            return false;
        }

        #endregion //METHODS
    }
}
