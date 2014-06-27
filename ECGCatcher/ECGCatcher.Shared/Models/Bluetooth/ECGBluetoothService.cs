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

        public ECGBluetoothService(Guid UUID) // TODO: remove IoC.Get, solution: use Ninject?
            : base(UUID)
        {
            //var dataDrawer = IoC.Get<MainViewModel>().Drawer;
        }

        protected async override void ReceiveStringLoop(DataReader dataReader)
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
                AddReadDataToContainer(readStringData);

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

        private void AddReadDataToContainer(String readData)
        {
            String[] splitData = readData.Split(BluetoothSpecification.CustomDataSeparator);

            var dataDrawer = IoC.Get<MainViewModel>().Drawer; // TODO: change it, maybe use Ninject ?

            foreach (var data in splitData)
                dataDrawer.GraphData.Enqueue(Double.Parse(data));
        }

    }
}
