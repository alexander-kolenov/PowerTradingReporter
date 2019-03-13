using Reporter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WindowsService
{
    public partial class Service : ServiceBase
    {
        EventLog Log;
        TradingReporter TradingReporter;

        public Service()
        {
            InitializeComponent();
            Log = new EventLog();
            Log.Source = ServiceName;
            Log.Log = "Application";
            Log.BeginInit();
            if (!EventLog.SourceExists(Log.Source))
            {
                EventLog.CreateEventSource(Log.Source, Log.Log);
            }
            Log.EndInit();
            TradingReporterConfiguration config = new TradingReporterConfiguration();
            config.UpdateFromAppConfig();

            TradingReporter = new TradingReporter(config);
        }

        protected override void OnStart(string[] args)
        {
            Log.WriteEntry("Started");
            TradingReporter.OnStart();
        }

        protected override void OnStop()
        {
            Log.WriteEntry("Stopped");
            TradingReporter.OnStop();
        }

        protected override void OnPause()
        {
            Log.WriteEntry("Paused");
            TradingReporter.OnPause();
        }

        protected override void OnContinue()
        {
            Log.WriteEntry("Continue");
            TradingReporter.OnContinue();
        }
    }
}
