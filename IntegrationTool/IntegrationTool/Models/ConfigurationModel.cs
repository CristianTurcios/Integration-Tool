using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntegrationTool.Models
{
    public class ConfigurationModel
    {
        private IntegrationToolEntities ConfigurationDB;

        public ConfigurationModel()
        {
            ConfigurationDB = new IntegrationToolEntities();
        }

        // ================================================================================================================
        // Obtener parametros del sistema.
        // ================================================================================================================
        public List<ActiveDirectoryParameter> getActiveDirectories()
        {
            List<ActiveDirectoryParameter> activeDirectories = (from ad in ConfigurationDB.ActiveDirectoryParameters
                                    select ad).ToList();
            return activeDirectories;
        }

        public List<ServerSMTPParameter> getServersSMTP()
        {
            List<ServerSMTPParameter> serverSmtp = (from server in ConfigurationDB.ServerSMTPParameters
                            select server).ToList();
            return serverSmtp;
        }

        public List<Query> getQueries()
        {
            List<Query> queries = (from query in ConfigurationDB.Queries
                        select query).ToList();
            return queries;
        }

        public List<FlatFilesParameter> getFlatFiles()
        {
            List<FlatFilesParameter> flatFiles = (from ff in ConfigurationDB.FlatFilesParameters
                        select ff).ToList();
            return flatFiles;
        }

        public List<PathReport> getPathReports()
        {
            List<PathReport> pathReports = (from pr in ConfigurationDB.PathReports
                                                  select pr).ToList();
            return pathReports;
        }

        public List<DatabaseParameter> getDatabases()
        {
            List<DatabaseParameter> databases = (from db in ConfigurationDB.DatabaseParameters
                            select db).ToList();
            return databases;
        }

        public List<WebService> getWebServices()
        {
            List<WebService> webServices = (from ws in ConfigurationDB.WebServices
                                            select ws).ToList();
            return webServices;
        }

        public List<Engine> getDataBaseEngines()
        {
            List<Engine> engines = (from dbEngine in ConfigurationDB.Engines
                        select dbEngine).ToList();
            return engines;
        }

        public List<IntegrationsType> getIntegrationsType()
        {
            List<IntegrationsType> integrationsType = (from it in ConfigurationDB.IntegrationsTypes
                                    select it).ToList();
            return integrationsType;
        }

        public List<Key> getKeys()
        {
            List<Key> keys = (from k in ConfigurationDB.Keys
                                select k).ToList();
            return keys;
        }

        public ActiveDirectoryParameter getActiveDirectory(int id)
        {
            ActiveDirectoryParameter activeDirectory = ConfigurationDB.ActiveDirectoryParameters.Where(param => param.ActiveDirectoryId == id).ToList()[0];
            return activeDirectory;
        }

        public DatabaseParameter getDatabase(int id)
        {
            DatabaseParameter dataBase = ConfigurationDB.DatabaseParameters.Where(param => param.DatabaseParametersId == id).ToList()[0];
            return dataBase;
        }

        public FlatFilesParameter getFlatFile(int id)
        {
            FlatFilesParameter flatFile = ConfigurationDB.FlatFilesParameters.Where(param => param.FlatFileParameterId == id).ToList()[0];
            return flatFile;
        }

        public PathReport getPathReport(int id)
        {
            PathReport pathReport = ConfigurationDB.PathReports.Where(param => param.PathReportId == id).ToList()[0];
            return pathReport;
        }

        public Query getQuery(int id)
        {
            Query query = ConfigurationDB.Queries.Where(param => param.QueryId == id).ToList()[0];
            return query;
        }

        public ServerSMTPParameter getServerSMTP(int id)
        {
            ServerSMTPParameter serverSMTP = ConfigurationDB.ServerSMTPParameters.Where(param => param.ServerSMTPParametersId == id).ToList()[0];
            return serverSMTP;
        }

        public WebService getWebService(int id)
        {
            WebService webService = ConfigurationDB.WebServices.Where(param => param.WebServiceId == id).ToList()[0];
            return webService;
        }

        // ================================================================================================================
        // Almacenar parametros del sistema.
        // ================================================================================================================
        public void saveActiveDirectory(string domain, string path)
        {
            ActiveDirectoryParameter ad = new ActiveDirectoryParameter();
            ad.ADPath = domain;
            ad.ADDomain = path;

            ConfigurationDB.ActiveDirectoryParameters.Add(ad);
            ConfigurationDB.SaveChanges();
        }

        public void saveServerSMPT(string nameServerSMTP, string port, string usernameSMTP, string passwordSMTP, string from, string subject, string body)
        {
            ServerSMTPParameter serverSmtp = new ServerSMTPParameter();
            serverSmtp.NameServerSMTP = nameServerSMTP;
            serverSmtp.Port = port;
            serverSmtp.UsernameSMTP = usernameSMTP;
            serverSmtp.PasswordSMTP = passwordSMTP;
            serverSmtp.EmailFrom = from;
            serverSmtp.Subject = subject;
            serverSmtp.Body = body;

            ConfigurationDB.ServerSMTPParameters.Add(serverSmtp);
            ConfigurationDB.SaveChanges();
        }

        public void saveQuery(string queryString, string queryName, string description, string integrationType)
        {
            Query query = new Query();
            query.Query1 = queryString;
            query.QueryName = queryName;
            query.Description = description;
            query.IntegrationTypeId = Convert.ToInt32(integrationType);

            ConfigurationDB.Queries.Add(query);
            ConfigurationDB.SaveChanges();
        }

        public void saveFlatFiles(string location)
        {
            FlatFilesParameter flatFile = new FlatFilesParameter();

            flatFile.Location = location;

            ConfigurationDB.FlatFilesParameters.Add(flatFile);
            ConfigurationDB.SaveChanges();
        }

        public void savePathReports(string location)
        {
            PathReport pathReport = new PathReport();

            pathReport.Location = location;

            ConfigurationDB.PathReports.Add(pathReport);
            ConfigurationDB.SaveChanges();
        }

        public void saveDatabase(string ip, string name, string username, string password, string engineId, string port = null, string instance = null)
        {
            DatabaseParameter db = new DatabaseParameter();
            db.Ip = ip;
            db.Instance = instance;
            db.Name = name;
            db.Username = username;
            db.Password = password;
            db.Port = port;

            int intEngineId = Convert.ToInt32(engineId);

            // Engine engine = SystemConfigurationDB.Engines.Where(param => param.EngineId == intEngineId).ToList()[0];

            db.EngineId = intEngineId;

            ConfigurationDB.DatabaseParameters.Add(db);
            ConfigurationDB.SaveChanges();
        }

        public void saveWebService(string endpoint, string username, string password)
        {
            WebService webService = new WebService();
            webService.Endpoint = endpoint;
            webService.Username = username;
            webService.Password = password;

            ConfigurationDB.WebServices.Add(webService);
            ConfigurationDB.SaveChanges();
        }

        // ================================================================================================================
        // Actualizar parametros del sistema.
        // ================================================================================================================
        public void updateActiveDirectory(int id, string domain, string path)
        {
            ActiveDirectoryParameter ad = ConfigurationDB.ActiveDirectoryParameters.Where(param => param.ActiveDirectoryId == id).ToList()[0];
            ad.ADPath = domain;
            ad.ADDomain = path;

            ConfigurationDB.SaveChanges();
        }

        public void updateDatabase(int id, string ip, string name, string username, string password, int engineId, string port = null, string instance = null)
        {
            DatabaseParameter db = ConfigurationDB.DatabaseParameters.Where(param => param.DatabaseParametersId == id).ToList()[0];
            db.Ip = ip;
            db.Instance = instance;
            db.Name = name;
            db.Username = username;
            db.Password = password;
            db.Port = port;
            db.EngineId = engineId;

            ConfigurationDB.SaveChanges();
        }

        public void updateFlatFile(int id, string location)
        {
            FlatFilesParameter flatFile = ConfigurationDB.FlatFilesParameters.Where(param => param.FlatFileParameterId == id).ToList()[0];
            flatFile.Location = location;
            ConfigurationDB.SaveChanges();
        }

        public void updatePathReport(int id, string location)
        {
            PathReport pathReport = ConfigurationDB.PathReports.Where(param => param.PathReportId == id).ToList()[0];
            pathReport.Location = location;
            ConfigurationDB.SaveChanges();
        }

        public void updateQuery(int id, string queryString, string queryName, string description, int integrationTypeId)
        {
            Query query = ConfigurationDB.Queries.Where(param => param.QueryId == id).ToList()[0];
            query.Query1 = queryString;
            query.QueryName = queryName;
            query.Description = description;
            query.IntegrationTypeId = integrationTypeId;

            ConfigurationDB.SaveChanges();
        }

        public void updateServerSMPT(int id, string nameServerSMTP, string port, string usernameSMTP, string passwordSMTP, string from, string subject, string body)
        {
            ServerSMTPParameter serverSmtp = ConfigurationDB.ServerSMTPParameters.Where(param => param.ServerSMTPParametersId == id).ToList()[0];
            serverSmtp.NameServerSMTP = nameServerSMTP;
            serverSmtp.Port = port;
            serverSmtp.UsernameSMTP = usernameSMTP;
            serverSmtp.PasswordSMTP = passwordSMTP;
            serverSmtp.EmailFrom = from;
            serverSmtp.Subject = subject;
            serverSmtp.Body = body;

            ConfigurationDB.SaveChanges();
        }

        public void updateWebService(int id, string endpoint, string username, string password)
        {
            WebService webService = ConfigurationDB.WebServices.Where(param => param.WebServiceId == id).ToList()[0];
            webService.Endpoint = endpoint;
            webService.Username = username;
            webService.Password = password;

            ConfigurationDB.SaveChanges();
        }

        // ================================================================================================================
        // Eliminar parametros del sistema.
        // ================================================================================================================
        public void deleteActiveDirectory(int activeDirectoryId)
        {
            ActiveDirectoryParameter activeDirectory = ConfigurationDB.ActiveDirectoryParameters.Where(param => param.ActiveDirectoryId == activeDirectoryId).ToList()[0];

            ConfigurationDB.ActiveDirectoryParameters.Remove(activeDirectory);
            ConfigurationDB.SaveChanges();
        }

        public void deleteDataBase(int dataBaseId)
        {
            DatabaseParameter database = ConfigurationDB.DatabaseParameters.Where(param => param.DatabaseParametersId == dataBaseId).ToList()[0];

            ConfigurationDB.DatabaseParameters.Remove(database);
            ConfigurationDB.SaveChanges();
        }

        public void deleteFlatFile(int flatFileIdId)
        {
            FlatFilesParameter flatFile = ConfigurationDB.FlatFilesParameters.Where(param => param.FlatFileParameterId == flatFileIdId).ToList()[0];

            ConfigurationDB.FlatFilesParameters.Remove(flatFile);
            ConfigurationDB.SaveChanges();
        }

        public void deletePathReport(int pathReportId)
        {
            PathReport pathReport = ConfigurationDB.PathReports.Where(param => param.PathReportId == pathReportId).ToList()[0];

            ConfigurationDB.PathReports.Remove(pathReport);
            ConfigurationDB.SaveChanges();
        }

        public void deleteQuery(int queryId)
        {
            Query query = ConfigurationDB.Queries.Where(param => param.QueryId == queryId).ToList()[0];

            ConfigurationDB.Queries.Remove(query);
            ConfigurationDB.SaveChanges();
        }

        public void deleteServerSMTP(int serverSMTPId)
        {
            ServerSMTPParameter serverSMTP = ConfigurationDB.ServerSMTPParameters.Where(param => param.ServerSMTPParametersId == serverSMTPId).ToList()[0];

            ConfigurationDB.ServerSMTPParameters.Remove(serverSMTP);
            ConfigurationDB.SaveChanges();
        }

        public void deleteWebService(int webServiceId)
        {
            WebService webService = ConfigurationDB.WebServices.Where(param => param.WebServiceId == webServiceId).ToList()[0];

            ConfigurationDB.WebServices.Remove(webService);
            ConfigurationDB.SaveChanges();
        }
    }
}
