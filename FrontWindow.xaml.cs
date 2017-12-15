using MVVMApplication.View;
using System.Windows;

namespace MVVMApplication
{
    public partial class FrontWindow : Window
    {
        public FrontWindow()
        {
            InitializeComponent();
        }

        private void Plain_Click(object sender, RoutedEventArgs e)
        {
            MainWindowPlain mwp = new MainWindowPlain();
            mwp.ShowDialog();
        }

        private void PropBagProxyEmit_Click(object sender, RoutedEventArgs e)
        {
            //Class1 test = new Class1();
            //test.MapOcAndCleanUp();

            MainWindow mw = new MainWindow();
            mw.ShowDialog();
        }

    }
}
