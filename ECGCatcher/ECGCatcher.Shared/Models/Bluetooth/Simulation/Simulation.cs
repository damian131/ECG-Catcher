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
        /// <summary>
        /// The file name
        /// </summary>
        private string fileName;
        /// <summary>
        /// Gets the faked data which is read from the faked data reader buffer.
        /// </summary>
        /// <value>
        /// The faked data.
        /// </value>
        public Queue<String> FakedData { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Simulation"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public Simulation(String fileName)
        {
            this.fileName = fileName;
            this.FakedData = new Queue<String>();
        }

        /// <summary>
        /// Splits received string from the fail and aggregates it in the FakedData container.
        /// </summary>
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

        /// <summary>
        /// Nexts the faked data. Return faked data reader with specified frame format.
        /// </summary>
        /// <returns></returns>
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
