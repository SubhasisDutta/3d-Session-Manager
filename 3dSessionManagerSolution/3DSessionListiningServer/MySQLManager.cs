using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace _3DSessionListiningServer
{
    class InstanceMessage
    {
        public string id ;
        public string dataType;
        public string data;
    }
    public class MySQLManager
    {
        private Entities db = new Entities();

        public MySQLManager()
        {
           
        }
        public Boolean pushData(String message)
        {            
            try
            {
                //Receive Client message and parse it to a JSON Object
                InstanceMessage msg = JsonConvert.DeserializeObject<InstanceMessage>(message.Replace("<EOF>", ""));
                //Save the message to a message log
                messagelog messageLogObj = new messagelog();
                messageLogObj.timestamp = DateTime.Now;
                messageLogObj.externalID = msg.id;
                messageLogObj.dataType = msg.dataType;
                messageLogObj.data = msg.data;
                db.messagelogs.Add(messageLogObj);
                db.SaveChanges();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return true;
        }
    }
}
