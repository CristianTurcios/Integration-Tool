using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using ClassLibrary;

namespace IntegrationToolService
{
    public partial class IntegrationTool : ServiceBase
    {
        public IntegrationTool()
        {
            InitializeComponent();           
        }
      
        protected override void OnStart(string[] args)
        {
            //var timer = new Timer { AutoReset = true, Interval = 3600000 };
            var timer = new Timer { AutoReset = true, Interval = 60000 };
            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }

        protected override void OnStop()
        {

        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {                
           GenerateIntegration coneDatabase = new GenerateIntegration();
           coneDatabase.ObtainQueryToVerifyTimeToExecutionIntegration();
           //coneDatabase.prueba();
        }
    }
}
