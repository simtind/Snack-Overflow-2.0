using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;

namespace SnackOverflowC
{
    class Cart
    {

        private List<Item> cart;
        private Item lastItem;
        public double total;
        //private Item lastItem;
        public Cart()
        {
            cart = new List<Item>();
        }
        public void addItemToCart(Item item, ref StackPanel sp_items, ref ScrollViewer sv_items, ref TextBlock tb_total)
        {
            if(item.upc != lastItem.upc || sp_items.Children.Count == 0)
            {
                uc_item child = new uc_item();
                child.tb_name.Text = item.alias;
                child.tb_price.Text = string.Format("{0:N2}", Math.Round(item.price, 2));
                child.rect_colorgroup.Fill = new SolidColorBrush(Color.FromRgb(item.color[0], item.color[1], item.color[2]));
                child.tb_quantity.Text = "1";

                sp_items.Children.Add(child);
                sv_items.ScrollToBottom();
            }
            else
            {
                string qty = sp_items.Children.OfType<uc_item>().LastOrDefault().tb_quantity.Text;
                sp_items.Children.OfType<uc_item>().LastOrDefault().tb_quantity.Text = (int.Parse(qty) + 1).ToString();
            }

            lastItem = item;
            cart.Add(item);
            updateTotal(ref tb_total);
        }

        private void updateTotal(ref TextBlock tb_total)
        {
            double total = 0;
            foreach (var items in cart)
            {
                total += items.price;
            }
            this.total = total;

            tb_total.Text = string.Format("{0:N2}", Math.Round(total, 2));
        }

        public void clearCart(ref StackPanel sp_items, ref TextBlock tb_total)
        {
            cart.Clear();
            total = 0;
            sp_items.Children.Clear();
            tb_total.Text = "0";

        }

        public void recordPurchase(User user)
        {
            string dir = Assembly.GetExecutingAssembly().Location;
            dir = dir.Replace(dir.Split('\\').Last(), "");
            try
            {


                using (System.IO.StreamWriter file =
                    new System.IO.StreamWriter(string.Format(dir + "reports/{0}.csv", DateTime.Now.ToString("MMMM yy")), true))
                {
                    foreach (var item in cart)
                    {
                        file.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6}", DateTime.Now.ToString("dd MMM"), DateTime.Now.ToString("HH:mm:ss"), user.name, user.username, item.upc, item.name, item.price));
                    }
                    file.WriteLine(string.Format(",,,,,,,{0}",user.balance));
                }
            }
            catch
            {
                MessageBox.Show("Purchase was drawn from account, but not recorded. Contact admin!");
            }
        }
    }
}
