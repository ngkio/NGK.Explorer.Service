using System.Collections.Generic;

namespace Explorer.Service.WebApi.Scheduler.Config
{
    public sealed class QuartzOptions
    {
        public IList<JobConfig> JobConfigs { get; set; }
    }

    public sealed class JobConfig
    {
        public string JobName { get; set; }
        public string Interval { get; set; }
    }
}