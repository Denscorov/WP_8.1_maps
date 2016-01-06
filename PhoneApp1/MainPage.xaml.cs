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
using System.Device.Location;


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
            geowach.PositionChanged += Geowach_PositionChanged;
            geowach.StatusChanged += Geowach_StatusChanged;
            geowach.Start();
        }

        private void Geowach_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            map.Center = e.Position.Location;
        }

        private void Geowach_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            SystemTray.ProgressIndicator = new ProgressIndicator() { Text = e.Status.ToString(),IsVisible = true };
        }
    }
}