using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnackOverflowC
{
    class DatabaseInstance
    {
        private NpgsqlConnection conn;
        
        
        public DatabaseInstance()
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
            using (var cmd = new NpgsqlCommand("SELECT upc,name,alias,price FROM items WHERE upc = @upc",conn))
            {
                cmd.Parameters.Add(new NpgsqlParameter("upc", upc));
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(reader.GetString(0));
                        item.upc = reader.GetString(0);
                        item.name = reader.GetString(1);
                        item.alias = reader.GetString(2);
                        item.price = reader.GetDouble(3);
                    }
                }
            }
            return item;
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


        



    }
}
