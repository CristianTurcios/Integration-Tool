using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class WriteFileController
    {
        public string writeFileinFlatFile(string resultQuery, string locationToSave, string nameIntegration,Integration integration)
        {
            string nameFile = nameIntegration+"-"+returnDatetimeNow()+".txt";
            string path = locationToSave + "/" + nameFile;

            try
            {
                FileStream fs = new FileStream(path, FileMode.Append);
                using (StreamWriter file = new StreamWriter(fs, Encoding.UTF8))
                {
                    string[] resultParse = resultQuery.Split('%');

                    for (int i = 0; i < resultParse.Length; i++)
                    {
                        file.WriteLine(resultParse[i]);
                    }

                    file.Close();
                }
            }
            catch (DirectoryNotFoundException e)
            {
                string message = e.Message;
                message = message.Replace("'","");
                string query = "insert into SystemLogs (Description,ErrorDate, IntegrationId) values('Class WriteFile: " + message + "','" + DateTime.Now + "'," + integration.integrationId + ")";             
                integration.insertLog(query);
                throw new DirectoryNotFoundException(e.Message);
            }
        
            return path+"|"+nameFile;
        }

        public string writeIntegrationinExcel(string resultQuery, string locationToSave, string nameIntegration)
        {
            StringBuilder csvContent = new StringBuilder();
            string nameFile = nameIntegration + returnDatetimeNow() + ".csv";

            string[] resultParse = resultQuery.Split('%');

            for (int i = 0; i < resultParse.Length; i++)
            {               
                csvContent.AppendLine(resultParse[i]);
            }
                   
            string path = locationToSave + "/" + nameFile;
            File.AppendAllText(path, csvContent.ToString());

            return path+"|"+nameFile;
        }

        private string returnDatetimeNow()
        {
            return DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        }
    }
}
