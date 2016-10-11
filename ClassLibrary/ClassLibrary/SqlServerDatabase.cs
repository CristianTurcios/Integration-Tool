using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace ClassLibrary
{
    public class SqlServerDatabase : InterfaceDatabase
    {
        private string ip;
        private string port;
        private string nameDataBase;
        private string serverInstance;
        private string username;
        private string password;
        private Integration integration;

        private SqlConnection con = null;

        public SqlServerDatabase(string ip, string port, string nameDataBase, string serverInstance, string username, string password, Integration integration = null)
        {
            this.ip = ip;
            this.port = (port == string.Empty) ? null : "," + port;
            this.nameDataBase = nameDataBase;
            this.serverInstance = (serverInstance == string.Empty) ? null : "\\" +serverInstance;
            this.username = username;
            this.password = password;
            this.integration= integration;
        }

        public void openConnection()
        {
           
            string conectionString = "Data Source=" + this.ip + this.serverInstance + this.port + ";" + 
                                    "Initial Catalog=" + this.nameDataBase + ";" + 
                                    "User ID="  + this.username + ";" + 
                                    "Password=" + this.password;

            /*string conectionString = "Data Source=" + this.serverInstance + ";" +
                        "Initial Catalog=" + this.nameDataBase + ";" +
                        "Integrated Security=true;";*/

            this.con = new SqlConnection(conectionString);

            try
            {
                this.con.Open();
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                string message = e.Message;
                message = message.Replace("'", "");
                string queryToLog2 = "insert into SystemLogs (Description,ErrorDate, IntegrationId) values('Class SqlServer: " + message + "','" + DateTime.Now + "'," + this.integration.integrationId + ")";
                integration.insertLog(queryToLog2);

                Integration.flag = false;

                throw e;
            }       
        }

        public void closeConnection()
        {
            if (con != null)
                con.Close();

            con = null;
        }

        public string executeQuery(string query)
        {
            SqlCommand queryCommand = new SqlCommand(query, con);
            string result = "";


            try
            {
                SqlDataReader reader = queryCommand.ExecuteReader();
            
                while (reader.Read())
                    result += reader.GetString(0) + "%";
            }
            catch (System.InvalidOperationException e)
            {
                string message = e.Message;
                message = message.Replace("'", "");
                string queryToLog = "insert into SystemLogs (Description,ErrorDate, IntegrationId) values('Class SqlServer: " + message + "','" + DateTime.Now + "'," + this.integration.integrationId + ")";
                integration.insertLog(queryToLog);

                Integration.flag = false;

                throw e;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                string message = e.Message;
                message = message.Replace("'", "");
                string queryToLog = "insert into SystemLogs (Description,ErrorDate, IntegrationId) values('Class SqlServer: " + message + "','" + DateTime.Now + "'," + this.integration.integrationId + ")";
                integration.insertLog(queryToLog);

                Integration.flag = false;
                
                throw e;
            }           
        
            return result;
        }
    }
}
