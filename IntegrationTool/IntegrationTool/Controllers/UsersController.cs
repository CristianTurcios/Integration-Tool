using ClassLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IntegrationTool.Models;
using System.Web.Configuration;
using System.DirectoryServices;

namespace IntegrationTool.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private UsersModel userModel;
        private Encrypt encryptor;

        // ================================================================================================================
        // Retornar las vistas relacionadas con la gestión de usuarios.
        // ================================================================================================================
        [HttpGet]
        public ActionResult listUsers()
        {
            return View();
        }

        [HttpGet]
        public ActionResult formLocalUser()
        {
            return View();
        }

        [HttpGet]
        public ActionResult formADUser()
        {
            return View();
        }

        [HttpGet]
        public ActionResult changePassword()
        {
            return View();
        }

        // ================================================================================================================
        // Obtener datos de la base de datos relacionados con la gestión de usuarios.
        // ================================================================================================================
        [HttpGet]
        public void getUsers()
        {
            string resp = "";
            try
            {
                connectModel();
                List<User> users = userModel.getUsers();

                foreach (User u in users)
                {
                    u.Password = encryptor.decryptData(u.Password);
                }

                resp = serializeObject(users);
            }
            catch (Exception)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Can not be loaded the users. Please try again.\"}";
            }

            response(resp);
        }

        [HttpGet]
        public void getUser(int id)
        {
            string resp = "";
            try
            {
                connectModel();
                User user = userModel.getUser(id);

                user.Password = encryptor.decryptData(user.Password);

                resp = serializeObject(user);
            }
            catch (Exception)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Can not be loaded the user. Please try again.\"}";
            }

            response(resp);
        }

        [HttpGet]
        public void searchUser(string search)
        {
            string resp = "";
            try
            {
                connectModel();
                User user = userModel.searchUser(search);

                user.Password = encryptor.decryptData(user.Password);

                if(user != null)
                    resp = serializeObject(user);
                else
                    resp = serializeObject(user);
            }
            catch (Exception)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Can not be find the user. Please try again.\"}";
            }

            response(resp);
        }

        [HttpGet]
        public void getUserTypes()
        {
            string resp = "";
            try
            {
                connectModel();
                List<UsersType> userTypes = userModel.getUserTypes();

                resp = serializeObject(userTypes);
            }
            catch (Exception)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Can not be loaded the user types. Please try again.\"}";
            }

            response(resp);
        }

        [HttpGet]
        public void getResources()
        {
            string resp = "";
            try
            {
                connectModel();
                List<Resource> resources = userModel.getResources();

                resp = serializeObject(resources);
            }
            catch (Exception)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Can not be loaded the resources. Please try again.\"}";
            }

            response(resp);
        }

        // ================================================================================================================
        // Almacenar datos relacionados con la gestón de usuarios en el sistema.
        // ================================================================================================================
        [HttpPut]
        public void saveUser()
        {
            string resp = "";
            try
            {
                string[] permissionsIds = Request.Form["Permissions"].Split('|');

                connectModel();
                userModel.saveUser(Request.Form["UserTypeId"], Request.Form["Name"], Request.Form["Username"], Request.Form["Email"], encryptor.encryptData(Request.Form["Password"]), permissionsIds);

                resp = "{\"type\":\"success\", \"message\":\"User created succesfully..\"}";
            }
            catch (Exception)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Can not be created the user. Please try again.\"}";
            }

            response(resp);
        }

        // ================================================================================================================
        // Actualizar datos relacionados con la gestón de usuarios en el sistema.
        // ================================================================================================================
        [HttpPost]
        public void updateUser()
        {
            string resp = "";
            try
            {   
                string[] permissionsIds = Request.Form["Permissions"].Split('|');

                connectModel();
                userModel.updateUser(Request.Form["UserId"], Request.Form["UserTypeId"], Request.Form["Name"],
                                        Request.Form["Username"], Request.Form["Email"], encryptor.encryptData(Request.Form["Password"]), 
                                        permissionsIds);

                resp = "{\"type\":\"success\", \"message\":\"User updated succesfully.\"}";
            }
            catch (Exception)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Can not be updated the user. Please try again.\"}";
            }
            response(resp);
        }

        [HttpPost]
        public void updatePassword()
        {
            string resp = "";
            try
            {
                connectModel();
                bool isUpdated = userModel.changePassword(Request.Form["UserId"], encryptor.encryptData(Request.Form["Password"]), encryptor.encryptData(Request.Form["NewPassword"]));

                if(isUpdated)
                    resp = "{\"type\":\"success\", \"message\":\"Password changed succesfully.\"}";
                else
                    resp = "{\"type\":\"danger\", \"message\":\"Password incorrect.\"}";
            }
            catch (Exception)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Can not be changed the password. Please try again.\"}";
            }
            response(resp);
        }


        // ================================================================================================================
        // Cambiar estados de usuarios.
        // ================================================================================================================
        [HttpGet]
        public void disableUser(int id)
        {
            string resp = "";
            try
            {
                connectModel();
                userModel.disableUser(id);

                resp = "{\"type\":\"success\", \"message\":\"User disable successfully.\"}";
            }
            catch (Exception)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Can not be disable user. Please try again.\"}";
            }

            response(resp);
        }

        [HttpGet]
        public void enableUser(int id)
        {
            string resp = "";
            try
            {
                connectModel();
                userModel.enableUser(id);

                resp = "{\"type\":\"success\", \"message\":\"User enable successfully.\"}";
            }
            catch (Exception)
            {
                resp = "{\"type\":\"danger\", \"message\":\"Can not be enable user. Please try again.\"}";
            }

            response(resp);
        }

        // ================================================================================================================
        // Metodos privados que proveen funcionalidad a las acciones del controlador.
        // ================================================================================================================
        private void connectModel()
        {
            userModel = new UsersModel();
            encryptor = new Encrypt();
        }

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

    }
}
