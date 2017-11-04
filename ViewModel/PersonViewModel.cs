using DRM.PropBag;
using DRM.PropBag.ControlModel;
using DRM.TypeSafePropertyBag;
using MVVMApplication.Infra;

namespace MVVMApplication.ViewModel
{
    public class PersonVM : PropBag
    {
        PropModel _pm;

        public PersonVM()
        {
            System.Diagnostics.Debug.WriteLine("Constructing PersonViewModel -- no Params.");
        }

        // TODO: Fix this
        // Either build a universal Type Resolver
        // or better yet, have GetVal and SetVal use Types instead of strings to indentify the Type.
        public PersonVM(PropModel pm, string fullClassName, IPropFactory propFactory) : base(pm, fullClassName, propFactory)
        {
            // Save a reference to the model used to define our properties.
            _pm = pm;
        }
    }

  
}
