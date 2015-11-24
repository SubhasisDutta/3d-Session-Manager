using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace _3DSessionListiningServer
{
    public class MySQLManager
    {
        MySqlConnection conDatabase = null;
        public MySQLManager()
        {
            string conststring = "Server=127.0.0.1;port=3306;Database=3dsessiondb;username=root;password=root;";
            conDatabase = new MySqlConnection(conststring);
            //conDatabase.Open();
        }
        public Boolean pushData(String message)
        {
            string query = "insert into messagelog (externalId,timestamp,dataType,data) VALUES ('TestingID','2015-01-01','TESTTYPE','" + message + "');";
            MySqlCommand cmdDatabase = new MySqlCommand(query, this.conDatabase);
            try
            {
                conDatabase.Open();
                cmdDatabase.ExecuteReader();
                //MySqlDataReader myReader = cmdDatabase.ExecuteReader();
                Console.WriteLine("SAVED To DB" + message);
                conDatabase.Close();
                //while (myReader.Read())
                //{

                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return true;
        }
    }
}
