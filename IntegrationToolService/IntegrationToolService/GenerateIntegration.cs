using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using ClassLibrary;
using System.IO;

namespace IntegrationToolService
{
    public class GenerateIntegration
    {                
        private string dataConnection;
        private SqlConnection connection;
        private Integration integration = new Integration();
        private Encrypt decrypt = new Encrypt();
        private Curl curl = new Curl();
        private  WriteFileController writeFileController= new WriteFileController();
        private string emails = "";
        //private string location = "";
            
        public GenerateIntegration(string server = "172.20.33.13", string databaseName = "IntegrationTool", string userId="SISUser",string password="test2016!")
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

        //1
        public void ObtainQueryToVerifyTimeToExecutionIntegration()
        {                  
            DateTime datetimeNow = DateTime.Now;
           
            string query =
                "SELECT  dbo.Calendars.IntegrationId,dbo.Calendars.CalendarId,dbo.Calendars.NextExecutionDate,dbo.Calendars.ExecutionEndDate," +
                " dbo.Calendars.Emails,dbo.Recurrences.RecurrenceId" +
                " FROM dbo.Calendars INNER JOIN dbo.Recurrences ON dbo.Calendars.RecurrenceId = dbo.Recurrences.RecurrenceId";

            OpenConnection();
            var table = DataTable(query);
            CloseConnection();
          
            VerifyTimeToExecutionIntegration(table, datetimeNow);         
        }

        //
        private bool ObtainStateIntegration(int integrationId)
        {
            int Enable = 1;
            

            string query =
                "select StatusId from Integrations where IntegrationId ='"+integrationId+"'";

            OpenConnection();
            var table = DataTable(query);
            CloseConnection();

            int status = Convert.ToInt32(table.Rows[0]["StatusId"]);

            return VerfiyStateIntegration(Enable, status);                               
        }

        private static bool VerfiyStateIntegration(int Enable, int status)
        {
            if (status == Enable)
                return true;

            else
                return false;
        }

        //2
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

        //3
        private void VerifyTimeToExecutionIntegration(DataTable table, DateTime datetimeNow)
        {
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (ObtainStateIntegration(Convert.ToInt32(table.Rows[i]["IntegrationId"])))
                {
                    DateTime ExecutionEndDate = Convert.ToDateTime(table.Rows[i]["ExecutionEndDate"]);

                    if (compareEndDatetime(datetimeNow, ExecutionEndDate))
                    {
                        NextExecutionDate(table, datetimeNow, i);
                    }
                }                     
            }
        }

        //3.0
        private static bool compareEndDatetime(DateTime datetimeNow, DateTime TimeExecutionDate)
        {
            if (DateTime.Compare(datetimeNow, TimeExecutionDate) < 0)
              return true;
            
            else
              return false;        
        }
          
        //3.1
        private void NextExecutionDate(DataTable table, DateTime datetimeNow, int i)
        {
            DateTime nextExecutionDate = Convert.ToDateTime(table.Rows[i]["NextExecutionDate"]);
            if (compareDatetime(datetimeNow, nextExecutionDate))
            {              
                int integrationId = Convert.ToInt32(table.Rows[i]["IntegrationId"]);
                int recurrenceId = Convert.ToInt32(table.Rows[i]["RecurrenceId"]);
                int calendarId = Convert.ToInt32(table.Rows[i]["CalendarId"]);
                emails = Convert.ToString(table.Rows[i]["Emails"]);

                updateTimeToExecuteIntegration(calendarId, setNewTimeToExecuteIntegration(recurrenceId, nextExecutionDate));

                try
                {
                   integration.initIntegrationAutomatic(integrationId, emails);
                }catch(Exception e)
                {
                    Console.WriteLine();
                    Console.WriteLine("Error ocurred");
                    Console.WriteLine("/////////////////");
                    Console.WriteLine(e.Message);
                    Console.WriteLine("/////////////////");
                }
            }
        }

        //3.2
        private static bool compareDatetime(DateTime datetimeNow, DateTime TimeExecutionDate)
        {
            return datetimeNow.ToString("yyyy-MM-dd-HH-mm").Equals(TimeExecutionDate.ToString("yyyy-MM-dd-HH-mm"));
        }

        //4
        private DateTime setNewTimeToExecuteIntegration(int recurrenceId, DateTime nextExecutionDate)
        {
            DateTime nexTime = DateTime.Now;

            int recurrenceHourly = 1;
            int dailyRecurrence = 2;
            int recurrenceEveryWeek = 3;
            int monthlyRecurrence = 4;

            if (recurrenceId == recurrenceHourly)
                nexTime = nextExecutionDate.AddHours(1);

            else
                if (recurrenceId == dailyRecurrence)
                    nexTime = nextExecutionDate.AddDays(1);

                else
                    if (recurrenceId == recurrenceEveryWeek)
                        nexTime = nextExecutionDate.AddDays(7);

                    else if (recurrenceId == monthlyRecurrence)
                        nexTime = nextExecutionDate.AddMonths(1);

            return nexTime;
        }

        //5
        private void updateTimeToExecuteIntegration(int calendarId, DateTime nextExecutionDate)
        {
            string query =
              "Update Calendars set NextExecutionDate='" + nextExecutionDate + "' where CalendarId=" + calendarId;

            OpenConnection();
            SqlCommand sqlCommand = new SqlCommand(query, connection);
            sqlCommand.ExecuteNonQuery();
            CloseConnection();
        }  
    }
}
