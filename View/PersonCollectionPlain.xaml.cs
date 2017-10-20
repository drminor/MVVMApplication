using MVVMApplication.ViewModel;
using System;
using System.Windows.Controls;

namespace MVVMApplication.View
{
    /// <summary>
    /// Interaction logic for PersonCollectionPlain.xaml
    /// </summary>
    public partial class PersonCollectionPlain : UserControl
    {

        public PersonCollectionPlainViewModel OurData => (PersonCollectionPlainViewModel)this.DataContext;

        public PersonCollectionPlain()
        {
            InitializeComponent();
        }

    }
}
