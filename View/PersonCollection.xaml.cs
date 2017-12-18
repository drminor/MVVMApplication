using MVVMApplication.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MVVMApplication.View
{
    public partial class PersonCollection : UserControl
    {
        public PersonCollection()
        {
            InitializeComponent();

            this.DataContextChanged += PersonCollection_DataContextChanged;
        }

        private void Pcvm_GridNeedsRefreshing(object sender, System.EventArgs e)
        {
            DataGrid dg = (DataGrid) FindName("PersonListDataGrid");
            dg.Items.Refresh();
        }

        private void PersonCollection_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.DataContext is PersonCollectionViewModel pcvm)
            {
                pcvm.GridNeedsRefreshing += Pcvm_GridNeedsRefreshing;

                ListCollectionView people = pcvm.GetIt<ListCollectionView>("PersonListView");

                people.CurrentChanged += People_CurrentChanged;

                var x = FindName("PersonListDataGrid");

                if(x is DataGrid dg)
                {
                    dg.SelectionChanged += Dg_SelectionChanged;
                }
            }
        }

        private void Dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = (DataGrid)sender;

            

            PersonVM selectedP = (PersonVM)(dg).SelectedItem;

            ((PersonCollectionViewModel)DataContext).PersonCollectionView.MoveCurrentTo(selectedP);

            //((PersonCollectionViewModel)DataContext).SetIt<PersonVM>( (PersonVM) ((DataGrid)sender).SelectedItem, "SelectedPerson");
        }

        private void People_CurrentChanged(object sender, System.EventArgs e)
        {
            //PersonVM selectedPerson = ((PersonCollectionViewModel)DataContext).GetIt<PersonVM>("SelectedPerson");

            PersonVM personVmAlt = (PersonVM) ((PersonCollectionViewModel)DataContext).GetIt<ListCollectionView>("PersonListView").CurrentItem;

            //bool theSame = ReferenceEquals(selectedPerson, personVmAlt);

            //bool theSame2 = int.Equals(selectedPerson?.GetIt<int>("Id"), personVmAlt?.GetIt<int>("Id"));

        }
    }
}
