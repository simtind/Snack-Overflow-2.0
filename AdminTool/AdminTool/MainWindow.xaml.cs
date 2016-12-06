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
        public event EventHandler RFIDScanned;
        public MainWindow()
        {

            InitializeComponent();

            
            bt_add_balance.Click += bt_add_balance_Click;
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
            {
                input = input.Replace("[RFID] ", "");
                OnRFIDScanned(EventArgs.Empty);
            }
            else if (input.StartsWith("[BARCODE]"))
            {
                input = input.Replace("[BARCODE] ", "");
            }
        }
        private void bt_add_balance_Click(object sender, RoutedEventArgs e)
        {
            AddBalanceWindow add_balance = new AddBalanceWindow();
            add_balance.ShowDialog();
        }

        public void OnRFIDScanned(EventArgs e)
        {
            EventHandler handler = RFIDScanned;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        
    }
}
