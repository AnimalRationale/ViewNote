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
using Microsoft.Phone.Tasks;

namespace ViewNote
{
    public partial class HelpPage : PhoneApplicationPage
    {
        IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

        public HelpPage()
        {
            InitializeComponent();
            UseSettings();
        }

        private void sendMsgToAuthor_Click(object sender, RoutedEventArgs e)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask();
            emailComposeTask.Subject = "Message from ViewNote user";
            emailComposeTask.Body = msgToAuthor.Text;
            emailComposeTask.To = "wojciech.mardyla@gmail.com";
            emailComposeTask.Show(); ;
        }

        private void appbarAdd_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AddNotePage.xaml", UriKind.Relative));
        }

        private void appbarBack_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void appbarSettings_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }

        private void appbarDelete_Click(object sender, EventArgs e)
        {
            if ( !settings.Contains("DeleteAllConf") || settings["DeleteAllConf"] as string == "Yes" )
            {
                MessageBoxResult mBox = MessageBox.Show("ALL notes will be irreversibly deleted.", "Deleting ALL notes", MessageBoxButton.OKCancel);
                if ( mBox == MessageBoxResult.OK )
                {
                    App.ViewModel.DeleteAllVNoteItems();
                }
            }
            else
            {
                App.ViewModel.DeleteAllVNoteItems();
            }
            this.Focus();
        }

        private void UseSettings()
        {           

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

        private void pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            
            if ( e.NavigationMode == System.Windows.Navigation.NavigationMode.Back )
            {                
                UseSettings();
            }
        }
    }
}