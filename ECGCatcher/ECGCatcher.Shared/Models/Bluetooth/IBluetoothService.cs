using ECGCatcher.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECGCatcher.Models.Bluetooth
{
    public interface IBluetoothService
    {
        /// <summary>
        /// Gets the list paired devices with unique UID number.
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetListPairedDevices();
        /// <summary>
        /// Connects the specified selected service and starts data reading from the service.
        /// </summary>
        /// <param name="SelectedServiceIndex">Index of the selected service.</param>
        /// <returns></returns>
        Task<BluetoothStatus> Connect(int SelectedServiceIndex);
        /// <summary>
        /// Disconnects the service and closes all sockets.
        /// </summary>
        void Disconnect();
    }
}
