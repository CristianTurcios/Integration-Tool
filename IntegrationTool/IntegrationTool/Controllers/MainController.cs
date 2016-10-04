using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using IntegrationTool.Models;

using ClassLibrary;

namespace IntegrationTool.Controllers
{
    [Authorize]
    public class MainController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public ActionResult index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult layout()
        {
            return View();
        }

        [HttpGet]
        public ActionResult main()
        {
            return View();
        }

        private MainConfiguration mainConfiguration;
       
        private void connectModel()
        {
            mainConfiguration = new MainConfiguration();
           
        }

        private void response(string json)
        {
            Response.Clear();
            Response.ContentType = "application/json; charset=utf-8";
            Response.Write(json);
            Response.End();
        }

        [HttpGet]
        public void getListCountManualIntegrationPerMonth()
        {
            string resp = "";
            try
            {
                connectModel();

                List<DataRow> ManualIntegrations = mainConfiguration.ParamToGenerateGraphicsForManualIntegration();

                resp = Newtonsoft.Json.JsonConvert.SerializeObject(ManualIntegrations,
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
        public void getListCountAutomaticIntegrationPerMonth()
        {
            string resp = "";
            try
            {
                connectModel();

                List<DataRow> AutomaticIntegrations = mainConfiguration.ParamToGenerateGraphicsForAutomaticIntegration();

                resp = Newtonsoft.Json.JsonConvert.SerializeObject(AutomaticIntegrations,
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
        public void getListCountUsersLocalsInSystem()
        {
            string resp = "";
            try
            {
                connectModel();

                List<DataRow> localUsers = mainConfiguration.ParamToGenerateGraphicsForUsersLocalsInSystem();

                resp = Newtonsoft.Json.JsonConvert.SerializeObject(localUsers,
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
        public void getListCountUsersActiveDirectoryInSystem()
        {
            string resp = "";
            try
            {
                connectModel();

                List<DataRow> activeDirectoryUsers = mainConfiguration.ParamToGenerateGraphicsForUsersActiveDirectoryInSystem();

                resp = Newtonsoft.Json.JsonConvert.SerializeObject(activeDirectoryUsers,
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
    }
}
