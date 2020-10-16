using System;
using System.Threading.Tasks;
using AspectCore.Injector;
using Explorer.Service.WebApi.Models;
using Thor.Framework.Common.IoC;
using Quartz;

namespace Explorer.Service.WebApi.Scheduler.Jobs
{
    public sealed class RefreshTokenAccountsJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await Run();
        }

        private async Task Run()
        {
            try
            {
                var logic = AspectCoreContainer.CreateScope().Resolve<TokenServiceModel>();
                var tokenConfigs = logic.GetAllTokenCodes();

                tokenConfigs.ForEach(c =>
                {
                    try
                    {
                        logic.GetAccountsOfToken(c.Code, c.PrimaryKey);
                    }
                    catch (Exception)
                    {
                    }
                });
            }
            catch (Exception exception)
            {
            }
        }
    }
}