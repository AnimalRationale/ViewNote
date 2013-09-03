using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using System.IO.IsolatedStorage;

namespace ViewNote
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void AppColorChanged(object sender, RoutedEventArgs e)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            String option = ( (RadioButton)sender ).Name;
            switch ( option )
            {
                case "AppBackBlack":
                    this.LayoutRoot.Background = new SolidColorBrush(Colors.Black);
                    if ( !settings.Contains("AppBackColor") )
                    {
                        settings.Add("AppBackColor", "Black");
                    }
                    else
                    {
                        settings["AppBackColor"] = "Black";
                    }
                    settings.Save();
                    break;

                case "AppBackGray":
                    this.LayoutRoot.Background = new SolidColorBrush(Colors.Gray);
                    if ( !settings.Contains("AppBackColor") )
                    {
                        settings.Add("AppBackColor", "Gray");
                    }
                    else
                    {
                        settings["AppBackColor"] = "Gray";
                    }
                    settings.Save();
                    break;

                case "AppbarBlack":
                    ApplicationBar.BackgroundColor = new Color() { A = 0, R = 0, G = 0, B = 0 };
                    if ( !settings.Contains("AppbarColor") )
                    {
                        settings.Add("ApprColor", "Dark");
                    }
                    else
                    {
                        settings["AppbarkColor"] = "Dark";
                    }
                    settings.Save();
                    break;

                case "AppbarAccent":
                    ApplicationBar.BackgroundColor = (Color)Resources["PhoneAccentColor"];
                    if ( !settings.Contains("AppbarColor") )
                    {
                        settings.Add("AppbarColor", "Accent");
                    }
                    else
                    {
                        settings["AppbarkColor"] = "Accent";
                    }
                    settings.Save();
                    break;
            }
        }

        private void appbarAdd_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AddNotePage.xaml", UriKind.Relative));
        }

        private void appbarHelp_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/HelpPage.xaml", UriKind.Relative));
        }

        private void appbarBack_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void appbarDelete_Click(object sender, EventArgs e)
        {

        }
    }
}