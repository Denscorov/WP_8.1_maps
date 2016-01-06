using System;
using System.Windows;

using System.Device.Location;
using System.Windows.Shapes;
using System.Windows.Media;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Maps.Services;
using System.Collections.Generic;
using Windows.Devices.Geolocation;

namespace PhoneApp1
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Конструктор
        public MainPage()
        {
            InitializeComponent();
            this.GetCoordinates();
            
        }

        private async void GetCoordinates()
        {
            // Get the phone's current location.
            Geolocator MyGeolocator = new Geolocator();
            MyGeolocator.DesiredAccuracyInMeters = 1;
            Geoposition MyGeoPosition = null;
            try
            {
                MyGeoPosition = await MyGeolocator.GetGeopositionAsync(TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(10));
                MyCoordinates.Add(new GeoCoordinate(MyGeoPosition.Coordinate.Point.Position.Latitude, MyGeoPosition.Coordinate.Point.Position.Longitude));
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Location is disabled in phone settings or capabilities are not checked.");
            }
            catch (Exception ex)
            {
                // Something else happened while acquiring the location.
                MessageBox.Show(ex.Message);
            }
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


        RouteQuery MyQuery = null;
        GeocodeQuery Mygeocodequery = null;
        List<GeoCoordinate> MyCoordinates = new List<GeoCoordinate>();

        private void route_Click(object sender, EventArgs e)
        {
            Mygeocodequery = new GeocodeQuery();
            Mygeocodequery.SearchTerm = "хмельницький зарічанська 10";
            Mygeocodequery.GeoCoordinate = new GeoCoordinate(46.202215,25.020216);
            Mygeocodequery.QueryCompleted += Mygeocodequery_QueryCompleted;
            Mygeocodequery.QueryAsync();

        }

        private void Mygeocodequery_QueryCompleted(object sender, QueryCompletedEventArgs<System.Collections.Generic.IList<MapLocation>> e)
        {
            if (e.Error == null)
            {
                MyQuery = new RouteQuery();
                MyCoordinates.Add(e.Result[0].GeoCoordinate);
                MyQuery.Waypoints = MyCoordinates;
                MyQuery.QueryCompleted += MyQuery_QueryCompleted;
                MyQuery.QueryAsync();
                Mygeocodequery.Dispose();
            }
        }

        private void MyQuery_QueryCompleted(object sender, QueryCompletedEventArgs<Route> e)
        {
            if (e.Error == null)
            {
                Route MyRoute = e.Result;
                MapRoute MyMapRoute = new MapRoute(MyRoute);
                map.AddRoute(MyMapRoute);
                MyQuery.Dispose();
            }
        }
    }
}