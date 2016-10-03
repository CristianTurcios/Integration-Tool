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
    class SqlServerDataBaseTest
    {
        [TestCase("172.20.33.13", "", "IntegrationTool", "", "", "", "SQLServer", "SELECT 'abc'")]
        public void sqlServerDataBase_Connect_ReturnQueryResult(string ip, string port, string nameDataBase, string serverInstance, string username, string password, string engine, string query)
        {

        }
    }
}
