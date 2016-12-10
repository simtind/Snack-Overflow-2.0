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
using System.Drawing;

namespace AdminTool
{
    /// <summary>
    /// Interaction logic for AddUserWindow.xaml
    /// </summary>
    public partial class AddItemWindow : Window
    {
        SnackOverflowC.Database db;
        SnackOverflowC.Item item;
        UdpClient client;

        public AddItemWindow()
        {
            InitializeComponent();
            db = new SnackOverflowC.Database();


            if (!db.openDB())
            {
                MessageBox.Show("Error opening database");
                this.Close();
            }

            bt_apply.Click += bt_apply_Click;
            bt_close.Click += bt_close_Click;

            ReadLoop();

        }

        private async Task ReadLoop()
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

        private void handleInput(string input)
        {
            if (input.StartsWith("[BARCODE]"))
            {
                input = input.Replace("[BARCODE] ", "");
                if (db.itemExists(input))
                {
                    MessageBox.Show("Item already exists in database");
                }
                else
                {
                    bt_apply.IsEnabled = true;
                    tb_waiting.Text = "Barcode Found!";
                    tb_barcode.Text = input;
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
            if (tb_name.Text != "" && tb_alias.Text != "" && cp_colorpicker.SelectedColorText!="")
            {
                item = new SnackOverflowC.Item();
                bt_apply.IsEnabled = false;

                item.alias = tb_alias.Text;
                item.name = tb_name.Text;
                item.upc = tb_barcode.Text;
                item.price = double.Parse(dc_price.Value.ToString());
                System.Drawing.Color col = ColorTranslator.FromHtml(cp_colorpicker.SelectedColorText);

                item.color = new byte[3];
                item.color[0] = col.R;
                item.color[1] = col.G;
                item.color[2] = col.B;

                try
                {
                    db.addItem(item);
                    tb_alias.Text = "";
                    tb_name.Text = "";
                    tb_barcode.Text = "";
                    dc_price.Value=0;
                    tb_waiting.Text = "Waiting for barcode";
                    MessageBox.Show("Successfully added item");
                }
                catch(Exception f)
                {
                    MessageBox.Show("Error adding item");
                }

            }
            else MessageBox.Show("Please fill out all empty fields");

        }
    }
}

