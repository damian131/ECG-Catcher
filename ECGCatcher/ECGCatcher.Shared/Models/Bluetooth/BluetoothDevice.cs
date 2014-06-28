using System;

namespace ECGCatcher
{
    public class BluetoothDevice
    {
        
        /// <summary>
        /// Summary:
        ///     Gets the display name of the peer.
        ///
        /// Returns:
        ///     The display name of the peer.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        public string DisplayName { get { return _DisplayName; } }
        private string _DisplayName;
        
        /// <summary>
        /// Summary:
        ///     Gets the hostname or IP address of the peer.
        ///
        /// Returns:
        ///    The hostname or IP address of the peer.
        /// </summary>
        /// <value>
        /// The name of the host.
        /// </value>
        public string HostName { get { return _HostName; } }
        private string _HostName;
        
        /// <summary>
        /// Summary:
        ///     Gets the service name or TCP port number of the peer.
        ///
        /// Returns:
        ///     The service name or TCP port number of the peer.
        /// </summary>
        /// <value>
        /// The name of the service.
        /// </value>
        public string ServiceName { get { return _ServiceName; } }
        private string _ServiceName;

        /// <summary>
        /// Initializes a new instance of the <see cref="BluetoothDevice"/> class.
        /// </summary>
        /// <param name="hostName">Name of the host.</param>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="displayName">The display name.</param>
        public BluetoothDevice(string hostName, string serviceName, string displayName)
        {
            _HostName = hostName;
            _ServiceName = serviceName;
            _DisplayName = displayName;

        }

    }
}

