﻿using RedShot.Desktop.Skia.Wpf.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RedShot.Desktop.WPF.Host
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var compositionRoot = new CompositionRoot();
            root.Content = new global::Uno.UI.Skia.Platform.WpfHost(Dispatcher, () => new RedShot.Desktop.App(compositionRoot));
        }
    }
}
