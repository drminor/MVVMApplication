using DRM.PropBag;
using DRM.PropBag.ControlModel;
using DRM.TypeSafePropertyBag;

namespace MVVMApplication.ViewModel
{
    public class VanillaViewModelTemplate : PropBag
    {
        PropModel _pm;

        public VanillaViewModelTemplate() : base()
        {
            System.Diagnostics.Debug.WriteLine("Constructing PersonViewModel -- no Params.");
        }

        public VanillaViewModelTemplate(byte dummy) : base(dummy) { }

        public VanillaViewModelTemplate(PropBagTypeSafetyMode safetyMode) : base(safetyMode) { }

        public VanillaViewModelTemplate(PropBagTypeSafetyMode typeSafetyMode, AbstractPropFactory thePropFactory)
            : base(typeSafetyMode, thePropFactory) { }

        public VanillaViewModelTemplate(PropModel pm) : base(pm)
        {
            // Save a reference to the model used to defined our properties.
            _pm = pm;
        }
    }

  
}
