using DRM.PropBag;
using DRM.PropBag.AutoMapperSupport;
using DRM.PropBag.ControlModel;
using DRM.PropBag.ControlsWPF;
using DRM.TypeSafePropertyBag;

using MVVMApplication.Infra;
using MVVMApplication.Model;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace MVVMApplication.ViewModel
{
    public partial class PersonCollectionViewModel : PropBag
    {
        public event EventHandler GridNeedsRefreshing;

        ObservableCollection<PersonVM> _mappedPeople;
        //CollectionViewSource _cvs;

        public PersonCollectionViewModel(PropModel pm, string fullClassName, IPropFactory propFactory)
            : base(pm, fullClassName, propFactory)
        {
            System.Diagnostics.Debug.WriteLine("Constructing PersonCollectionViewModel -- with PropModel.");

            //// TODO: These should be part of the PropModel.
            //bool wasAdded =  SubscribeToPropChanged(DoWhenBusinessChanges, "Business", typeof(Business));
            //RegisterBinding<Business>("Business", "../Business");

            //wasAdded = SubscribeToPropChanged(DoWhenSelectedPersonChanges, "SelectedPerson", typeof(PersonVM));
        }


        private void DoWhenBusinessChanges(object sender, PcGenEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Business was changed.");
            if (e.OldValue == null && e.NewValue != null)
            {
                Business business = (Business)e.NewValue;
                FetchData(business);
            }
        }

        //private void DoWhenSelectedPersonChanges(object sender, PcGenEventArgs e)
        //{
        //    System.Diagnostics.Debug.WriteLine("The selected person was changed.");

        //    PersonVM old = (PersonVM) e.OldValue;
        //    PersonVM newVal = (PersonVM)e.NewValue;
        //}

        //private void DoWhenBusinessChanges(object sender, PCTypedEventArgs<Business> e)
        //{
        //    System.Diagnostics.Debug.WriteLine("Business was changed.");
        //    if(e.OldValue == null && e.NewValue != null)
        //    {
        //        Business business = (Business)e.NewValue;
        //        FetchData(business);
        //    }
        //}

        public CollectionViewSource CVS = new CollectionViewSource();

        public ListCollectionView PersonCollectionView
        {
            get
            {
                ListCollectionView result = GetIt<ListCollectionView>("PersonListView");
                return result;
            }
            set
            {
                SetIt(value, "PersonListView");
            }
        }

        private PersonVM TheSelectedPerson
        {
            get
            {
                ListCollectionView cv = PersonCollectionView;
                if(cv != null)
                {
                    return (PersonVM) cv.CurrentItem;
                }
                else
                {
                    return null;
                }
            }
        }

        public void FetchData(Business business)
        {
            // Get List of people for this business.
            _mappedPeople = GetMappedPeople(business);

            // Set our PersonList property.
            //SetIt(mappedPeople, "PersonList");

            //CVS.Source = _mappedPeople;
            //PersonCollectionView = (ListCollectionView)CollectionViewSource.GetDefaultView(cvs);

            PersonCollectionView = (ListCollectionView)CollectionViewSource.GetDefaultView(_mappedPeople);
            PersonCollectionView.MoveCurrentToNext();

            PersonVM personVM = TheSelectedPerson;
            if(personVM != null)
                SetIt(personVM, "SelectedPerson");

            //// Set the selected person the first item.
            //if (_mappedPeople != null && _mappedPeople.Count > 0)
            //{
            //    PersonVM personVM = TheSelectedPerson;
            //    SetIt(personVM, "SelectedPerson");
            //}
        }

        private ObservableCollection<PersonVM> GetMappedPeople(Business business)
        {
            var people = business.Get(10);

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
                var x = new RelayCommand(AddPerson);
                x.CanExecuteChanged += X_CanExecuteChanged;
                return x;
            }
        }

        private void X_CanExecuteChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("The state of 'CanExecute' has changed for the Add command.");
        }

        private void AddPerson(object o)
        {
            //ObservableCollection<PersonVM> personList;
            try
            {
                PersonVM newPerson = Mapper.MapToDestination(new Person());

                PersonCollectionView.AddNewItem(newPerson);

                //personList = GetIt<ObservableCollection<PersonVM>>("PersonList");
                //personList.Add(newPerson);

                //PersonCollectionView.MoveCurrentTo(newPerson);

                //SetIt<PersonVM>(newPerson, "SelectedPerson");
                //OnGridNeedsRefreshing();

                //ShowMessage("Changes are saved !");
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }
        }

        private void ShowMessage(string msg)
        {
            SetIt(msg, "WMessage");

            // TODO: How can we reset the value automatically?
            SetIt<string>(null, "WMessage");
        }

        public RelayCommand Save => new RelayCommand(SavePerson);

        private void SavePerson(object o)
        {
            try
            {
                //ObservableCollection<PersonVM> personList = GetIt<ObservableCollection<PersonVM>>("PersonList");

                ////PersonVM selectedPerson = GetIt<PersonVM>("SelectedPerson");

                //PersonVM selectedPerson = (PersonVM) PersonCollectionView.CurrentItem;

                PersonVM selectedPerson = TheSelectedPerson;

                //PersonVM personVmAlt = GetIt<PersonVM>("SelectedPerson");

                //bool theSame = ReferenceEquals(selectedPerson, personVmAlt);

                //bool theSame2 = int.Equals(selectedPerson.GetIt<int>("Id"), personVmAlt.GetIt<int>("Id"));

                Person unMapped = GetUnMappedPerson(selectedPerson);

                Business business = GetIt<Business>("Business");
                business.Update(unMapped);

                //OnGridNeedsRefreshing();

                ShowMessage("Changes are saved !");
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }
        }

        public RelayCommand Delete => new RelayCommand(DeletePerson);

        private void DeletePerson(object o)
        {
            //PersonVM selectedPerson = GetIt<PersonVM>("SelectedPerson");
            //PersonVM selectedPerson = (PersonVM)PersonCollectionView.CurrentItem;

            PersonVM selectedPerson = TheSelectedPerson;

            if (selectedPerson == null) return;

            Person unMapped = GetUnMappedPerson(selectedPerson);

            Business business = GetIt<Business>("Business");
            business.Delete(unMapped);

            PersonCollectionView.Remove(selectedPerson);

            //ObservableCollection<PersonVM> personList = GetIt<ObservableCollection<PersonVM>>("PersonList");
            //personList.Remove(selectedPerson);

            //OnGridNeedsRefreshing();

            ShowMessage("Selected Person has been removed!");
        }

        private void OnGridNeedsRefreshing()
        {
            GridNeedsRefreshing?.Invoke(this, new EventArgs());
        }

        #region AutoMapper Support

        //private string PersonVM_MapperRequest_Key = "PersonVM_Mapper";

        private IPropBagMapper<Person, PersonVM> _mapper;
        private IPropBagMapper<Person, PersonVM> Mapper
        {
            get
            {
                if (_mapper == null)
                {
                    string resourceKey = GetMapperRequestKey();

                    System.Diagnostics.Debug.WriteLine($"Creating PersonVM Mapper using {resourceKey}.");

                    DRM.PropBag.ControlModel.MapperRequest mr = JustSayNo.PropModelProvider.GetMapperRequest(resourceKey);
                    IPropBagMapperKeyGen mapperRequest = JustSayNo.AutoMapperProvider.RegisterMapperRequest(mr);
                    _mapper = (IPropBagMapper<Person, PersonVM>) JustSayNo.AutoMapperProvider.GetMapper(mapperRequest);
                }
                return _mapper;
            }
        }

        private string GetMapperRequestKey()
        {
            string result;
            string packageConfigName = JustSayNo.PackageConfigName;
            if(packageConfigName == "Emit_Proxy")
            {
                result = "PersonVM_Mapper_Emit_Proxy";
            }
            else
            {
                result = "PersonVM_Mapper_Extra_Members";
            }
            return result;
        }

        private Person GetUnMappedPerson(PersonVM mappedPerson)
        {
            Person result;
            if(mappedPerson != null)
            {
                result = Mapper?.MapToSource(mappedPerson);
            }
            else
            {
                result = null;
            }
            //if (mappedPerson == null) return null;

            //if (Mapper == null) return null;

            //Person result = Mapper.MapToSource(mappedPerson);

            return result;
        }

        #endregion
    }
}
