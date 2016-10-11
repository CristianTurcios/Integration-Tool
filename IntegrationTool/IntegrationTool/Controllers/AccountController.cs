using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using IntegrationTool.Models;
using System.Security.Principal;
using System.Threading;
using Newtonsoft.Json;
using ClassLibrary;

namespace IntegrationTool.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public ActionResult viewLogin()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public void login()
        {
            AccountModel accountModel = new AccountModel();
            Encrypt encryptor = new Encrypt();

            string resp = "";

            // Usuarios Locales
            if (accountModel.validateLocalUser(Request.Form["Username"], encryptor.encryptData(Request.Form["Password"])))
            {
                User user = accountModel.getUser(Request.Form["Username"]);
                if (validatePassword(user))
                    resp = responseByStatus(user);
                else
                    resp = "{\"type\":\"danger\", \"message\":\"User or password incorrect. Please try again.\"}";
            }
            // Usuarios Active Directory
            else
            {
                if (Membership.ValidateUser(Request.Form["Username"], Request.Form["Password"]))
                {
                    try
                    {
                        User user = accountModel.getUser(Request.Form["Username"]);
                        resp = responseByStatus(user);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        resp = "{\"type\":\"danger\", \"message\":\"User don´t have permissions.\"}";
                    }
                }
                else
                {
                    resp = "{\"type\":\"danger\", \"message\":\"User or password incorrect. Please try again.\"}";
                }
                
            }

            response(resp);
        }

        [HttpPost]
        public void Logout()
        {
            FormsAuthentication.SignOut();
            string resp = "{}";
            response(resp);
        }

        // ================================================================================================================
        // Metodos privados que proveen funcionalidad a las acciones del controlador.
        // ================================================================================================================
        private void response(string json)
        {
            Response.Clear();
            Response.ContentType = "application/json; charset=utf-8";
            Response.Write(json);
            Response.End();
        }

        private string serializeObject(Object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
        }

        private bool verifyStatus(User user)
        {
            if (user.Status.Name == "Enable")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string responseByStatus(User user)
        {
            if (verifyStatus(user))
            {
                addAuthCookie(Request.Form["Username"]);
                return serializeObject(user);
            }
            else
            {
                return "{\"type\":\"danger\", \"message\":\"User is disable.\"}";
            }
        }

        private bool validatePassword(User user)
        {
            Encrypt encryptor = new Encrypt();
            if (user.Password == encryptor.encryptData("ActiveDirectory"))
                return false;
            else
                return true;
        }

        private void addAuthCookie(string username)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                      username,
                      DateTime.Now,
                      DateTime.Now.AddMinutes(30),
                      true,
                      "",
                      FormsAuthentication.FormsCookiePath);

            // Encrypt the ticket.
            string encTicket = FormsAuthentication.Encrypt(ticket);

            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);

            cookie.Expires = ticket.Expiration;

            // Create the cookie.
            Response.Cookies.Add(cookie);
        }
    }
}
