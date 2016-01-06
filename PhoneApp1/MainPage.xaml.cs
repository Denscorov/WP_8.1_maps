using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PhoneApp1.Resources;
using Microsoft.Phone.Maps.Toolkit;
using Microsoft.Phone.Maps.Controls;
using System.Device.Location;
using System.Threading.Tasks;
using Microsoft.Phone.Maps.Toolkit;

namespace PhoneApp1
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Конструктор
        public MainPage()
        {
            InitializeComponent();
            
        }
        MapLayer ml = new MapLayer();

        private void Location_Click(object sender, EventArgs e)
        {
            GeoCoordinate geocor = new GeoCoordinate();
            GeoCoordinateWatcher geowach = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
            geowach.PositionChanged += Geowach_PositionChanged;
            geowach.StatusChanged += Geowach_StatusChanged;
            geowach.Start();
            
        }

        private void Geowach_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            //map.Center = e.Position.Location;
            Pushpin p = new Pushpin();
            p.Content = e.Position.Location.Course.ToString();
            p.GeoCoordinate = e.Position.Location;

            MapChildCollection mc = new MapChildCollection();
            mc.Add(p);
            MapOverlay mo = new MapOverlay() { Content = p };
            ml.Add(mo);

            
        }

        private void Geowach_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
           
            SystemTray.ProgressIndicator = new ProgressIndicator();
            SystemTray.ProgressIndicator.Text = e.Status.ToString();
            SystemTray.ProgressIndicator.IsVisible = true;
            Task.Delay(1000);
            SystemTray.IsVisible = false;
        }
    }
}