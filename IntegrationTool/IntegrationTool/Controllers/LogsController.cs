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
                resp = "{\"type\":\"danger\", \"message\":\"Can not be loaded the Integration Logs"+e.Message+". Please try again.\"}";
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
            catch (Exception)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Can not be loaded the System Logs. Please try again.\"}";
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

                //curl = "curl -w '%{http_code}' -u " + user + ":" + password + " --url " + endPoint + "dataSetStatus/" + referenceCode;
                 curl = "curl -w '%{http_code}' -u e2ba3a85-24f2-4b30-aab3-a90b978090ca:eUc*13P --url https://eud-eval.blackboard.com/webapps/bb-data-integration-flatfile-BBLEARN/endpoint/dataSetStatus/eebda8337f0c49f3a81b2e0bc6892adb";

                System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/c " + curl);
                procStartInfo.CreateNoWindow = true;
                procStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                procStartInfo.UseShellExecute = false;
                procStartInfo.RedirectStandardOutput = true;
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();

                proc.WaitForExit();
                
                string respProc = proc.StandardOutput.ReadToEnd();

                Regex regex = new Regex(@"'200'");
                Match match = regex.Match(respProc);

                if (match.Success)
                {
                    respProc = respProc.Remove(respProc.IndexOf("'200'"));

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(respProc);

                    resp = JsonConvert.SerializeXmlNode(doc);
                }
                else
                {
                    resp = "{\"type\":\"danger\", \"message\":\"Error connecting to web service. Please try again.\"}";
                }
            }
            catch(Exception e)
            {
                resp = "{\"type\":\"danger\", \"message\":\"" + e.Message + "\"}";
                // resp = "{\"type\":\"danger\", \"message\":\"Can not be loaded the details. Please try again.\"}";
            }

            response(resp);
        }    
    }
}
