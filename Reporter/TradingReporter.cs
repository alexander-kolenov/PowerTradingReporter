using Quartz;
using Quartz.Impl;

namespace Reporter
{
    public class TradingReporter
    {
        private readonly IScheduler _scheduler;

        public TradingReporter()
        {
            ISchedulerFactory factory = new StdSchedulerFactory();
            _scheduler = factory.GetScheduler().GetAwaiter().GetResult();
        }


        public void OnStart()
        {
            _scheduler.Clear();

            IJobDetail job = JobBuilder.Create<MakeReportJob>()
                    .WithIdentity("MakeReport", "Reporter")
                    .RequestRecovery()
                    .Build();

            var config = ConfigReader.ReadTradingReporterConfigurationFromAppConfig();
            job.JobDataMap.Add(nameof(TradingReporterConfiguration), config);

            var ReportingInterval = ConfigReader.ReadReportingIntervalFromAppConfig();
            ITrigger trigger = TriggerBuilder.Create()
                     .WithIdentity("PeriodicalTrigger", "Reporter")
                     .StartNow()
                     .WithSimpleSchedule(x => x
                         .WithInterval(ReportingInterval)
                         .RepeatForever()
                         )
                     .Build();

            _scheduler.ScheduleJob(job, trigger);
            _scheduler.Start();
        }

        public void OnStop()
        {
            _scheduler.Shutdown().Wait();
        }

        public void OnPause()
        {
            _scheduler.Standby();
        }

        public void OnContinue()
        {
            _scheduler.ResumeAll();
        }

    }
}
