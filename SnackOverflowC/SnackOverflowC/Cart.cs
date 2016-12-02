﻿using System;
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

namespace SnackOverflowC
{
    class Cart
    {

        private List<Item> cart;
        private Item lastItem;
        //private Item lastItem;
        public Cart()
        {
            cart = new List<Item>();
        }
        public void addItemToCart(Item item, ref StackPanel sp_items, ref ScrollViewer sv_items, ref TextBlock tb_total)
        {
            if(item.upc != lastItem.upc)
            {
                uc_item child = new uc_item();
                child.tb_name.Text = item.alias;
                child.tb_price.Text = string.Format("{0:N2}", Math.Round(item.price, 2)).Replace('.', ',');
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
            
            tb_total.Text = string.Format("{0:N2}", Math.Round(total, 2)).Replace('.',',');
        }

        public void clearCart(ref StackPanel sp_items)
        {
            cart.Clear();
            sp_items.Children.Clear();
        }


    }
}