using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnackOverflowC
{
    class Database
    {
        private NpgsqlConnection conn;


        public Database()
        {
            conn = new NpgsqlConnection("Host=127.0.0.1;Port=5432;Username=postgres;Password=admin;Database=snackoverflow");
        }

        public bool checkDB()
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

        public bool itemExists(string upc)
        {
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

        public bool userExists(string rfid)
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

        public void pullBalance(string rfid, double amount)
        {
            amount = Math.Abs(amount);
            double balance = 0;
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

            using (var cmd = new NpgsqlCommand("UPDATE students SET (balance) = (@balance) WHERE rfid = @rfid",conn))
            {
                cmd.Parameters.Add(new NpgsqlParameter("balance", balance-amount));
                cmd.Parameters.Add(new NpgsqlParameter("rfid", rfid));
                cmd.ExecuteNonQuery();
            }


        }

        public void addBalance(string rfid, double amount)
        {
            amount = Math.Abs(amount);
            double balance = 0;
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

            using (var cmd = new NpgsqlCommand("UPDATE students SET (balance) = (@balance) WHERE rfid = @rfid", conn))
            {
                cmd.Parameters.Add(new NpgsqlParameter("balance", balance + amount));
                cmd.Parameters.Add(new NpgsqlParameter("rfid", rfid));
                cmd.ExecuteNonQuery();
            }


        }

        public List<string> getPictureGroups()
        {
            List<string> list = new List<string>();
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

        public void addUser(User user)
        {
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
    }
}
