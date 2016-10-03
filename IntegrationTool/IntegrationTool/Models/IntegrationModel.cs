using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntegrationTool.Models
{
    public class IntegrationModel
    {
        private IntegrationToolEntities integrationDB;

        public IntegrationModel()
        {
            integrationDB = new IntegrationToolEntities();
        }

        // ================================================================================================================
        // Almacenar integraciones en el sistema.
        // ================================================================================================================
        public int saveIntegrationManual(int userId, string integrationName, int webServiceId, int databaseParametersId,
                                            int flatFileParameterId, int integrationTypeId, int queryId,
                                            int operationWebServiceId, List<QueryParameter> queryParameters,
                                            string curlParameters = null)
        {
            Integration integration = new Integration();

            integration.IntegrationDate = DateTime.Now;
            integration.UserId = userId;
            integration.IntegrationName = integrationName;
            integration.CurlParameters = curlParameters;
            integration.WebServiceId = webServiceId;
            integration.OperationWebServiceId = operationWebServiceId;
            integration.DatabaseParametersId = databaseParametersId;
            integration.FlatFileId = integrationDB.FlatFiles.Where(param => param.Name == "Default").ToList()[0].FlatFileId;
            integration.FlatFileParameterId = flatFileParameterId;
            integration.IntegrationTypeId = integrationTypeId;
            integration.QueryId = queryId;
            integration.IntegrationCategoryId = integrationDB.IntegrationCategories.Where(param => param.Name == "Manual").ToList()[0].IntegrationCategoryId;
            integration.StatusId = integrationDB.Status.Where(param => param.Name == "Enable").ToList()[0].StatusId;

            integration.QueryParameters = queryParameters;

            integrationDB.Integrations.Add(integration);
            integrationDB.SaveChanges();

            return integration.IntegrationId;
        }

        public void saveIntegrationSchedule(int userId, string integrationName, int webServiceId, int databaseParametersId,
                                            int flatFileParameterId, int integrationTypeId, int queryId,
                                            int operationWebServiceId, List<QueryParameter> queryParameters,
                                            DateTime executionStartDate, DateTime executionEndDate, int recurenceId, int statusId, string emails = null,
                                            string curlParameters = null)
        {
            Integration integration = new Integration();

            integration.IntegrationDate = DateTime.Now;
            integration.UserId = userId;
            integration.IntegrationName = integrationName;
            integration.CurlParameters = curlParameters;
            integration.WebServiceId = webServiceId;
            integration.OperationWebServiceId = operationWebServiceId;
            integration.DatabaseParametersId = databaseParametersId;
            integration.FlatFileId = integrationDB.FlatFiles.Where(param => param.Name == "Default").ToList()[0].FlatFileId;
            integration.FlatFileParameterId = flatFileParameterId;
            integration.IntegrationTypeId = integrationTypeId;
            integration.QueryId = queryId;
            integration.IntegrationCategoryId = integrationDB.IntegrationCategories.Where(param => param.Name != "Manual").ToList()[0].IntegrationCategoryId;
            integration.StatusId = statusId;

            integration.QueryParameters = queryParameters;

            Calendar calendar = new Calendar();
            calendar.Emails = emails;
            calendar.ExecutionEndDate = executionEndDate;
            calendar.ExecutionStartDate = executionStartDate;
            calendar.NextExecutionDate = executionStartDate;
            calendar.RecurrenceId = recurenceId;

            integration.Calendars.Add(calendar);

            integrationDB.Integrations.Add(integration);
            integrationDB.SaveChanges();
        }

        // ================================================================================================================
        // Actualizar integraciones en el sistema.
        // ================================================================================================================
        public void updateIntegrationSchedule(int integrationId, int userId, string integrationName, int webServiceId, int databaseParametersId,
                                            int flatFileParameterId, int integrationTypeId, int queryId,
                                            int operationWebServiceId, List<QueryParameter> queryParameters,
                                            DateTime executionStartDate, DateTime executionEndDate, int recurenceId, int statusId, string emails = null,
                                            string curlParameters = null)
        {
            Integration integration = integrationDB.Integrations.Where(param => param.IntegrationId == integrationId).ToList()[0];

            integration.IntegrationDate = DateTime.Now;
            integration.UserId = userId;
            integration.IntegrationName = integrationName;
            integration.CurlParameters = curlParameters;
            integration.WebServiceId = webServiceId;
            integration.OperationWebServiceId = operationWebServiceId;
            integration.DatabaseParametersId = databaseParametersId;
            integration.FlatFileId = integrationDB.FlatFiles.Where(param => param.Name == "Default").ToList()[0].FlatFileId;
            integration.FlatFileParameterId = flatFileParameterId;
            integration.IntegrationTypeId = integrationTypeId;
            integration.QueryId = queryId;
            integration.IntegrationCategoryId = integrationDB.IntegrationCategories.Where(param => param.Name != "Manual").ToList()[0].IntegrationCategoryId;
            integration.StatusId = statusId;

            integration.QueryParameters = queryParameters;

            integration.Calendars.ToList()[0].Emails = emails;
            integration.Calendars.ToList()[0].ExecutionEndDate = executionEndDate;
            integration.Calendars.ToList()[0].ExecutionStartDate = executionStartDate;
            integration.Calendars.ToList()[0].NextExecutionDate = executionStartDate;
            integration.Calendars.ToList()[0].RecurrenceId = recurenceId;

            integrationDB.SaveChanges();
        }


        // ================================================================================================================
        // Obtener información para realizar una integración
        // ================================================================================================================
        public List<IntegrationCategory> getIntegrationCategories()
        {
            List<IntegrationCategory> integrationCategories = integrationDB.IntegrationCategories.ToList();
            return integrationCategories;
        }

        public List<Query> getQueriesByIntegrationType(int id)
        {
            List<Query> queries = integrationDB.Queries.Where(param => param.IntegrationTypeId == id).ToList();
            return queries;
        }

        public List<OperationsWebService> getOperationsWebServices()
        {
            List<OperationsWebService> operations = integrationDB.OperationsWebServices.ToList();
            return operations;
        }

        public List<Recurrence> getRecurrences()
        {
            List<Recurrence> recurrences = integrationDB.Recurrences.ToList();
            return recurrences;
        }

        public List<Integration> getScheduleIntegrations()
        {
            List<Integration> integrations = integrationDB.Integrations.Where(param => param.IntegrationCategoryId == 2).ToList();
            return integrations;
        }

        public List<Integration> getManualIntegrations()
        {
            List<Integration> integrations = integrationDB.Integrations.Where(param => param.IntegrationCategoryId == 1).ToList();
            return integrations;
        }

        public Integration getIntegration(int id)
        {
            Integration integration = integrationDB.Integrations.Where(param => param.IntegrationId == id).ToList()[0];
            return integration;
        }

        public List<Status> getStatus()
        {
            List<Status> status = integrationDB.Status.ToList();
            return status;
        }
    }
}
