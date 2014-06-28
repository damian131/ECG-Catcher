using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;

namespace ECGCatcher.Models
{
    public class Simulation
    {
        private string fileName;
        public Queue<String> FakedData { get; private set; }

        public Simulation(String fileName)
        {
            this.fileName = fileName;
            this.FakedData = new Queue<String>();
        }

        async public void Run()
        {
            StorageFile sfile = await StorageFile.GetFileFromApplicationUriAsync( new Uri("ms-appx:///Resources/SampleECGData/" + fileName) );

            String stringData = await Windows.Storage.FileIO.ReadTextAsync(sfile);

            foreach (var str in stringData.Split(' '))
            {
                if (str != String.Empty)
                {
                    FakedData.Enqueue(str);
                }
            }

        }

        public async Task<DataReader> NextFakedData(){

            await Task.Delay(100); // TODO: posiible problem with thread integration

            StringBuilder fakeStr = new StringBuilder();

            for( int i=0; i < 20 && FakedData.Count != 0; ++i )
                fakeStr.Append( FakedData.Dequeue() + ":");

            fakeStr.Remove(fakeStr.Length - 1, 1);

            UInt32 length = (uint)fakeStr.Length;

            byte[] lengthArray = BitConverter.GetBytes(length);

            byte[] dataArray = Encoding.UTF8.GetBytes( fakeStr.ToString() );

            byte[] readyFakeDataArray = new byte[ lengthArray.Length + dataArray.Length ];

            System.Buffer.BlockCopy( lengthArray, 0, readyFakeDataArray, 0, lengthArray.Length );

            System.Buffer.BlockCopy( dataArray, 0, readyFakeDataArray, lengthArray.Length, dataArray.Length );

            DataReader dataReader = Windows.Storage.Streams.DataReader. FromBuffer( readyFakeDataArray.AsBuffer() );

            return dataReader;
        }
    }
}
