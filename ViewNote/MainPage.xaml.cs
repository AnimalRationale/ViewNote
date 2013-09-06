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
        IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

        // Constructor
        public MainPage()
        {
            InitializeComponent();            
            UseSettings();
           
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
            if ( !settings.Contains("DeleteAllConf") || settings["DeleteAllConf"] as string == "Yes" )
            {
                MessageBox.Show("ALL notes will be irreversibly deleted.", "Deleting ALL notes", MessageBoxButton.OKCancel);
            }
            else
            {
                MessageBox.Show("ALL NOTES DELETED!");
            }
        }

        private void pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void UseSettings()
        {            

            if ( !settings.Contains("AppBackColor") )
            {
                infoText01.Text = "No Settings.\n";

                settings.Add("AppBackColor", "Black");
                settings.Add("AppbarColor", "Dark");
                settings.Add("DeleteAllConf", "Yes");
                settings.Add("DeleteNoteConf", "Yes");
                settings.Save();
            }
            else
            {
                infoText01.Text = " AppBackColor: " + settings["AppBackColor"];
                infoText01.Text =  infoText01.Text + "\n AppbarColor: " + settings["AppbarColor"];
                infoText01.Text = infoText01.Text + "\n DeleteAllConf: " + settings["DeleteAllConf"];
                infoText01.Text = infoText01.Text + "\n DeleteNoteConf: " + settings["DeleteNoteConf"];

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