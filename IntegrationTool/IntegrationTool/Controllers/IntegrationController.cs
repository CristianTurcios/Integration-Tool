using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using Newtonsoft.Json;
using IntegrationTool.Models;
using ClassLibrary;
using System.Text.RegularExpressions;

namespace IntegrationTool.Controllers
{
    [Authorize]
    public class IntegrationController : Controller
    {
        private IntegrationModel integrationConfigurationModel;
        private Encrypt encryptor;

        // ================================================================================================================
        // Retornar las vitas del sistema.
        // ================================================================================================================
        [HttpGet]
        public ActionResult configurationScheduled()
        {
            return View();
        }

        [HttpGet]
        public ActionResult configurationManual()
        {
            return View();
        }

        [HttpGet]
        public ActionResult manual()
        {
            return View();
        }

        [HttpGet]
        public ActionResult calendar()
        {
            return View();
        }

        // ================================================================================================================
        // Almacenar en la base de datos integraciones automaticas y manuales.
        // ================================================================================================================
        [HttpPut]
        public void saveIntegrationManual()
        {
            string resp = "";
            try
            {
                List<QueryParameter> queryParameters = getQueryParameters(Request);

                connectModel();

                int integrationId = integrationConfigurationModel.saveIntegrationManual(Convert.ToInt32(Request.Form["UserId"]),
                                                            Request.Form["IntegrationName"],
                                                            Convert.ToInt32(Request.Form["WebServiceId"]),
                                                            Convert.ToInt32(Request.Form["DatabaseParametersId"]),
                                                            Convert.ToInt32(Request.Form["FlatFileParameterId"]),
                                                            Convert.ToInt32(Request.Form["IntegrationTypeId"]),
                                                            Convert.ToInt32(Request.Form["QueryId"]),
                                                            Convert.ToInt32(Request.Form["OperationWebServiceId"]),
                                                            queryParameters,
                                                            Request.Form["CurlParameters"]);

                executeIntegrationManual(integrationId);

                resp = "{\"type\":\"success\", \"message\":\"Integration successful stored.\"}";
            }
            catch (Exception)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Integration unsuccessful stored. Please try again.\"}";
            }


            response(resp);
        }

        [HttpPut]
        public void saveIntegrationScheduled()
        {
            string resp = "";
            try
            {
                List<QueryParameter> queryParameters = getQueryParameters(Request);

                connectModel();

                string format = "ddd MMM dd yyyy HH:mm:ss 'GMT'K '(Central America Standard Time)'";

                DateTime executionStartDate = DateTime.ParseExact(Request.Form["ExecutionStartDate"], format, CultureInfo.CurrentCulture);
                DateTime executionEndDate = DateTime.ParseExact(Request.Form["ExecutionEndDate"], format, CultureInfo.CurrentCulture);

                integrationConfigurationModel.saveIntegrationSchedule(Convert.ToInt32(Request.Form["UserId"]),
                                                            Request.Form["IntegrationName"],
                                                            Convert.ToInt32(Request.Form["WebServiceId"]),
                                                            Convert.ToInt32(Request.Form["DatabaseParametersId"]),
                                                            Convert.ToInt32(Request.Form["FlatFileParameterId"]),
                                                            Convert.ToInt32(Request.Form["IntegrationTypeId"]),
                                                            Convert.ToInt32(Request.Form["QueryId"]),
                                                            Convert.ToInt32(Request.Form["OperationWebServiceId"]),
                                                            queryParameters,
                                                            executionStartDate,
                                                            executionEndDate,
                                                            Convert.ToInt32(Request.Form["RecurrenceId"]),
                                                            Convert.ToInt32(Request.Form["StatusId"]),
                                                            Request.Form["Emails"],
                                                            Request.Form["CurlParameters"]);

                resp = "{\"type\":\"success\", \"message\":\"Integration successful stored.\"}";
            }
            catch (Exception)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Integration unsuccessful stored. Please try again.\"}";
            }


            response(resp);
        }

        // ================================================================================================================
        // Actualizar en la base de datos integraciones automaticas.
        // ================================================================================================================
        [HttpPost]
        public void updateIntegrationScheduled()
        {
            string resp = "";
            try
            {
                List<QueryParameter> queryParameters = getQueryParameters(Request);

                connectModel();

                string format = "ddd MMM dd yyyy HH:mm:ss 'GMT'K '(Central America Standard Time)'";

                DateTime executionStartDate = DateTime.ParseExact(Request.Form["ExecutionStartDate"], format, CultureInfo.CurrentCulture);
                DateTime executionEndDate = DateTime.ParseExact(Request.Form["ExecutionEndDate"], format, CultureInfo.CurrentCulture);

                integrationConfigurationModel.updateIntegrationSchedule(Convert.ToInt32(Request.Form["IntegrationId"]),
                                                            Convert.ToInt32(Request.Form["UserId"]),
                                                            Request.Form["IntegrationName"],
                                                            Convert.ToInt32(Request.Form["WebServiceId"]),
                                                            Convert.ToInt32(Request.Form["DatabaseParametersId"]),
                                                            Convert.ToInt32(Request.Form["FlatFileParameterId"]),
                                                            Convert.ToInt32(Request.Form["IntegrationTypeId"]),
                                                            Convert.ToInt32(Request.Form["QueryId"]),
                                                            Convert.ToInt32(Request.Form["OperationWebServiceId"]),
                                                            queryParameters,
                                                            executionStartDate,
                                                            executionEndDate,
                                                            Convert.ToInt32(Request.Form["RecurrenceId"]),
                                                            Convert.ToInt32(Request.Form["StatusId"]),
                                                            Request.Form["Emails"],
                                                            Request.Form["CurlParameters"]);

                resp = "{\"type\":\"success\", \"message\":\"Integration successful updated.\"}";
            }
            catch (Exception)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Integration unsuccessful updated. Please try again.\"}";
            }


            response(resp);
        }

        // ================================================================================================================
        // Obtiene y retorna datos necesarios para realizar una integración.
        // ================================================================================================================
        [HttpGet]
        public void getQueriesByIntegrationType(int id)
        {
            string resp = "";
            try
            {
                connectModel();
                List<Query> queries = integrationConfigurationModel.getQueriesByIntegrationType(id);

                foreach (Query query in queries)
                {
                    query.Query1 = encryptor.decryptData(query.Query1);
                    query.QueryName = encryptor.decryptData(query.QueryName);
                    query.Description = encryptor.decryptData(query.Description);
                }

                resp = serializeObject(queries);
            }
            catch (Exception)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Can not be loaded the queries. Please try again.\"}";
            }

            response(resp);
        }

        [HttpGet]
        public void getOperationsWebServices()
        {
            string resp = "";
            try
            {
                connectModel();
                List<OperationsWebService> operations = integrationConfigurationModel.getOperationsWebServices();

                resp = serializeObject(operations);
            }
            catch (Exception)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Can not be loaded the operations web services. Please try again.\"}";
            }

            response(resp);
        }

        [HttpGet]
        public void getIntegrationCategories()
        {
            string resp = "";
            try
            {
                connectModel();
                List<IntegrationCategory> integrationCategories = integrationConfigurationModel.getIntegrationCategories();

                resp = serializeObject(integrationCategories);
            }
            catch (Exception)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Can not be loaded the integration categories. Please try again.\"}";
            }

            response(resp);
        }

        [HttpGet]
        public void getRecurrences()
        {
            string resp = "";
            try
            {
                connectModel();
                List<Recurrence> recurrences = integrationConfigurationModel.getRecurrences();

                resp = serializeObject(recurrences);
            }
            catch (Exception)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Can not be loaded the recurrences. Please try again.\"}";
            }

            response(resp);
        }

        [HttpGet]
        public void getSheduleIntegrations()
        {
            string resp = "";
            try
            {
                connectModel();
                List<IntegrationTool.Models.Integration> integrations = integrationConfigurationModel.getScheduleIntegrations();

                resp = serializeObject(integrations);
            }
            catch (Exception)
            {              
               resp = "{\"type\":\"danger\", \"message\":\"Can not be loaded the schedule integration. Please try again.\"}";
            }

            response(resp);
        }


        [HttpGet]
        public void getManualIntegrations()
        {
            string resp = "";
            
            try
            {
                connectModel();
                List<IntegrationTool.Models.Integration> integrations = integrationConfigurationModel.getManualIntegrations();

                
                for (int index = 0; index < integrations.Count; index++ )
                {
                    try
                    {
                        integrations[index].WebService.Endpoint = encryptor.decryptData(integrations[index].WebService.Endpoint);
                    }
                    catch (Exception)
                    {
                        
                    }

                    try
                    {
                        integrations[index].DatabaseParameter.Name = encryptor.decryptData(integrations[index].DatabaseParameter.Name);
                    }
                    catch (Exception)
                    {

                    }
            
                    try
                    {
                        integrations[index].DatabaseParameter.Port = (integrations[index].DatabaseParameter.Port != null) ? encryptor.decryptData(integrations[index].DatabaseParameter.Port) : null;
                    }
                    catch (Exception)
                    {

                    }

                    try
                    {
                        integrations[index].DatabaseParameter.Instance = (integrations[index].DatabaseParameter.Instance != null) ? encryptor.decryptData(integrations[index].DatabaseParameter.Instance) : null;
                    }
                    catch (Exception)
                    {

                    }

                    try
                    {
                        integrations[index].DatabaseParameter.Instance = (integrations[index].DatabaseParameter.Instance != null) ? encryptor.decryptData(integrations[index].DatabaseParameter.Instance) : null;
                    }
                    catch (Exception)
                    {
                        
                    }

                    try
                    {
                        integrations[index].DatabaseParameter.Ip = encryptor.decryptData(integrations[index].DatabaseParameter.Ip);
                    }
                    catch (Exception)
                    {

                    }

                    try
                    {
                        integrations[index].FlatFilesParameter.Location = encryptor.decryptData(integrations[index].FlatFilesParameter.Location);
                    }
                    catch (Exception)
                    {

                    }

                    try
                    {
                        integrations[index].Query.Query1 = encryptor.decryptData(integrations[index].Query.Query1);
                    }
                    catch (Exception)
                    {

                    }
                }

                resp = serializeObject(integrations);
            }
            catch (Exception)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Can not be loaded the manual integration. Please try again.\"}";
            }

            response(resp);
        }

        [HttpGet]
        public void getIntegration(int id)
        {
            string resp = "";
            try
            {
                connectModel();
                IntegrationTool.Models.Integration integration = integrationConfigurationModel.getIntegration(id);

                resp = serializeObject(integration);
            }
            catch (Exception)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Can not be loaded the integration selected. Please try again.\"}";
            }

            response(resp);
        }

        [HttpGet]
        public void getStatus()
        {
            string resp = "";
            try
            {
                connectModel();
                IList<Status> status = integrationConfigurationModel.getStatus();

                resp = serializeObject(status);
            }
            catch (Exception)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Can not be loaded the integration selected. Please try again.\"}";
            }

            response(resp);
        }

        [HttpGet]
        public void executeIntegration(int id)
        {
            string resp = "";
            try
            {
                executeIntegrationManual(id);
                resp = "{\"type\":\"success\", \"message\":\"Integration successful executed.\"}";
            }
            catch (Exception)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Can not be loaded the integration selected. Please try again.\"}";
            }

            response(resp);
        }

        // ================================================================================================================
        // Metodos privados que proveen funcionalidad a las acciones del controlador.
        // ================================================================================================================
        private void connectModel()
        {
            integrationConfigurationModel = new IntegrationModel();
            encryptor = new Encrypt();
        }

        private void response(string json)
        {
            Response.Clear();
            Response.ContentType = "application/json; charset=utf-8";
            Response.Write(json);
            Response.End();
        }

        private void executeIntegrationManual(int integrationId)
        {
            ClassLibrary.Integration integration = new ClassLibrary.Integration();
            integration.executeIntegration(integrationId);
        }

        private List<QueryParameter> getQueryParameters(HttpRequestBase req)
        {
            List<QueryParameter> queryParameters = new List<QueryParameter>();
            Regex regex = new Regex(@"\[\[.*\]\]");

            for (int index = 0; index < req.Form.Count; index++)
            {
                Match match = regex.Match(Request.Form.AllKeys[index]);
                if (match.Success)
                {
                    QueryParameter qp = new QueryParameter();
                    qp.Name = Request.Form.AllKeys[index];
                    string[] values = Request.Form[qp.Name].Split('|');

                    string value = "";
                    foreach(string v in values)
                    {
                        value += "'" +  v + "',";
                    }
                    qp.Value = value.TrimEnd(',');

                    queryParameters.Add(qp);
                }
            }

            return queryParameters;
        }

        private string serializeObject(Object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
        }
    }
}
