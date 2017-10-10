
using DRM.PropBag;
using DRM.PropBag.ControlModel;
using MVVMApplication.Model;

using MVVMApplication.Infra;
using DRM.PropBag.ControlsWPF;

namespace MVVMApplication.ViewModel
{
    public partial class MainWindowViewModel : PropBagMin
    {
        PropModel _pm;

        public MainWindowViewModel(byte dummy) : base(dummy) { }

        public MainWindowViewModel(PropBagTypeSafetyMode safetyMode) : base(safetyMode) { }

        public MainWindowViewModel(PropBagTypeSafetyMode typeSafetyMode, AbstractPropFactory thePropFactory)
            : base(typeSafetyMode, thePropFactory) { }

        public MainWindowViewModel(PropModel pm, IPropFactory pf) : base(pm, pf)
        {
            // Save a reference to the model used to defined our properties.
            _pm = pm;

            

            System.Diagnostics.Debug.WriteLine("Beginning to construct MainWindowViewModel -- From PropModel.");

            InitMe();

            PopPeople();
            System.Diagnostics.Debug.WriteLine("Completed Constructing MainWindowViewModel -- From PropModel.");
        }

        private void InitMe()
        {
            base.SetIt<Business>(new Business(), "Business");
            //Business = new Business()

            //var pcvm = new PersonCollectionViewModel(PropBagTypeSafetyMode.Tight);
            //string pbtResourceKey = "PersonCollectionVM";
            //PropBagTemplate pbt = AutoMapperHelpers.PropBagTemplates[pbtResourceKey];
            //PersonCollectionViewModel pcvm = AutoMapperHelpers.GetNewViewModel<PersonCollectionViewModel>(pbt);

            string instanceKey = "PersonCollectionVM";
            PersonCollectionViewModel pcvm 
                = AutoMapperHelpers.GetNewViewModel<PersonCollectionViewModel>
                (instanceKey: instanceKey, propFactory: SettingsExtensions.ThePropFactory);

            pcvm.SubscribeToPropChanged<PersonVM>(SelectedPersonChanged, "SelectedPerson");

            string propertyName = "PersonCollectionVM";
            base.SetIt<PersonCollectionViewModel>(pcvm, propertyName);
            //PersonCollectionVM = new PersonCollectionViewModel(PropBagTypeSafetyMode.Tight);
        }

        private void SelectedPersonChanged(PersonVM oldval, PersonVM newVal)
        {
            //int a = 0;
        }

    }
}
