using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Explorer.Service.WebApi.Scheduler.Config;
using Explorer.Service.WebApi.Scheduler.Jobs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Quartz;
using Quartz.Impl;

namespace Explorer.Service.WebApi.Scheduler
{
    public sealed class QuartzStartup
    {
        private readonly QuartzOptions _quartzOptions;
        private readonly IApplicationLifetime _lifeTime;
        private IScheduler _scheduler;

        public QuartzStartup(IOptions<QuartzOptions> options, IApplicationLifetime lifetime)
        {
            _quartzOptions = options.Value;
            _lifeTime = lifetime;
        }

        public void Init()
        {
            _lifeTime.ApplicationStarted.Register(Start);
            _lifeTime.ApplicationStopped.Register(Stop);
        }

        public void Start()
        {
            if (_scheduler != null)
            {
                throw new InvalidOperationException("Already started.");
            }

            var properties = new NameValueCollection
            {
                ["quartz.scheduler.instanceName"] = "MyScheduler",
                ["quartz.serializer.type"] = "json",
                ["quartz.jobStore.type"] = "Quartz.Simpl.RAMJobStore, Quartz",
                ["quartz.threadPool.threadCount"] = "3"
            };

            var schedulerFactory = new StdSchedulerFactory(properties);
            _scheduler = schedulerFactory.GetScheduler().Result;
            _scheduler.Start().Wait();

            var jobs = new Dictionary<string, IJobDetail>
            {
                {
                    "RefreshLastBlockJob", JobBuilder.Create<RefreshLastBlockJob>().WithIdentity("RefreshLastBlockJob").Build()
                },
                {
                    "RefreshTokenAccountsJob", JobBuilder.Create<RefreshTokenAccountsJob>().WithIdentity("RefreshTokenAccountsJob").Build()
                }
            };

            foreach (var jobConfig in _quartzOptions.JobConfigs)
            {
                if (!jobs.TryGetValue(jobConfig.JobName, out var job)) continue;

                var trigger = TriggerBuilder.Create()
                    .WithIdentity($"{jobConfig.JobName}Trigger")
                    .StartNow()
                    .WithCronSchedule(jobConfig.Interval)
                    .Build();

                _scheduler.ScheduleJob(job, trigger).Wait();
            }
        }

        public void Stop()
        {
            if (_scheduler == null)
            {
                return;
            }

            if (_scheduler.Shutdown(true).Wait(30000))
            {
                _scheduler = null;
            }
        }
    }
}