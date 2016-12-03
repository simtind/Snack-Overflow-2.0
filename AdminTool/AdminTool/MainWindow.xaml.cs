﻿using System;
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

namespace AdminTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            
            bt_add_balance.Click += bt_add_balance_Click;

        }

        private void bt_add_balance_Click(object sender, RoutedEventArgs e)
        {
            AddBalanceWindow add_balance = new AddBalanceWindow();
            add_balance.ShowDialog();
        }
    }
}