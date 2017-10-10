using MVVMApplication.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MVVMApplication.Model
{
    public class PersonListC : ObservableCollection<PersonVM>
    {
        public PersonListC(IEnumerable<PersonVM> coll) : base(coll) { }
    }
}
