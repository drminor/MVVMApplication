using DRM.PropBag;
using DRM.PropBag.ControlModel;
using MVVMApplication.Infra;

namespace MVVMApplication.ViewModel
{
    public class PersonVM : PropBagMin
    {
        PropModel _pm;

        public PersonVM()
        {
            System.Diagnostics.Debug.WriteLine("Constructing PersonViewModel -- no Params.");
        }

        public PersonVM(byte dummy) : base(dummy) { }

        public PersonVM(PropBagTypeSafetyMode safetyMode) : base(safetyMode) { }

        public PersonVM(PropBagTypeSafetyMode typeSafetyMode, AbstractPropFactory thePropFactory)
            : base(typeSafetyMode, thePropFactory) { }

        // TODO: Fix this
        // Either build a universal Type Resolver
        // or better yet, have GetVal and SetVal use Types instead of strings to indentify the Type.
        public PersonVM(PropModel pm) : this(pm, SettingsExtensions.ThePropFactory) { }

        public PersonVM(PropModel pm, IPropFactory pf) : base(pm, pf)
        {
            // Save a reference to the model used to defined our properties.
            _pm = pm;
        }
    }

  
}
