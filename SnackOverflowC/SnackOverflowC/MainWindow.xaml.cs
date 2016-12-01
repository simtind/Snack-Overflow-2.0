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
        private Cart cart;
        private ResetTimer cartTimer;
        private ResetTimer overlayTimer;






        public MainWindow()
        {
            InitializeComponent();

            db = new DatabaseInstance();
            activeGrid = grid_idle;
            cart = new Cart();
            cartTimer = new ResetTimer(25);
            overlayTimer = new ResetTimer(10);

            cartTimer.ThresholdReached += cartTimer_ThresholdReached;
            cartTimer.TimeChanged += cartTimer_TimeChanged;

            if (!db.checkDB())
            {
                MessageBox.Show("Error connecting to database");
                Environment.Exit(0);
            }


            
            



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
            //change timer label
            cartTimer.stopTimer();
            cartTimer.resetTimer();
            cartTimer.startTimer();

            Item item;
            if (db.itemExists(barcode))
                item = db.getItem(barcode);
            else
                item = db.getItem("666"); //666 is the UPC for a non-existent item

            changeGrid(grid_cart);
            
            cart.addItemToCart(item,ref sp_items,ref sv_items,ref tb_total);
        }

       


        void handleRFID(string rfid)
        {
            Console.WriteLine("RFID event handler");
            if (db.userExists(rfid))
                //update user data
                //update grid_purchase
                changeGrid(grid_purchase);
            else {
                //if not found in db, prompt to enter info
            }
            
        }

        void changeGrid(Grid grid)
        {
            if (activeGrid != grid)
            {
                activeGrid.Visibility = Visibility.Hidden;
                activeGrid = grid;
                activeGrid.Visibility = Visibility.Visible;
            }

        }

        private void cartTimer_ThresholdReached(object sender, EventArgs e)
        {
            cart.clearCart(ref sp_items);
            changeGrid(grid_idle);
        }
        private void cartTimer_TimeChanged(object sender, EventArgs e)
        {
            Console.WriteLine(cartTimer.countdown);
        }



    }
}
