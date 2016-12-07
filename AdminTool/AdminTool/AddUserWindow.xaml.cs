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
    /// Interaction logic for AddUserWindow.xaml
    /// </summary>
    public partial class AddUserWindow : Window
    {
        SnackOverflowC.Database db;
        SnackOverflowC.User user;
        UdpClient client;
        public AddUserWindow()
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

            UpdatePictureComboBox();
            ReadLoop();

        }

        public void UpdatePictureComboBox()
        {
            cb_picturegroup.Items.Clear();
            db.getPictureGroups();

            var groups = db.getPictureGroups();
            foreach (var group in groups)
            {
                cb_picturegroup.Items.Add(group);
            }
            cb_picturegroup.SelectedItem = cb_picturegroup.Items.GetItemAt(0);
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
                if (db.userExists(input))
                {
                    MessageBox.Show("User already exists in database");
                }
                else
                {
                    bt_apply.IsEnabled = true;
                    tb_waiting.Text = "RFID Found!";
                    tb_RFID.Text = input;
                    Console.WriteLine(cb_picturegroup.SelectedValue);
                }
                
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
            //if one field is empty, do none of the below
            //MessageBox.Show(string.Format("Successfully added \n Name: {0}\n Username {1} \n Picture group {2}", user.username));
            user = new SnackOverflowC.User();
            bt_apply.IsEnabled = false;
            tb_waiting.Text = "Waiting for RFID";
            user.balance = 0;
            user.name = tb_name.Text;
            user.username = tb_username.Text;
            user.rfid = tb_RFID.Text;
            user.picturegroup = cb_picturegroup.SelectedValue.ToString();
            db.addUser(user);

            
            tb_name.Text = "";
            tb_username.Text = "";
            tb_RFID.Text = "";
            cb_picturegroup.SelectedItem = cb_picturegroup.Items.GetItemAt(0);

        }
    }
}

