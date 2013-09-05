using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage;

namespace ViewNote
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            UseSettings();            
        }

        private void buttonLoad_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonPopulate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void appbarAdd_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AddNotePage.xaml", UriKind.Relative));
        }

        private void appbarHelp_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/HelpPage.xaml", UriKind.Relative));
        }

        private void appbarSettings_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }

        private void appbarDelete_Click(object sender, EventArgs e)
        {

        }

        private void pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void UseSettings()
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

            if ( !settings.Contains("AppBackColor") )
            {
                settings.Add("AppBackColor", "Black");
                settings.Add("AppbarColor", "Dark");
                settings.Save();
            }
            else
            {
                if ( settings["AppBackColor"] as string == "userColor01" )
                {
                    this.LayoutRoot.Background = VNcolors.ColorAppBg;
                }
                else
                {
                    this.LayoutRoot.Background = new SolidColorBrush(Colors.Black);
                }
                if ( settings["AppbarColor"] as string == "Accent" )
                {
                    ApplicationBar.BackgroundColor = (Color)Resources["PhoneAccentColor"];
                }
                else
                {
                    ApplicationBar.BackgroundColor = VNcolors.ColorDark;
                }
            }
        }
    }
}