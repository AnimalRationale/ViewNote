using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using System.Windows.Media;


namespace ViewNote
{
    public partial class AddNote : PhoneApplicationPage
    {
        public AddNote()
        {
            InitializeComponent();
            UseSettings();
        }

        private void appbarCheck_Click(object sender, EventArgs e)
        {
            if ( NavigationService.CanGoBack )
            {
                NavigationService.GoBack();
            };
        }

        private void appbarHelp_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/HelpPage.xaml", UriKind.Relative));
        }

        private void appbarCancel_Click(object sender, EventArgs e)
        {
            if ( NavigationService.CanGoBack )
            {
                NavigationService.GoBack();
            }
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