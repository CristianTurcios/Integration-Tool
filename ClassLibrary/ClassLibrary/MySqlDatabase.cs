using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ClassLibrary
{
    public class MySqlDatabase : InterfaceDatabase
    {
        private string Ip;
        private string nameDatabase;
        private string serverInstance;
        private string username;
        private string password;
        private string port;
        private MySqlConnection connection;
        private Integration integration;


        public MySqlDatabase(string ip, string nameDatabase, string serverInstance, string username, string password, string port,Integration integration)
        {
            this.Ip = ip;
            this.nameDatabase = nameDatabase;
            this.serverInstance = serverInstance;
            this.username = username;
            this.password = password;
            this.port = port;
            this.integration = integration;

            connectionStringDatabase();
        }

        private void connectionStringDatabase()
        {
            MySqlConnectionStringBuilder instanceDatabase = new MySqlConnectionStringBuilder();
            instanceDatabase.Server = this.serverInstance;
            instanceDatabase.UserID = this.username;
            instanceDatabase.Password = this.password;
            instanceDatabase.Database = this.nameDatabase;
            instanceDatabase.Port = Convert.ToUInt32(this.port);

            connectionDatabase(instanceDatabase.ToString());
        }

        private void connectionDatabase(string instanceDatabase)
        {
            connection = new MySqlConnection(instanceDatabase);
        }

        public void openConnection()
        {
            try
            {
                connection.Open();
            }
            catch (MySqlException e)
            {
                string message = e.Message;
                message = message.Replace("'", "");
                string query = "insert into SystemLogs (Description,ErrorDate, IntegrationId) values('Class Mysql: " + message + "','" + DateTime.Now + "'," + this.integration.integrationId + ")";
                integration.insertLog(query);    
            }           
        }

        public void closeConnection()
        {
            try
            {
                connection.Close();
            }
            catch (MySql.Data.MySqlClient.MySqlException e)
            {
                string message = e.Message;
                message = message.Replace("'", "");
                string queryToLog2 = "insert into SystemLogs (Description,ErrorDate, IntegrationId) values('Class MySql: " + message + "','" + DateTime.Now + "'," + this.integration.integrationId + ")";
                integration.insertLog(queryToLog2);    
            }              
        }

        public string executeQuery(string query)
        {
            string responseQuery = "";

            MySqlCommand command = connection.CreateCommand();
            command.CommandText = query;
            //openConnection();
            try
            {
                command.ExecuteNonQuery();
                MySqlDataReader returnQuery = command.ExecuteReader();

                while (returnQuery.Read())
                {
                    string response = returnQuery.GetString(0);
                    responseQuery += response + "%";
                }
            }
            catch (InvalidOperationException e)
            {
                string message = e.Message;
                message = message.Replace("'", "");
                string queryToLog = "insert into SystemLogs (Description,ErrorDate, IntegrationId) values('Class MySql: " + message + "','" + DateTime.Now + "'," + this.integration.integrationId + ")";
                integration.insertLog(queryToLog); 
            }
                  
            //closeConnection();
            return responseQuery;
        }  
    }
}
