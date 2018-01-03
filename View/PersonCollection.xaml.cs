using MVVMApplication.ViewModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace MVVMApplication.View
{
    public partial class PersonCollection : UserControl
    {
        public PersonCollection()
        {
            InitializeComponent();
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DeleteCurRow();
        }

        private DataGrid MyDataGrid => FindName("PersonListDataGrid") as DataGrid;
        //{
        //    get
        //    {
        //        object grid = FindName("PersonListDataGrid");
        //        if(grid is DataGrid dg)
        //        {
        //            return dg;
        //        }
        //        else
        //        {
        //            return null;
        //        }

        //    }
        //}

        private object SelectedItem => MyDataGrid?.SelectedItem;


        private void DeleteCurRow()
        {
            if(SelectedItem != null)
                MyDataGrid?.Items?.Remove(SelectedItem);

        }

    }
}
