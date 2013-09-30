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
using ViewNote.Model;


namespace ViewNote
{
    public partial class AddNote : PhoneApplicationPage
    {
        CameraCaptureTask cameraCaptureTask;
        PhotoChooserTask photoChooserTask;
        static string VNotePhotoFileName;

        public AddNote()
        {
            InitializeComponent();
            // Set the page DataContext property to the ViewModel.
            this.DataContext = App.ViewModel;
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
                //addPhotoStatus.Text = "Photo added";
                addPhotoStatus.Text = e.OriginalFileName;
                VNotePhotoFileName = e.OriginalFileName;
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
                //addImageStatus.Text = "Image added";
                addImageStatus.Text = e.OriginalFileName;
                VNotePhotoFileName = e.OriginalFileName;
            }
        }

        private void appbarCheck_Click(object sender, EventArgs e)
        {
            if ( newNoteTitleTextBox.Text.Length > 0 || newNoteContentTextBox.Text.Length > 0 )
            {
                // Create a new to-do item.
                // VNoteCategory fixedCat = new VNoteCategory();
                VNoteItem newVNoteItem = new VNoteItem
                {
                    VNoteTitle = newNoteTitleTextBox.Text,
                    VNoteText = newNoteContentTextBox.Text,
                    VNotePhoto = VNotePhotoFileName,
                    VNoteDate = DateTime.Now,
                    Category = (VNoteCategory)categoriesListPicker.SelectedItem
                   //  Category = fixedCat
                };

                // Add the item to the ViewModel.
                App.ViewModel.AddVNoteItem(newVNoteItem);

                // Return to the main page.
                if ( NavigationService.CanGoBack )
                {
                    NavigationService.GoBack();
                }
            }            
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
            System.Diagnostics.Debug.WriteLine("Orientation changed");

            if ( e.Orientation == PageOrientation.Landscape || e.Orientation == PageOrientation.LandscapeLeft || e.Orientation == PageOrientation.LandscapeRight )
            {
                pivotAddNoteText.Header = null;
                pivotAddNotePhoto.Header = null;
                buttonAddPhoto.Width = 181;
                buttonAddImage.Width = 181;
                iconAddPhoto.Margin = new Thickness(0, -28, 0, 0);
                iconAddImage.Margin = new Thickness(0, -28, 0, 0);
                pivotAddNote.Margin = new Thickness(0, -150, 0, 0);
            }
            else
            {
                pivotAddNoteText.Header = "Note";
                pivotAddNoteText.Header = "Photo";
                buttonAddPhoto.Width = 121;
                buttonAddImage.Width = 121;
                iconAddPhoto.Margin = new Thickness(0, 0, 0, 0);
                iconAddImage.Margin = new Thickness(0, 0, 0, 0);
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