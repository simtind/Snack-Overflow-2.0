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
using System.IO;
using System.Reflection;

namespace SnackOverflowC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 



    /*TODO:
     * Create administrative tool for Snack Overflow
        - Add/Remove/Edit item from database
        - Add/Remove/Edit users from database
        - Add money to users
        - Edit items in stock
     * Create system that keeps track of items in stock
     * Checks to make sure that database editing is recorded
     * Graphic that makes sure the  user knows if the purchase didn't go through
     */
    public partial class MainWindow : Window
    {
        private Database db;
        private Grid activeGrid;
        private Cart cart;
        private ResetTimer cartTimer;
        private ResetTimer overlayTimer;
        private string dir;






        public MainWindow()
        {
            InitializeComponent();
            
            


            db = new Database();
            activeGrid = grid_idle;
            cart = new Cart();
            cartTimer = new ResetTimer(15);
            overlayTimer = new ResetTimer(5);
            //dir = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.G‌​etCurrentProcess().M‌​ainModule.FileName);
            //logo.Source = new BitmapImage(new Uri(string.Format("{0}/img/logo2.jpg",dir)));
            dir = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.G‌​etCurrentProcess().M‌​ainModule.FileName);
            logo.Source = new BitmapImage(new Uri(@"C:\Users\Jakob Lover\Documents\GitHub\Snack-Overflow-2.0\SnackOverflowC\SnackOverflowC\img\logo.jpg"));

            cartTimer.ThresholdReached += cartTimer_ThresholdReached;
            cartTimer.TimeChanged += cartTimer_TimeChanged;
            overlayTimer.ThresholdReached += overlayTimer_ThresholdReached;

            if (!db.checkDB())
            {
                MessageBox.Show("Error connecting to database");
                Environment.Exit(0);
            }



            

            ReadLoop();
        }

        #region Input handlers
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
            overlayTimer.stopTimer();
            overlayTimer.resetTimer();


            Item item;
            if (db.itemExists(barcode))
                item = db.getItem(barcode);
            else
                item = db.getItem("666"); //666 is the UPC for a non-existent item

            changeGrid(grid_cart);

            tb_resetting.Text = cartTimer.threshold.ToString();
            cart.addItemToCart(item,ref sp_items,ref sv_items,ref tb_total);
        }

        void handleRFID(string rfid)
        {
            Console.WriteLine("RFID event handler");
            User user;

            Console.WriteLine(db.checkDB());
            if (db.userExists(rfid))
            {
                //put all of this in a try catch to prevent db issues
                //notify user if purchase didnt go through
                //TODO: Put the overlay in new class. this is atrocious.

                

                
                user = db.getUser(rfid);
                if(user.balance-cart.total<0)
                {
                    MessageBox.Show("Insufficient funds");
                }
                else
                {
                    overlayTimer.stopTimer();
                    overlayTimer.resetTimer();
                    overlayTimer.startTimer();
                    cartTimer.stopTimer();
                    cartTimer.resetTimer();

                    db.pullBalance(rfid, cart.total);
                    user = db.getUser(rfid); //get updated balance after we have pulled balance

                    p_tb_student.Text = string.Format("{0} ({1})", user.name, user.username);
                    p_tb_amount.Text = tb_total.Text;

                    string tmp = string.Format("{0:N2}", Math.Round(user.balance, 2)).Replace(',','_').Replace('.',',');
                    p_tb_balance.Text = tmp.Replace('_', '.');

                    changeOverlayPicture(user.picturegroup);
                    changeGrid(grid_purchase);
                    cart.clearCart(ref sp_items, ref tb_total);
                }
                

            }    
            else {
                MessageBox.Show("Couldn't find RFID" + rfid);
            }
            
            
        }

        #endregion

        #region Helper functions
        void changeGrid(Grid grid)
        {
            if (activeGrid != grid)
            {
                activeGrid.Visibility = Visibility.Hidden;
                activeGrid = grid;
                activeGrid.Visibility = Visibility.Visible;
            }

        }

        void changeOverlayPicture(string picturegroup)
        {
            int fCount = Directory.EnumerateFiles(string.Format(@"C:\Users\Jakob Lover\Documents\GitHub\Snack-Overflow-2.0\SnackOverflowC\SnackOverflowC\img\{0}", picturegroup), "*.png", SearchOption.AllDirectories).Count();
            Random rnd = new Random();
            int rand = rnd.Next(1, fCount+1);
            p_image.Source = new BitmapImage(new Uri(string.Format(@"C:\Users\Jakob Lover\Documents\GitHub\Snack-Overflow-2.0\SnackOverflowC\SnackOverflowC\img\{0}\{1}.png",picturegroup,rand)));
        }
        #endregion

        #region Events
        private void cartTimer_ThresholdReached(object sender, EventArgs e)
        {
            cart.clearCart(ref sp_items, ref tb_total);
            changeGrid(grid_idle);
        }
        private void cartTimer_TimeChanged(object sender, EventArgs e)
        {
            tb_resetting.Text = cartTimer.countdown.ToString();
        }

        private void overlayTimer_ThresholdReached(object sender, EventArgs e)
        {
            
            p_tb_amount.Text = "0";
            p_tb_balance.Text = "0";
            p_tb_student.Text = "Invalid Student";
            p_image.Source = null;

            cart.clearCart(ref sp_items, ref tb_total);
            changeGrid(grid_idle);
        }

        #endregion



    }
}
