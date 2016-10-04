using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "https://qa04-04.blackboard.laureate.net/webapps/bb-data-integration-flatfile-BBLEARN/endpoint/person/store";

            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
           
            myReq.Method = "POST";
            myReq.ContentType = "text/plain; charset=UTF-8";

            string usernamePassword = "725942cc-87f0-45dc-a386-de0934d8ff93:test123";
            UTF8Encoding enc = new UTF8Encoding();

            myReq.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(enc.GetBytes(usernamePassword)));

            try
            {
                HttpWebResponse wr = (HttpWebResponse)myReq.GetResponse();
                Stream receiveStream = wr.GetResponseStream();
                StreamReader reader = new StreamReader(receiveStream, Encoding.UTF8);
                string content = reader.ReadToEnd();
                Console.WriteLine("Content "+content);
                Console.WriteLine("Status "+wr.StatusCode);
                Console.ReadLine();
            }
            catch (WebException e)
            {
                Console.WriteLine("Status en Catch "+e.Status);
                Console.ReadLine();
            }
        }
    }
}
