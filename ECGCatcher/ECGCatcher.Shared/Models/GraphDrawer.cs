using Caliburn.Micro;
using ECGCatcher.ViewModels;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace ECGCatcher.Models
{
    public enum GraphDrawerStatus
    {
        Started,
        Paused,
        Stopped
    }

    public class GraphDrawer : PropertyChangedBase
    {
        private IGraphSpace _GraphSpace;

        #region CONSTANT FACTORS
        private readonly Color _StrokeColor = Colors.Red;
        private readonly double _StrokeThickness = 3;
        public readonly int DataOffset = 3;
        public readonly int ShiftOffsetMultiplier = 10;
        #endregion // CONSTANT FACTORS

        public GraphDrawer(IGraphSpace graphSpace)
        {
            _GraphSpace = graphSpace;
            Shifter = new GraphShifter(_GraphSpace);

            CurrentPoints = new ObservableCollection<ECGPoint>();

            CurrentStatus = GraphDrawerStatus.Stopped;

            GraphData = new ConcurrentQueue<double>();
        }

        // Thread-safe collection of graph data provided by bluetooth connection
        public ConcurrentQueue<double> GraphData { get; set; }

        private ObservableCollection<ECGPoint> _CurrentPoints;
        public ObservableCollection<ECGPoint> CurrentPoints
        {
            get { return _CurrentPoints; }
            set
            {
                _CurrentPoints = value;
                NotifyOfPropertyChange(() => CurrentPoints);
            }
        }

        public GraphShifter Shifter { get; private set; }
        public GraphDrawerStatus CurrentStatus { get; private set; }

        public void StartDrawingGraph()
        {
            CurrentStatus = GraphDrawerStatus.Started;

            double coordinate;
            if (!GraphData.TryDequeue(out coordinate))
            {
                CurrentStatus = GraphDrawerStatus.Stopped;
                return;
            }

            _GraphSpace.PreviousPoint = new Point(_GraphSpace.Width, _GraphSpace.ZeroLevelCoordinate - coordinate * _GraphSpace.DataScaleFactor);

            DrawData();
            //Task.Run(() => DrawData() );
        }


        async private void DrawData()
        {
            // TODO: it could draw or try to draw until there is available bluetooth connection

            double coordinate;
            while (GraphData.TryDequeue(out coordinate) && CurrentStatus != GraphDrawerStatus.Stopped)
            {

                while (CurrentStatus == GraphDrawerStatus.Paused)
                    await Task.Delay(1000);

                double y = _GraphSpace.ZeroLevelCoordinate - coordinate * _GraphSpace.DataScaleFactor; // scaled coordinate
                Point p = new Point(_GraphSpace.PreviousPoint.X + DataOffset, y);

                var ecgPoint = new ECGPoint()
                {
                    #region ECG POINT PROPERTIES
                    X1 = _GraphSpace.PreviousPoint.X,
                    Y1 = _GraphSpace.PreviousPoint.Y,
                    X2 = p.X,
                    Y2 = p.Y,
                    StrokeColor = _StrokeColor,
                    StrokeThickness = _StrokeThickness
                    #endregion EGC POINT PROPERTIES
                };

                //await Windows.ApplicationModel.Core.CoreApplication.MainView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                //CurrentPoints.Add(ecgPoint)); 

                CurrentPoints.Add(ecgPoint);

                if (p.X + Shifter.TranslateX >= _GraphSpace.Width)
                    Shifter.ShiftGraph(DataOffset);

                _GraphSpace.PreviousPoint = new Point(p.X, p.Y);

                await Task.Delay(TimeSpan.FromMilliseconds(0.5)); // TODO: too slow, maybe draw it on additional thread
            }
        }

        public void PauseDrawingGraph()
        {
            CurrentStatus = GraphDrawerStatus.Paused;
        }

        public void ContinueDrawingGraph()
        {
            CurrentStatus = GraphDrawerStatus.Started;
        }

        public void RestoreStartingState()
        {
            Shifter.RestoreStartingShift();
            CurrentPoints.Clear();

            CurrentStatus = GraphDrawerStatus.Stopped;
            GraphData = new ConcurrentQueue<double>();
        }
    }
}
