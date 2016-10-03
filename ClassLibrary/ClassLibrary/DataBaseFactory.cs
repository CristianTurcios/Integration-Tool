using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class DataBaseFactory
    {
        public InterfaceDatabase createInstanceDataBase(string ip, string port, string nameDataBase, string serverInstance, string username, string password, string engine, Integration integration)
        {
            if (engine == "SQLSERVER")
                return new SqlServerDatabase(ip, port, nameDataBase, serverInstance, username, password,integration);
            else
                return new MySqlDatabase(ip, nameDataBase, serverInstance, username, password,port,integration);
        }
    }
}
