﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Trace.TraceError("rrrrrrrrrrrrrrr");
            Screenshot.Screenshot1.RegisterHotKey(this, ModifierKeys.Alt, System.Windows.Forms.Keys.B);
        }

        private void Fun(BitmapSource obj)
        {
            Dispatcher.Invoke(() =>
            {
                image_1.Source = obj;
            });
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Screenshot.Screenshot1.ReturnScreenShotEvent = Fun;
            Screenshot.Screenshot1.CaptureRegion();
        }
    }
}