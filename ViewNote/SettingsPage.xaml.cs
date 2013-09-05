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
        IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

        public SettingsPage()
        {
            InitializeComponent();

            if ( settings["AppBackColor"] as string == "userColor01" )
            {
                rbAppBackUser01.IsChecked = true;
                rbAppBackBlack.IsChecked = false;
            }
            else
            {
                rbAppBackUser01.IsChecked = false;
                rbAppBackBlack.IsChecked = true;
            }

            if ( settings["AppbarColor"] as string == "Accent" )
            {
                rbAppbarBlack.IsChecked = false;
                rbAppbarAccent.IsChecked = true;
            }
            else
            {
                rbAppbarBlack.IsChecked = true;
                rbAppbarAccent.IsChecked = false;
            }

            if ( settings["DeleteAllConf"] as string == "Yes" )
            {
                cbDeleteAllConf.IsChecked = true;
            }

            if ( settings["DeleteNoteConf"] as string == "Yes" )
            {
                cbDeleteNoteConf.IsChecked = true;
            }
        }

        private void AppColorChanged(object sender, RoutedEventArgs e)
        {
            String option = ( (RadioButton)sender ).Name;
            switch ( option )
            {
                case "rbAppBackBlack":
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

                case "rbAppBackUser01":
                    this.LayoutRoot.Background = VNcolors.ColorAppBg;
                    if ( !settings.Contains("AppBackColor") )
                    {
                        settings.Add("AppBackColor", "userColor01");
                    }
                    else
                    {
                        settings["AppBackColor"] = "userColor01";
                    }
                    settings.Save();
                    break;

                case "rbAppbarBlack":
                    ApplicationBar.BackgroundColor = VNcolors.ColorDark;
                    if ( !settings.Contains("AppbarColor") )
                    {
                        settings.Add("ApprColor", "Dark");
                    }
                    else
                    {
                        settings["AppbarColor"] = "Dark";
                    }
                    settings.Save();
                    break;

                case "rbAppbarAccent":
                    ApplicationBar.BackgroundColor = (Color)Resources["PhoneAccentColor"];
                    if ( !settings.Contains("AppbarColor") )
                    {
                        settings.Add("AppbarColor", "Accent");
                    }
                    else
                    {
                        settings["AppbarColor"] = "Accent";
                    }
                    settings.Save();
                    break;
            }
        }

        private void DeleteConfChanged(object sender, RoutedEventArgs e)
        {
            String option = ( (CheckBox)sender ).Name;
            switch ( option )
            {
                case "cbDeleteAllConf":
                    if ( !settings.Contains("DeleteAllConf") )
                    {
                        if ( cbDeleteAllConf.IsChecked == true )
                        {
                            settings.Add("DeleteAllConf", "Yes");
                        }
                        else
                        {
                            settings.Add("DeleteAllConf", "No");
                        }
                    }
                    else
                    {
                        if ( cbDeleteAllConf.IsChecked == true )
                        {
                            settings["DeleteAllConf"] = "Yes";
                        }
                        else
                        {
                            settings["DeleteAllConf"] = "No";
                        }
                    }
                    settings.Save();
                    break;

                case "cbDeleteNoteConf":
                    if ( !settings.Contains("DeleteNoteConf") )
                    {
                        if ( cbDeleteAllConf.IsChecked == true )
                        {
                            settings.Add("DeleteNoteConf", "Yes");
                        }
                        else
                        {
                            settings.Add("DeleteNoteConf", "No");
                        }
                    }
                    else
                    {
                        if ( cbDeleteAllConf.IsChecked == true )
                        {
                            settings["DeleteNoteConf"] = "Yes";
                        }
                        else
                        {
                            settings["DeleteNoteConf"] = "No";
                        }
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

        private void pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}