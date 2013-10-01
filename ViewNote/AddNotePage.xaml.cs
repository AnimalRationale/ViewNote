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
using System.IO;


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
                // addedPhoto.Source = new BitmapImage(new Uri(e.OriginalFileName));
                addImageStatus.Text = "";

                WriteableBitmap writeableBitmap = new WriteableBitmap(1600, 1200);
                writeableBitmap.LoadJpeg(e.ChosenPhoto);

                string imageFolder = "ViewNotePhotos";
                string imageFolderTile = "Shared/ShellContent";

                string datetime = DateTime.Now.ToString().Replace("/", "");
                datetime = datetime.Replace(":", "");                
                datetime = datetime.Replace(" ", "");
                string imageFileName = "VNPhoto_" + datetime + ".jpg";                
                System.Diagnostics.Debug.WriteLine("Image filename: {0}", imageFileName);

                using ( var isoFile = IsolatedStorageFile.GetUserStoreForApplication() )
                {

                    if ( !isoFile.DirectoryExists(imageFolder) )
                    {
                        isoFile.CreateDirectory(imageFolder);
                    }

                    string filePath = System.IO.Path.Combine(imageFolder, imageFileName);
                    string filePathTile = System.IO.Path.Combine(imageFolderTile, imageFileName);
                    using ( var stream = isoFile.CreateFile(filePath) )
                    {
                        writeableBitmap.SaveJpeg(stream, writeableBitmap.PixelWidth, writeableBitmap.PixelHeight, 0, 100);
                        System.Diagnostics.Debug.WriteLine("Save filePath: {0}", filePath);
                    }
                    using ( var stream = isoFile.CreateFile(filePathTile) )
                    {
                        writeableBitmap.SaveJpeg(stream, writeableBitmap.PixelWidth, writeableBitmap.PixelHeight, 0, 100);
                        System.Diagnostics.Debug.WriteLine("Save filePathTile: {0}", filePathTile);
                    }
                }

                                                

                BitmapImage imageFromStorage = new BitmapImage();

                using ( var isoFile = IsolatedStorageFile.GetUserStoreForApplication() )
                {
                    string filePath = System.IO.Path.Combine(imageFolder, imageFileName);
                    System.Diagnostics.Debug.WriteLine("Read filePath: {0}", filePath);
                    using ( var imageStream = isoFile.OpenFile(
                        filePath, FileMode.Open, FileAccess.Read) )
                    {
                        imageFromStorage.SetSource(imageStream);                       
                        VNotePhotoFileName = imageFileName;
                        System.Diagnostics.Debug.WriteLine("Photo filename: {0}", VNotePhotoFileName);
                    }
                }

                addPhotoStatus.Text = "Photo added";
                addedPhoto.Source = imageFromStorage;                
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
                };

                // Add the item to the ViewModel.
                App.ViewModel.AddVNoteItem(newVNoteItem);
                ViewNote.MainPage.UpdateLiveTiles();
                
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