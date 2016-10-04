using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using IntegrationTool.Models;
using ClassLibrary;

namespace IntegrationTool.Controllers
{
    [Authorize]
    public class ConfigurationController : Controller
    {
        private ConfigurationModel systemConfigurationModel;
        private Encrypt encryptor;

        // ================================================================================================================
        // Retornar formularios para insertar o actualizar parametros del sistema.
        // ================================================================================================================
        [HttpGet]
        public ActionResult formActiveDirectory()
        {
            return View();
        }

        [HttpGet]
        public ActionResult formServerSMTP()
        {
            return View();
        }

        [HttpGet]
        public ActionResult formDatabase()
        {
            return View();
        }

        [HttpGet]
        public ActionResult formWebService()
        {
            return View();
        }

        [HttpGet]
        public ActionResult formFlatFile()
        {
            return View();
        }

        [HttpGet]
        public ActionResult formPathReport()
        {
            return View();
        }

        [HttpGet]
        public ActionResult formQuery()
        {
            return View();
        }

        [HttpGet]
        public ActionResult formHeaders()
        {
            return View();
        }

        // ================================================================================================================
        // Vistas para listar parametros del sistema.
        // ================================================================================================================
        [HttpGet]
        public ActionResult listActiveDirectories()
        {
            return View();
        }

        [HttpGet]
        public ActionResult listDatabases()
        {
            return View();
        }

        [HttpGet]
        public ActionResult listFlatFiles()
        {
            return View();
        }

        [HttpGet]
        public ActionResult listPathReports()
        {
            return View();
        }

        [HttpGet]
        public ActionResult listHeaders()
        {
            return View();
        }

        [HttpGet]
        public ActionResult listQueries()
        {
            return View();
        }

        [HttpGet]
        public ActionResult listServerSMTP()
        {
            return View();
        }

        [HttpGet]
        public ActionResult listWebServices()
        {
            return View();
        }

        [HttpGet]
        public ActionResult listKeys()
        {
            return View();
        }

        // ================================================================================================================
        // Almacenar en la base de datos parametros del sistema.
        // ================================================================================================================
        [HttpPut]
        public void saveActiveDirectory()
        {
            string resp = "";
            try
            {
                connectModel();

                systemConfigurationModel.saveActiveDirectory(encryptor.encryptData(Request.Form["ADDomain"]), encryptor.encryptData(Request.Form["ADPath"]));

                resp = "{\"type\":\"success\", \"message\":\"Configuration Active Directory Successful.\"}";
            }
            catch (Exception e)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Configuration Active Directory Unsuccessful. Error: " + e.Message + ". Please try again.\"}";
            }
            

            response(resp);
        }

        [HttpPut]
        public void saveServerSmtp()
        {
            string resp = "";
            try
            {
                connectModel();
                systemConfigurationModel.saveServerSMPT(encryptor.encryptData(Request.Form["NameServerSMTP"]), encryptor.encryptData(Request.Form["Port"]),
                                                        encryptor.encryptData(Request.Form["UsernameSMTP"]), encryptor.encryptData(Request.Form["PasswordSMTP"]),
                                                        encryptor.encryptData(Request.Form["EmailFrom"]), encryptor.encryptData(Request.Form["Subject"]),
                                                        encryptor.encryptData(Request.Form["Body"]));

                resp = "{\"type\":\"success\", \"message\":\"Configuration Server SMTP Successful.\"}";
            }
            catch (Exception e)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Configuration Server SMTP Unsuccessful. Error: " + e.Message + ". Please try again.\"}";
            }

            response(resp);
        }

        [HttpPut]
        public void saveDataBase()
        {
            string resp = "";
            try
            {
                connectModel();
                string port = (Request.Form["Port"] != null) ? encryptor.encryptData(Request.Form["Port"]) : null;
                string instance = (Request.Form["Instance"] != null) ? encryptor.encryptData(Request.Form["Instance"]) : null;
                systemConfigurationModel.saveDatabase(encryptor.encryptData(Request.Form["Ip"]),
                                                    encryptor.encryptData(Request.Form["Name"]), 
                                                    encryptor.encryptData(Request.Form["Username"]),
                                                    encryptor.encryptData(Request.Form["Password"]),
                                                    Request.Form["EngineId"],
                                                    port,
                                                    instance);

                resp = "{\"type\":\"success\", \"message\":\"Configuration DataBase Successful.\"}";
            }
            catch (Exception e)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Configuration DataBase Unsuccessful. Error: " + e.Message + ". Please try again.\"}";
            }

            response(resp);
        }

        [HttpPut]
        public void saveWebService()
        {
            string resp = "";
            try
            {
                connectModel();
                systemConfigurationModel.saveWebService(encryptor.encryptData(Request.Form["Endpoint"]), encryptor.encryptData(Request.Form["Username"]),
                                                        encryptor.encryptData(Request.Form["Password"]));

                resp = "{\"type\":\"success\", \"message\":\"Configuration Web Service Successful.\"}";
            }
            catch (Exception e)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Configuration Web Service Unsuccessful. Error: " + e.Message + ". Please try again.\"}";
            }

            response(resp);
        }

        [HttpPut]
        public void saveFlatFile()
        {
            string resp = "";
            try
            {
                connectModel();
                systemConfigurationModel.saveFlatFiles(encryptor.encryptData(Request.Form["Location"]));

                resp = "{\"type\":\"success\", \"message\":\"Configuration Flat File Successful.\"}";
            }
            catch (Exception e)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Configuration Flat File Unsuccessful. Error: " + e.Message + ". Please try again.\"}";
            }

            response(resp);
        }

        [HttpPut]
        public void savePathReport()
        {
            string resp = "";
            try
            {
                connectModel();
                systemConfigurationModel.savePathReports(encryptor.encryptData(Request.Form["Location"]));

                resp = "{\"type\":\"success\", \"message\":\"Configuration Path Report Successful.\"}";
            }
            catch (Exception e)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Configuration Path Report Unsuccessful. Error: " + e.Message + ". Please try again.\"}";
            }

            response(resp);
        }

        [HttpPut]
        public void saveQuery()
        {
            string resp = "";
            try
            {
                connectModel();
                systemConfigurationModel.saveQuery(encryptor.encryptData(Request.Form["Query1"]), encryptor.encryptData(Request.Form["QueryName"]), encryptor.encryptData(Request.Form["Description"]), Request.Form["IntegrationTypeId"]);

                resp = "{\"type\":\"success\", \"message\":\"Configuration Query Successful.\"}";
            }
            catch (Exception e)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Configuration Query Unsuccessful. Error: " + e.Message + ". Please try again.\"}";
            }

            response(resp);
        }

        // ================================================================================================================
        // Actualizar en base de datos parametros del sistema.
        // ================================================================================================================
        [HttpPost]
        public void updateActiveDirectory()
        {
            string resp = "";
            try
            {
                connectModel();
                systemConfigurationModel.updateActiveDirectory(Convert.ToInt32(Request.Form["ActiveDirectoryId"]),
                                                                encryptor.encryptData(Request.Form["ADDomain"]),
                                                                encryptor.encryptData(Request.Form["ADPath"]));

                resp = "{\"type\":\"success\", \"message\":\"Configuration Active Directory Successful.\"}";
            }
            catch (Exception e)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Configuration Active Directory Unsuccessful. Error: " + e.Message + ". Please try again.\"}";
            }


            response(resp);
        }

        [HttpPost]
        public void updateServerSmtp()
        {
            string resp = "";
            try
            {
                connectModel();
                systemConfigurationModel.updateServerSMPT(Convert.ToInt32(Request.Form["ServerSMTPParametersId"]),
                                                            encryptor.encryptData(Request.Form["NameServerSMTP"]),
                                                            encryptor.encryptData(Request.Form["Port"]),
                                                            encryptor.encryptData(Request.Form["UsernameSMTP"]),
                                                            encryptor.encryptData(Request.Form["PasswordSMTP"]),
                                                            encryptor.encryptData(Request.Form["EmailFrom"]), 
                                                            encryptor.encryptData(Request.Form["Subject"]),
                                                            encryptor.encryptData(Request.Form["Body"]));

                resp = "{\"type\":\"success\", \"message\":\"Configuration Server SMTP Successful.\"}";
            }
            catch (Exception e)
            {
               resp = "{\"type\":\"danger\", \"message\":\"Configuration Server SMTP Unsuccessful. Error: " + e.Message + ". Please try again.\"}";
            }

            response(resp);
        }

        [HttpPost]
        public void updateDataBase()
        {
            string resp = "";
            try
            {
                connectModel();
                systemConfigurationModel.updateDatabase(Convert.ToInt32(Request.Form["DatabaseParametersId"]),
                                                        encryptor.encryptData(Request.Form["Ip"]),
                                                        encryptor.encryptData(Request.Form["Name"]),
                                                        encryptor.encryptData(Request.Form["Username"]),
                                                        encryptor.encryptData(Request.Form["Password"]),
                                                        Convert.ToInt32(Request.Form["EngineId"]),
                                                        encryptor.encryptData(Request.Form["Port"]),
                                                        encryptor.encryptData(Request.Form["Instance"]));

                resp = "{\"type\":\"success\", \"message\":\"Configuration DataBase Successful.\"}";
            }
            catch (Exception e)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Configuration DataBase Unsuccessful. Error: " + e.Message + ". Please try again.\"}";
            }

            response(resp);
        }

        [HttpPost]
        public void updateWebService()
        {
            string resp = "";
            try
            {
                connectModel();
                systemConfigurationModel.updateWebService(Convert.ToInt32(Request.Form["WebServiceId"]),
                                                        encryptor.encryptData(Request.Form["Endpoint"]),
                                                        encryptor.encryptData(Request.Form["Username"]),
                                                        encryptor.encryptData(Request.Form["Password"]));

                resp = "{\"type\":\"success\", \"message\":\"Configuration Web Service Successful.\"}";
            }
            catch (Exception e)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Configuration Web Service Unsuccessful. Error: " + e.Message + ". Please try again.\"}";
            }

            response(resp);
        }

        [HttpPost]
        public void updateFlatFile()
        {
            string resp = "";
            try
            {
                connectModel();
                systemConfigurationModel.updateFlatFile(Convert.ToInt32(Request.Form["FlatFileParameterId"]),
                                                        encryptor.encryptData(Request.Form["Location"]));

                resp = "{\"type\":\"success\", \"message\":\"Configuration Flat File Successful.\"}";
            }
            catch (Exception e)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Configuration Flat File Unsuccessful. Error: " + e.Message + ". Please try again.\"}";
            }

            response(resp);
        }

        [HttpPost]
        public void updatePathReport()
        {
            string resp = "";
            try
            {
                connectModel();
                systemConfigurationModel.updatePathReport(Convert.ToInt32(Request.Form["PathReportId"]),
                                                        encryptor.encryptData(Request.Form["Location"]));

                resp = "{\"type\":\"success\", \"message\":\"Configuration Path Report Successful.\"}";
            }
            catch (Exception e)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Configuration Path Report Unsuccessful. Error: " + e.Message + ". Please try again.\"}";
            }

            response(resp);
        }

        [HttpPost]
        public void updateQuery()
        {
            string resp = "";
            try
            {
                connectModel();
                systemConfigurationModel.updateQuery(Convert.ToInt32(Request.Form["QueryId"]),
                                                    encryptor.encryptData(Request.Form["Query1"]),
                                                    encryptor.encryptData(Request.Form["QueryName"]),
                                                    encryptor.encryptData(Request.Form["Description"]),
                                                    Convert.ToInt32(Request.Form["IntegrationTypeId"]));

                resp = "{\"type\":\"success\", \"message\":\"Configuration Query Successful.\"}";
            }
            catch (Exception e)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Configuration Query Unsuccessful. Error: " + e.Message + ". Please try again.\"}";
            }

            response(resp);
        }

        // ================================================================================================================
        // Obtener y retornar parametros del sistema.
        // ================================================================================================================
        [HttpGet]
        public void getDataBaseEngines()
        {
            string resp = "";
            try
            {
                connectModel();
                List<Engine> engines = systemConfigurationModel.getDataBaseEngines();

                resp = serializeObject(engines);
            }
            catch (Exception e)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Can not be loaded the engines. Error: " + e.Message + ". Please try again.\"}";
            }

            response(resp);
        }

        [HttpGet]
        public void getIntegrationsType()
        {
            string resp = "";
            try
            {
                connectModel();
                List<IntegrationsType> integrationsType = systemConfigurationModel.getIntegrationsType();

                resp = serializeObject(integrationsType);
            }
            catch (Exception e)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Can not be loaded the type queries. Error: " + e.Message + ". Please try again.\"}";
            }

            response(resp);
        }

        [HttpGet]
        public void getlistQueries()
        {
            string resp = "";
            try
            {
                connectModel();
                List<Query> queries = systemConfigurationModel.getQueries();

                foreach (Query query in queries)
                {
                    query.Query1 = encryptor.decryptData(query.Query1);
                    query.QueryName = encryptor.decryptData(query.QueryName);
                    query.Description = encryptor.decryptData(query.Description);
                }

                resp = serializeObject(queries);
            }
            catch (Exception e)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Can not be loaded the queries. Error: " + e.Message + ". Please try again.\"}";
            }

            response(resp);
        }

        [HttpGet]
        public void getlistActiveDirectories()
        {
            string resp = "";
            try
            {
                connectModel();
                List<ActiveDirectoryParameter> activeDirectories = systemConfigurationModel.getActiveDirectories();

                foreach (ActiveDirectoryParameter activeDirectory in activeDirectories)
                {
                    activeDirectory.ADDomain = encryptor.decryptData(activeDirectory.ADDomain);
                    activeDirectory.ADPath = encryptor.decryptData(activeDirectory.ADPath);
                }

                resp = serializeObject(activeDirectories);
            }
            catch (Exception e)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Can not be loaded the active directories. Error: " + e.Message + ". Please try again.\"}";
            }

            response(resp);
        }

        [HttpGet]
        public void getListFlatFiles()
        {
            string resp = "";
            try
            {
                connectModel();
                List<FlatFilesParameter> flatFiles = systemConfigurationModel.getFlatFiles();

                foreach (FlatFilesParameter flatFile in flatFiles)
                {
                    flatFile.Location = encryptor.decryptData(flatFile.Location);
                }

                resp = serializeObject(flatFiles);
            }
            catch (Exception e)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Can not be loaded the flat files. Error: " + e.Message + ". Please try again.\"}";
            }

            response(resp);
        }

        [HttpGet]
        public void getListPathReports()
        {
            string resp = "";
            try
            {
                connectModel();
                List<PathReport> pathReports = systemConfigurationModel.getPathReports();

                foreach (PathReport pathReport in pathReports)
                {
                    pathReport.Location = encryptor.decryptData(pathReport.Location);
                }

                resp = serializeObject(pathReports);
            }
            catch (Exception e)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Can not be loaded the path Report. Error: " + e.Message + ". Please try again.\"}";
            }

            response(resp);
        }

        [HttpGet]
        public void getListServersSMTP()
        {
            string resp = "";
            try
            {
                connectModel();
                List<ServerSMTPParameter> serverSMTP = systemConfigurationModel.getServersSMTP();

                foreach (ServerSMTPParameter server in serverSMTP)
                {
                    server.NameServerSMTP = encryptor.decryptData(server.NameServerSMTP);
                    server.Port = encryptor.decryptData(server.Port);
                    server.UsernameSMTP = encryptor.decryptData(server.UsernameSMTP);
                    server.PasswordSMTP = encryptor.decryptData(server.PasswordSMTP);
                    server.EmailFrom = encryptor.decryptData(server.EmailFrom);
                    server.Subject = encryptor.decryptData(server.Subject);
                    server.Body = encryptor.decryptData(server.Body);
                }

                resp = serializeObject(serverSMTP);
            }
            catch (Exception e)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Can not be loaded the servers SMTP. Error: " + e.Message + ". Please try again.\"}";
            }

            response(resp);
        }

        [HttpGet]
        public void getListDatabases()
        {
            string resp = "";
            
            try
            {
                connectModel();
                List<DatabaseParameter> databases = systemConfigurationModel.getDatabases();

                foreach (DatabaseParameter database in databases)
                {
                    database.Ip = encryptor.decryptData(database.Ip);
                    database.Port = (database.Port != null) ? encryptor.decryptData(database.Port) : null;
                    database.Instance = (database.Instance != null) ? encryptor.decryptData(database.Instance) : null;
                    database.Name = encryptor.decryptData(database.Name);
                    database.Username = encryptor.decryptData(database.Username);
                    database.Password = encryptor.decryptData(database.Password);
                }

                resp = serializeObject(databases);
            }
            catch (Exception e)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Can not be loaded the databases. Error: " + e.Message + ". Please try again.\"}";
            }

            response(resp);
        }

        [HttpGet]
        public void getListWebServices()
        {
            string resp = "";
            try
            {
                connectModel();
                List<WebService> webServices = systemConfigurationModel.getWebServices();

                foreach (WebService webService in webServices)
                {
                    webService.Endpoint = encryptor.decryptData(webService.Endpoint);
                    webService.Username = encryptor.decryptData(webService.Username);
                    webService.Password = encryptor.decryptData(webService.Password);
                }

                resp = serializeObject(webServices);
            }
            catch (Exception e)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Can not be loaded the web services. Error: " + e.Message + ". Please try again.\"}";
            }

            response(resp);
        }

        [HttpGet]
        public void getListKeys()
        {
            string resp = "";
            try
            {
                connectModel();
                List<Key> keys = systemConfigurationModel.getKeys();

                resp = serializeObject(keys);
            }
            catch (Exception e)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Can not be loaded the keys. Error: " + e.Message + ". Please try again.\"}";
            }

            response(resp);
        }

        [HttpGet]
        public void getActiveDirectory(int id)
        {
            string resp = "";
            try
            {
                connectModel();
                ActiveDirectoryParameter activeDirectory = systemConfigurationModel.getActiveDirectory(id);

                activeDirectory.ADDomain = encryptor.decryptData(activeDirectory.ADDomain);
                activeDirectory.ADPath = encryptor.decryptData(activeDirectory.ADPath);

                resp = serializeObject(activeDirectory);
            }
            catch (Exception e)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Can not be loaded the active directory. Error: " + e.Message + ". Please try again.\"}";
            }

            response(resp);
        }

        [HttpGet]
        public void getDatabase(int id)
        {
            string resp = "";
            try
            {
                connectModel();
                DatabaseParameter database = systemConfigurationModel.getDatabase(id);

                database.Ip = encryptor.decryptData(database.Ip);
                database.Port = (database.Port != null) ? encryptor.decryptData(database.Port) : null;
                database.Instance = (database.Instance != null) ? encryptor.decryptData(database.Instance) : null;
                database.Name = encryptor.decryptData(database.Name);
                database.Username = encryptor.decryptData(database.Username);
                database.Password = encryptor.decryptData(database.Password);

                resp = serializeObject(database);
            }
            catch (Exception e)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Can not be loaded the database. Error: " + e.Message + ". Please try again.\"}";
            }

            response(resp);
        }

        [HttpGet]
        public void getFlatFile(int id)
        {
            string resp = "";
            try
            {
                connectModel();
                FlatFilesParameter flatFile = systemConfigurationModel.getFlatFile(id);

                flatFile.Location = encryptor.decryptData(flatFile.Location);

                resp = serializeObject(flatFile);
            }
            catch (Exception e)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Can not be loaded the flat file. Error: " + e.Message + ". Please try again.\"}";
            }

            response(resp);
        }

        [HttpGet]
        public void getPathReport(int id)
        {
            string resp = "";
            try
            {
                connectModel();
                 PathReport pathReport = systemConfigurationModel.getPathReport(id);

                 pathReport.Location = encryptor.decryptData(pathReport.Location);

                 resp = serializeObject(pathReport);
            }
            catch (Exception e)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Can not be loaded the Path Report. Error: " + e.Message + ". Please try again.\"}";
            }

            response(resp);
        }

        [HttpGet]
        public void getQuery(int id)
        {
            string resp = "";
            try
            {
                connectModel();
                Query query = systemConfigurationModel.getQuery(id);

                query.Query1 = encryptor.decryptData(query.Query1);
                query.QueryName = encryptor.decryptData(query.QueryName);
                query.Description = encryptor.decryptData(query.Description);

                resp = serializeObject(query);
            }
            catch (Exception e)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Can not be loaded the query. Error: " + e.Message + ". Please try again.\"}";
            }

            response(resp);
        }

        [HttpGet]
        public void getServerSMTP(int id)
        {
            string resp = "";
            try
            {
                connectModel();
                ServerSMTPParameter serverSMTP = systemConfigurationModel.getServerSMTP(id);

                serverSMTP.NameServerSMTP = encryptor.decryptData(serverSMTP.NameServerSMTP);
                serverSMTP.Port = encryptor.decryptData(serverSMTP.Port);
                serverSMTP.UsernameSMTP = encryptor.decryptData(serverSMTP.UsernameSMTP);
                serverSMTP.PasswordSMTP = encryptor.decryptData(serverSMTP.PasswordSMTP);
                serverSMTP.EmailFrom = encryptor.decryptData(serverSMTP.EmailFrom);
                serverSMTP.Subject = encryptor.decryptData(serverSMTP.Subject);
                serverSMTP.Body = encryptor.decryptData(serverSMTP.Body);

                resp = serializeObject(serverSMTP);
            }
            catch (Exception e)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Can not be loaded the server SMTP. Error: " + e.Message + ". Please try again.\"}";
            }

            response(resp);
        }

        [HttpGet]
        public void getWebService(int id)
        {
            string resp = "";
            try
            {
                connectModel();
                WebService webService = systemConfigurationModel.getWebService(id);

                webService.Endpoint = encryptor.decryptData(webService.Endpoint);
                webService.Username = encryptor.decryptData(webService.Username);
                webService.Password = encryptor.decryptData(webService.Password);

                resp = serializeObject(webService);
            }
            catch (Exception e)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Can not be loaded the web service. Error: " + e.Message + ". Please try again.\"}";
            }

            response(resp);
        }

        // ================================================================================================================
        // Eliminar parametros del sistema
        // ================================================================================================================
        [HttpDelete]
        public void deleteActiveDirectory(int id)
        {
            string resp = "";
            try
            {
                connectModel();
                systemConfigurationModel.deleteActiveDirectory(id);

                resp = "{\"type\":\"success\", \"message\":\"Active directory deleted Successfully.\"}";

            }
            catch (Exception e)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Can not be deleted the active directory. Error: " + e.Message + ". Please try again.\"}";
            }

            response(resp);
        }

        [HttpDelete]
        public void deleteDataBase(int id)
        {
            string resp = "";
            try
            {
                connectModel();
                systemConfigurationModel.deleteDataBase(id);

                resp = "{\"type\":\"success\", \"message\":\"Database deleted Successfully.\"}";

            }
            catch (Exception e)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Can not be deleted the database. Error: " + e.Message + ". Please try again.\"}";
            }

            response(resp);
        }

        [HttpDelete]
        public void deleteFlatFile(int id)
        {
            string resp = "";
            try
            {
                connectModel();
                systemConfigurationModel.deleteFlatFile(id);

                resp = "{\"type\":\"success\", \"message\":\"Flat file deleted Successfully.\"}";

            }
            catch (Exception e)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Can not be deleted the flat file. Error: " + e.Message + ". Please try again.\"}";
            }

            response(resp);
        }

        [HttpDelete]
        public void deletePathReport(int id)
        {
            string resp = "";
            try
            {
                connectModel();
                systemConfigurationModel.deletePathReport(id);

                resp = "{\"type\":\"success\", \"message\":\"Path Report deleted Successfully.\"}";

            }
            catch (Exception e)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Can not be deleted the Path Report. Error: " + e.Message + ". Please try again.\"}";
            }

            response(resp);
        }

        [HttpDelete]
        public void deleteQuery(int id)
        {
            string resp = "";
            try
            {
                connectModel();
                systemConfigurationModel.deleteQuery(id);

                resp = "{\"type\":\"success\", \"message\":\"Query deleted Successfully.\"}";

            }
            catch (Exception e)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Can not be deleted the query. Error: " + e.Message + ". Please try again.\"}";
            }

            response(resp);
        }

        [HttpDelete]
        public void deleteServerSMTP(int id)
        {
            string resp = "";
            try
            {
                connectModel();
                systemConfigurationModel.deleteServerSMTP(id);

                resp = "{\"type\":\"success\", \"message\":\"Server SMTP deleted Successfully.\"}";

            }
            catch (Exception e)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Can not be deleted the server SMTP. Error: " + e.Message + ". Please try again.\"}";
            }

            response(resp);
        }

        [HttpDelete]
        public void deleteWebService(int id)
        {
            string resp = "";
            try
            {
                connectModel();
                systemConfigurationModel.deleteWebService(id);

                resp = "{\"type\":\"success\", \"message\":\"Web service deleted Successfully.\"}";

            }
            catch (Exception e)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Can not be deleted the web service. Error: " + e.Message + ". Please try again.\"}";
            }

            response(resp);
        }

        // ================================================================================================================
        // Metodos privados que proveen funcionalidad a las acciones del controlador.
        // ================================================================================================================
        private void connectModel()
        {
            systemConfigurationModel = new ConfigurationModel();
            encryptor = new Encrypt();
        }

        private void response(string json)
        {
            Response.Clear();
            Response.ContentType = "application/json; charset=utf-8";
            Response.Write(json);
            Response.End();
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
