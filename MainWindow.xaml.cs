using DRM.TypeSafePropertyBag;
using MVVMApplication.Infra;
using MVVMApplication.ViewModel;
using System.Windows;
using System.Windows.Data;

namespace MVVMApplication
{
    public partial class MainWindow : Window
    {
        MainWindowViewModel OurData => (MainWindowViewModel)this.DataContext;

        public MainWindow(string packageConfigName)
        {
            System.Diagnostics.Debug.WriteLine("Just before MainWindow InitComp.");

            JustSayNo.PackageConfigName = packageConfigName;

            InitializeComponent();
            System.Diagnostics.Debug.WriteLine("Just afer MainWindow InitComp.");

            // TODO: How is this done in other frameworks?
            OurData.SubscribeToPropChanged<string>(WMessageHasArrived, "WMessage");
        }

        private void WMessageHasArrived(object sender, PcTypedEventArgs<string> e)
        {
            if (e.NewValue != null)
                MessageBox.Show(e.NewValue, "MVVM Application", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            OurData?.Dispose();
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            //OurData.SubscribeToPropChanged<string>(WMessageHasArrived, "WMessage");

            //OurData.KickMe();
        }

        //private void Window_Loaded(object sender, RoutedEventArgs e)
        //{
        //    //OurData.SubscribeToPropChanged<string>(WMessageHasArrived, "WMessage");
        //    //OurData.Start();
        //    //OurData.Start();
        //}
    }
}
