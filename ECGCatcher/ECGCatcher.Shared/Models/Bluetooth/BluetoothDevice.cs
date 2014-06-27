using System;

namespace ECGCatcher
{
    public class BluetoothDevice
    {
        // Summary:
        //     Gets the display name of the peer.
        //
        // Returns:
        //     The display name of the peer.
        public string DisplayName { get { return _DisplayName; } }
        private string _DisplayName;
        //
        // Summary:
        //     Gets the hostname or IP address of the peer.
        //
        // Returns:
        //     The hostname or IP address of the peer.
        public string HostName { get { return _HostName; } }
        private string _HostName;
        //
        // Summary:
        //     Gets the service name or TCP port number of the peer.
        //
        // Returns:
        //     The service name or TCP port number of the peer.
        public string ServiceName { get { return _ServiceName; } }
        private string _ServiceName;

        public BluetoothDevice(string hostName, string serviceName, string displayName)
        {
            _HostName = hostName;
            _ServiceName = serviceName;
            _DisplayName = displayName;

        }

    }
}

