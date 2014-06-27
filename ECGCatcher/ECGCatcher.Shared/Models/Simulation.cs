using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace ECGCatcher.Models
{
    public class Simulation
    {
        private string fileName;

        public Simulation(String fileName)
        {
            this.fileName = fileName;
        }

        async public Task<String> Run()
        {
            StorageFile sfile = await StorageFile.GetFileFromApplicationUriAsync( new Uri("ms-appx:///Resources/SampleECGData/" + fileName) );

            String stringData = await Windows.Storage.FileIO.ReadTextAsync(sfile);

            return stringData;
        }

        private void FillFakeGraphData(  ConcurrentQueue<double> SourceData, String stringData ){

            foreach (var str in stringData.Split(' '))
            {
                if (str != String.Empty)
                {
                    double parsed;
                    if (!double.TryParse(str, out parsed))
                    {
                        throw new InvalidCastException();
                    }
                    SourceData.Enqueue(parsed);
                }
            }
        }
    }
}
