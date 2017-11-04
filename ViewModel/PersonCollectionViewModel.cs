using DRM.PropBag;
using DRM.PropBag.AutoMapperSupport;
using DRM.PropBag.ControlModel;
using DRM.TypeSafePropertyBag;
using MVVMApplication.Infra;
using MVVMApplication.Model;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MVVMApplication.ViewModel
{
    public partial class PersonCollectionViewModel : PropBag
    {
        //PropModel _pm;

        Business _business;

        public EventHandler ShowMessageBox = delegate { };
        public event EventHandler GridNeedsRefreshing;

        public PersonCollectionViewModel() { }

        public PersonCollectionViewModel(PropModel pm, string fullClassName, IPropFactory propFactory) : base(pm, fullClassName, propFactory)
        {
            System.Diagnostics.Debug.WriteLine("Constructing PersonCollectionViewModel -- with PropModel.");

            // Save a reference to the model used to defined our properties.
            //_pm = pm;

            //this.SelectedPersonChanged += PersonCollectionViewModel_SelectedPersonChanged;
            //this.PropertyChanged += PersonCollectionViewModel_PropertyChanged;
        }

        public ObservableCollection<PersonVM> GetMappedPeople(Business business)
        {
            _business = business;
            var people = business.Get();

            if (Mapper == null)
                return new ObservableCollection<PersonVM>();

            IEnumerable<PersonVM> mappedPeopleRaw = Mapper.MapToDestination(people);

            ObservableCollection<PersonVM> mappedPeople = new ObservableCollection<PersonVM>(mappedPeopleRaw);
            return mappedPeople;
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
            ObservableCollection<PersonVM> personList;
            try
            {
                personList = GetIt<ObservableCollection<PersonVM>>("PersonList");
                Person p = new Person();
                PersonVM newPerson = Mapper.MapToDestination(p);

                personList.Add(newPerson);

                SetIt<PersonVM>(newPerson, "SelectedPerson");
                OnGridNeedsRefreshing();

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

        public RelayCommand Save => new RelayCommand(SavePerson, true);

        private void SavePerson()
        {
            try
            {
                ObservableCollection<PersonVM> personList;
                personList = GetIt<ObservableCollection<PersonVM>>("PersonList");

                PersonVM selectedPerson = GetIt<PersonVM>("SelectedPerson");
                Person unMapped = GetUnMappedPerson(selectedPerson);
                //Person unMapped = personList.FirstOrDefault((x) => x.GetIt<int>("Id") == unMapped.Id);

                //bool newP = unMapped.Id == 0;

                _business.Update(unMapped);


                //if(TryGetListSource("PersonList", typeof(ObservableCollection<PersonVM>), out IListSource listSource))
                //{
                //    personList = (ObservableCollection<PersonVM>) listSource.GetList();
                //    PersonVM person = personList.FirstOrDefault((x) => x.GetIt<int>("Id") == unMapped.Id);

                //    Person unMapped = GetUnMappedPerson(selectedPerson);
                //    bool newP = unMapped.Id == 0;

                //    _business.Update(unMapped);
                //}

                OnGridNeedsRefreshing();

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

        public RelayCommand Delete => new RelayCommand(DeletePerson, true);

        private void DeletePerson()
        {
            PersonVM selectedPerson = GetIt<PersonVM>("SelectedPerson");

            if (selectedPerson == null) return;

            Person unMapped = GetUnMappedPerson(selectedPerson);
            _business.Delete(unMapped);

            if (selectedPerson != null)
                {
                ObservableCollection<PersonVM> personList = GetIt<ObservableCollection<PersonVM>>("PersonList");
                personList.Remove(selectedPerson);

                //if (TryGetListSource("PersonList", typeof(ObservableCollection<PersonVM>), out IListSource listSource))
                //{
                //    ObservableCollection<PersonVM> personList = (ObservableCollection<PersonVM>)listSource.GetList();
                //    personList.Remove(selectedPerson);
                //}

                OnGridNeedsRefreshing();

                ShowMessageBox(this, new MessageEventArgs()
                {
                    Message = "Selected Person has been removed!"
                });

           }
        }

        private void OnGridNeedsRefreshing()
        {
            GridNeedsRefreshing?.Invoke(this, new EventArgs());
        }

        #region PropBag Support

        private readonly string PERSON_INSTANCE_KEY = "PersonVM";

        private IPropBagMapper<Person, PersonVM> _mapper;
        private IPropBagMapper<Person, PersonVM> Mapper
        {
            get
            {
                if (_mapper == null)
                {
                    IPropBagMapperKey<Person, PersonVM> mapperRequest
                        = JustSayNo.AutoMapperProvider.RegisterMapperRequest<Person, PersonVM>
                         (
                             PERSON_INSTANCE_KEY,
                            typeof(PersonVM),
                            configPackageName: "Extra_Members", // "Emit_Proxy",
                            propFactory: null);

                    _mapper = JustSayNo.AutoMapperProvider.GetMapper(mapperRequest);

                }
                return _mapper;
            }
        }

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
