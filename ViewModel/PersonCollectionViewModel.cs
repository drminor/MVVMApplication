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
using System.Collections.Specialized;
using System.Windows.Data;

namespace MVVMApplication.ViewModel
{
    public partial class PersonCollectionViewModel : PropBag
    {
        int ITEMS_PER_PAGE = 10;
        Business _business;
        int _page = 0;

        public PersonCollectionViewModel(PropModel pm, string fullClassName, IPropFactory propFactory)
            : base(pm, fullClassName, propFactory)
        {
            System.Diagnostics.Debug.WriteLine("Constructing PersonCollectionViewModel -- with PropModel.");

            IPropData pg= GetPropGen<ListCollectionView>("PersonListView", false, null, false, true, true, null, out bool wasRegistered, out UInt32 propId);

            if(pg.TypedProp is INotifyItemEndEdit iniee)
            {
                iniee.ItemEndEdit += Iniee_ItemEndEdit;
            }

            if(pg.TypedProp is INotifyCollectionChanged incc)
            {
                incc.CollectionChanged += Incc_CollectionChanged;
            }

            CheckListSource();
        }

        private void Incc_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    PersonVM selectedPerson = item as PersonVM;

                    if (selectedPerson == null) return;

                    Person unMapped = GetUnMappedPerson(selectedPerson);

                    _business.Delete(unMapped);

                    ListCollectionView lcv = GetIt<ListCollectionView>("PersonListView");
                    //lcv.Remove(selectedPerson);

                    //ShowMessage("Selected Person has been removed!");
                }
            }
        }

        private void Iniee_ItemEndEdit(object sender, EventArgs e)
        {
            PersonVM selectedPerson = (PersonVM)sender;

            if (selectedPerson == null) return;

            Person unMapped = GetUnMappedPerson(selectedPerson);

            _business.Update(unMapped);

            //ShowMessage("Changes are saved !");
        }

        private void CheckListSource()
        {
            //// Create a CollectionViewSource and register the property into the store.
            //CViewProp<CollectionViewSource, PersonVM> CVS = new CViewProp<CollectionViewSource, PersonVM>(null);
            //AddProp("CVS", CVS);

            // Refresh the PersonListView property.
            //RefreshIt(null);
        }

        #region Command Handlers

        private void AddPerson(object o)
        {
            try
            {
                //ListCollectionView lcv = (ListCollectionView)GetIt<CollectionViewSource>("CVS").View;
                ListCollectionView lcv = GetIt<ListCollectionView>("PersonListView");

                PersonVM newPerson = Mapper.GetNewDestination();
                lcv.AddNewItem(newPerson);
                lcv.MoveCurrentTo(newPerson);
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
                ListCollectionView lcv = GetIt<ListCollectionView>("PersonListView");
                if (lcv.IsEditingItem)
                {
                    lcv.CommitEdit();
                    ShowMessage("Changes are saved !");
                }
                else
                {
                    ShowMessage("No Pending Changes.");
                }

                ////ListCollectionView lcv = (ListCollectionView)GetIt<CollectionViewSource>("CVS").View;
                //ListCollectionView lcv = GetIt<ListCollectionView>("PersonListView");

                //PersonVM selectedPerson = (PersonVM)lcv.CurrentItem;

                //if (selectedPerson == null) return;

                //Person unMapped = GetUnMappedPerson(selectedPerson);

                //_business.Update(unMapped);

                //ShowMessage("Changes are saved !");
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }
        }

        private void DeletePerson(object o)
        {
            //ListCollectionView lcv = (ListCollectionView)GetIt<CollectionViewSource>("CVS").View;
            ListCollectionView lcv = GetIt<ListCollectionView>("PersonListView");

            PersonVM selectedPerson = (PersonVM)lcv.CurrentItem;

            if (selectedPerson == null) return;

            //Person unMapped = GetUnMappedPerson(selectedPerson);

            //_business.Delete(unMapped);
            lcv.Remove(selectedPerson);

            ShowMessage("Selected Person has been removed!");
        }

        private void PageUpCom(object o)
        {
            //ShowMessage("We Got a PageUp.");
            if (--_page < 0) _page = 0;
            FetchData(_business, _page * ITEMS_PER_PAGE);
        }

        private void PageDownCom(object o)
        {
            //ShowMessage("We Got a PageDown.");
            if (++_page > 10) _page = 10;
            FetchData(_business, _page * ITEMS_PER_PAGE);
        }

        private void ShowMessage(string msg)
        {
            string x = null;
            SetIt<string>(msg, ref x, "WMessage");
        }

        public void RefreshIt(object o)
        {
            //CollectionViewSource cvs = GetIt<CollectionViewSource>("CVS");
            //if (cvs != null)
            //{
            //    RefreshPersonListView(cvs);
            //}
            //else
            //{
            //    System.Diagnostics.Debug.WriteLine($"The CVS has no value.");
            //}
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

        public RelayCommand PageUp => new RelayCommand(PageUpCom);

        public RelayCommand PageDown => new RelayCommand(PageDownCom);

        public RelayCommand Refresh => new RelayCommand(RefreshIt);


        #endregion

        #region Event Handlers

        private void DoWhenBusinessChanges(object sender, PcTypedEventArgs<Business> e)
        {
            System.Diagnostics.Debug.WriteLine("DoWhenBusinessChanges was called.");

            if (e.NewValueIsUndefined) e.NewValue = null;

            if (e.NewValue != _business)
            {
                _business = e.NewValue;
                FetchData(_business, 0);
            }
        }

        public void FetchData(Business business, int start = 0)
        {
            //FetchData_Test();

            //if (business == null) return;
            //CollectionViewSource cvsPrior = GetIt<CollectionViewSource>("CVS");
            //ICollectionView icvPrior = cvsPrior?.View;

            // Set our PersonList property to the list of people for this business and .
            //ObservableCollection<PersonVM> mappedPeople = GetMappedPeople(business, start);
            ListOfPersons mappedPeople = GetMappedPeople(business, start);
            ObservableCollection<PersonVM> rr = (ObservableCollection<PersonVM>)mappedPeople;

            SetIt(rr, "PersonList");
        }


        //public void FetchData_Test()
        //{
        //    IProvideAView viewProvider = null;
        //    ListCollectionView lcv = null;

        //    IProvideAView viewProvider2 = null;
        //    ListCollectionView lcv2 = null;

        //    if (TryGetViewManager(this, "PersonList", typeof(ObservableCollection<PersonVM>), out IManageCViews cViewManager))
        //    {
        //        viewProvider = cViewManager.GetViewProvider();
        //        lcv = viewProvider.View as ListCollectionView;
        //    }

        //    if (TryGetViewManager(this, "PersonList", typeof(ObservableCollection<PersonVM>), out IManageCViews cViewManager2))
        //    {
        //        viewProvider2 = cViewManager2.GetViewProvider();
        //        lcv2 = viewProvider2.View as ListCollectionView;
        //    }

        //    bool test1 = ReferenceEquals(cViewManager, cViewManager2);
        //    bool test2 = ReferenceEquals(cViewManager.DataSourceProvider, cViewManager2.DataSourceProvider);
        //    bool test3 = ReferenceEquals(viewProvider, viewProvider2);
        //    bool test4 = ReferenceEquals(lcv, lcv2);

        //    object d1 = cViewManager.DataSourceProvider.Data;

        //    object d2 = cViewManager.Data;

        //    object d3 = cViewManager.DataSourceProviderProvider.DataSourceProvider.Data;

        //    //var x = cvs.View;

        //    //ListCollectionView ff = GetIt<ListCollectionView>("PersonListView");

        //    //IEnumerable gg = ff?.SourceCollection;

        //    //if(gg != null)
        //    //{
        //    //    List<object> hh = new List<object>(gg.Cast<object>());

        //    //    int cnt = hh.Count;
        //    //}



        //    System.Diagnostics.Debug.WriteLine("You may want to set a break point here.");

        //    //ICollectionView icv = cvs?.View;

        //    //bool testC = ReferenceEquals(cvsPrior, cvs);
        //    //bool testI = ReferenceEquals(icvPrior, icv);

        //    //if (cvs.View is ListCollectionView lcv)
        //    //{
        //    //    SetIt<ListCollectionView>(lcv, "PersonListView");
        //    //}
        //    //else
        //    //{
        //    //    System.Diagnostics.Debug.WriteLine("The default view of the CollectionViewSource: CVS does not implement ListCollectionView.");
        //    //    SetIt<ListCollectionView>(null, "PersonListView");
        //    //}

        //}

        //private void RefreshPersonListView(CollectionViewSource cvs)
        //{
        //    if (cvs?.Source is DataSourceProvider dsp)
        //    {
        //        dsp.Refresh();
        //    }
        //    else
        //    {
        //        if (cvs?.Source != null)
        //        {
        //            throw new InvalidOperationException("The Source is not a DataSourceProvider.");
        //        }
        //    }
        //}

        #endregion

        #region AutoMapper Support

        private ListOfPersons GetMappedPeople(Business business, int start)
        {
            ListOfPersons result = new ListOfPersons();

            if (business == null || Mapper == null)
                return result;

            IEnumerable<Person> people = business.Get(start, ITEMS_PER_PAGE);
            IEnumerable<PersonVM> mappedPeopleRaw = Mapper.MapToDestination(people);

            //ObservableCollection<PersonVM> mappedPeople = new ObservableCollection<PersonVM>(mappedPeopleRaw);

            foreach (PersonVM p in mappedPeopleRaw)
            {
                result.Add(p);
            }
            return result;
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
