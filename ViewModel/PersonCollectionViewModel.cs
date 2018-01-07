using DRM.PropBag;
using DRM.PropBag.AutoMapperSupport;
using DRM.PropBag.ControlModel;
using DRM.PropBag.ControlsWPF;
using DRM.TypeSafePropertyBag;

using MVVMApplication.Infra;
using MVVMApplication.Model;
using MVVMApplication.ServiceAdapters;
using MVVMApplication.Services;
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
        CrudWithMapping<Business, Person, PersonVM> _dal;
        //int _page = 0;

        public PersonCollectionViewModel(PropModel pm, string fullClassName, IPropFactory propFactory)
            : base(pm, fullClassName, propFactory)
        {
            System.Diagnostics.Debug.WriteLine("Constructing PersonCollectionViewModel -- with PropModel.");

            IProp<ListCollectionView> typedProp = GetTypedProp<ListCollectionView>("PersonListView");
            //IPropData pg= GetPropGen<ListCollectionView>("PersonListView", false, null, false, true, true, null, out bool wasRegistered, out UInt32 propId);

            if(typedProp is INotifyItemEndEdit iniee)
            {
                iniee.ItemEndEdit += Iniee_ItemEndEdit;
            }

            if(typedProp is INotifyCollectionChanged incc)
            {
                incc.CollectionChanged += Incc_CollectionChanged;
            }
        }

        private void Incc_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    PersonVM selectedPerson = item as PersonVM;
                    if (selectedPerson == null) return;

                    _dal.Delete(selectedPerson);
                }
            }
        }

        private void Iniee_ItemEndEdit(object sender, EventArgs e)
        {
            PersonVM selectedPerson = (PersonVM)sender;
            if (selectedPerson == null) return;

            _dal.Update(selectedPerson);
        }

        #region Command Handlers

        private void AddPerson(object o)
        {
            try
            {
                ListCollectionView lcv = GetIt<ListCollectionView>("PersonListView");

                PersonVM newPerson = _dal.GetNewItem(); //Mapper.GetNewDestination();
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
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }
        }

        private void DeletePerson(object o)
        {
            ListCollectionView lcv = GetIt<ListCollectionView>("PersonListView");

            PersonVM selectedPerson = (PersonVM)lcv.CurrentItem;
            if (selectedPerson == null) return;

            lcv.Remove(selectedPerson);
            ShowMessage("Selected Person has been removed!");
        }

        private void PageUpCom(object o)
        {
            //ShowMessage("We Got a PageUp.");
            //if (--_page < 0) _page = 0;
            //FetchData(_bw, _page * ITEMS_PER_PAGE);
            ListCollectionView lcv = GetIt<ListCollectionView>("PersonListView");
            int pos = lcv.CurrentPosition;
            pos -= ITEMS_PER_PAGE;
            if (pos < 0) pos = 0;
            lcv.MoveCurrentToPosition(pos);
        }

        private void PageDownCom(object o)
        {
            ////ShowMessage("We Got a PageDown.");
            ////if (++_page > 10) _page = 10;
            ////FetchData(_bw, _page * ITEMS_PER_PAGE);

            ListCollectionView lcv = GetIt<ListCollectionView>("PersonListView");
            int pos = lcv.CurrentPosition;
            pos += ITEMS_PER_PAGE;
            if (pos > lcv.Count - ITEMS_PER_PAGE) pos = lcv.Count - ITEMS_PER_PAGE;
            lcv.MoveCurrentToPosition(pos);
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
                _dal = new CrudWithMapping<Business, Person, PersonVM>(_business, Mapper);
                FetchData(_dal, 0);
            }
        }

        private void FetchData(CrudWithMapping<Business, Person, PersonVM> business, int start)
        {
            ListOfPersons people = new ListOfPersons();

            if (business != null)
            {
                IEnumerable<PersonVM> rawPeople = business.Get(0, 200, x => x.Id); // (start, ITEMS_PER_PAGE, x => x.Id);

                foreach (PersonVM p in rawPeople)
                {
                    people.Add(p);
                }
            }

            ObservableCollection<PersonVM> rr = (ObservableCollection<PersonVM>)people;

            SetIt(rr, "PersonList");
        }

        #endregion

        #region AutoMapper Support

        //private ListOfPersons GetMappedPeople(CrudWithMapping<Person, PersonVM> business, int start)
        //{
        //    //ListOfPersons result = new ListOfPersons();

        //    //if (business == null || Mapper == null)
        //    //    return result;

        //    //IEnumerable<Person> people = business.Get(start, ITEMS_PER_PAGE);
        //    //IEnumerable<PersonVM> mappedPeopleRaw = Mapper.MapToDestination(people);

        //    ////ObservableCollection<PersonVM> mappedPeople = new ObservableCollection<PersonVM>(mappedPeopleRaw);

        //    //foreach (PersonVM p in mappedPeopleRaw)
        //    //{
        //    //    result.Add(p);
        //    //}
        //    //return result;

        //    ListOfPersons result = new ListOfPersons();

        //    if (business != null)
        //    {
        //        IEnumerable<PersonVM> people = business.Get(start, ITEMS_PER_PAGE, x => x.Id);

        //        foreach (PersonVM p in people)
        //        {
        //            result.Add(p);
        //        }
        //    }

        //    return result;
        //}

        //private Person GetUnMappedPerson(PersonVM mappedPerson)
        //{
        //    Person result;
        //    if (mappedPerson != null)
        //    {
        //        result = Mapper?.MapToSource(mappedPerson);
        //    }
        //    else
        //    {
        //        result = null;
        //    }

        //    return result;
        //}

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
                    _mapper = (IPropBagMapper<Person, PersonVM>)JustSayNo.AutoMapperProvider.GetMapper(mapperRequest);
                }
                return _mapper;
            }
        }

        private string GetMapperRequestKey()
        {
            string result;
            string packageConfigName = JustSayNo.PackageConfigName;
            if (packageConfigName == "Emit_Proxy")
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
