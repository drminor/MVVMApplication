using DRM.PropBag;
using DRM.PropBag.ControlModel;
using DRM.TypeSafePropertyBag;
using MVVMApplication.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace MVVMApplication.ViewModel
{
    public class PersonVM : PropBag, ICloneable
    {
        public PersonVM(PropModel pm, string fullClassName, IPropFactory propFactory)
            : base(pm, fullClassName, propFactory)
        {
        }

        private PersonVM(PersonVM copySource)
            : base(copySource)
        {
        }

        new public object Clone()
        {
            return new PersonVM(this);
        }

        public override string ToString()
        {
            IDictionary<string, ValPlusType> x = GetAllPropNamesAndTypes();

            StringBuilder result = new StringBuilder();
            int cnt = 0;
            foreach(KeyValuePair<string, ValPlusType> kvp in x)
            {
                if(cnt++  == 0) result.Append("\n\r");

                result.Append($" -- {kvp.Key}: {kvp.Value.Value}");
            }
            return result.ToString();
        }
    }

  
}
