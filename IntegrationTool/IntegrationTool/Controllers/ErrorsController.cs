using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntegrationTool.Controllers
{
    [Authorize]
    public class ErrorsController : Controller
    {
        [HttpGet]
        public ActionResult forbidden403()
        {
            return View();
        }

        [HttpGet]
        public ActionResult internalServerError500()
        {
            return View();
        }

    }
}
