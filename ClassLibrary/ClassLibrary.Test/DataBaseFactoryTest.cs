using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ClassLibrary;

namespace IntegrationTool.Tests
{
    [TestFixture]
    class DataBaseFactoryTest
    {
        [TestCase("172.20.33.13", "", "IntegrationTool", "", "SISUser", "test2016!", "SQLServer", "SELECT 'abc'")]
        public void sqlServerDataBase_ConnectWithFactoryMethod_ReturnQueryResult(string ip, string port, string nameDataBase, string serverInstance, string username, string password, string engine, string query)
        {

        }
    }
}
