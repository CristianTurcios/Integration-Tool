using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntegrationTool.Models
{
    public class ReportsConfiguration
    {
        private IntegrationToolEntities ReportsConfigurationDB;

        public ReportsConfiguration()
        {
            ReportsConfigurationDB = new IntegrationToolEntities();
        }

        // ================================================================================================================
        // Obtener parametros del sistema.
        // ================================================================================================================      
        public List<PathReport> getPathReport()
        {
            List<PathReport> pathReport = (from pr in ReportsConfigurationDB.PathReports
                                                             select pr).ToList();
            return pathReport;
        }

        public List<IntegrationCategory> getCategoryIntegration()
        {           
            List<IntegrationCategory> categoryIntegration = (from ci in ReportsConfigurationDB.IntegrationCategories
                        select ci).ToList();
            return categoryIntegration;
        }

        public List<IntegrationsType> getIntegrationType()
        {
            List<IntegrationsType> IntegrationType = (from it in ReportsConfigurationDB.IntegrationsTypes
                                          select it).ToList();
            return IntegrationType;
        }

        public List<OperationsWebService> getOperationWebServices()
        {
            List<OperationsWebService> operationWebServices = (from ows in ReportsConfigurationDB.OperationsWebServices
                                                      select ows).ToList();
            return operationWebServices;
        }

        public List<DatabaseParameter> getDatabaseParameter()
        {
            List<DatabaseParameter> databaseParameter = (from dp in ReportsConfigurationDB.DatabaseParameters
                                                               select dp).ToList();
            return databaseParameter;
        }

        public List<IntegrationLog> getIntegrationLogForReport()
        {
            List<IntegrationLog> integrationLog = (from il in ReportsConfigurationDB.IntegrationLogs
                                                         select il).ToList();
            return integrationLog;
        }
    }
}