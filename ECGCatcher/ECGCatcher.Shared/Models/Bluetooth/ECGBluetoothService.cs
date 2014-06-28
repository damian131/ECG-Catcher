using Caliburn.Micro;
using ECGCatcher.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage.Streams;

namespace ECGCatcher.Models.Bluetooth
{
    

    public class ECGBluetoothService : BluetoothService
    {
        //GraphDrawer dataDrawer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ECGBluetoothService"/> class.
        /// </summary>
        /// <param name="UUID">The UUID.</param>
        public ECGBluetoothService(Guid UUID) // TODO: remove IoC.Get, solution: use Ninject?
            : base(UUID)
        {
            //var dataDrawer = IoC.Get<MainViewModel>().Drawer;
        }

        /// <summary>
        /// Starts data loop and reads data from specified data reader until the data receiving status equals started.
        /// </summary>
        /// <param name="dataReader">The data reader.</param>
        protected async override void ReceiveStringLoop(DataReader dataReader)
        {
            if (Status == DataReceivingStatus.Start)
            {

                try
                {
                    uint size = await dataReader.LoadAsync(sizeof(uint));
                    if (size < sizeof(uint))
                    {
                        // The underlying socket was closed before we were able to read the whole data
                        return;
                    }

                    uint stringLength = dataReader.ReadUInt32();
                    uint actualStringLength = await dataReader.LoadAsync(stringLength);
                    if (actualStringLength != stringLength)
                    {
                        // The underlying socket was closed before we were able to read the whole data
                        return;
                    }

                    String readStringData = dataReader.ReadString(stringLength);
                    AddReceivedDataToContainer(readStringData);

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
                            //MainPage.Current.NotifyUser("Read stream failed with error: " + ex.Message, NotifyType.ErrorMessage);
                            Disconnect(); // TODO: update status
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Disconnects this service.
        /// </summary>
        public override void Disconnect()
        {

            if (Writer != null)
            {
                Writer.DetachStream();
                Writer = null;
            }

            lock (this)
            {
                if (Socket != null)
                {
                    Socket.Dispose();
                    Socket = null;
                }
            }

            var dataDrawer = IoC.Get<MainViewModel>().Drawer;
            dataDrawer.RestoreStartingState();

            Status = DataReceivingStatus.Stop;

        }

        /// <summary>
        /// Adds the received data to queue container.
        /// </summary>
        /// <param name="readData">The read data.</param>
        protected void AddReceivedDataToContainer(String readData)
        {
            String[] splitData = readData.Split(BluetoothSpecification.CustomDataSeparator);

            var dataDrawer = IoC.Get<MainViewModel>().Drawer; // TODO: change it, maybe use Ninject ?

            foreach (var data in splitData)
                dataDrawer.GraphData.Enqueue(Double.Parse(data));
        }

    }
}
