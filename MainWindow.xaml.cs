﻿using DRM.PropBag.ControlsWPF;
using MVVMApplication.Infra;
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
        //Dictionary<string, BoundPropBag> _boundPropBags;

        [PropBagInstanceAttribute("MainVM", "There is only one ViewModel in this View.")]
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

        public MainWindow(MainWindowViewModel mainViewModel)
        {
            //byte b = 0xf;
            //OurData = new MainWindowViewModel(b);


            System.Diagnostics.Debug.WriteLine("Just before MainWindow InitComp.");
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine("Just afer MainWindow InitComp.");

            System.Diagnostics.Debug.WriteLine("Just before setting DataContext to the MainWindowVM provided in the constructor.");
            OurData = mainViewModel;
            System.Diagnostics.Debug.WriteLine("Just after setting DataContext to the new MainWindowVM provided in the constructor.");


            OurData.ShowMessageBox += delegate (object sender, EventArgs args)
            {
                MessageBox.Show(((MessageEventArgs)args).Message);
            };

            //Grid topGrid = (Grid)this.FindName("TopGrid");
            //_boundPropBags = ViewModelGenerator.StandUpViewModels(topGrid, this);
        }
    }
}
