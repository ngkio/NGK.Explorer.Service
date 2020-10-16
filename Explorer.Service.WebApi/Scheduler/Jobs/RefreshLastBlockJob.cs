using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspectCore.Injector;
using Explorer.Service.Common;
using Explorer.Service.WebApi.Hubs;
using Explorer.Service.WebApi.Models;
using Thor.Framework.Common.IoC;
using Microsoft.AspNetCore.SignalR;
using Quartz;

namespace Explorer.Service.WebApi.Scheduler.Jobs
{
    public sealed class RefreshLastBlockJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await Run();
        }

        private async Task Run()
        {
            try
            {
                var tasks = new List<Task>();

                tasks.Add(Task.Run(async () =>
                {
                    var hubContext = AspectCoreContainer.CreateScope().Resolve<IHubContext<BlockHub>>();
                    var logic = AspectCoreContainer.CreateScope().Resolve<BlockServiceModel>();
                    var model = logic.GetLatestStatistical();
                    await hubContext.Clients.All.SendAsync("refreshLatestBlock", model);
                }));

                tasks.Add(Task.Run(async () =>
                {
                    var hubContext = AspectCoreContainer.CreateScope().Resolve<IHubContext<BlockHub>>();
                    var logic = AspectCoreContainer.CreateScope().Resolve<BlockServiceModel>();
                    var blocks = await logic.GetListLatest();
                    await hubContext.Clients.All.SendAsync("refreshLatestList", blocks);
                }));

                tasks.Add(Task.Run(async () =>
                {
                    var hubContext = AspectCoreContainer.CreateScope().Resolve<IHubContext<BlockHub>>();
                    var logic = AspectCoreContainer.CreateScope().Resolve<BlockServiceModel>();
                    var producers = await logic.GetListProducers();
                    await hubContext.Clients.All.SendAsync("refreshProducerLatestList", producers);
                }));

                tasks.Add(Task.Run(async () =>
                {
                    var hubContext = AspectCoreContainer.CreateScope().Resolve<IHubContext<BlockHub>>();
                    var trxLogic = AspectCoreContainer.CreateScope().Resolve<TransactionServiceModel>();
                    var trxs = trxLogic.GetListLatest(ConfigDataKey.TransactionListLatestTotal);
                    await hubContext.Clients.All.SendAsync("refreshTrxLatestList", trxs);
                }));

                Task.WaitAll(tasks.ToArray());
            }
            catch (Exception exception)
            {
            }
        }
    }
}