using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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

namespace SnackOverflowC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DatabaseInstance db;
        private Grid activeGrid;

        private string barcode;
        private string rfid;
        


        public MainWindow()
        {
            InitializeComponent();

            db = new DatabaseInstance();
            if (!db.checkDB())
            {
                MessageBox.Show("Error connecting to database");
                Environment.Exit(0);
            }

            

            activeGrid = grid_idle;
            barcode = "";
            rfid = "";

            ReadLoop();
        }

        public async Task ReadLoop()
        {
            #region irrelevant
            //await Task.Run(async () =>
            // {
            //     using (var udpClient = new UdpClient(25565))
            //     {
            //         string loggingEvent = "";
            //         while (true)
            //         {
            //            //IPEndPoint object will allow us to read datagrams sent from any source.
            //            var receivedResults = await udpClient.ReceiveAsync();
            //             loggingEvent += Encoding.ASCII.GetString(receivedResults.Buffer);
            //         }
            //     }
            // });
            //////////////////////////////////////////////////////////////////////////////////////////////////////////
            //byte[] data = new byte[1024];
            //IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 25565);
            //UdpClient newsock = new UdpClient(ipep);

            //IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);

            //data = newsock.Receive(ref sender);

            //Console.WriteLine("Message received from {0}:", sender.ToString());
            //Console.WriteLine(Encoding.ASCII.GetString(data, 0, data.Length)); // this will be "data you want to send"
            #endregion
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
            
            if (input.StartsWith("[BARCODE]"))
                handleBarcode(input.Replace("[BARCODE] ", ""));
            else if(input.StartsWith("[RFID]"))
                handleRFID(input.Replace("[RFID] ", ""));

            
        }

        void handleBarcode(string barcode)
        {
            Console.WriteLine("Barcode event handler");
            Console.WriteLine("Barcode: " + barcode);

            //check if there are no items in cart. then update items accordingly
            //changeGrid(grid_cart);

            db.itemExists(barcode);
            if (db.itemExists(barcode))
            {
                Console.WriteLine("barcode exists!!");
                Item item = db.getItem(barcode);

                Console.WriteLine(item.upc);
                Console.WriteLine(item.name);
                Console.WriteLine(item.alias);
                Console.WriteLine(item.price);
            }
            //update grid_cart  
            else {
                Console.WriteLine("barcode doesn't exist");
                //if not found in db, show invalid UPC
            }

        }

        void handleRFID(string rfid)
        {
            Console.WriteLine("RFID event handler");
            if (db_checkValidity_RFID(rfid))
                //update user data
                //update grid_purchase
                changeGrid(grid_purchase);
            else {
                //if not found in db, prompt to enter info
            }
            
        }

        bool db_checkValidity_RFID(string rfid)
        {
            //if rfid is found in db, return true
            return true;
        }

        void changeGrid(Grid grid)
        {
            activeGrid.Visibility = Visibility.Hidden;
            activeGrid = grid;
            activeGrid.Visibility = Visibility.Visible;
        }

    }
}
