using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace ClassLibrary
{
    class ReadFile
    {
        public string Read(string pathLog,int integrationId,Integration integration)
        {
            try
            {
               StreamReader file = new StreamReader(pathLog);

               string line = "";
               string line2 = "";

               // Read the file and display it line by line.

               while ((line = file.ReadLine()) != null)
               {
                    line2 += line + "%";
               }

               line2 = line2.Replace("'", "");
               string[] ContentFile = line2.Split('%');

               if (ContentFile[1].Equals("200"))
                   return ReturnSuccessStatus(integrationId, file, ContentFile);

               else
                   return ReturnErrorStatus(integrationId, file, line2);

            }
            catch (System.IO.IOException e)
            {
                string message = e.Message;
                message = message.Replace("'", "");
                string queryToLog = "insert into SystemLogs (Description,ErrorDate, IntegrationId) values('Class SqlServer: " + message + "','" + DateTime.Now + "'," + integration.integrationId + ")";
                integration.insertLog(queryToLog);
                return "";
            }                  
        }

        private static string ReturnSuccessStatus(int integrationId, StreamReader file, string[] ContentFile)
        {
            string[] ContentFile2 = ContentFile[0].Split(' ');
            string query = "insert into IntegrationLogs (ReferenceCode,Date,IntegrationId,status) values('" + ContentFile2[9] + "','" + DateTime.Now + "'," + integrationId + "," + ContentFile[1] + ")";
            file.Close();
            return query;
        }

        private static string ReturnErrorStatus(int integrationId, StreamReader file, string line2)
        {
            string status = line2.Substring((line2.Length - 4), 3);
            string TypeError = "";

            if (status.Equals("403"))           
               TypeError = "Forbidden";
            
            else           
               TypeError = "Internal server error";
            
            string query = "insert into IntegrationLogs (ReferenceCode,Date,IntegrationId,status) values('" + TypeError + "','" + DateTime.Now + "'," + integrationId + "," + status + ")";

            file.Close();
            return query;
        }    
    }
}
