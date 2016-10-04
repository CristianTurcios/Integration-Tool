using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using IntegrationTool.Models;
using ClassLibrary;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using System.Net;
using System.Text;

namespace IntegrationTool.Controllers
{
    [Authorize]
    public class LogsController : Controller
    {
        private LogsConfiguration logsConfigurationModel;
        private Encrypt encrypt;
       
        private void connectModel()
        {
            logsConfigurationModel = new LogsConfiguration();
            encrypt = new Encrypt();
        }

        private void response(string json)
        {
            Response.Clear();
            Response.ContentType = "application/json; charset=utf-8";
            Response.Write(json);
            Response.End();
        }

        [HttpGet]
        public ActionResult listIntegrationLogs()
        {
            return View();
        }

        [HttpGet]
        public void getListIntegrationLogs(int id)
        {
            string resp = "";
            try
            {
                connectModel();

                List<IntegrationLog> integrationLogs = logsConfigurationModel.getIntegrationLogs(id);

                resp = Newtonsoft.Json.JsonConvert.SerializeObject(integrationLogs,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
            }
            catch (Exception e)
            {
                resp = "{\"type\":\"danger\", \"message\":\"" + e.Message + ".\"}";
            }

            response(resp);
        }

        [HttpGet]
        public ActionResult listSystemLogs()
        {
            return View();
        }


        [HttpGet]
        public void getListSystemLogs(int id)
        {
            string resp = "";
            try
            {
                connectModel();
                List<SystemLog> systemLogs = logsConfigurationModel.getSystemLogs(id);

                resp = Newtonsoft.Json.JsonConvert.SerializeObject(systemLogs,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
            }
            catch (Exception e)
            {
                resp = "{\"type\":\"danger\", \"message\":\"" + e.Message + ".\"}";
            }

            response(resp);
        }

        [HttpGet]
        public ActionResult viewDetails()
        {
            return View();
        }


        [HttpGet]
        public void getDetails(int integrationId, string referenceCode)
        {
            string user = "";
            string password = "";
            string endPoint = "";
            string path = "";
            string curl = "";
            string resp = "";

            try
            {
                connectModel();
                user = encrypt.decryptData(logsConfigurationModel.getWebServiceUser(integrationId));
                password = encrypt.decryptData(logsConfigurationModel.getWebServicePassword(integrationId));
                endPoint = encrypt.decryptData(logsConfigurationModel.getWebServiceEndPoint(integrationId));
                path = encrypt.decryptData(logsConfigurationModel.getPath(integrationId)) + "/details.xml";
            }
            catch(Exception e)
            {
                resp = "{\"type\":\"danger\", \"message\":\"" + e.Message + ".\"}";
                response(resp);
                return;
            }

            /*
            string url = "https://eud-eval.blackboard.com/webapps/bb-data-integration-flatfile-BBLEARN/endpoint/dataSetStatus/eebda8337f0c49f3a81b2e0bc6892adb";
            string userAndPassword = "e2ba3a85-24f2-4b30-aab3-a90b978090ca:eUc*13P";
            */

            string url = endPoint + "dataSetStatus/" + referenceCode;
            string userAndPassword = user + ":" + password;

            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
            myReq.Method = "POST";
            myReq.ContentType = "text/plain; charset=UTF-8";

            UTF8Encoding enc = new UTF8Encoding();
            myReq.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(enc.GetBytes(userAndPassword)));

            try
            {
                HttpWebResponse wr = (HttpWebResponse)myReq.GetResponse();
                Stream receiveStream = wr.GetResponseStream();
                StreamReader reader = new StreamReader(receiveStream, Encoding.UTF8);
                string responseWebService = reader.ReadToEnd();

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(responseWebService);

                resp = JsonConvert.SerializeXmlNode(doc);
            }
            catch (WebException e)
            {
                resp = "{\"type\":\"danger\", \"message\":\"" + e.Message + ".\"}";
            }

            response(resp);
        }    
    }
}
