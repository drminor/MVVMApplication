using DRM.PropBag;
using DRM.PropBag.ControlModel;
using DRM.PropBag.ControlsWPF;
using DRM.TypeSafePropertyBag;
using MVVMApplication.Infra;
using MVVMApplication.Model;
using System;
using System.Threading;

namespace MVVMApplication.ViewModel
{
    public class MainWindowViewModel : PropBag
    {
        public event EventHandler<EventArgs> RequestClose;

        public MainWindowViewModel(PropModel pm, string fullClassName, IPropFactory propFactory)
            : base(pm, fullClassName, propFactory)
        {
            //System.Diagnostics.Debug.WriteLine("Beginning to construct MainWindowViewModel -- From PropModel.");
            //System.Diagnostics.Debug.WriteLine("Completed Constructing MainWindowViewModel -- From PropModel.");
        }

        public void CloseTheWindow()
        {
            Interlocked.CompareExchange(ref RequestClose, null, null)?.Invoke(this, EventArgs.Empty);
        }

        public RelayCommand Close => new RelayCommand(CloseIt);

        private void CloseIt(object o)
        {
            CloseTheWindow();
        }

    }
}
