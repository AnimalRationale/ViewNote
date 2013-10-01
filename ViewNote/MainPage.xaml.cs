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
using System.Windows.Media.Imaging;
using ViewNote.Model;
using ViewNote;
using Microsoft.Phone.Shell;
using System.Windows.Data;
using System.IO;

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
            // Set the page DataContext property to the ViewModel.
            this.DataContext = App.ViewModel;
            UpdateLiveTiles();
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            // Save changes to the database.
            App.ViewModel.SaveChangesToDB();
        }

        public void UpdateLiveTiles()
        {
            ShellTile currentTiles = ShellTile.ActiveTiles.First();
            if ( currentTiles != null )
            {
                string noteText;
                Uri tileBackImage;

                int noteCount = App.ViewModel.AllNotesItems.Count;
                if ( noteCount != 0 )
                {
                    noteText = App.ViewModel.AllNotesItems.Last().VNoteTitle;
                    if ( App.ViewModel.AllNotesItems.Last().VNotePhoto.Length > 0 )
                    {
                        tileBackImage = new Uri(@"isostore:/Shared/ShellContent/" + App.ViewModel.AllNotesItems.Last().VNotePhoto, UriKind.Absolute);
                        System.Diagnostics.Debug.WriteLine("BackTile photo filename: {0}", tileBackImage);
                        
                    }
                    else
                    {
                        tileBackImage = new Uri("/Images/edittext.png", UriKind.Relative);
                    }
                }
                else
                {
                    noteText = "Add note... ";
                    tileBackImage = new Uri("/Images/edittext.png", UriKind.Relative);
                }
                //ShellTile currentTiles = ShellTile.ActiveTiles.First();
                var tilesUpdatedData = new StandardTileData
                {
                    Title = "ViewNote ",
                    Count = noteCount,                    
                    BackBackgroundImage = tileBackImage,
                    BackTitle = noteText
                };

                currentTiles.Update(tilesUpdatedData);
            }
        }

        public class PathToImageConverter : IValueConverter
        {
            private static IsolatedStorageFile isoStorage = IsolatedStorageFile.GetUserStoreForApplication();

            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                string path = value as string;
                System.Diagnostics.Debug.WriteLine("Converter input path: {0}", path);

                if ( string.IsNullOrEmpty(path) || path.Length < 6 )
                    return null;

                if ( ( path.Length > 9 ) && ( path.ToLower().Substring(0, 9).Equals("isostore:") ) )
                {
                    using ( var sourceFile = isoStorage.OpenFile(path.Substring(9), FileMode.Open, FileAccess.Read) )
                    {
                        BitmapImage image = new BitmapImage();
                        image.SetSource(sourceFile);
                        return image;
                    }
                }
                else
                {
                    BitmapImage image = new BitmapImage(new Uri(path));
                    return image;
                }
            }

            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        private void deleteNoteButton_Click(object sender, RoutedEventArgs e)
        {
            // Cast the parameter as a button.
            var button = sender as Button;
            if ( button != null )
            {
                if ( !settings.Contains("DeleteNoteConf") || settings["DeleteNoteConf"] as string == "Yes" )
                {
                    MessageBoxResult mBox = MessageBox.Show("This note will be irreversibly deleted.", "Deleting selected note", MessageBoxButton.OKCancel);
                    if ( mBox == MessageBoxResult.OK )
                    {
                        VNoteItem noteForDelete = button.DataContext as VNoteItem;
                        App.ViewModel.DeleteVNoteItem(noteForDelete);
                    }
                }
                else
                {
                    // Get a handle for the to-do item bound to the button.
                    VNoteItem noteForDelete = button.DataContext as VNoteItem;
                    App.ViewModel.DeleteVNoteItem(noteForDelete);
                }

            }

            // Put the focus back to the main page.
            this.Focus();
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

        private void pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void PhoneApplicationPage_OrientationChanged(object sender, OrientationChangedEventArgs e)
        {
            if ( e.Orientation == PageOrientation.Landscape || e.Orientation == PageOrientation.LandscapeLeft || e.Orientation == PageOrientation.LandscapeRight )
            {
                pivot1.Header = null;
                pivot2.Header = null;
                pivot3.Header = null;
                pivot4.Header = null;
                pivotMain.Margin = new Thickness(0, -150, 0, 0);
            }
            else
            {
                pivot1.Header = "All notes";
                pivot2.Header = "Memos";
                pivot3.Header = "Travel";
                pivot4.Header = "Fun";
                pivotMain.Margin = new Thickness(0, 0, 0, 0);
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {

            if ( e.NavigationMode == System.Windows.Navigation.NavigationMode.Back )
            {
                UseSettings();
            }
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
                infoText01.Text = infoText01.Text + "\n AppbarColor: " + settings["AppbarColor"];
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