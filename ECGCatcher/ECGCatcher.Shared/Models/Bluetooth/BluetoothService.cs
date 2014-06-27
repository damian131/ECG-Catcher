
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Devices.Enumeration;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using ECGCatcher.ViewModels;
using Caliburn.Micro;

//using SDKTemplate;

namespace ECGCatcher.Models.Bluetooth
{

    public partial class BluetoothService : IBluetoothService
    {
        protected readonly Guid UUID;

        // The Id of the Service Name SDP attribute
        protected const UInt16 SdpServiceNameAttributeId = 0x100;

        // The SDP Type of the Service Name SDP attribute.
        // The first byte in the SDP Attribute encodes the SDP Attribute Type as follows :
        //    -  the Attribute Type size in the least significant 3 bits,
        //    -  the SDP Attribute Type value in the most significant 5 bits.
        protected const byte SdpServiceNameAttributeType = (4 << 3) | 5;

        protected StreamSocket Socket;
        protected DataWriter Writer;
        protected RfcommDeviceService Service;
        protected DeviceInformationCollection ServiceInfoCollection;

        public BluetoothService( Guid UUID )
        {
            this.UUID = UUID;

            Socket = null;
            Writer = null;
            Service = null;
            ServiceInfoCollection = null;

        }

        public async Task<List<string>> GetListPairedDevices()
        {
            // Clear any previous messages

            // Find all paired instances of the Rfcomm chat service
            ServiceInfoCollection = await DeviceInformation.FindAllAsync(
                RfcommDeviceService.GetDeviceSelector(RfcommServiceId.FromUuid(UUID)));

            if (ServiceInfoCollection.Count > 0)
            {
                List<string> items = new List<string>();
                foreach (var ServiceInfo in ServiceInfoCollection)
                {
                    items.Add(ServiceInfo.Name);
                }

                return items;
            }
            else
            {
                ////MainPage.Current.NotifyUser(
                //    "No chat services were found. Please pair with a device that is advertising the chat service.",
                //    NotifyType.ErrorMessage);
            }
            return null;
        }

        public async Task<BluetoothStatus> Connect( int SelectedServiceIndex )
        {
            try
            {

                var ServiceInfo = ServiceInfoCollection[SelectedServiceIndex];
                Service = await RfcommDeviceService.FromIdAsync(ServiceInfo.Id);

                if (Service == null)
                {
                    return BluetoothStatus.NoAccess;
                }

                var attributes = await Service.GetSdpRawAttributesAsync();
                if (!attributes.ContainsKey(SdpServiceNameAttributeId))
                {
                    return BluetoothStatus.WrongService;
                }

                var attributeReader = DataReader.FromBuffer(attributes[SdpServiceNameAttributeId]);
                var attributeType = attributeReader.ReadByte();
                if (attributeType != SdpServiceNameAttributeType)
                {
                    return BluetoothStatus.UnexpectedDataFormat;
                }

                var serviceNameLength = attributeReader.ReadByte();

                // The Service Name attribute requires UTF-8 encoding.
                attributeReader.UnicodeEncoding = UnicodeEncoding.Utf8;

                lock (this)
                {
                    Socket = new StreamSocket();
                }

                await Socket.ConnectAsync(Service.ConnectionHostName, Service.ConnectionServiceName);

                Writer = new DataWriter(Socket.OutputStream);

                DataReader dataReader = new DataReader(Socket.InputStream);
                ReceiveStringLoop(dataReader);
            }
            catch (Exception ex)
            {
                return BluetoothStatus.UnexpectedConnectionError;
            }
            return BluetoothStatus.Connected;
        }

        protected async virtual void ReceiveStringLoop(DataReader dataReader)
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

                // DATA FORMAT!!!!

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

        public void Disconnect()
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

            //RunButton.IsEnabled = true;
            //ServiceSelector.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            //ChatBox.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            //ConversationList.Items.Clear();
        }

        //private async void SendButton_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        chatWriter.WriteUInt32((uint)MessageTextBox.Text.Length);
        //        chatWriter.WriteString(MessageTextBox.Text);

        //        await chatWriter.StoreAsync();
        //        //ConversationList.Items.Add("Sent: " + MessageTextBox.Text);

        //        //MessageTextBox.Text = "";
        //    }
        //    catch (Exception ex)
        //    {
        //        //MainPage.Current.NotifyUser("Error: " + ex.HResult.ToString() + " - " + ex.Message,
        //        //    NotifyType.StatusMessage);
        //    }
        //}
    }
}