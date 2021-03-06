﻿using System;
using System.Collections.Generic;
using System.Text;
using Caliburn.Micro;
using System.Collections.ObjectModel;
using Windows.Networking.Proximity;
using System.Threading.Tasks;
using ECGCatcher.Models.Bluetooth;

namespace ECGCatcher.ViewModels
{
    /// <summary>
    /// Describses current bluetooth connection status.
    /// </summary>
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
        NoAccess,
        //    "The ECG service is not advertising the Service Name attribute (attribute id=0x100). " +
        //    "Please verify that you are running the BluetoothRfcommChat server.",
        WrongService,
        //    "The ECG service is using an unexpected format for the Service Name attribute. " +
        //    "Please verify that you are running the BluetoothRfcommECG server.",
        UnexpectedDataFormat,
        UnexpectedConnectionError
    }

    /// <summary>
    /// Class responsible for bluetooth panel items and communication with them.
    /// </summary>
    public class BluetoothPanelViewModel : Screen
    {
        /// <summary>
        /// The status table - includes messages associated with Bluetooth status.
        /// </summary>
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
        /// <summary>
        /// Gets a value indicating whether the service is connected.
        /// </summary>
        /// <value>
        /// <c>true</c> if this service is connected; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsConnected { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BluetoothPanelViewModel"/> class.
        /// </summary>
        public BluetoothPanelViewModel()
        {
            UpdateStatus(BluetoothStatus.NotInitialized);
            SelectedIndex = -1;

            Devices = new ObservableCollection<BluetoothDevice>();

            ConnectEnabled = false;
            DisconnectEnabled = false;

            IsCheckedSimulation = false;
        }

        #region BINDED PROPERTIES
        private String _Status;
        /// <summary>
        /// Gets or sets the current bluettoth connection status.
        /// </summary>
        /// <value>
        /// The current bluettoth connection status.
        /// </value>
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
        /// <summary>
        /// Gets or sets the index of the selected available device.
        /// </summary>
        /// <value>
        /// The index of the selected available device.
        /// </value>
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
        /// <summary>
        /// Gets or sets a value indicating whether [connect enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [connect enabled]; otherwise, <c>false</c>.
        /// </value>
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
        /// <summary>
        /// Gets or sets a value indicating whether [disconnect enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [disconnect enabled]; otherwise, <c>false</c>.
        /// </value>
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

        private bool _IsCheckedSimulation;
        /// <summary>
        /// Gets or sets a value indicating whether simulation mode is checked.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is checked simulation mode is started; otherwise, <c>false</c>.
        /// </value>
        public bool IsCheckedSimulation
        {
            get { return _IsCheckedSimulation; }
            set
            {
                _IsCheckedSimulation = value;

                if (_IsCheckedSimulation)
                    _Client = new ECGBluetoothServiceSimulation(BluetoothSpecification.RfcommServiceUuid);
                else
                    _Client = new ECGBluetoothService(BluetoothSpecification.RfcommServiceUuid);

                NotifyOfPropertyChange(() => IsCheckedSimulation);
            }
        }

        #endregion //BINDED PROPERTIES

        #region EVENT HANDLERS

        /// <summary>
        /// Handles the get paired device click.
        /// </summary>
        async private void GetPairedDeviceButton_Clicked()
        {
            Devices.Clear();

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

        /// <summary>
        /// Handles the connect bluetooth connection click.
        /// </summary>
        async private void ConnectButton_Clicked()
        {
            var currentStatus = await _Client.Connect(SelectedIndex);
            UpdateStatus(currentStatus);

            if (currentStatus == BluetoothStatus.Connected)
            {
                ConnectEnabled = false;
                DisconnectEnabled = true;
            }

        }

        /// <summary>
        /// Handles the disconnect bluetooth connection click.
        /// </summary>
        private void DisconnectButton_Clicked()
        {
            _Client.Disconnect();
            UpdateStatus(BluetoothStatus.Disconnected);

            ConnectEnabled = true;
            DisconnectEnabled = false;
        }

        #endregion // EVENT HANDLERS

        #region METHODS

        /// <summary>
        /// Updates the current bluetooth connection status.
        /// </summary>
        /// <param name="status">The status.</param>
        private void UpdateStatus(BluetoothStatus status)
        {
            Status = StatusTable[(int)status];
        }

        /// <summary>
        /// Gets and fills the list of paired devices.
        /// </summary>
        /// <returns></returns>
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
