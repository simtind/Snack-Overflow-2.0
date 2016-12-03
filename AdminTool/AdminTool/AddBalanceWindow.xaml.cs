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
        public AddBalanceWindow()
        {
            InitializeComponent();

            bt_apply.Click += bt_apply_Click;
            bt_close.Click += bt_close_Click;

            ReadLoop();
        }

        public async Task ReadLoop()
        {
            using (UdpClient client = new UdpClient(25565))
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
                Console.WriteLine(input.Replace("[RFID] ", ""));
                //handleRFID(input.Replace("[RFID] ", ""));
        }

        private void bt_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void bt_apply_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
