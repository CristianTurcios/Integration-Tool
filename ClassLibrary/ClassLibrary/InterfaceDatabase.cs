using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public interface InterfaceDatabase
    {
        void openConnection();
        void closeConnection();
        string executeQuery(string query);
    }
}
