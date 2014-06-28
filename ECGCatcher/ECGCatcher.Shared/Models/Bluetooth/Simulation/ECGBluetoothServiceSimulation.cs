using ECGCatcher.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace ECGCatcher.Models.Bluetooth
{
    public class ECGBluetoothServiceSimulation : ECGBluetoothService
    {

        private readonly String[] ECGBaseName = { "ecgca102.txt", "ecgca154.txt" };

        public ECGBluetoothServiceSimulation( Guid UUID ) : base( UUID ){
        }

        public override async Task<List<string>> GetListPairedDevices(){
            await Task.FromResult(0);

            var FakedDeviceList = new List<string>();

            FakedDeviceList.Add("Faked Device 1-1");
            FakedDeviceList.Add("Faked Device 2-2");

            return FakedDeviceList;
        }

        private Simulation simulation;

        public async override Task<BluetoothStatus> Connect(int SelectedServiceIndex){

            simulation = new Simulation(ECGBaseName[SelectedServiceIndex]);
            simulation.Run();

            Status = DataReceivingStatus.Start;

            var fakedDataReader = await simulation.NextFakedData();
            ReceiveStringLoop(fakedDataReader);

            return BluetoothStatus.Connected;
        }

        protected async override void ReceiveStringLoop(DataReader dataReader)
        {
            if (Status == DataReceivingStatus.Start)
            {
                try
                {
                    //uint size = await dataReader.LoadAsync(sizeof(uint));
                    //if (size < sizeof(uint))
                    //{
                    //    // The underlying socket was closed before we were able to read the whole data
                    //    return;
                    //}

                    byte[] byteTab = new byte[4];
                    dataReader.ReadBytes(byteTab);

                    uint stringLength = BitConverter.ToUInt32(byteTab, 0);

                    //uint actualStringLength = await dataReader.LoadAsync(stringLength);
                    //if (actualStringLength != stringLength)
                    //{
                    //     The underlying socket was closed before we were able to read the whole data
                    //    return;
                    //}

                    String readStringData = dataReader.ReadString(stringLength);
                    AddReceivedDataToContainer(readStringData);

                    dataReader = await simulation.NextFakedData();
                    ReceiveStringLoop(dataReader);
                }
                catch (Exception ex)
                {
                    lock (this)
                    {
                        if (Socket == null)
                        {
                            // Do not print anything here -  the user closed the socket.
                        }
                        else
                        {
                            Disconnect();
                        }
                    }
                }
            }
        }

    }
}
