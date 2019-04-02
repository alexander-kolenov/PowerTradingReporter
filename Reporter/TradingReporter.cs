using Quartz;
using Quartz.Impl;
using System;

namespace Reporter
{
    public class TradingReporter
    {
        private ISchedulerFactory _factory;
        private IScheduler _scheduler;

        public TradingReporter()
        {
            _factory = new StdSchedulerFactory();
        }


        public void OnStart()
        {
            _scheduler = _factory.GetScheduler().GetAwaiter().GetResult();

            JobKey jobKey = new JobKey("MakeReport", "Reporter");
            TriggerKey triggerKey = new TriggerKey("PeriodicalTrigger", "Reporter");

            IJobDetail job = JobBuilder.Create(typeof(MakeReportJob))
                    .WithIdentity(jobKey)
                    .Build();

            var config = ConfigReader.ReadTradingReporterConfigurationFromAppConfig();
            job.JobDataMap.Add(nameof(TradingReporterConfiguration), config);

            var ReportingInterval = ConfigReader.ReadReportingIntervalFromAppConfig();
            ReportingInterval = TimeSpan.FromSeconds(10);
            ITrigger trigger = TriggerBuilder.Create()
                     .WithIdentity(triggerKey)
                     .StartNow()
                     .WithSimpleSchedule(x => x
                         .WithInterval(ReportingInterval)
                         .RepeatForever()
                         )
                     .Build();

            _scheduler.ScheduleJob(job, trigger);
            //In case of failue repeat job immediately
            _scheduler.RepeatJobAfterFall(job);


            _scheduler.Start();
        }

        public void OnStop()
        {
            _scheduler.Shutdown(waitForJobsToComplete:true).Wait();
        }

        public void OnPause()
        {
            _scheduler.PauseAll();
        }

        public void OnContinue()
        {
            _scheduler.ResumeAll();
        }

    }
}
