using MVVMApplication.Infra;
using MVVMApplication.Model;
using System;

namespace MVVMApplication.ViewModel
{
    public class MainWindowPlainViewModel : NotificationClass, IDisposable
    {
        public EventHandler<MessageEventArgs> ShowMessageBox = delegate { };

        Business _business;

        public MainWindowPlainViewModel() 
        {
            if(JustSayNo.InDesignMode())
            {
                _business = null;
            }
            else
            {
                _business = new Business();
            }

            PersonCollectionPlainViewModel = new PersonCollectionPlainViewModel(_business);

            personCollectionPlainViewModel.MessageHasArrived += delegate (object sender, MessageEventArgs args)
            {
                // Pass messages from the child view model to our listeners (i.e., our parent window.)
                ShowMessageBox(sender, args);
            };

        }

        private PersonCollectionPlainViewModel personCollectionPlainViewModel;
        public PersonCollectionPlainViewModel PersonCollectionPlainViewModel
        {
            get { return personCollectionPlainViewModel; }
            set
            {
                personCollectionPlainViewModel = value;
                OnPropertyChanged(nameof(PersonCollectionPlainViewModel));
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _business.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~MainWindowPlainViewModel() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
