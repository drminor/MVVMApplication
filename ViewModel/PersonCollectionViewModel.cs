using DRM.TypeSafePropertyBag;
using MVVMApplication.Model;

using System.Linq;

namespace MVVMApplication.ViewModel
{
    public partial class PersonCollectionViewModel
    {

        public PersonCollectionViewModel()
        {
            System.Diagnostics.Debug.WriteLine("Constructing PersonCollectionViewModel -- no Params.");
        }

        public void PopPeople(Business business)
        {
            var people = business.Get();

            if (Mapper == null) return;

            var mappedPeople = Mapper.MapToDestination(people);

            PersonListC pList = new PersonListC(mappedPeople);
            base.SetIt<PersonListC>(pList, "PersonList");
        }

        IProp<PersonListC> _personListCached;
        private IProp<PersonListC> PersonListCached
        {
            get
            {
                if(_personListCached == null)
                {
                    _personListCached = base.GetTypedProp<PersonListC>("PersonList");
                }
                return _personListCached;
            }
        }
        private PersonListC PList
        {
            get
            {
                return PersonListCached.TypedValue;
                //return base.GetIt<PersonListC>("PesonList");
            }
            set
            {
                PersonListCached.TypedValue = value;
            }
        }

        //PersonVM _sel;
        public PersonVM SelectedPerson2
        {
            get
            {
                //return _sel;
                return base.GetIt<PersonVM>("SelectedPerson");
            }
            set
            {
                //_sel = value;
                base.SetIt<PersonVM>(value, "SelectedPerson");
                //base.OnPropertyChanged("SelectedPerson2");
            }
        }
       

        public event PropertyChangedWithTValsHandler<PersonVM> SelectedPersonChanged
        {
            add
            {
                AddToPropChanged<PersonVM>(value, nameof(SelectedPersonChanged));
            }
            remove
            {
                RemoveFromPropChanged<PersonVM>(value, nameof(SelectedPersonChanged));
            }
        }

    }
}
