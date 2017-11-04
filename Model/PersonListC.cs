using DRM.PropBag.Collections;
using System;
using System.Collections;

namespace MVVMApplication.Model
{
    public class PersonListC : PbListSource
    {
        public PersonListC(Func<object, IList> getter, object component) : base(getter, component)
        {
        }

        //public IListSource ff 

        //public PersonListC(Func<ObservableCollection<PersonVM>> observableCollectionGetter) : base(observableCollectionGetter)
        //{
        //}

        //public PersonListC(Func<IEnumerable<PersonVM>> enumerableGetter) : base(enumerableGetter)
        //{
        //}
    }

    //public class PersonListC : PbCollection<PersonVM>
    //{
    //    public PersonListC(PropModel propModel, IPropFactory propFactory, string listName)
    //        : base(propModel, propFactory, listName) { }

    //    public PersonListC(PropModel propModel, IPropFactory propFactory, string listName, IEnumerable<PersonVM> coll)
    //        : base(propModel, propFactory, listName, coll) { }
    //}

    //public class PersonListC : ObservableCollection<PersonVM>
    //{
    //    public PersonListC(IEnumerable<PersonVM> coll) : base(coll) { }
    //}
}
