using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media.Imaging;
// Directive for the data model.
using ViewNote.Model;


namespace ViewNote.ViewModel
{
    public class ViewNoteViewModel : INotifyPropertyChanged
    {
        // LINQ to SQL data context for local database.
        private VNoteDataContext viewNoteDB;

        // Class constructor creating the data context object.
        public ViewNoteViewModel(string viewNoteDBConnectionString)
        {
            viewNoteDB = new VNoteDataContext(viewNoteDBConnectionString);
        }

        // All Notes items.
        private ObservableCollection<VNoteItem> _allNotesItems;
        public ObservableCollection<VNoteItem> AllNotesItems
        {
            get { return _allNotesItems; }
            set
            {
                _allNotesItems = value;
                NotifyPropertyChanged("AllNotesItems");
            }
        }
                

        // Notes items associated with Memo category.
        private ObservableCollection<VNoteItem> _memoNotesItems;
        public ObservableCollection<VNoteItem> MemoNotesItems
        {
            get { return _memoNotesItems; }
            set
            {
                _memoNotesItems = value;
                NotifyPropertyChanged("MemoNotesItems");
            }
        }

        // Notes items associated with Travel category.
        private ObservableCollection<VNoteItem> _travelNotesItems;
        public ObservableCollection<VNoteItem> TravelNotesItems
        {
            get { return _travelNotesItems; }
            set
            {
                _travelNotesItems = value;
                NotifyPropertyChanged("TravelNotesItems");
            }
        }

        // Notes items associated with Fun category.
        private ObservableCollection<VNoteItem> _funNotesItems;
        public ObservableCollection<VNoteItem> FunNotesItems
        {
            get { return _funNotesItems; }
            set
            {
                _funNotesItems = value;
                NotifyPropertyChanged("FunNotesItems");
            }
        }

        // A list of all categories used in AddNotePage.
        private List<VNoteCategory> _categoriesList;
        public List<VNoteCategory> CategoriesList
        {
            get { return _categoriesList; }
            set
            {
                _categoriesList = value;
                NotifyPropertyChanged("CategoriesList");
            }
        }

        // Query database to load collections and list used by pivot pages.
        public void LoadCollectionsFromDatabase()
        {

            // Query for all VNote items in the database.
            var viewNoteItemsInDB = from VNoteItem note in viewNoteDB.Items
                                    select note;

            // Query the database and load all VNotes items.
            AllNotesItems = new ObservableCollection<VNoteItem>(viewNoteItemsInDB);

            // Query for all categories in the database.
            var viewNoteCategoriesInDB = from VNoteCategory category in viewNoteDB.Categories
                                         select category;


            // Query the database and load all associated items to proper collections.
            foreach ( VNoteCategory category in viewNoteCategoriesInDB )
            {
                switch ( category.Name )
                {
                    case "Memo":
                        MemoNotesItems = new ObservableCollection<VNoteItem>(category.Notes);
                        break;
                    case "Travel":
                        TravelNotesItems = new ObservableCollection<VNoteItem>(category.Notes);
                        break;
                    case "Fun":
                        FunNotesItems = new ObservableCollection<VNoteItem>(category.Notes);
                        break;
                    default:
                        break;
                }
            }

            // Load a list of all categories.
            CategoriesList = viewNoteDB.Categories.ToList();
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

        // Add VNote item to the database and collections.
        public void AddVNoteItem(VNoteItem newVNoteItem)
        {
            // Add VNote item to the data context.
            viewNoteDB.Items.InsertOnSubmit(newVNoteItem);

            // Save changes to the database.
            viewNoteDB.SubmitChanges();

            // Add VNote item to the "all" observable collection.
            AllNotesItems.Add(newVNoteItem);

            // Add VNote item to the appropriate filtered collection.
            switch ( newVNoteItem.Category.Name )
            {
                case "Memo":
                    MemoNotesItems.Add(newVNoteItem);
                    break;
                case "Travel":
                    TravelNotesItems.Add(newVNoteItem);
                    break;
                case "Fun":
                    FunNotesItems.Add(newVNoteItem);
                    break;
                default:
                    break;
            }
        }

        // Remove VNote item from the database and collections.
        public void DeleteVNoteItem(VNoteItem noteToDelete)
        {

            // Remove Vnote item from "all" observable collection.
            AllNotesItems.Remove(noteToDelete);

            // Remove VNote item from  data context.
            viewNoteDB.Items.DeleteOnSubmit(noteToDelete);

            // Remove VNote from category.   
            switch ( noteToDelete.Category.Name )
            {
                case "Memo":
                    MemoNotesItems.Remove(noteToDelete);
                    break;
                case "Travel":
                    TravelNotesItems.Remove(noteToDelete);
                    break;
                case "Fun":
                    FunNotesItems.Remove(noteToDelete);
                    break;
                default:
                    break;
            }

            // Save changes to the database.
            viewNoteDB.SubmitChanges();
        }


        public void DeleteAllVNoteItems()
        {
            var allNotes = from VNoteItem note in viewNoteDB.Items
                           select note;
            viewNoteDB.Items.DeleteAllOnSubmit(allNotes);
            System.Diagnostics.Debug.WriteLine("Count after deleteAll: {0}", allNotes.Count());
            viewNoteDB.SubmitChanges();
            AllNotesItems.Clear();
            MemoNotesItems.Clear();
            TravelNotesItems.Clear();
            FunNotesItems.Clear();

        }


        // Write changes in data context to database.
        public void SaveChangesToDB()
        {
            viewNoteDB.SubmitChanges();
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Notify app about property change.
        private void NotifyPropertyChanged(string propertyName)
        {
            if ( PropertyChanged != null )
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}