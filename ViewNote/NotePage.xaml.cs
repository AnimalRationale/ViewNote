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
using Microsoft.Phone.Shell;

namespace ViewNote
{
    public partial class NotePage : PhoneApplicationPage
    {
        IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
        string thisPageUri;
        VNoteItem noteContext = null;            

        public NotePage()
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

        private void appbarBack_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void appbarSettings_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }

        private void appbarPinUnpin_Click(object sender, EventArgs e)
        {
            ShellTile tile = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains(thisPageUri));
            if ( tile == null )
            {
                // Pin doesn't exist, so create new one.                
                Uri tileBackImage;
                
                if ( noteContext.VNotePhoto != null )
                {
                    tileBackImage = new Uri(@"isostore:/Shared/ShellContent/" + noteContext.VNotePhoto, UriKind.Absolute);
                    System.Diagnostics.Debug.WriteLine("Pinned BackTile photo filename: {0}", tileBackImage);

                }
                else
                {
                    tileBackImage = new Uri("/Images/edittext.png", UriKind.Relative);
                }


                StandardTileData tileData = new StandardTileData
                {
                    BackgroundImage = tileBackImage,
                    Title = noteContext.VNoteTitle,
                    BackTitle = noteContext.VNoteDate.ToString(),
                    BackContent = noteContext.VNoteText,
                    BackBackgroundImage = null
                };

                // Change PinUnpin button state.
                var appbarPinUnpin = ApplicationBar.Buttons[3] as ApplicationBarIconButton;
                appbarPinUnpin.Text = "Unpin note";
                appbarPinUnpin.IconUri = new Uri("/Images/pin.remove.png", UriKind.Relative);

                // Pin to start.
                ShellTile.Create(new Uri(thisPageUri, UriKind.Relative), tileData);
            }
            else
            {
                // This tile already pinned, unpinning.
                tile.Delete();
                var appbarPinUnpin = ApplicationBar.Buttons[3] as ApplicationBarIconButton;
                appbarPinUnpin.Text = "Pin note";
                appbarPinUnpin.IconUri = new Uri("/Images/pin.png", UriKind.Relative);
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if ( e.NavigationMode == System.Windows.Navigation.NavigationMode.Back )
            {
                UseSettings();
            }

            int noteID = int.Parse(NavigationContext.QueryString["ID"]);            

            foreach ( VNoteItem note in App.ViewModel.AllNotesItems )
            {
                if ( note.VNoteItemId == noteID )
                {
                    DataContext = note;
                    noteContext = note;
                    break;
                }
            }
            if ( noteContext.VNotePhoto != null )
            {
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
                        System.Diagnostics.Debug.WriteLine("Reading image for pano: {0}", 0);
                    }
                }
                notePhoto.Source = imageFromStorage;
            }

            // Check if pinned
            thisPageUri = String.Format("/NotePage.xaml?ID={0}", noteID);
            ShellTile tile = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains(thisPageUri));

            if ( tile == null )
            {
                var appbarPinUnpin = ApplicationBar.Buttons[3] as ApplicationBarIconButton;
                appbarPinUnpin.Text = "Pin note to Start";
                appbarPinUnpin.IconUri = new Uri("/Images/pin.png", UriKind.Relative);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Tile: {0}", tile.ToString());
                var appbarPinUnpin = ApplicationBar.Buttons[3] as ApplicationBarIconButton;
                appbarPinUnpin.Text = "Unpin note";
                appbarPinUnpin.IconUri = new Uri("/Images/pin.remove.png", UriKind.Relative);
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
