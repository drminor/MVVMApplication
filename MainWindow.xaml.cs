using MVVMApplication.Infra;
using MVVMApplication.View;
using MVVMApplication.ViewModel;
using System;
using System.Windows;

namespace MVVMApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowViewModel OurData
        {
            get
            {
                object x = this.DataContext;
                if(x is MainWindowViewModel)
                {
                    return (MainWindowViewModel)this.DataContext;
                } 
                else
                {
                    System.Diagnostics.Debug.WriteLine("DataContext for MainWindow is not of type MainWindowViewModel");
                    return null;
                }
            }
            set
            {
                this.DataContext = value;
            }
        }

        public MainWindow()
        {

            System.Diagnostics.Debug.WriteLine("Just before MainWindow InitComp.");
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine("Just afer MainWindow InitComp.");

            //System.Diagnostics.Debug.WriteLine("Just before setting DataContext to the MainWindowVM provided in the constructor.");
            //OurData =  mainViewModel;
            //System.Diagnostics.Debug.WriteLine("Just after setting DataContext to the new MainWindowVM provided in the constructor.");

            OurData.ShowMessageBox += delegate (object sender, EventArgs args)
            {
                MessageBox.Show(((MessageEventArgs)args).Message);
            };

            //this.Loaded += MainWindow_Loaded;
        }

        //private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        //{
        //    //PersonCollection pc = (PersonCollection) this.FindName("pcGrid");
        //    //DataGrid dg = (DataGrid)pc.FindName("PersonListDataGrid");
        //    //OurData.UpdatePersonListDataGrid(dg);
        //}

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            PersonCollection pc = (PersonCollection)this.FindName("pcGrid");

            object dc = pc.DataContext;

            //pc.DataContext = OurData.Pcvm;
            //pc.Dc = null;
            //pc.Dc = OurData;
            //Grid topGrid = (Grid)this.FindName("TopGrid");

        }
    }
}
