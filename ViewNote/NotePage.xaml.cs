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
using ViewNote.Model;
using System.Windows.Media.Imaging;
using System.IO;

namespace ViewNote
{
    public partial class NotePage : PhoneApplicationPage
    {
        IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

        public NotePage()
        {
            InitializeComponent();
            // this.DataContext = App.ViewModel;
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

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            int noteID = int.Parse(NavigationContext.QueryString["ID"]);
            VNoteItem noteContext;
            noteContext = null;

            foreach ( VNoteItem note in App.ViewModel.AllNotesItems )
            {
                if ( note.VNoteItemId == noteID )
                {
                    DataContext = note;
                    noteContext = note;
                    break;
                }
            }

            BitmapImage imageFromStorage = new BitmapImage();
            string imageFolder = "Shared/ShellContent";
            string imageFileName = noteContext.VNotePhoto;

            using ( var isoFile = IsolatedStorageFile.GetUserStoreForApplication() )
            {
                string filePath = System.IO.Path.Combine(imageFolder, imageFileName);
                System.Diagnostics.Debug.WriteLine("Read filePath: {0}", filePath);
                using ( var imageStream = isoFile.OpenFile(
                    filePath, FileMode.Open, FileAccess.Read) )
                {
                    imageFromStorage.SetSource(imageStream);
                    System.Diagnostics.Debug.WriteLine("Readin image for pano: {0}", 0);
                }
            }
            notePhoto.Source = imageFromStorage;

            if ( e.NavigationMode == System.Windows.Navigation.NavigationMode.Back )
            {
                UseSettings();
            }
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
    }
}
