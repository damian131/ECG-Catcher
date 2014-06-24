using Caliburn.Micro;
using System;
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
        public readonly int DataOffset = 10;
        #endregion // CONSTANT FACTORS

        public GraphDrawer(IGraphSpace graphSpace){
            _GraphSpace = graphSpace;
            Shifter = new GraphShifter(_GraphSpace);

            CurrentPoints = new ObservableCollection<ECGPoint>();

            CurrentStatus = GraphDrawerStatus.Stopped;
            //IfDrawingStarted = false;
            //IfContinueDrawing = false;
        }

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
        //public Boolean IfDrawingStarted { get; private set; }
        //public Boolean IfContinueDrawing { get; private set; }

        private List<int> _SourceData;

        public void StartDrawingGraph( List<int> sourceData )
        {
            //IfDrawingStarted = true;
            CurrentStatus = GraphDrawerStatus.Started;

            _SourceData = sourceData; // optimal ?
            DrawData();
            //Task.Run(() => DrawData() );
        }


        async private void DrawData()
        {
            foreach (var coordinate in _SourceData)
            {
                while ( CurrentStatus == GraphDrawerStatus.Paused)
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

                await Task.Delay(200);
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
    }
}
