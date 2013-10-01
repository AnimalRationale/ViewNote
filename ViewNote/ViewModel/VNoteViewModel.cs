using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

// Directive for the data model.
using ViewNote.Model;


namespace ViewNote.ViewModel
{
    public class ViewNoteViewModel : INotifyPropertyChanged
    {
        // LINQ to SQL data context for the local database.
        private VNoteDataContext viewNoteDB;

        // Class constructor, create the data context object.
        public ViewNoteViewModel(string viewNoteDBConnectionString)
        {
            viewNoteDB = new VNoteDataContext(viewNoteDBConnectionString);
        }

        // All to-do items.
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
                

        // To-do items associated with the home category.
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

        // To-do items associated with the work category.
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

        // To-do items associated with the hobbies category.
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

        // A list of all categories, used by the add task page.
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

        // Query database and load the collections and list used by the pivot pages.
        public void LoadCollectionsFromDatabase()
        {

            // Specify the query for all to-do items in the database.
            var viewNoteItemsInDB = from VNoteItem note in viewNoteDB.Items
                                    select note;

            // Query the database and load all to-do items.
            AllNotesItems = new ObservableCollection<VNoteItem>(viewNoteItemsInDB);

            // Specify the query for all categories in the database.
            var viewNoteCategoriesInDB = from VNoteCategory category in viewNoteDB.Categories
                                         select category;


            // Query the database and load all associated items to their respective collections.
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

        // Add a to-do item to the database and collections.
        public void AddVNoteItem(VNoteItem newVNoteItem)
        {
            // Add a to-do item to the data context.
            viewNoteDB.Items.InsertOnSubmit(newVNoteItem);

            // Save changes to the database.
            viewNoteDB.SubmitChanges();

            // Add a to-do item to the "all" observable collection.
            AllNotesItems.Add(newVNoteItem);

            // Add a to-do item to the appropriate filtered collection.
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

        // Remove a to-do task item from the database and collections.
        public void DeleteVNoteItem(VNoteItem noteToDelete)
        {

            // Remove the to-do item from the "all" observable collection.
            AllNotesItems.Remove(noteToDelete);

            // Remove the to-do item from the data context.
            viewNoteDB.Items.DeleteOnSubmit(noteToDelete);

            // Remove the to-do item from the appropriate category.   
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


        // Write changes in the data context to the database.
        public void SaveChangesToDB()
        {
            viewNoteDB.SubmitChanges();
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify the app that a property has changed.
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