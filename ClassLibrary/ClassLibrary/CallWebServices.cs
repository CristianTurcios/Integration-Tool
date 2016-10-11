using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    class CallWebServices
    {
        public string webServicesCall(string url, string usernameAndPassword,string fullPath,Integration integration)
        {
            string status = "";
            string query = "";


            if (Integration.flag)
            {
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
                myReq.Method = "POST";
                myReq.ContentType = "text/plain; charset=UTF-8";

                UTF8Encoding enc = new UTF8Encoding();
                myReq.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(enc.GetBytes(usernameAndPassword)));


                StreamReader sourceStream = new StreamReader(fullPath);
                byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                sourceStream.Close();
                myReq.ContentLength = fileContents.Length;

                Stream requestStream = myReq.GetRequestStream();
                requestStream.Write(fileContents, 0, fileContents.Length);
                requestStream.Close();

                try
                {
                    HttpWebResponse wr = (HttpWebResponse)myReq.GetResponse();
                    Stream receiveStream = wr.GetResponseStream();
                    StreamReader reader = new StreamReader(receiveStream, Encoding.UTF8);
                    string content = reader.ReadToEnd();
                    status = Convert.ToString(wr.StatusCode);

                    query = ReturnSuccessStatus(content, "200", integration.integrationId);

                }
                catch (WebException e)
                {
                    status = Convert.ToString(e.Status);
                    query = ReturnErrorStatus(status, "403", integration.integrationId);

                    throw e;
                }

            }

            else
            {
                Console.WriteLine(Integration.flag);
                query = ReturnErrorInExecutionIntegration("Review System Logs", "000", integration.integrationId);
            }
                        
            return query;
        }

        private static string ReturnSuccessStatus(string content,string status ,int integrationId)
        {
            string[] Content2 = content.Split(' ');
            string query = "insert into IntegrationLogs (ReferenceCode,Date,IntegrationId,status) values('" + Content2[9] + "','" + DateTime.Now + "'," + integrationId + "," + status + ")";            
            return query;
        }

        private static string ReturnErrorStatus(string content, string status, int integrationId)
        {          
            string query = "insert into IntegrationLogs (ReferenceCode,Date,IntegrationId,status) values('" + content + "','" + DateTime.Now + "'," + integrationId + "," + status + ")";           
            return query;
        }

        private static string ReturnErrorInExecutionIntegration(string content, string status, int integrationId)
        {
            string query = "insert into IntegrationLogs (ReferenceCode,Date,IntegrationId,status) values('" + content + "','" + DateTime.Now + "'," + integrationId + "," + status + ")";
            return query;
        }
    }
}
