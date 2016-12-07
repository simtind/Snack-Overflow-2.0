using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        SnackOverflowC.Database db;
        SnackOverflowC.User user;
        UdpClient client;
        
        public AddBalanceWindow()
        {
            InitializeComponent();
            db = new SnackOverflowC.Database();

            if (!db.checkDB())
            {
                MessageBox.Show("Error opening database");
                this.Close();
            }

            bt_apply.Click += bt_apply_Click;
            bt_close.Click += bt_close_Click;

            ReadLoop();

        }

        public async Task ReadLoop()
        {
            using (client = new UdpClient(25565))
            {
                while (true)
                {
                    var data = await client.ReceiveAsync();
                    handleInput(Encoding.ASCII.GetString(data.Buffer));
                }
            }
        }

        void handleInput(string input)
        {
            if (input.StartsWith("[RFID]"))
            {
                input = input.Replace("[RFID] ", "");
                Console.WriteLine(input);
                if (db.userExists(input))
                {
                    user = db.getUser(input);
                    bt_apply.IsEnabled = true;
                    tb_waiting.Text = "RFID Found!";
                    tb_currentBalance.Text = user.balance.ToString();
                    tb_name.Text = user.name;
                    tb_username.Text = user.username;
                    //Change overlay to show data from RFID
                }
                else MessageBox.Show("Invalid RFID");

            }
        }

        //The below code is super ugly. But it works! Throws an exception every time you close the UDP socket, because it's apparently already closed. idk. weird.

        private void bt_close_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client.Client.Close();
            }
            catch
            {
                Console.WriteLine("Already closed UDPClient");
            }

            this.Close();
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = false;
            try
            {
                client.Client.Close();
            }
            catch
            {
                Console.WriteLine("Already closed UDPClient");
            }

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
