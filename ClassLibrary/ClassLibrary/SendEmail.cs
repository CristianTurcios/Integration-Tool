using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace ClassLibrary
{
    public class SendEmail
    {
        public void sendMail(string smtpUser, string smtpPassword, string smtpServer, string smtpPort, string from, string destination, string subject, string body, string Attachment, Integration integration)
         {
             try
             {             
                MailMessage mail = new MailMessage(from, destination, subject, body);
                SmtpClient smtp = new SmtpClient();

                Attachment data = new Attachment(Attachment, MediaTypeNames.Application.Octet);
                //Attachment data2 = new Attachment(pathLog, MediaTypeNames.Application.Octet);
                
                mail.Attachments.Add(data);
                //mail.Attachments.Add(data2);

                smtp.Host = smtpServer;
                smtp.Port = Convert.ToInt32(smtpPort);
                smtp.Credentials = new NetworkCredential(smtpUser, smtpPassword);

                smtp.EnableSsl = true;
                smtp.Send(mail);
             }
             catch (System.ArgumentException e)
             {               
                 string message = e.Message;
                 message = message.Replace("'", "");
                 string queryToLog2 = "insert into SystemLogs (Description,ErrorDate, IntegrationId) values('Class Send Mails: " + message + "','" + DateTime.Now + "'," + integration.integrationId + ")";
                 integration.insertLog(queryToLog2);
             }
             catch (System.IO.IOException e)
             {               
                 string message = e.Message;
                 message = message.Replace("'", "");
                 string queryToLog2 = "insert into SystemLogs (Description,ErrorDate, IntegrationId) values('Class Send Mails: " + message + "','" + DateTime.Now + "'," + integration.integrationId + ")";
                 integration.insertLog(queryToLog2);
             }
             catch (System.Net.Mail.SmtpException e)
             {
                 string message = e.Message;
                 message = message.Replace("'", "");
                 string queryToLog2 = "insert into SystemLogs (Description,ErrorDate, IntegrationId) values('Class Send Mails: " + message + "','" + DateTime.Now + "'," + integration.integrationId + ")";
                 integration.insertLog(queryToLog2);

                 throw new ArgumentException(e.Message);
             }
             catch (System.FormatException e)
             {
                 string message = e.Message;
                 message = message.Replace("'", "");
                 string queryToLog2 = "insert into SystemLogs (Description,ErrorDate, IntegrationId) values('Class Send Mails: " + message + "','" + DateTime.Now + "'," + integration.integrationId + ")";
                 integration.insertLog(queryToLog2);

                 throw new ArgumentException(e.Message);
             }                             
         }
    }
}