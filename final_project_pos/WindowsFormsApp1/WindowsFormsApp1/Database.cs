using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp1
{
    class Database
    {
        string dbHost = "sql104.epizy.com";
        string dbUser = "epiz_24401974";
        string dbPass = "yRx0VB7wLJ3KwM";
        string dbName = "epiz_24401974_ezskin";
        string dbport = "3306";
        public MySqlConnection conn;

        public void Connect()
        {
            string connectionString = "server=140.115.200.11;uid=choichiwai;pwd=;database=final_project_pos;charset=utf8;";
            conn = new MySqlConnection(connectionString);
        }

        public MySqlDataReader GetData(string sql)
        {
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            return cmd.ExecuteReader();
        }

        public long InsertData(string sql)
        {
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteScalar();
            return cmd.LastInsertedId;
        }

        public int UpdateData(string sql)
        {
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            int n = cmd.ExecuteNonQuery();
            return n;
        }

        public bool ConnectionOpen()
        {
            try
            {
                conn.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("無法連接到伺服器");

                        break;
                    case 1042:
                        Console.WriteLine("無效的主機名稱");
                        break;
                    case 1045:
                        Console.WriteLine("使用者名稱/密碼錯誤");
                        break;
                }
                return false;
            }
        }

        public void ConnectionClose()
        {
            conn.Close();
        }
    }
}
