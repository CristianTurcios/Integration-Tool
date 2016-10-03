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
    class WriteFileTest
    {            
        [TestCase("ğçşüexternal_course_key|course_id|course_name|term_keyñá|duration|master_course_key|new_data_source_key", "CHE101002|CHE101002|General Chemistry 2 002|201206|t|CHE101M|201206_COURSES", @"C:\Users\cturcios\Desktop", "COURSES")]
        public void WriteInFile2(string headers, string resultQuery, string location,string nameIntegration)
        {

        }

        [Test]
        public void writeFileinExcel()
        {

        }     
    }
}
