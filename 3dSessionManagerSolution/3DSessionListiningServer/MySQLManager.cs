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
                String safeMessage = message.Replace("<EOF>", "");               
                InstanceMessage msg = JsonConvert.DeserializeObject<InstanceMessage>(safeMessage);
                //Save the message to a message log
                messagelog messageLogObj = new messagelog();
                messageLogObj.timestamp = DateTime.Now;
                messageLogObj.externalID = msg.id;
                messageLogObj.dataType = msg.dataType;
                messageLogObj.data = msg.data;
                db.messagelogs.Add(messageLogObj);
                db.SaveChanges();

                instance instanceObj = db.instances.Where(a => a.externalId == msg.id).SingleOrDefault();
                List<location> allLocation = db.locations.Where(a => a.instanceId == instanceObj.id).ToList();
                List<session> activeSessions = new List<session>();
                foreach (var location in allLocation)
                {
                    var sessionsObj = db.sessions.Where(a => a.setupId == location.setupId && a.isActive == true).ToList();
                    activeSessions.AddRange(sessionsObj);
                }
                foreach (var activesession in activeSessions)
                {
                    sessiondata obj = new sessiondata();
                    obj.sessionId = activesession.id;
                    obj.instanceId = instanceObj.id;
                    obj.dataType = msg.dataType;
                    obj.data = msg.data;
                    obj.timeStamp = DateTime.Now;
                    db.sessiondatas.Add(obj);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return true;
        }
    }
}
