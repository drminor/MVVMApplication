using DRM.PropBag;
using DRM.PropBag.ControlModel;
using DRM.TypeSafePropertyBag;
using MVVMApplication.Infra;
using MVVMApplication.Model;

namespace MVVMApplication.ViewModel
{
    public class MainWindowViewModel : PropBag
    {
        public MainWindowViewModel(PropModel pm, string fullClassName, IPropFactory propFactory)
            : base(pm, fullClassName, propFactory)
        {
            System.Diagnostics.Debug.WriteLine("Beginning to construct MainWindowViewModel -- From PropModel.");

            //// TODO: This should be part of the PropModel.
            //// Have our WMessage property updated, anytime the value of the WMessage property on our child PersonCollection VM changes.
            //RegisterBinding<string>("WMessage", "./PersonCollectionVM/WMessage");

            //// The Main View Model must initialize each of its child view models.
            //Initialize_PersonCollectionVM();

            ////Intialize our Business property.
            //Business business = new Business();
            //SetIt(business, "Business");

            //PersonCollectionViewModel pcvm = GetIt<PersonCollectionViewModel>("PersonCollectionVM");
            //pcvm.SetIt<Business>(business, "Business");

            System.Diagnostics.Debug.WriteLine("Completed Constructing MainWindowViewModel -- From PropModel.");
        }

        //private void Initialize_PersonCollectionVM()
        //{
        //    // Create a new PersonCollection ViewModel.
        //    string resourceKey = "PersonCollectionVM";
        //    PersonCollectionViewModel pcvm = (PersonCollectionViewModel)JustSayNo.ViewModelHelper.GetNewViewModel(resourceKey);
        //    SetIt(pcvm, "PersonCollectionVM");
        //}

        public void Start()
        {
            Business business = new Business();
            SetIt(business, "Business");
        }

    }
}
