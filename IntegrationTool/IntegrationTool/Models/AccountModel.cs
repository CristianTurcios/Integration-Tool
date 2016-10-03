using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntegrationTool.Models
{
    public class AccountModel
    {
        private IntegrationToolEntities AccountDB;

        public AccountModel()
        {
            AccountDB = new IntegrationToolEntities();
        }

        public User getUser(string username)
        {
            User user = AccountDB.Users.Where(param => param.Username == username).ToList()[0];
            return user;
        }

        public bool validateLocalUser(string username, string password)
        {
            List<User> users = AccountDB.Users.Where(param => param.Username == username && param.Password == password).ToList();

            if (users.Count == 1)
                return true;
            else
                return false;
        }
    }
}
