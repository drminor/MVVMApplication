using DRM.PropBag.Collections;
using MVVMApplication.Infra;
using MVVMApplication.Model;
using System;
using System.Collections.ObjectModel;

namespace MVVMApplication.ViewModel
{
    public class PersonCollectionPlainViewModel : NotificationClass
    {
        public EventHandler ShowMessageBox = delegate { };

        Business _business;

        public PersonCollectionPlainViewModel(Business business)
        {
            System.Diagnostics.Debug.WriteLine("Constructing PersonCollectionPlainViewModel.");

            _business = business;
            if (JustSayNo.InDesignMode())
            {
                PersonCollection = new ObservableCollection<Person>();
            }
            else
            {
                PersonCollection = new ObservableCollection<Person>(business.Get());
            }
            //if (SettingsExtensions.InDesignMode())
            //{
            //    PersonCollection = new PbCollection<Person>();
            //}
            //else
            //{
            //    PersonCollection = new PbCollection<Person>(business.Get());
            //}
        }

        private ObservableCollection<Person> personCollection;
        public ObservableCollection<Person> PersonCollection
        {
            get { return personCollection; }
            set
            {
                personCollection = value;
                OnPropertyChanged(nameof(PersonCollection));
            }
        }

        //private PbCollection<Person> personCollection;
        //public PbCollection<Person> PersonCollection
        //{
        //    get { return personCollection; }
        //    set
        //    {
        //        personCollection = value;
        //        OnPropertyChanged(nameof(PersonCollection));
        //    }
        //}

        private Person _person;
        public Person SelectedPerson
        {
            get
            {
                return _person;
            }
            set
            {
                _person = value;
                OnPropertyChanged(nameof(SelectedPerson));
            }
        }

        public RelayCommand Add
        {
            get
            {
                var x = new RelayCommand(AddPerson, true);
                x.CanExecuteChanged += X_CanExecuteChanged;
                return x;

            }
        }

        private void X_CanExecuteChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("The state of 'CanExecute' has changed for the Add command.");
        }

        private void AddPerson()
        {
            try
            {
                SelectedPerson = new Person();
                OnPropertyChanged(nameof(SelectedPerson));
            }
            catch (Exception ex)
            {
                ShowMessageBox(this, new MessageEventArgs()
                {
                    Message = ex.Message
                });
            }
        }

        public RelayCommand Save
        {
            get
            {
                return new RelayCommand(SavePerson, true);
            }
        }

        private void SavePerson()
        {
            try
            {
                _business.Update(SelectedPerson);
                OnPropertyChanged(nameof(PersonCollection));

                ShowMessageBox(this, new MessageEventArgs()
                {
                    Message = "Changes are saved !"
                });
            }
            catch (Exception ex)
            {
                ShowMessageBox(this, new MessageEventArgs()
                {
                    Message = ex.Message
                });
            }

        }

        public RelayCommand Delete
        {
            get
            {
                return new RelayCommand(DeletePerson, true);
            }
        }

        private void DeletePerson()
        {
            _business.Delete(SelectedPerson);
            OnPropertyChanged(nameof(PersonCollection));
        }
    }
}
