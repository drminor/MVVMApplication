using DRM.PropBag;
using DRM.PropBag.AutoMapperSupport;
using DRM.PropBag.ControlModel;
using DRM.PropBag.ControlsWPF;
using DRM.PropBagWPF;
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
        Business _business;

        public PersonCollectionViewModel(PropModel pm, string fullClassName, IPropFactory propFactory)
            : base(pm, fullClassName, propFactory)
        {
            System.Diagnostics.Debug.WriteLine("Constructing PersonCollectionViewModel -- with PropModel.");

            BuildListSource();
        }

        private void BuildListSource()
        {
            //// Create a CollectionViewSource and register the property into the store.
            //CViewProp<CollectionViewSource, PersonVM> CVS = new CViewProp<CollectionViewSource, PersonVM>(null);
            //AddProp("CVS", CVS);

            // See if we can access the new property.
            CollectionViewSource testAccess = GetIt<CollectionViewSource>("CVS");
            System.Diagnostics.Debug.Assert(testAccess != null, $"The CVS prop did not get added.");
        }

        #region Command Handlers

        private void AddPerson(object o)
        {
            try
            {
                ListCollectionView lcv = (ListCollectionView)GetIt<CollectionViewSource>("CVS").View;
                PersonVM newPerson = Mapper.GetNewDestination();
                lcv.AddNewItem(newPerson);
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }
        }

        private void SavePerson(object o)
        {
            try
            {
                ListCollectionView lcv = (ListCollectionView)GetIt<CollectionViewSource>("CVS").View;
                PersonVM selectedPerson = (PersonVM)lcv.CurrentItem;

                if (selectedPerson == null) return;

                Person unMapped = GetUnMappedPerson(selectedPerson);

                _business.Update(unMapped);

                ShowMessage("Changes are saved !");
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }
        }

        private void DeletePerson(object o)
        {
            ListCollectionView lcv = (ListCollectionView)GetIt<CollectionViewSource>("CVS").View;
            PersonVM selectedPerson = (PersonVM)lcv.CurrentItem;

            if (selectedPerson == null) return;

            Person unMapped = GetUnMappedPerson(selectedPerson);

            _business.Delete(unMapped);

            ShowMessage("Selected Person has been removed!");
        }

        private void ShowMessage(string msg)
        {
            string x = null;
            SetIt<string>(msg, ref x, "WMessage");
        }

        #endregion

        #region Command Relayers

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

        public RelayCommand Save => new RelayCommand(SavePerson);

        public RelayCommand Delete => new RelayCommand(DeletePerson);


        #endregion

        #region Event Handlers

        private void DoWhenBusinessChanges(object sender, PcTypedEventArgs<Business> e)
        {
            System.Diagnostics.Debug.WriteLine("DoWhenBusinessChanges was called.");

            if(!e.NewValueIsUndefined && e.NewValue != _business)
            {
                _business = e.NewValue;
                FetchData(_business);
            }
            else if(e.NewValueIsUndefined && _business != null)
            {
                _business = null;
                FetchData(_business);
            }
        }

        public void FetchData(Business business)
        {
            // Set our PersonList property to the list of people for this business and .
            ObservableCollection<PersonVM> mappedPeople = GetMappedPeople(business);
            SetIt(mappedPeople, "PersonList");

            // TODO: Move these three lines to the PropTemplate.
            CollectionViewSource cvs = GetIt<CollectionViewSource>("CVS");
            ObservableCollection<PersonVM> temp = GetIt<ObservableCollection<PersonVM>>("PersonList");
            cvs.Source = temp;
        }

        #endregion

        #region AutoMapper Support

        private ObservableCollection<PersonVM> GetMappedPeople(Business business)
        {
            if (business == null || Mapper == null)
                return new ObservableCollection<PersonVM>();

            IEnumerable<Person> people = business.Get(10);
            IEnumerable<PersonVM> mappedPeopleRaw = Mapper.MapToDestination(people);
            ObservableCollection<PersonVM> mappedPeople = new ObservableCollection<PersonVM>(mappedPeopleRaw);
            return mappedPeople;
        }

        private Person GetUnMappedPerson(PersonVM mappedPerson)
        {
            Person result;
            if (mappedPerson != null)
            {
                result = Mapper?.MapToSource(mappedPerson);
            }
            else
            {
                result = null;
            }

            return result;
        }

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

        #endregion
    }
}
