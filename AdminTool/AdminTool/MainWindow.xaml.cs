using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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
            bt_add_user.Click += bt_add_user_Click;
            bt_add_item.Click += Bt_add_item_Click;

        }

        private void Bt_add_item_Click(object sender, RoutedEventArgs e)
        {
            AddItemWindow window = new AddItemWindow();
            window.ShowDialog();
        }

        private void bt_add_balance_Click(object sender, RoutedEventArgs e)
        {
                AddBalanceWindow window = new AddBalanceWindow();
                window.ShowDialog();  
        }

        private void bt_add_user_Click(object sender, RoutedEventArgs e)
        {
                AddUserWindow window = new AddUserWindow();
                window.ShowDialog();
        }
    }
}
