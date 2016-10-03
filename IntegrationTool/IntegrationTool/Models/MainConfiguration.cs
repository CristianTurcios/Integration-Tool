using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace IntegrationTool.Models
{
    public class MainConfiguration
    {
        private string dataConnection;
        private SqlConnection connection;
       

        public MainConfiguration(string server = "172.20.33.13", string databaseName = "IntegrationTool", string userId = "SISUser", string password = "test2016!")
        {
            this.dataConnection = "Data Source=" + server + ";Initial Catalog=" + databaseName + ";User ID="+userId+";Password="+password;  
            InitialConnection(this.dataConnection);
        }

        //0
        private void InitialConnection(string dataConnection)
        {
            connection = new SqlConnection();
            connection.ConnectionString = dataConnection;
        }

        //0.1
        private void OpenConnection()
        {
            try
            {
                connection.Open();      
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                Console.WriteLine(e.Message);
            }           
        }

        //0.2
        private void CloseConnection()
        {
            try
            {
                connection.Close();    
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                Console.WriteLine(e.Message);
            }                 
        }

        public List<DataRow> ParamToGenerateGraphicsForManualIntegration()
        {
            string query = "select COUNT(IntegrationId) as Count_Integrations,MONTH(IntegrationDate) as Month_Integrations from Integrations where IntegrationCategoryId= 1 "+
                           " group by MONTH(IntegrationDate);";
       
            OpenConnection();
            var table = DataTable(query);
            CloseConnection();

            List<DataRow> list = table.AsEnumerable().ToList();

            return list;
        }

        public List<DataRow> ParamToGenerateGraphicsForAutomaticIntegration()
        {
            string query = "select COUNT(IntegrationId) as Count_Integrations,MONTH(IntegrationDate) as Month_Integrations from Integrations where IntegrationCategoryId= 2 " +
                          " group by MONTH(IntegrationDate);";
                    
            OpenConnection();         
            var table = DataTable(query);
            CloseConnection();

            List<DataRow> list = table.AsEnumerable().ToList();

            return list;        
        }

        public List<DataRow> ParamToGenerateGraphicsForUsersLocalsInSystem()
        {
            string query = "select count(Users.UserId) as Local_Users from Users where UserTypeId =1;";

            OpenConnection();
            var table = DataTable(query);
            CloseConnection();

            List<DataRow> list = table.AsEnumerable().ToList();

            return list;
        }

        public List<DataRow> ParamToGenerateGraphicsForUsersActiveDirectoryInSystem()
        {
            string query = "select count(Users.UserId) as ActiveDirectory_Users from Users where UserTypeId=2;";

            OpenConnection();
            var table = DataTable(query);
            CloseConnection();

            List<DataRow> list = table.AsEnumerable().ToList();

            return list;
        }
                   
        private DataTable DataTable(string query)
        {
            SqlCommand sqlCommand = new SqlCommand(query, connection);
            DataTable table = new DataTable();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

            try
            {
                sqlDataAdapter.Fill(table);
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                Console.WriteLine(e.Message);
            }

            return table;            
        } 
    }
}