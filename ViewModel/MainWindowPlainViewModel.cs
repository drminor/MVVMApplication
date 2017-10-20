using MVVMApplication.Infra;
using MVVMApplication.Model;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MVVMApplication.ViewModel
{
    public class MainWindowPlainViewModel : NotificationClass
    {
        public EventHandler ShowMessageBox = delegate { };

        Business _business;

        public MainWindowPlainViewModel() 
        {
            if(SettingsExtensions.InDesignMode())
            {
                _business = null;
            }
            else
            {
                _business = new Business();
            }

            PersonCollectionPlainViewModel = new PersonCollectionPlainViewModel(_business);

            personCollectionPlainViewModel.ShowMessageBox += delegate (object sender, EventArgs args)
            {
                // Pass messages from the child view model to our listeners (i.e., our parent window.)
                ShowMessageBox(sender, args);
            };

        }

        private PersonCollectionPlainViewModel personCollectionPlainViewModel;
        public PersonCollectionPlainViewModel PersonCollectionPlainViewModel
        {
            get { return personCollectionPlainViewModel; }
            set
            {
                personCollectionPlainViewModel = value;
                OnPropertyChanged(nameof(PersonCollectionPlainViewModel));
            }
        }


    }
}
