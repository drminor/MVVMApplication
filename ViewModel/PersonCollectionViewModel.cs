using DRM.PropBag.ControlsWPF.WPFHelpers;
using DRM.TypeSafePropertyBag;
using MVVMApplication.Infra;
using MVVMApplication.Model;

using DRM.PropBag.ControlsWPF;

using System.Linq;
using System;
using DRM.PropBag.ControlModel;
using DRM.PropBag.AutoMapperSupport;
using DRM.PropBag;

namespace MVVMApplication.ViewModel
{
    public partial class PersonCollectionViewModel : PropBagMin
    {
        //PropModel _pm;

        Business _business;
        public EventHandler ShowMessageBox = delegate { };
        public event EventHandler GridNeedsRefreshing = delegate { };


        public PersonCollectionViewModel() : this(PropBagTemplateResources.GetPropBagTemplate("PersonCollectionVM").GetPropModel(),
                  SettingsExtensions.ThePropFactory)
        {
            System.Diagnostics.Debug.WriteLine("Constructing PersonCollectionViewModel -- using PropBagTemplateResources.");
        }

        public PersonCollectionViewModel(PropModel pm, IPropFactory pf) : base(pm, pf)
        {
            System.Diagnostics.Debug.WriteLine("Constructing PersonCollectionViewModel -- with PropModel.");

            // Save a reference to the model used to defined our properties.
            //_pm = pm;

            //this.SelectedPersonChanged += PersonCollectionViewModel_SelectedPersonChanged;
            //this.PropertyChanged += PersonCollectionViewModel_PropertyChanged;
        }

        public void PopPeople(Business business)
        {
            _business = business;
            var people = business.Get();

            if (Mapper == null) return;

            var mappedPeople = Mapper.MapToDestination(people);

            PersonListC pList = new PersonListC(mappedPeople);
            base.SetIt<PersonListC>(pList, "PersonList");
        }

        public RelayCommand Add
        {
            get
            {
                var x = new RelayCommand(AddPerson, true);
                x.CanExecuteChanged += X_CanExecuteChanged;
                return x;

            }
        }

        private void X_CanExecuteChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("The state of 'CanExecute' has changed for the Add command.");
        }

        private void AddPerson()
        {
            // This is the code needed for Mapping Strategy = ExtraMembers.

            try
            {
                Person p = new Person();
                PersonVM newPerson = Mapper.MapToDestination(p);
                SetIt<PersonVM>(newPerson, "SelectedPerson");
            }
            catch (Exception ex)
            {
                ShowMessageBox(this, new MessageEventArgs()
                {
                    Message = ex.Message
                });
            }
        }

        public RelayCommand Save
        {
            get
            {
                return new RelayCommand(SavePerson, true);
            }
        }

        private void SavePerson()
        {
            try
            {
                PersonVM selectedPerson = GetIt<PersonVM>("SelectedPerson");

                Person unMapped = GetUnMappedPerson(selectedPerson);
                bool newP = unMapped.Id == 0;

                _business.Update(unMapped);

                if (newP)
                {
                    PopPeople(_business);
                }
                else
                {
                    OnGridNeedsRefreshing();
                }

                ShowMessageBox(this, new MessageEventArgs()
                {
                    Message = "Changes are saved !"
                });
            }
            catch (Exception ex)
            {
                ShowMessageBox(this, new MessageEventArgs()
                {
                    Message = ex.Message
                });
            }
        }

        public RelayCommand Delete
        {
            get
            {
                return new RelayCommand(DeletePerson, true);
            }
        }

        private void DeletePerson()
        {
            PersonVM selectedPerson = GetIt<PersonVM>("SelectedPerson");

            if (selectedPerson == null) return;

            Person unMapped = GetUnMappedPerson(selectedPerson);
            _business.Delete(unMapped);

            PopPeople(_business);
            ShowMessageBox(this, new MessageEventArgs()
            {
                Message = "Items has been deleted!"
            });

        }

        private void OnGridNeedsRefreshing()
        {
            GridNeedsRefreshing?.Invoke(this, new EventArgs());
        }

        #region PropBag Support

        private readonly string PERSON_VM_INSTANCE_KEY = "PersonVM";

        private PropBagMapper<Person, PersonVM> _mapper;
        private PropBagMapper<Person, PersonVM> Mapper
        {
            get
            {
                if (_mapper == null)
                {
                    _mapper = AutoMapperHelpers.GetMapper<Person, PersonVM>(PERSON_VM_INSTANCE_KEY);
                }
                return _mapper;
            }
        }


        //private PropBagMapper<PersonVM, Person> _mapperB;
        //private PropBagMapper<PersonVM, Person> MapperB
        //{
        //    get
        //    {
        //        if (_mapperB == null)
        //        {
        //            _mapperB = AutoMapperHelpers.GetMapper<PersonVM, Person>(PERSON_VM_INSTANCE_KEY);
        //        }
        //        return _mapperB;
        //    }
        //}

        private Person GetUnMappedPerson(PersonVM mappedPerson)
        {
            if (Mapper == null) return null;

            if (mappedPerson == null) return null;
            return Mapper.MapToSource(mappedPerson, new Person());

            //return new Person()
            //{
            //    FirstName = mappedPerson.GetIt<string>("FirstName"),
            //    LastName = mappedPerson.GetIt<string>("LastName"),
            //    CityOfResidence = mappedPerson.GetIt<string>("CityOfResidence"),
            //    Profession = mappedPerson.GetIt<Profession>("Profession"),
            //    Id = mappedPerson.GetIt<int>("Id")
            //};
        }

        #endregion

        #region Unused

        //private void PersonCollectionViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    string ss = e.PropertyName;
        //    //this.OnPropertyChanged("SelectedPerson");
        //}

        //private void PersonCollectionViewModel_SelectedPersonChanged(object sender, DRM.TypeSafePropertyBag.PropertyChangedWithTValsEventArgs<PersonVM> e)
        //{
        //    //base.OnPropertyChanged("SelectedPerson2");
        //    System.Diagnostics.Debug.WriteLine("The SelectedPersonChanged event was raised.");
        //}

        //IProp<PersonListC> _personListCached;
        //private IProp<PersonListC> PersonListCached
        //{
        //    get
        //    {
        //        if(_personListCached == null)
        //        {
        //            _personListCached = base.GetTypedProp<PersonListC>("PersonList");
        //        }
        //        return _personListCached;
        //    }
        //}
        //private PersonListC PList
        //{
        //    get
        //    {
        //        return PersonListCached.TypedValue;
        //        //return base.GetIt<PersonListC>("PesonList");
        //    }
        //    set
        //    {
        //        PersonListCached.TypedValue = value;
        //    }
        //}

        //PersonVM _sel;
        //public PersonVM SelectedPerson2
        //{
        //    get
        //    {
        //        //return _sel;
        //        return base.GetIt<PersonVM>("SelectedPerson");
        //    }
        //    set
        //    {
        //        //_sel = value;
        //        base.SetIt<PersonVM>(value, "SelectedPerson");
        //        //base.OnPropertyChanged("SelectedPerson2");
        //    }
        //}


        //public event PropertyChangedWithTValsHandler<PersonVM> SelectedPersonChanged
        //{
        //    add
        //    {
        //        AddToPropChanged<PersonVM>(value, nameof(SelectedPersonChanged));
        //    }
        //    remove
        //    {
        //        RemoveFromPropChanged<PersonVM>(value, nameof(SelectedPersonChanged));
        //    }
        //}

        #endregion

    }
}
