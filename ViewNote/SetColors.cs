using System;
using System.Windows.Media;
using System.IO.IsolatedStorage;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using System.Windows;

namespace ViewNote
{
    class SetColors
    {
        public void UseSettings()
        {
            var currentPage = ((PhoneApplicationFrame)Application.Current.RootVisual).Content;

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
                    currentPage.LayoutRoot.Background = VNcolors.ColorAppBg;
                }
                else {
                    currentPage.LayoutRoot.Background = new SolidColorBrush(Colors.Black);
                }
                if (settings["AppbarColor"] as string == "Accent" )
                {
                    ApplicationBar.BackgroundColor = (Color)Resources["PhoneAccentColor"];
                }
                else {
                    ApplicationBar.BackgroundColor = VNcolors.ColorDark;
                }
            }
    }
        }
    }
}
