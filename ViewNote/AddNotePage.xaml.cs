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
using System.Windows.Media.Imaging;
using Microsoft.Phone.Tasks;


namespace ViewNote
{
    public partial class AddNote : PhoneApplicationPage
    {
        CameraCaptureTask cameraCaptureTask;
        PhotoChooserTask photoChooserTask;

        public AddNote()
        {
            InitializeComponent();
            UseSettings();
            cameraCaptureTask = new CameraCaptureTask();
            cameraCaptureTask.Completed += new EventHandler<PhotoResult>(cameraCaptureTask_Completed);
            photoChooserTask = new PhotoChooserTask();
            photoChooserTask.Completed += new EventHandler<PhotoResult>(photoChooserTask_Completed);
        }

        void cameraCaptureTask_Completed(object sender, PhotoResult e)
        {
            if ( e.TaskResult == TaskResult.OK )
            {
                addedPhoto.Source = new BitmapImage(new Uri(e.OriginalFileName));
                addImageStatus.Text = "";
                addPhotoStatus.Text = "Photo added";
            }
        }

        void photoChooserTask_Completed(object sender, PhotoResult e)
        {
            if ( e.TaskResult == TaskResult.OK )
            {
                BitmapImage bmp = new BitmapImage();
                bmp.SetSource(e.ChosenPhoto);
                addedPhoto.Source = bmp;
                addPhotoStatus.Text = "";
                addImageStatus.Text = "Image added";
            }
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

        private void pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void PhoneApplicationPage_OrientationChanged(object sender, OrientationChangedEventArgs e)
        {
            if ( e.Orientation == PageOrientation.Landscape || e.Orientation == PageOrientation.LandscapeLeft || e.Orientation == PageOrientation.LandscapeRight )
            {
                pivotAddNoteText.Header = null;
                pivotAddNotePhoto.Header = null;
                pivotAddNote.Margin = new Thickness(0, -150, 0, 0);
            }
            else
            {
                pivotAddNoteText.Header = "Note";
                pivotAddNoteText.Header = "Photo";
                pivotAddNote.Margin = new Thickness(0);
            }
        }

        private void AddPhoto_Click(object sender, RoutedEventArgs e)
        {
            cameraCaptureTask.Show();
        }

        private void addImage_Click(object sender, RoutedEventArgs e)
        {
            photoChooserTask.Show();
        }
    }
}