using System.Windows;

using MVVMApplication.View;
using System.Collections.ObjectModel;
using MVVMApplication.ViewModel;
using System;

namespace MVVMApplication
{
    /// <summary>
    /// Interaction logic for FrontWindow.xaml
    /// </summary>
    public partial class FrontWindow : Window
    {
        public FrontWindow()
        {
            //GetObsCollType(typeof(PersonVM));
            InitializeComponent();
        }

        private void Plain_Click(object sender, RoutedEventArgs e)
        {
            //MainWindowViewModel mainViewModel =
            //    AutoMapperHelpers.GetNewViewModel<MainWindowViewModel>("MainWindowVM", propFactory: SettingsExtensions.ThePropFactory);

            MainWindowPlain mwp = new MainWindowPlain();

            mwp.ShowDialog();
        }

        private void PropBagProxyEmit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();

            mw.ShowDialog();
        }

        static Type GetObsCollType(Type itemType)
        {
            ////Dictionary<int, int> testD = null;

            Type typDestDType = typeof(ObservableCollection<PersonVM>);

            string strDestDType = typDestDType.FullName;
            string asqnTypeName = typDestDType.AssemblyQualifiedName;

            //strDestDType = "System.Collections.Generic.Dictionary`2[[System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089],[System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]";
            //strDestDType = "System.Collections.Generic.Dictionary`2[[System.Int32],[System.Int32]]";


            Type tt = Type.GetType(asqnTypeName);

            Type pt = typeof(PersonVM);
            string asqnPTName = pt.AssemblyQualifiedName;

            //string typeName2 = $"System.Collections.ObjectModel.ObservableCollection`1[[MVVMApplication.ViewModel.PersonVM, MVVMApplication, Version = 1.0.0.0, Culture = neutral, PublicKeyToken = null]], System, Version = 4.0.0.0, Culture = neutral, PublicKeyToken = b77a5c561934e089";
            //Type result = Type.GetType(typeName2);



            //string typeName = $"System.Collections.ObjectModel.ObservableCollection`1[[System.String]]";
            //Type result = Type.GetType(typeName);

            string typeName = $"System.Collections.ObjectModel.ObservableCollection`1[[{itemType.FullName}]], System, Version = 4.0.0.0, Culture = neutral, PublicKeyToken = b77a5c561934e089";
            Type result = Type.GetType(typeName);

            //typeName = $"System.Collections.ObjectModel.ObservableCollection`1[[{itemType.FullName},{itemType.Assembly.FullName}]]";
            //result = Type.GetType(typeName);

            return result;
        }
    }
}
