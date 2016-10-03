using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace IntegrationTool.Models
{
    public class UsersModel
    {
        private IntegrationToolEntities usersDB;

        public UsersModel()
        {
            usersDB = new IntegrationToolEntities();
        }

        // ================================================================================================================
        // Almacenar datos relacionados con la gestón de usuarios en el sistema.
        // ================================================================================================================
        public void saveUser(string userTypeId, string name, string username, string email, string password, string[] resourcesIds)
        {
            User user = new User();
            user.UserTypeId = Convert.ToInt32(userTypeId);
            user.Name = name;
            user.Username = username;
            user.Email = email;
            user.Password = password;

            int status = usersDB.Status.Where(param => param.Name == "Enable").ToList()[0].StatusId;

            user.StatusId = status;

            usersDB.Users.Add(user);
            usersDB.SaveChanges();

            foreach(string resource in resourcesIds)
            {
                Permission permission = new Permission();
                permission.ResourceId = Convert.ToInt32(resource);
                permission.UserId = user.UserId;
                usersDB.Permissions.Add(permission);
            }

            usersDB.SaveChanges();
        }

        // ================================================================================================================
        // Actualizar datos relacionados con la gestón de usuarios en el sistema.
        // ================================================================================================================
        public void updateUser(string userId, string userTypeId, string name, string username, string email, string password, string[] resourcesIds)
        {
            int userIdInt = Convert.ToInt32(userId);
            User user = usersDB.Users.Where(param => param.UserId == userIdInt).ToList()[0];
            user.UserTypeId = Convert.ToInt32(userTypeId);
            user.Name = name;
            user.Username = username;
            user.Email = email;
            user.Password = password;

            List<Permission> permissionsAdd = new List<Permission>();
            List<Permission> permissionsRemove = new List<Permission>();

            foreach(Permission p in user.Permissions)
            {
                bool remove = true;
                foreach (string resource in resourcesIds)
                {
                    if (p.ResourceId == Convert.ToInt32(resource))
                    {
                        remove = false;
                        break;
                    }
                }
                if (remove)
                {
                    permissionsRemove.Add(p);
                }
            }

            foreach (string resource in resourcesIds)
            {
                bool add = true;
                foreach (Permission p in user.Permissions)
                {
                    if (p.ResourceId == Convert.ToInt32(resource))
                    {
                        add = false;
                        break;
                    }
                }
                if (add)
                {
                    Permission permission = new Permission();
                    permission.ResourceId = Convert.ToInt32(resource);
                    permission.UserId = user.UserId;
                    permissionsAdd.Add(permission);
                }
            }

            foreach (Permission p in permissionsRemove)
            {
                usersDB.Permissions.Remove(p);
            }

            foreach (Permission p in permissionsAdd)
            {
                usersDB.Permissions.Add(p);
            }

            usersDB.SaveChanges();
        }

        public void enableUser(int id)
        {
            User user = usersDB.Users.Where(param => param.UserId == id).ToList()[0];
            int enable = usersDB.Status.Where(param => param.Name == "Enable").ToList()[0].StatusId;

            user.StatusId = enable;

            usersDB.SaveChanges();
        }

        public void disableUser(int id)
        {
            User user = usersDB.Users.Where(param => param.UserId == id).ToList()[0];
            int disable = usersDB.Status.Where(param => param.Name == "Disable").ToList()[0].StatusId;

            user.StatusId = disable;

            usersDB.SaveChanges();
        }

        public bool changePassword(string userId, string password, string newPassword)
        {
            int id = Convert.ToInt32(userId);

            User user = usersDB.Users.Where(param => param.UserId == id).ToList()[0];

            if (user.Password == password)
            {
                user.Password = newPassword;
                usersDB.SaveChanges();
                return true;
            }
            else
                return false;
        }


        // ================================================================================================================
        // Obtener datos relacionados con la gestón de usuarios en el sistema.
        // ================================================================================================================
        public List<User> getUsers()
        {
            List<User> users = usersDB.Users.ToList();
            return users;
        }

        public User getUser(int id)
        {
            User user = usersDB.Users.Where(param => param.UserId == id).ToList()[0];
            return user;
        }

        public List<UsersType> getUserTypes()
        {
            List<UsersType> userTypes = usersDB.UsersTypes.ToList();
            return userTypes;
        }

        public List<Resource> getResources()
        {
            List<Resource> resources = usersDB.Resources.ToList();
            return resources;
        }

        public User searchUser(string stringSearch)
        {
            string domain = WebConfigurationManager.AppSettings["ADDomain"];
            string path = WebConfigurationManager.AppSettings["ADPath"];

            DirectoryEntry entry = new DirectoryEntry(path);

            Object obj = entry.NativeObject;
            DirectorySearcher search = new DirectorySearcher(entry);

            search.Filter = "(&(objectClass=user)(sAMAccountName=" + stringSearch + "))";
            search.PropertiesToLoad.Add("sAMAccountName");
            search.PropertiesToLoad.Add("cn");
            search.PropertiesToLoad.Add("displayName");
            search.PropertiesToLoad.Add("mail");

            SearchResult result = search.FindOne();

            if (result == null)
            {
                return null;
            }

            User user = new User();
            user.Name = result.Properties["displayName"][0].ToString();
            user.Username = result.Properties["sAMAccountName"][0].ToString();
            user.Email = result.Properties["mail"][0].ToString();
            user.Password = "wsfHCXPULUlICR67Did9BA==";

            return user;
        }
    }
}
