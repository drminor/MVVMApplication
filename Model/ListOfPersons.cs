using DRM.TypeSafePropertyBag;
using MVVMApplication.ViewModel;
using System;
using System.Collections.ObjectModel;

namespace MVVMApplication.Model
{
    public class ListOfPersons : ObservableCollection<PersonVM>, INotifyItemEndEdit
    {
        public event EventHandler<EventArgs> ItemEndEdit;

        protected override void InsertItem(int index, PersonVM item)
        {
            base.InsertItem(index, item);

            // handle any EndEdit events relating to this item
            item.ItemEndEdit += ItemEndEditHandler;
        }

        void ItemEndEditHandler(object sender, EventArgs e)
        {
            // simply forward any EndEdit events
            ItemEndEdit?.Invoke(sender, e);
        }

    }
}
