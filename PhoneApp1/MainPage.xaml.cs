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
            Pushpin p = new Pushpin();
            p.Content = e.Position.Location.Course.ToString();
            p.GeoCoordinate = e.Position.Location;
            MapOverlay mo = new MapOverlay() { Content = p };
            mo.GeoCoordinate = e.Position.Location;
            MapLayer ml = new MapLayer();
            ml.Add(mo);
            map.Layers.Add(ml);
        }

        private void Geowach_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            MessageBox.Show(e.Status.ToString());
        }

       

        
    }
}