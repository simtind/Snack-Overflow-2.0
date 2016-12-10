using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace SnackOverflowC
{
    class Database
    {
        private NpgsqlConnection conn;


        public Database()
        {
            conn = new NpgsqlConnection("Host=127.0.0.1;Port=5432;Username=postgres;Password=admin;Database=snackoverflow");
        }

        public bool openDB()
        {
            try
            {
                conn.Open();
                Console.WriteLine("Database opened successfully");

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error opening the database. Exiting.");
                return false;
            }


        }

        public Item getItem(string upc)
        {
            Item item = new Item();
            using (var cmd = new NpgsqlCommand("SELECT upc,name,alias,price,color FROM items WHERE upc = @upc", conn))
            {
                cmd.Parameters.Add(new NpgsqlParameter("upc", upc));
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        item.upc = reader.GetString(0);
                        item.name = reader.GetString(1);
                        item.alias = reader.GetString(2);
                        item.price = reader.GetDouble(3);

                        string[] colorStrings;
                        try
                        {
                            colorStrings = (reader.GetString(4).Split(','));
                        }
                        catch (Exception e)
                        {
                            colorStrings = new string[3] { "255", "0", "0" };
                        }

                        item.color = new byte[3];
                        for (int i = 0; i < 3; i++)
                            item.color[i] = byte.Parse(colorStrings[i]);
                    }
                }
            }
            return item;
        }

        public User getUser(string rfid)
        {
            User user = new User();
            user.error = false;
            try {
                using (var cmd = new NpgsqlCommand("SELECT name,balance,username,picture FROM students WHERE rfid = @rfid", conn))
                {
                    cmd.Parameters.Add(new NpgsqlParameter("rfid", rfid));
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user.name = reader.GetString(0);
                            user.balance = reader.GetDouble(1);
                            user.username = reader.GetString(2);
                            user.picturegroup = reader.GetString(3);
                            user.rfid = rfid;
                        }
                    }
                }
                return user;
            }
            catch(Exception e)
            {
                MessageBox.Show("Critical error fetching user. Contact administrator immediately");
                user.error = true;
                return user;
            }
        }

        public bool itemExists(string upc)
        {
            try {
                using (var cmd = new NpgsqlCommand("SELECT EXISTS(SELECT upc FROM items WHERE upc = @upc)", conn))
                {
                    cmd.Parameters.Add(new NpgsqlParameter("upc", upc));
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return reader.GetBoolean(0);
                        }
                    }
                }
                return false;
            }
            catch(Exception e){
                MessageBox.Show("Error checking existence of item");
                return false;
            }
        }

        public bool userExists(string rfid)
        {
            try
            {
                using (var cmd = new NpgsqlCommand("SELECT EXISTS(SELECT rfid FROM students WHERE rfid = @rfid)", conn))
                {
                    cmd.Parameters.Add(new NpgsqlParameter("rfid", rfid));
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return reader.GetBoolean(0);
                        }
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error checking existence of user");
                return false;
            }
        }

        public void pullBalance(string rfid, double amount)
        {
            amount = Math.Abs(amount);
            double balance = 0;

            try
            {
                using (var cmd = new NpgsqlCommand("SELECT balance FROM students WHERE rfid = @rfid", conn))
                {
                    cmd.Parameters.Add(new NpgsqlParameter("rfid", rfid));
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            balance = reader.GetDouble(0);

                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error fetching balance. Your balance is unaffected");
                return;
            }


            try
            {
                using (var cmd = new NpgsqlCommand("UPDATE students SET (balance) = (@balance) WHERE rfid = @rfid", conn))
                {
                    double toBePulled = Math.Round(balance - amount, 2, MidpointRounding.AwayFromZero);
                    cmd.Parameters.Add(new NpgsqlParameter("balance", toBePulled));
                    cmd.Parameters.Add(new NpgsqlParameter("rfid", rfid));
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error pulling balance from account. Your balance is unaffected");
                return;
            }


        }

        public void addBalance(string rfid, double amount)
        {
            amount = Math.Abs(amount);
            double balance = 0;
            try
            {
                using (var cmd = new NpgsqlCommand("SELECT balance FROM students WHERE rfid = @rfid", conn))
                {
                    cmd.Parameters.Add(new NpgsqlParameter("rfid", rfid));
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            balance = reader.GetDouble(0);

                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error fetching balance. Your balance is unaffected");
                return;
            }




            try
            {
                using (var cmd = new NpgsqlCommand("UPDATE students SET (balance) = (@balance) WHERE rfid = @rfid", conn))
                {
                    double toBeAdded = Math.Round(balance + amount, 2, MidpointRounding.AwayFromZero);
                    cmd.Parameters.Add(new NpgsqlParameter("balance", toBeAdded));
                    cmd.Parameters.Add(new NpgsqlParameter("rfid", rfid));
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error adding balance. Your balance is unaffected");
                return;
            }

        }

        public List<string> getPictureGroups()
        {
            List<string> list = new List<string>();
            try
            {
                using (var cmd = new NpgsqlCommand("SELECT * FROM PICTUREGROUPS", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(reader.GetString(0));
                        }
                    }
                }
                return list;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error fetching picture groups");
                return list;
            }
        } 

        public void addUser(User user)
        {
            try {
                using (var cmd = new NpgsqlCommand("INSERT INTO students (rfid,name,balance,picture,username) VALUES (@rfid,@name,@balance,@picture,@username)", conn))
                {
                    cmd.Parameters.Add(new NpgsqlParameter("rfid", user.rfid));
                    cmd.Parameters.Add(new NpgsqlParameter("name", user.name));
                    cmd.Parameters.Add(new NpgsqlParameter("username", user.username));
                    cmd.Parameters.Add(new NpgsqlParameter("balance", user.balance));
                    cmd.Parameters.Add(new NpgsqlParameter("picture", user.picturegroup));

                    cmd.ExecuteNonQuery();
                }
            } 
            catch (Exception e)
            {
                MessageBox.Show("Error adding user. Your balance is unaffected");
                return;
            }
        
        }


        public void addItem(Item item)
        {
            try
            {
                using (var cmd = new NpgsqlCommand("INSERT INTO items (upc,name,alias,price,color) VALUES (@upc,@name,@alias,@price,@color)", conn))
                {
                    cmd.Parameters.Add(new NpgsqlParameter("upc", item.upc));
                    cmd.Parameters.Add(new NpgsqlParameter("name", item.name));
                    cmd.Parameters.Add(new NpgsqlParameter("alias", item.alias));
                    cmd.Parameters.Add(new NpgsqlParameter("price", item.price));
                    cmd.Parameters.Add(new NpgsqlParameter("color", string.Format("{0},{1},{2}",item.color[0], item.color[1], item.color[2])));

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error adding item");
                return;
            }

        }
    }
}
