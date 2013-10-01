using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace ViewNote.Model
{
    public class VNoteDataContext : DataContext
    {
        // Connection string to the base class.
        public VNoteDataContext(string connectionString)
            : base(connectionString)
        { }

        // Table for VNoteItem items.
        public Table<VNoteItem> Items;

        // Table for categories.
        public Table<VNoteCategory> Categories;
    }

    [Table]
    public class VNoteItem : INotifyPropertyChanged, INotifyPropertyChanging
    {

        // ID: private field, public property, and database column.
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

        // Item name: private field, public property, and database column.
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

        // Completion value: private field, public property, and database column.
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

        // Version column for performance.
        [Column(IsVersion = true)]
        private Binary _version;

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Notify of property change
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

        // Notify of property being about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if ( PropertyChanging != null )
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion

        // Internal column for the associated VNoteCategory ID value
        [Column]
        internal int _categoryId;

        // Entity reference identifing the VnoteCategory "storage" table
        private EntityRef<VNoteCategory> _category;

        // Association describing relationship between this key and that "storage" table
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

        // ID: private field, public property, and database column.
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

        // Category name: private field, public property, and database column.
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

        // Entity set for collection side of relationship.
        private EntitySet<VNoteItem> _notes;

        [Association(Storage = "_notes", OtherKey = "_categoryId", ThisKey = "Id")]
        public EntitySet<VNoteItem> Notes
        {
            get { return this._notes; }
            set { this._notes.Assign(value); }
        }


        // Handlers for add and remove actions.
        public VNoteCategory()
        {
            _notes = new EntitySet<VNoteItem>(
                new Action<VNoteItem>(this.attach_Note), 
                new Action<VNoteItem>(this.detach_Note)
                );
        }

        // Add action
        private void attach_Note(VNoteItem note)
        {
            NotifyPropertyChanging("VNoteItem");
            note.Category = this;
        }

        // Remove action
        private void detach_Note(VNoteItem note)
        {
            NotifyPropertyChanging("VNoteItem");
            note.Category = null;
        }

        // Version column for performance.
        [Column(IsVersion = true)]
        private Binary _version;

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Notify property change
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

        // Notify property is about to change
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