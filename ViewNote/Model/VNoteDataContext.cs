using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace ViewNote.Model
{
    public class VNoteDataContext : DataContext
    {
        // Pass the connection string to the base class.
        public VNoteDataContext(string connectionString)
            : base(connectionString)
        { }

        // Specify a table for the to-do items.
        public Table<VNoteItem> Items;

        // Specify a table for the categories.
        public Table<VNoteCategory> Categories;
    }

    [Table]
    public class VNoteItem : INotifyPropertyChanged, INotifyPropertyChanging
    {

        // Define ID: private field, public property, and database column.
        private int _vNoteItemId;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int VNoteItemId
        {
            get { return _vNoteItemId; }
            set
            {
                if ( _vNoteItemId != value )
                {
                    NotifyPropertyChanging("VNoteItemId");
                    _vNoteItemId = value;
                    NotifyPropertyChanged("VNoteItemId");
                }
            }
        }

        // Define item name: private field, public property, and database column.
        private string _vNoteTitle;

        [Column]
        public string VNoteTitle
        {
            get { return _vNoteTitle; }
            set
            {
                if ( _vNoteTitle != value )
                {
                    NotifyPropertyChanging("VNoteTitle");
                    _vNoteTitle = value;
                    NotifyPropertyChanged("VNoteTitle");
                }
            }
        }

        private string _vNoteText;

        [Column]
        public string VNoteText
        {
            get { return _vNoteText; }
            set
            {
                if ( _vNoteText != value )
                {
                    NotifyPropertyChanging("VNoteText");
                    _vNoteText = value;
                    NotifyPropertyChanged("VNoteText");
                }
            }
        }

        private string _vNotePhoto;

        [Column]
        public string VNotePhoto
        {
            get { return _vNotePhoto; }
            set
            {
                if ( _vNotePhoto != value )
                {
                    NotifyPropertyChanging("VNotePhoto");
                    _vNotePhoto = value;
                    NotifyPropertyChanged("VNotePhoto");
                }
            }
        }

        private DateTime _vNoteDate;

        [Column(DbType = "DATETIME")]
        public DateTime VNoteDate
        {
            get { return _vNoteDate; }
            set
            {
                if ( _vNoteDate != value )
                {
                    NotifyPropertyChanging("VNoteDate");
                    _vNoteDate = value;
                    NotifyPropertyChanged("VNoteDate");
                }
            }
        }

        // Define completion value: private field, public property, and database column.
        private bool _isFavourite;

        [Column]
        public bool IsFavourite
        {
            get { return _isFavourite; }
            set
            {
                if ( _isFavourite != value )
                {
                    NotifyPropertyChanging("IsFavourite");
                    _isFavourite = value;
                    NotifyPropertyChanged("IsFavourite");
                }
            }
        }

        // Version column aids update performance.
        [Column(IsVersion = true)]
        private Binary _version;

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify that a property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if ( PropertyChanged != null )
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify that a property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if ( PropertyChanging != null )
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion

        // Internal column for the associated ToDoCategory ID value
        [Column]
        internal int _categoryId;

        // Entity reference, to identify the ToDoCategory "storage" table
        private EntityRef<VNoteCategory> _category;

        // Association, to describe the relationship between this key and that "storage" table
        [Association(Storage = "_category", ThisKey = "_categoryId", OtherKey = "Id", IsForeignKey = true)]
        public VNoteCategory Category
        {
            get { return _category.Entity; }
            set
            {
                NotifyPropertyChanging("Category");
                _category.Entity = value;

                if ( value != null )
                {
                    _categoryId = value.Id;
                }

                NotifyPropertyChanging("Category");
            }
        }
    }

    [Table]
    public class VNoteCategory : INotifyPropertyChanged, INotifyPropertyChanging
    {

        // Define ID: private field, public property, and database column.
        private int _id;

        [Column(DbType = "INT NOT NULL IDENTITY", IsDbGenerated = true, IsPrimaryKey = true)]
        public int Id
        {
            get { return _id; }
            set
            {
                NotifyPropertyChanging("Id");
                _id = value;
                NotifyPropertyChanged("Id");
            }
        }

        // Define category name: private field, public property, and database column.
        private string _name;

        [Column]
        public string Name
        {
            get { return _name; }
            set
            {
                NotifyPropertyChanging("Name");
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

        // Define the entity set for the collection side of the relationship.
        private EntitySet<VNoteItem> _notes;

        [Association(Storage = "_notes", OtherKey = "_categoryId", ThisKey = "Id")]
        public EntitySet<VNoteItem> Notes
        {
            get { return this._notes; }
            set { this._notes.Assign(value); }
        }


        // Assign handlers for the add and remove operations, respectively.
        public VNoteCategory()
        {
            _notes = new EntitySet<VNoteItem>(
                new Action<VNoteItem>(this.attach_Note), 
                new Action<VNoteItem>(this.detach_Note)
                );
        }

        // Called during an add operation
        private void attach_Note(VNoteItem note)
        {
            NotifyPropertyChanging("VNoteItem");
            note.Category = this;
        }

        // Called during a remove operation
        private void detach_Note(VNoteItem note)
        {
            NotifyPropertyChanging("VNoteItem");
            note.Category = null;
        }

        // Version column aids update performance.
        [Column(IsVersion = true)]
        private Binary _version;

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify that a property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if ( PropertyChanged != null )
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify that a property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if ( PropertyChanging != null )
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion
    }


}