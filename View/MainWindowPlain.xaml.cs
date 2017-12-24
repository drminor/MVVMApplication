using MVVMApplication.Infra;
using MVVMApplication.ViewModel;
using System;
using System.Windows;

namespace MVVMApplication.View
{
    /// <summary>
    /// Interaction logic for MainWindowPlain.xaml
    /// </summary>
    public partial class MainWindowPlain : Window
    {
        public MainWindowPlainViewModel OurData
        {
            get
            {
                return (MainWindowPlainViewModel)this.DataContext;
            }
            set
            {
                this.DataContext = value;
            }
        }
            
        public MainWindowPlain()
        {
            InitializeComponent();

            OurData.ShowMessageBox += delegate (object sender, MessageEventArgs args)
            {
                MessageBox.Show(args.Message);
            };

        }

    }
}
