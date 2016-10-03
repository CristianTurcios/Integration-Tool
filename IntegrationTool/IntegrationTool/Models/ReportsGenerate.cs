using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary;

namespace IntegrationTool.Models
{
    public class ReportsGenerate
    {       
        private string dataConnection;
        private SqlConnection connection;
        private Integration integration = new Integration();
        private Encrypt decrypt = new Encrypt();

        public ReportsGenerate(string server = "172.20.33.13", string databaseName = "IntegrationTool", string userId = "SISUser", string password = "test2016!")
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

        public DataTable ParamToGenerateReportForSystemLogs(string DateStart, string DateEnd, int IntegrationType = 0, int IntegrationCategory = 0, int OperationWebServices = 0, int DatabaseParameter = 0)
        {
            string RangeDateIntegrationDecision = "";
            string integrationTypeDecision = "";
            string integrationCategoryDecision = "";
            string operationWebServicesDecision = "";
            string DatabaseParameterDecision = "";

            integrationTypeDecision = AssignIntegrationTypeDecision(IntegrationType, integrationTypeDecision);
            integrationCategoryDecision = AssignIntegrationCategoryDecision(IntegrationCategory, integrationCategoryDecision);
            operationWebServicesDecision = AssignOperationWebServicesDecision(OperationWebServices, operationWebServicesDecision);
            DatabaseParameterDecision = AssignDatabaseParameterDecision(DatabaseParameter, DatabaseParameterDecision);
            RangeDateIntegrationDecision = AssignRangeDateIntegrationDecision(DateStart, DateEnd, RangeDateIntegrationDecision);

            string query =  "SELECT  dbo.IntegrationCategories.Name, dbo.IntegrationsType.Name AS TypeIntegration, dbo.OperationsWebServices.Name AS OperationWebServices,"+
                            " dbo.DatabaseParameters.Name AS DatabaseName, dbo.Integrations.IntegrationDate, "+
                            " dbo.SystemLogs.Description, dbo.SystemLogs.ErrorDate,dbo.Integrations.IntegrationName"+
                            " FROM dbo.DatabaseParameters INNER JOIN"+
                            " dbo.Integrations ON dbo.DatabaseParameters.DatabaseParametersId = dbo.Integrations.DatabaseParametersId INNER JOIN"+
                            " dbo.IntegrationCategories ON dbo.Integrations.IntegrationCategoryId = dbo.IntegrationCategories.IntegrationCategoryId INNER JOIN"+
                            " dbo.IntegrationsType ON dbo.Integrations.IntegrationTypeId = dbo.IntegrationsType.IntegrationTypeId INNER JOIN"+
                            " dbo.OperationsWebServices ON dbo.Integrations.OperationWebServiceId = dbo.OperationsWebServices.OperationWebServiceId INNER JOIN"+
                            " dbo.SystemLogs ON dbo.Integrations.IntegrationId = dbo.SystemLogs.IntegrationId"+
                            " where " + integrationCategoryDecision + " and " + integrationTypeDecision + " and " + operationWebServicesDecision + " and " + DatabaseParameterDecision+
                            " and " + RangeDateIntegrationDecision;


            switch (IntegrationType)
            {
                case 0:
                    integrationTypeDecision = "1=1";
                    break;
                default:
                    integrationTypeDecision = "IntegrationsType.IntegrationTypeId ="+IntegrationType;
                    break;             
            }


            OpenConnection();
            var table = DataTable(query);
            CloseConnection();

            return table;

        }

        public DataTable ParamToGenerateReportForIntegrationLogs(string DateStart, string DateEnd, int IntegrationType = 0, int IntegrationCategory = 0, int OperationWebServices = 0, int DatabaseParameter = 0)
        {           
            string RangeDateIntegrationDecision = "";
            string integrationTypeDecision = "";
            string integrationCategoryDecision = "";
            string operationWebServicesDecision = "";
            string DatabaseParameterDecision = "";

            integrationTypeDecision = AssignIntegrationTypeDecision(IntegrationType, integrationTypeDecision);
            integrationCategoryDecision = AssignIntegrationCategoryDecision(IntegrationCategory, integrationCategoryDecision);
            operationWebServicesDecision = AssignOperationWebServicesDecision(OperationWebServices, operationWebServicesDecision);
            DatabaseParameterDecision = AssignDatabaseParameterDecision(DatabaseParameter, DatabaseParameterDecision);
            RangeDateIntegrationDecision = AssignRangeDateIntegrationDecision(DateStart, DateEnd, RangeDateIntegrationDecision);
                           
            string query = "SELECT  dbo.IntegrationCategories.Name, dbo.IntegrationsType.Name AS TypeIntegration, dbo.OperationsWebServices.Name AS OperationWebServices,"+
                           " dbo.DatabaseParameters.Name AS DatabaseName, dbo.IntegrationLogs.ReferenceCode,"+
                           " dbo.IntegrationLogs.Date, dbo.IntegrationLogs.Status,dbo.Integrations.IntegrationName,Integrations.IntegrationDate" +
                           " FROM dbo.Integrations INNER JOIN"+
                           " dbo.IntegrationCategories ON dbo.Integrations.IntegrationCategoryId = dbo.IntegrationCategories.IntegrationCategoryId INNER JOIN"+
                           " dbo.IntegrationsType ON dbo.Integrations.IntegrationTypeId = dbo.IntegrationsType.IntegrationTypeId INNER JOIN"+
                           " dbo.OperationsWebServices ON dbo.Integrations.OperationWebServiceId = dbo.OperationsWebServices.OperationWebServiceId INNER JOIN"+
                           " dbo.DatabaseParameters ON dbo.Integrations.DatabaseParametersId = dbo.DatabaseParameters.DatabaseParametersId INNER JOIN"+
                           " dbo.IntegrationLogs ON dbo.Integrations.IntegrationId = dbo.IntegrationLogs.IntegrationId"+
                           " where "+integrationCategoryDecision +" and "+ integrationTypeDecision + " and "+ operationWebServicesDecision +" and "+DatabaseParameterDecision 
                           + " and "+ RangeDateIntegrationDecision;
                    
            OpenConnection();         
            var table = DataTable(query);
            CloseConnection();

            return table;
         
        }

        private static string AssignRangeDateIntegrationDecision(string DateStart, string DateEnd, string RangeDateIntegrationDecision)
        {
            switch (DateStart)
            {
                case "01-01-1901":
                    RangeDateIntegrationDecision = "1=1";
                    break;
                default:
                    RangeDateIntegrationDecision = "Integrations.IntegrationDate BETWEEN'" + Convert.ToDateTime(DateStart) + "' and '" + Convert.ToDateTime(DateEnd) + "'";
                    break;
            }
            return RangeDateIntegrationDecision;
        }

        private static string AssignDatabaseParameterDecision(int DatabaseParameter, string DatabaseParameterDecision)
        {
            switch (DatabaseParameter)
            {
                case 0:
                    DatabaseParameterDecision = "1=1";
                    break;
                default:
                    DatabaseParameterDecision = "DatabaseParameters.DatabaseParametersId=" + DatabaseParameter;
                    break;
            }
            return DatabaseParameterDecision;
        }

        private static string AssignOperationWebServicesDecision(int OperationWebServices, string operationWebServicesDecision)
        {
            switch (OperationWebServices)
            {
                case 0:
                    operationWebServicesDecision = "1=1";
                    break;
                default:
                    operationWebServicesDecision = "dbo.OperationsWebServices.OperationWebServiceId =" + OperationWebServices;
                    break;
            }
            return operationWebServicesDecision;
        }

        private static string AssignIntegrationCategoryDecision(int IntegrationCategory, string integrationCategoryDecision)
        {
            switch (IntegrationCategory)
            {
                case 0:
                    integrationCategoryDecision = "1=1";
                    break;
                default:
                    integrationCategoryDecision = "IntegrationCategories.IntegrationCategoryId =" + IntegrationCategory;
                    break;
            }
            return integrationCategoryDecision;
        }

        private static string AssignIntegrationTypeDecision(int IntegrationType, string integrationTypeDecision)
        {
            switch (IntegrationType)
            {
                case 0:
                    integrationTypeDecision = "1=1";
                    break;
                default:
                    integrationTypeDecision = "IntegrationsType.IntegrationTypeId =" + IntegrationType;
                    break;
            }
            return integrationTypeDecision;
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