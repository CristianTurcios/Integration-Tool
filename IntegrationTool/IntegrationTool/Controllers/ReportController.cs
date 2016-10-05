using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using IntegrationTool.Models;
//Para el manejo de Archivos
using System.IO;
//Clases necesarias de iTextSharp
using iTextSharp;
using iTextSharp.text.pdf;
using iTextSharp.text;
//Para la creacion de un excel
using DocumentFormat.OpenXml;
using ClosedXML.Excel;
using ClassLibrary;
using System.Data;

namespace IntegrationTool.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private ReportsConfiguration ReportsConfigurationModel;
        private Encrypt decrypt = new Encrypt();

        private void connectModel()
        {
            ReportsConfigurationModel = new ReportsConfiguration();

        }

        private void response(string json)
        {
            Response.Clear();
            Response.ContentType = "application/json; charset=utf-8";
            Response.Write(json);
            Response.End();
        }

        [HttpGet]
        public ActionResult getReport()
        {        
            return View();
        }

        [HttpGet]
        public void getListCategoryIntegration()
        {
            string resp = "";
            try
            {
                connectModel();

                List<IntegrationCategory> integrationCategory = ReportsConfigurationModel.getCategoryIntegration();

                resp = Newtonsoft.Json.JsonConvert.SerializeObject(integrationCategory,
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
        public void getListIntegrationType()
        {
            string resp = "";
            try
            {
                connectModel();

                List<IntegrationsType> integrationType = ReportsConfigurationModel.getIntegrationType();

                resp = Newtonsoft.Json.JsonConvert.SerializeObject(integrationType,
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
        public void getListOperationWebServices()
        {
            string resp = "";
            try
            {
                connectModel();

                List<OperationsWebService> operationWebServices = ReportsConfigurationModel.getOperationWebServices();

                resp = Newtonsoft.Json.JsonConvert.SerializeObject(operationWebServices,
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
        public void getListDatabaseParameter()
        {
            string resp = "";
            try
            {
                connectModel();

                List<DatabaseParameter> databaseParameter = ReportsConfigurationModel.getDatabaseParameter();

                foreach (DatabaseParameter database in databaseParameter)
                {
                    database.Name =  decrypt.decryptData(database.Name);                 
                }

                resp = serializeObject(databaseParameter);            
            }
            catch (Exception e)
            {
                resp = "{\"type\":\"danger\", \"message\":\"" + e.Message + ".\"}";
            }

            response(resp);
        }

        private string serializeObject(Object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
        }


        [HttpPost]
        public void getDocumentReport()
        {
            string resp = "";
           
            
            ReportsGenerate reportGenerate = new ReportsGenerate();
            DataTable table = new DataTable();
                     
            try
            {
                 DeterminateDateIsNullOrNot(reportGenerate, table);                                    
             
                 resp = "{\"type\":\"success\", \"message\":\"Report Generate " + Request.Form["Report"] + " Successful.\"}";
            }
            catch (Exception e)
            {
                resp = "{\"type\":\"danger\", \"message\":\"" + e.Message + ".\"}";
              
            }
            //response(resp);
        }

        private void DeterminateDateIsNullOrNot(ReportsGenerate reportGenerate, DataTable table)
        {
            string TimeStartandEnd = "01-01-1901";

            if (string.IsNullOrEmpty(Convert.ToString(Request.Form["start"])))
               ExtractDataToReportWithTimeStartAndEnd(reportGenerate, table, TimeStartandEnd);              
            
            else
               ExtractDataToReport(reportGenerate, table);
            
           
        }

        private void ExtractDataToReport(ReportsGenerate reportGenerate, DataTable table)
        {
            if (Request.Form["value2"].Equals("Integration Logs") || string.IsNullOrEmpty(Request.Form["value2"]))
            {

                table = reportGenerate.ParamToGenerateReportForIntegrationLogs(Convert.ToString(Request.Form["start"]), Convert.ToString(Request.Form["end"]), Convert.ToInt32(Request.Form["IntegrationType"]),
                                                               Convert.ToInt32(Request.Form["IntegrationCategory"]),Convert.ToInt32(Request.Form["OperationWebServices"]), Convert.ToInt32(Request.Form["DatabaseParameter"]));

                if (Request.Form["Report"].Equals("Excel"))
                    generateDocumentForIntegrationLogsExcel(table);
                else
                    generateDocumentForIntegrationLogsPDF(table);
            }

            else
            {
                table = reportGenerate.ParamToGenerateReportForSystemLogs(Convert.ToString(Request.Form["start"]), Convert.ToString(Request.Form["end"]), Convert.ToInt32(Request.Form["IntegrationType"]),
                                                               Convert.ToInt32(Request.Form["IntegrationCategory"]), Convert.ToInt32(Request.Form["OperationWebServices"]), Convert.ToInt32(Request.Form["DatabaseParameter"]));

                if (Request.Form["Report"].Equals("Excel"))
                    generateDocumentForSystemLogsExcel(table);
                else
                    generateDocumentForSystemLogsPDF(table);              
            }
            
        }

        private void ExtractDataToReportWithTimeStartAndEnd(ReportsGenerate reportGenerate, DataTable table, string TimeStartandEnd)
        {
            if (Request.Form["value2"].Equals("Integration Logs") || string.IsNullOrEmpty(Request.Form["value2"]))
            {
                table = reportGenerate.ParamToGenerateReportForIntegrationLogs(TimeStartandEnd, TimeStartandEnd, Convert.ToInt32(Request.Form["IntegrationType"]), Convert.ToInt32(Request.Form["IntegrationCategory"]),
                                                               Convert.ToInt32(Request.Form["OperationWebServices"]), Convert.ToInt32(Request.Form["DatabaseParameter"]));

                if (Request.Form["Report"].Equals("Excel"))
                    generateDocumentForIntegrationLogsExcel(table);
                else
                    generateDocumentForIntegrationLogsPDF(table);               
            }

            else
            {
                table = reportGenerate.ParamToGenerateReportForSystemLogs(TimeStartandEnd, TimeStartandEnd, Convert.ToInt32(Request.Form["IntegrationType"]), Convert.ToInt32(Request.Form["IntegrationCategory"]),
                                                               Convert.ToInt32(Request.Form["OperationWebServices"]), Convert.ToInt32(Request.Form["DatabaseParameter"]));

                if (Request.Form["Report"].Equals("Excel"))
                    generateDocumentForSystemLogsExcel(table);
                else
                    generateDocumentForSystemLogsPDF(table);               
            }
            
        }

        private void generateDocumentForIntegrationLogsPDF(DataTable table)
        {          
      
            //Creamos un tipo de archivo que solo se cargará en la memoria principal
            Document documento = new Document();
            
            //Obtenemos la fecha actual incluyendo horas, minutos y segundos, esto se agregara al path del contrato, esto se hace asi para poder tener varios reportes de contratos guardados con el mismo nombre
            //y que lo que diferencie a cada reporte de contrato sea la fecha y hora en que se han generado
            string DateNow = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");

            //Se crea el path (direccion y nombre con el cual se guardara el reporte generado )

            string path = obtainPathReportsFromDatabase();
            path += "/ReportIntegrationLogs" + DateNow + ".pdf";
              
            //Creamos la instancia para generar el archivo PDF
            //Le pasamos el documento creado arriba y con capacidad para abrir o Crear y de nombre Mi_Primer_PDF
            PdfWriter.GetInstance(documento, new FileStream(path, FileMode.OpenOrCreate));

            //Rotamos el documento para que quede horizontal y pueda caber mas informacion para ser visualizada por el usuario
            documento.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            //Abrimos el documento
            documento.Open();
                  
            //Le decimos que nuestro documento actualmente tendra 8 celdas (posible cambio en futuro)
            iTextSharp.text.pdf.PdfPTable aTable = new iTextSharp.text.pdf.PdfPTable(9);           
            //Se hace una instancia de la clase que es la encargada de hacer la consulta a la base de datos para retornar los contratos
           
            
            //Se crea una nueva celda y se le agrega un titulo
            PdfPCell cell = new PdfPCell(new Phrase("Logs For Integrations Stored On The System"));
            
            //Estas son solamente opciones de personalizacion y diseño de la celda          
            cell.HorizontalAlignment = 1;
            cell.UseVariableBorders = true;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(245, 92, 24);
            cell.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
            cell.Colspan = 9;
           
            //Se agrega la celda que se creo al documento
            aTable.AddCell(cell);

            //Se agregan los titulos de las celdas al documento

            PdfPCell headers = new PdfPCell(new Phrase("Reference Code"));
            headers.BorderWidthRight = 1;
            headers.HorizontalAlignment = 1;
            headers.BackgroundColor = new iTextSharp.text.BaseColor(247, 150, 70);
            headers.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
            headers.Colspan = 1;
            aTable.AddCell(headers);

            GenerateHeaders(aTable, headers, "Name Integration");
            GenerateHeaders(aTable, headers, "Status");
            GenerateHeaders(aTable, headers, "Date");
            GenerateHeaders(aTable, headers, "Category Integration");
            GenerateHeaders(aTable, headers, "Type Integration");
            GenerateHeaders(aTable, headers, "Operation Web Services");
            GenerateHeaders(aTable, headers, "Database Name");
            GenerateHeaders(aTable, headers, "Execution Date");
          
            for (int i = 0; i < table.Rows.Count; i++)
            {
                aTable.AddCell(Convert.ToString(table.Rows[i]["ReferenceCode"]));
                aTable.AddCell(Convert.ToString(table.Rows[i]["IntegrationName"]));
                aTable.AddCell(Convert.ToString(table.Rows[i]["Status"]));
                aTable.AddCell(Convert.ToString(table.Rows[i]["Date"]));
                aTable.AddCell(Convert.ToString(table.Rows[i]["Name"]));
                aTable.AddCell(Convert.ToString(table.Rows[i]["TypeIntegration"]));
                aTable.AddCell(Convert.ToString(table.Rows[i]["OperationWebServices"]));
                aTable.AddCell(decrypt.decryptData(Convert.ToString(table.Rows[i]["DatabaseName"])));
                aTable.AddCell(Convert.ToString(table.Rows[i]["IntegrationDate"]));             
            }
         

            //Se agrega toda la informacion traida desde la base de datos al documento
            documento.Add(aTable);
            //Se cierra el documento para que pueda ser visualizado por el usuario
            documento.Close();
                    
            //downloadAdjuntos(path);  
            downloadPDF(path);
        }


        private void generateDocumentForSystemLogsPDF(DataTable table)
        {
          
            //Creamos un tipo de archivo que solo se cargará en la memoria principal
            Document documento = new Document();

            //Obtenemos la fecha actual incluyendo horas, minutos y segundos, esto se agregara al path del contrato, esto se hace asi para poder tener varios reportes de contratos guardados con el mismo nombre
            //y que lo que diferencie a cada reporte de contrato sea la fecha y hora en que se han generado
            string DateNow = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");

            //Se crea el path (direccion y nombre con el cual se guardara el reporte generado )   

            string path = obtainPathReportsFromDatabase();
            path += "/ReportSystemLogs" + DateNow + ".pdf";

            //Creamos la instancia para generar el archivo PDF
            //Le pasamos el documento creado arriba y con capacidad para abrir o Crear y de nombre Mi_Primer_PDF
            PdfWriter.GetInstance(documento, new FileStream(path, FileMode.OpenOrCreate));

            //Rotamos el documento para que quede horizontal y pueda caber mas informacion para ser visualizada por el usuario
            documento.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            //Abrimos el documento
            documento.Open();

            //Le decimos que nuestro documento actualmente tendra 8 celdas (posible cambio en futuro)
            iTextSharp.text.pdf.PdfPTable aTable = new iTextSharp.text.pdf.PdfPTable(8);
            //Se hace una instancia de la clase que es la encargada de hacer la consulta a la base de datos para retornar los contratos


            //Se crea una nueva celda y se le agrega un titulo
            PdfPCell cell = new PdfPCell(new Phrase("Logs For System Stored On The System"));

            //Estas son solamente opciones de personalizacion y diseño de la celda          
            cell.HorizontalAlignment = 1;
            cell.UseVariableBorders = true;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(245, 92, 24);
            cell.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
            cell.Colspan = 9;

            //Se agrega la celda que se creo al documento
            aTable.AddCell(cell);

            //Se agregan los titulos de las celdas al documento

            PdfPCell headers = new PdfPCell(new Phrase("Description"));
            headers.BorderWidthRight = 1;
            headers.HorizontalAlignment = 1;
            headers.BackgroundColor = new iTextSharp.text.BaseColor(247, 150, 70);
            headers.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
            headers.Colspan = 1;
            aTable.AddCell(headers);

            GenerateHeaders(aTable, headers, "Name Integration");
            GenerateHeaders(aTable, headers, "Error Date");
            GenerateHeaders(aTable, headers, "Category Integration");
            GenerateHeaders(aTable, headers, "Type Integration");
            GenerateHeaders(aTable, headers, "Operation Web Services");
            GenerateHeaders(aTable, headers, "Database Name");
            GenerateHeaders(aTable, headers, "Date Integration");

            for (int i = 0; i < table.Rows.Count; i++)
            {
                aTable.AddCell(Convert.ToString(table.Rows[i]["Description"]));
                aTable.AddCell(Convert.ToString(table.Rows[i]["IntegrationName"]));
                aTable.AddCell(Convert.ToString(table.Rows[i]["ErrorDate"]));
                aTable.AddCell(Convert.ToString(table.Rows[i]["Name"]));
                aTable.AddCell(Convert.ToString(table.Rows[i]["TypeIntegration"]));
                aTable.AddCell(Convert.ToString(table.Rows[i]["OperationWebServices"]));
                aTable.AddCell(decrypt.decryptData(Convert.ToString(table.Rows[i]["DatabaseName"])));
                aTable.AddCell(Convert.ToString(table.Rows[i]["IntegrationDate"]));
            }


            //Se agrega toda la informacion traida desde la base de datos al documento
            documento.Add(aTable);
            //Se cierra el documento para que pueda ser visualizado por el usuario
            documento.Close();

            //downloadAdjuntos(path);
            downloadPDF(path);
        }

        private static void GenerateHeaders(iTextSharp.text.pdf.PdfPTable aTable, PdfPCell headers,string NameHeader)
        {
            headers = new PdfPCell(new Phrase(NameHeader));
            headers.BorderWidthRight = 1;
            headers.HorizontalAlignment = 1;
            headers.BackgroundColor = new iTextSharp.text.BaseColor(247, 150, 70);
            headers.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
            headers.Colspan = 1;
            aTable.AddCell(headers);
        }    

        private void generateDocumentForIntegrationLogsExcel(DataTable table)
        {
            ////                   
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Integration Logs");

            worksheet.Cell("A1").Value = "Reference Code";
            worksheet.Cell("B1").Value = "Name Integration";
            worksheet.Cell("C1").Value = "Status";
            worksheet.Cell("D1").Value = "Date";
            worksheet.Cell("E1").Value = "Category Integration";
            worksheet.Cell("F1").Value = "Type Integration";
            worksheet.Cell("G1").Value = "Operation Web Services";
            worksheet.Cell("H1").Value = "Database Name";
            worksheet.Cell("I1").Value = "Date Integration";
         
            int contador = 4;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                worksheet.Cell("A" + contador).Value = Convert.ToString(table.Rows[i]["ReferenceCode"]);
                worksheet.Cell("B" + contador).Value = Convert.ToString(table.Rows[i]["IntegrationName"]);                
                worksheet.Cell("C" + contador).Value = Convert.ToString(table.Rows[i]["Status"]);
                worksheet.Cell("D" + contador).Value = Convert.ToString(table.Rows[i]["Date"]);
                worksheet.Cell("E" + contador).Value = Convert.ToString(table.Rows[i]["Name"]);
                worksheet.Cell("F" + contador).Value = Convert.ToString(table.Rows[i]["TypeIntegration"]);
                worksheet.Cell("G" + contador).Value = Convert.ToString(table.Rows[i]["OperationWebServices"]);
                worksheet.Cell("H" + contador).Value = decrypt.decryptData(Convert.ToString(table.Rows[i]["DatabaseName"]));
                worksheet.Cell("I" + contador).Value = Convert.ToString(table.Rows[i]["IntegrationDate"]);
                contador++; 
            }
            
            worksheet.Columns().AdjustToContents();

            string path = obtainPathReportsFromDatabase();
            path += "/ReportIntegrationLogs" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".xlsx";

            workbook.SaveAs(path);

            downloadExcel(path);            
            ////
        }


        private void generateDocumentForSystemLogsExcel(DataTable table)
        {
            ////                   
            
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("System Logs");

            worksheet.Cell("A1").Value = "Description";
            worksheet.Cell("B1").Value = "Name Integration";
            worksheet.Cell("C1").Value = "Error Date";
            worksheet.Cell("D1").Value = "Category Integration";
            worksheet.Cell("E1").Value = "Type Integration";
            worksheet.Cell("F1").Value = "Operation Web Services";
            worksheet.Cell("G1").Value = "Database Name";
            worksheet.Cell("H1").Value = "Date Integration";

            int contador = 4;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                worksheet.Cell("A" + contador).Value = Convert.ToString(table.Rows[i]["Description"]);
                worksheet.Cell("B" + contador).Value = Convert.ToString(table.Rows[i]["IntegrationName"]);
                worksheet.Cell("C" + contador).Value = Convert.ToString(table.Rows[i]["ErrorDate"]);
                worksheet.Cell("D" + contador).Value = Convert.ToString(table.Rows[i]["Name"]);
                worksheet.Cell("E" + contador).Value = Convert.ToString(table.Rows[i]["TypeIntegration"]);
                worksheet.Cell("F" + contador).Value = Convert.ToString(table.Rows[i]["OperationWebServices"]);
                worksheet.Cell("G" + contador).Value = decrypt.decryptData(Convert.ToString(table.Rows[i]["DatabaseName"]));
                worksheet.Cell("H" + contador).Value = Convert.ToString(table.Rows[i]["IntegrationDate"]);
                contador++;
            }

            worksheet.Columns().AdjustToContents();

            string path = obtainPathReportsFromDatabase();
            path += "/ReportSystemLogs" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".xlsx";

            workbook.SaveAs(path);

            downloadExcel(path);            
            ////
        }

        private string obtainPathReportsFromDatabase()
        {
            string path="";
            try
            {
                connectModel();
                List<PathReport> pathReport = ReportsConfigurationModel.getPathReport();
              
                foreach(PathReport paths in pathReport)
                {
                    path = decrypt.decryptData(paths.Location);
                    break;
                }
            }
            catch (Exception)
            {}

            return path;
        }

        private void downloadExcel(string path)
        {
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.TransmitFile(path);
            Response.End();
        }

        private void downloadPDF(string path)
        {
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/pdf";
            Response.Flush();
            Response.TransmitFile(path);
            Response.End();
        }

    }
}
