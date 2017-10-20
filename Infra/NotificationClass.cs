using System.Collections.Specialized;
using System.ComponentModel;

using System.Collections.ObjectModel;


namespace MVVMApplication.Infra
{
    public class NotificationClass : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
