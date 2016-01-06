using System;
using System.Windows;

using System.Device.Location;
using System.Windows.Shapes;
using System.Windows.Media;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Controls;

namespace PhoneApp1
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Конструктор
        public MainPage()
        {
            InitializeComponent();
            
        }
       

        private void Location_Click(object sender, EventArgs e)
        {
            GeoCoordinate geocor = new GeoCoordinate();
            GeoCoordinateWatcher geowach = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
            geowach.MovementThreshold = 1;
            geowach.TryStart(true, new TimeSpan(0, 0, 0, 0, 1000));
            geowach.PositionChanged += Geowach_PositionChanged;
            geowach.StatusChanged += Geowach_StatusChanged;
            geowach.Start();
            
        }

        private void Geowach_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            map.Layers.Clear();

            Ellipse myCircle = new Ellipse();
            myCircle.Fill = new SolidColorBrush(Colors.Blue);
            myCircle.Height = 20;
            myCircle.Width = 20;
            myCircle.Opacity = 50;

            MapOverlay myLocationOverlay = new MapOverlay();
            myLocationOverlay.Content = myCircle;
            myLocationOverlay.PositionOrigin = new Point(0.5, 0.5);
            myLocationOverlay.GeoCoordinate = e.Position.Location;

            MapLayer myLocationLayer = new MapLayer();
            myLocationLayer.Add(myLocationOverlay);

            map.Layers.Add(myLocationLayer);
        }

        private void Geowach_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            MessageBox.Show(e.Status.ToString());
        }

    }
}