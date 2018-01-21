using MVVMApplication.View;
using System;
using System.Windows;

namespace MVVMApplication
{
    public partial class FrontWindow : Window
    {
        public FrontWindow()
        {
            InitializeComponent();

            SetDataDirLocation();
        }

        private void Plain_Click(object sender, RoutedEventArgs e)
        {
            MainWindowPlain mwp = new MainWindowPlain();
            mwp.ShowDialog();
        }

        private void PropBagProxyEmit_Click(object sender, RoutedEventArgs e)
        {
            ShowMain("Emit_Proxy");
        }

        private void PropBagExtraMembers_Click(object sender, RoutedEventArgs e)
        {
            ShowMain("Extra_Members");
        }

        private void ShowMain(string packageConfigName)
        {
            MainWindow mw = new MainWindow(packageConfigName);
            mw.ShowDialog();
        }

        private void SetDataDirLocation()
        {
            Environment.SpecialFolder comAppDataFolder = System.Environment.SpecialFolder.CommonApplicationData;
            string dataDirPath = Environment.GetFolderPath(comAppDataFolder);
            AppDomain.CurrentDomain.SetData("DataDirectory", dataDirPath);
        }

    }
}
