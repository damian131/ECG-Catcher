using ECGCatcher.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECGCatcher.Models.Bluetooth
{
    public interface IBluetoothService
    {
        Task<List<string>> GetListPairedDevices();
        Task<BluetoothStatus> Connect(int SelectedServiceIndex);
        void Disconnect();
    }
}
