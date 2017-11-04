using DRM.PropBag;
using DRM.PropBag.ControlModel;
using DRM.TypeSafePropertyBag;
using MVVMApplication.Infra;
using MVVMApplication.Model;
using System;
using System.Collections.ObjectModel;

namespace MVVMApplication.ViewModel
{
    public class MainWindowViewModel : PropBag
    {
        //PropModel _pm;

        public EventHandler ShowMessageBox = delegate { };

        public MainWindowViewModel(PropModel pm, string fullClassName, IPropFactory propFactory) : base(pm, fullClassName, propFactory)
        {
            // Save a reference to the model used to defined our properties.
            //_pm = pm;

            System.Diagnostics.Debug.WriteLine("Beginning to construct MainWindowViewModel -- From PropModel.");

            // The Main View Model must initialize each of its child view models.
            Initialize_PersonCollectionVM();

            System.Diagnostics.Debug.WriteLine("Completed Constructing MainWindowViewModel -- From PropModel.");
        }

        private void Initialize_PersonCollectionVM()
        {
            // Intialize our Business property.
            Business b = new Business();
            SetIt(b, "Business");

            // Create a new PersonCollection ViewModel.
            string resourceKey = "PersonCollectionVM";

            object test = JustSayNo.ViewModelHelper.GetNewViewModel(resourceKey);

            //PersonCollectionViewModel pcvm = (PersonCollectionViewModel)JustSayNo.ViewModelHelper.GetNewViewModel(resourceKey);

            PersonCollectionViewModel pcvm = (PersonCollectionViewModel)test;


            // Get PersonList from the child View Model.
            ObservableCollection<PersonVM> mappedPeople = pcvm.GetMappedPeople(b);

            // Set the child View Model's PersonList to the new list.
            //pcvm.SetIt<ObservableCollection<PersonVM>>(mappedPeople, "PersonList");
            pcvm.SetIt(mappedPeople, "PersonList");

            PersonVM personVM = null;

            // Set the selected person to null. -- TODO: get the currently selected item.
            //pcvm.SetIt<PersonVM>(personVM, "SelectedPerson");
            pcvm.SetIt(personVM, "SelectedPerson");

            // Subscribe to our child ViewModel's ShowMessageBox event.
            pcvm.ShowMessageBox += delegate (object sender, EventArgs args)
            {
                // Pass messages from the child view model to our listeners (i.e., our parent window.)
                ShowMessageBox(sender, args);
            };

            // Set the value of our Property Named: PersonCollectionVM.
            //SetIt<PersonCollectionViewModel>(pcvm, "PersonCollectionVM");
            SetIt(pcvm, "PersonCollectionVM");


            //pcvm.SubscribeToPropChanged<PersonVM>(SelectedPersonChanged, "SelectedPerson");
        }

        //private void SelectedPersonChanged(PersonVM oldval, PersonVM newVal)
        //{
        //    //int a = 0;
        //}

    }
}
