using MVVMApplication.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MVVMApplication.View
{
    /// <summary>
    /// Interaction logic for PersonCollection.xaml
    /// </summary>
    public partial class PersonCollection : UserControl
    {

        //public object Dc
        //{
        //    get
        //    {
        //        return this.DataContext;
        //    }
        //    set
        //    {
        //        this.DataContext = value;
        //    }

        //}

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
            if(this.DataContext is PersonCollectionViewModel pcvm)
            {
                pcvm.GridNeedsRefreshing += Pcvm_GridNeedsRefreshing;

                CollectionViewSource cvs = Cv;
                CollectionView x = (CollectionView)CollectionViewSource.GetDefaultView(cvs.Source);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is PersonCollectionViewModel pcvm)
            {
                pcvm.GridNeedsRefreshing += Pcvm_GridNeedsRefreshing;
                CollectionViewSource cvs = Cv;

                CollectionView x = (CollectionView) CollectionViewSource.GetDefaultView(cvs.Source);

                //pcvm.TheView = Cv;
            }
        }

        public CollectionViewSource Cv
        {
            get
            {
                CollectionViewSource collectionViewSource = (CollectionViewSource)this.Resources["cvsPersonList"];

                //ICollectionView cv = collectionViewSource.View;

                return collectionViewSource;
            }
        }
 
    }
}
