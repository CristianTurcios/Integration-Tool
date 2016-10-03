using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegrationToolService;
using NUnit.Framework;

namespace TestService
{
    [TestFixture]
    class test
    {
         [Test]
         public void add_EmptyString_ReturnNothing()
         {
             GenerateIntegration coneDatabase = new GenerateIntegration();
             coneDatabase.ObtainQueryToVerifyTimeToExecutionIntegration();
         }
    }
}
