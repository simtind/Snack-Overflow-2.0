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
using System.Windows.Shapes;

namespace AdminTool
{
    /// <summary>
    /// Interaction logic for AddBalanceWindow.xaml
    /// </summary>
    public partial class AddBalanceWindow : Window
    {
        SnackOverflowC.Database db = new SnackOverflowC.Database();
        SnackOverflowC.User user;
        UdpClient client;
        public AddBalanceWindow()
        {
            InitializeComponent();

            if (!db.checkDB())
            {
                MessageBox.Show("Error opening database");
                this.Close();
            }

            bt_apply.Click += bt_apply_Click;
            bt_close.Click += bt_close_Click;
            


        }

        
        //void handleInput(string input)
        //{
        //    if (input.StartsWith("[RFID]"))
        //    {
        //        input = input.Replace("[RFID] ", "");
        //        Console.WriteLine(input);
        //        if (db.userExists(input))
        //        {
        //            user = db.getUser(input);
        //            bt_apply.IsEnabled = true;
        //            tb_waiting.Text = "RFID Found!";
        //            tb_currentBalance.Text = user.balance.ToString();
        //            tb_name.Text = user.name;
        //            tb_username.Text = user.username;
        //            //Change overlay to show data from RFID
        //        }
        //        else MessageBox.Show("Invalid RFID");

        //    }
        //}

        private void bt_close_Click(object sender, RoutedEventArgs e)
        {
            client.Close();
            this.Close();
        }

        private void bt_apply_Click(object sender, RoutedEventArgs e)
        {
            db.addBalance(user.rfid, double.Parse(dc_balance.Value.ToString()));
            MessageBox.Show(string.Format("Successfully added {0} kr to {1}",dc_balance.Value,user.username));
            user = new SnackOverflowC.User();
            bt_apply.IsEnabled = false;
            tb_waiting.Text = "Waiting for RFID";
            tb_username.Text = "";
            tb_name.Text = "";
            tb_currentBalance.Text = "";
            dc_balance.Value = 0;
            
        }
    }
}
