using DRM.PropBag;
using MVVMApplication.Infra;
using MVVMApplication.Model;
using System;

namespace MVVMApplication.ViewModel
{
    public partial class MainWindowViewModel : IPropBagMin
    {

        public EventHandler ShowMessageBox = delegate { };
        public MainWindowViewModel()
        {
            System.Diagnostics.Debug.WriteLine("Beginning to construct MainWindowViewModel -- no Params.");
            _pm = null;
            InitMe();

            //PopPeople();

            System.Diagnostics.Debug.WriteLine("Completed Constructing MainWindowViewModel -- no Params.");
        }

        //public Business Business { get; }

        //private PersonCollectionViewModel _personCollectionVM;
        //public PersonCollectionViewModel PersonCollectionVM
        //{
        //    get { return _personCollectionVM; }
        //    set
        //    {
        //        _personCollectionVM = value;
        //        OnPropertyChanged(nameof(PersonCollectionVM));
        //    }
        //}

        public void PopPeople()
        {
            PersonCollectionViewModel pcvm = base.GetIt<PersonCollectionViewModel>("PersonCollectionVM");
            Business b = GetIt<Business>("Business");
            pcvm.PopPeople(b);
        }


        public RelayCommand Add
        {
            get
            {
                return new RelayCommand(AddPerson, true);
            }        
        }

        private void AddPerson()
        {
            //try
            //{
            //    SelectedPerson = new Person();                       
            //}
            //catch (Exception ex)
            //{
            //    ShowMessageBox(this, new MessageEventArgs()
            //    {
            //        Message = ex.Message
            //    });
            //}            
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
            //try
            //{
            //    _business.Update(SelectedPerson);

            //    PopPeople();

            //    ShowMessageBox(this, new MessageEventArgs()
            //    {
            //        Message = "Changes are saved !"
            //    });
            //}
            //catch (Exception ex)
            //{
            //    ShowMessageBox(this, new MessageEventArgs()
            //    {
            //        Message = ex.Message
            //    });
            //}               
          
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
            //_business.Delete(SelectedPerson);

            //    PopPeople();
        }
    }
}
