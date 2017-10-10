using DRM.PropBag;
using DRM.PropBag.AutoMapperSupport;
using DRM.PropBag.ControlModel;
using DRM.PropBag.ControlsWPF;
using DRM.PropBag.ViewModelBuilder;
using MVVMApplication.Infra;
using MVVMApplication.Model;
using System;
using System.Collections.Generic;
using System.Windows;

namespace MVVMApplication.ViewModel
{
    public partial class PersonCollectionViewModel : PropBagMin
    {
        PropModel _pm;

        public PersonCollectionViewModel(byte dummy) : base(dummy) { }

        public PersonCollectionViewModel(PropBagTypeSafetyMode safetyMode)
            : base(safetyMode)
        {
            System.Diagnostics.Debug.WriteLine("Constructing PersonCollectionViewModel -- with PropModel.");
        }

        public PersonCollectionViewModel(PropBagTypeSafetyMode typeSafetyMode, IPropFactory thePropFactory)
            : base(typeSafetyMode, thePropFactory) { }

        public PersonCollectionViewModel(PropModel pm, IPropFactory pf) : base(pm, pf)
        {
            // Save a reference to the model used to defined our properties.
            _pm = pm;

            this.SelectedPersonChanged += PersonCollectionViewModel_SelectedPersonChanged;
        }

        private void PersonCollectionViewModel_SelectedPersonChanged(object sender, DRM.TypeSafePropertyBag.PropertyChangedWithTValsEventArgs<PersonVM> e)
        {
            base.OnPropertyChanged("SelectedPerson2");
        }

        private readonly string PERSON_VM_INSTANCE_KEY = "PersonVM";

        private PropBagMapper<Person, PersonVM> _mapper;
        private PropBagMapper<Person, PersonVM> Mapper
        {
            get
            {
                if(_mapper == null)
                {
                    _mapper = AutoMapperHelpers.GetMapper<Person, PersonVM>(PERSON_VM_INSTANCE_KEY);
                }
                return _mapper;
            }
        }

    }
}
