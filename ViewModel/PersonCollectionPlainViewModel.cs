using DRM.PropBag.ControlsWPF;
using MVVMApplication.Infra;
using MVVMApplication.Model;
using MVVMApplication.Services;
using System;
using System.Collections.ObjectModel;

namespace MVVMApplication.ViewModel
{
    public class PersonCollectionPlainViewModel : NotificationClass
    {
        public event EventHandler<MessageEventArgs> MessageHasArrived;

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
                var x = new RelayCommand(AddPerson);
                x.CanExecuteChanged += X_CanExecuteChanged;
                return x;

            }
        }

        private void X_CanExecuteChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("The state of 'CanExecute' has changed for the Add command.");
        }

        private void AddPerson(object o)
        {
            try
            {
                SelectedPerson = new Person();
                OnPropertyChanged(nameof(SelectedPerson));
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }
        }

        public RelayCommand Save
        {
            get
            {
                return new RelayCommand(SavePerson);
            }
        }

        private void SavePerson(object o)
        {
            try
            {
                _business.Update(SelectedPerson);
                OnPropertyChanged(nameof(PersonCollection));

                ShowMessage("Changes are saved !");

            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }

        }

        public RelayCommand Delete
        {
            get
            {
                return new RelayCommand(DeletePerson);
            }
        }

        private void DeletePerson(object o)
        {
            _business.Delete(SelectedPerson);
            OnPropertyChanged(nameof(PersonCollection));
        }

        private void ShowMessage(string msg)
        {
            MessageHasArrived?.Invoke(this, new MessageEventArgs(msg));
        }
    }
}
