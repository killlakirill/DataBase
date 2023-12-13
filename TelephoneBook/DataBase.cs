using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace youtube
{

    public class archive
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string Numb { get; set; }
        public string User { get; set; }
        public string Data_add { get; set; }
        public string Ball { get; set; }
    }
    internal class DataBase
    {
        private NpgsqlConnection conn;

        public NpgsqlDataReader reader;
        public DataBase()
        {
            conn = new Npgsql.NpgsqlConnection("Server=localhost; Port=5432; User Id=postgres; Password=admin; Database=TelephoneBook");
            conn.Open();
        }
        public void quarry(string s, string[] param)
        {
            NpgsqlDataReader reader1;
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = s;
            for (int i = 0; i < param.Length; i++)
            {
                cmd.Parameters.AddWithValue($"@value{i + 1}", param[i]);
            }

            reader1 = cmd.ExecuteReader();
            reader = reader1;
        }
        public void close()
        {
            reader.Close();
        }
        public string[] read()
        {
            reader.Read();
            List<string> list = new List<string>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                list.Add(reader[i].ToString());
            }
            return list.ToArray();

        }

    }
}