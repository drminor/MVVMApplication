using DRM.PropBag;
using DRM.PropBag.ControlModel;
using DRM.PropBag.ControlsWPF;
using DRM.PropBag.ControlsWPF.WPFHelpers;
using DRM.TypeSafePropertyBag;
using MVVMApplication.Infra;
using MVVMApplication.Model;
using System;

namespace MVVMApplication.ViewModel
{
    public partial class MainWindowViewModel : PropBagMin
    {
        //PropModel _pm;

        public EventHandler ShowMessageBox = delegate { };

        public MainWindowViewModel() : this(PropBagTemplateResources.GetPropBagTemplate("MainWindowVM").GetPropModel(),
                  SettingsExtensions.ThePropFactory)
        {
            System.Diagnostics.Debug.WriteLine("Beginning to construct MainWindowViewModel, using default constructor, that loads a PropModel.");
            System.Diagnostics.Debug.WriteLine("Completed Constructing MainWindowViewModel, using default constructor, that loads a PropModel.");
        }

        public MainWindowViewModel(PropModel pm, IPropFactory pf) : base(pm, pf)
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
            SetIt(new Business(), "Business");

            string instanceKey = "PersonCollectionVM";
            PersonCollectionViewModel pcvm
                = AutoMapperHelpers.GetNewViewModel<PersonCollectionViewModel>
                (instanceKey: instanceKey, propFactory: SettingsExtensions.ThePropFactory);

            pcvm.ShowMessageBox += delegate (object sender, EventArgs args)
            {
                // Pass messages from the child view model to our listeners (i.e., our parent window.)
                ShowMessageBox(sender, args);
            };

            // Set the value of our Property Named: PersonCollectionVM.
            string propertyName = "PersonCollectionVM";
            SetIt<PersonCollectionViewModel>(pcvm, propertyName);

            PopPeople();

            //pcvm.SubscribeToPropChanged<PersonVM>(SelectedPersonChanged, "SelectedPerson");
        }

        public void PopPeople()
        {
            PersonCollectionViewModel pcvm = GetIt<PersonCollectionViewModel>("PersonCollectionVM");
            Business b = GetIt<Business>("Business");
            pcvm.PopPeople(b);

            PersonListC test = pcvm.GetIt<PersonListC>("PersonList");
        }

        //private void SelectedPersonChanged(PersonVM oldval, PersonVM newVal)
        //{
        //    //int a = 0;
        //}

    }
}
