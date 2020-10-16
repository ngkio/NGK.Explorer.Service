using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Explorer.Service.Contract.Config;
using Thor.Framework.Common.Helper;
using Thor.Framework.Data;
using Thor.Framework.Data.Model;
using Thor.Framework.Ext.NGKSharp;
using Thor.Framework.Ext.NGKSharp.Core;
using Thor.Framework.Ext.NGKSharp.Core.Api.v1;
using Thor.Framework.Ext.NGKSharp.Core.Exceptions;
using Microsoft.Extensions.Options;
using Action = Thor.Framework.Ext.NGKSharp.Core.Api.v1.Action;

namespace Explorer.Service.Contract
{
    public abstract class BaseClient
    {
        protected readonly Ngk Client;
        protected readonly NgkConfig NgkConfig;
        protected readonly NodeConfig Node;
        protected readonly ChainConfigurator NgkConfigurator;

        protected BaseClient(IOptions<NgkConfig> options)
        {
            NgkConfig = options.Value;
            Node = NgkConfig.Nodes.FirstOrDefault();
            NgkConfigurator = new ChainConfigurator
            {
                HttpEndpoint = Node.HttpAddress,
                ChainId = NgkConfig.ChainId,
                ExpireSeconds = Node.TimeOut
            };

            Client = new Ngk(NgkConfigurator);
        }

        protected virtual async Task<ExcutedResult<string>> PushAction(Action action)
        {
            var trans = new Transaction
            {
                actions = new List<Action>
                {
                    action
                }
            };

            try
            {
                var result = await Client.CreateTransaction(trans);

                if (string.IsNullOrEmpty(result))
                {
                    return ExcutedResult<string>.FailedResult(SysResultCode.DataNotExist, "Contract return null.");
                }

                return ExcutedResult<string>.SuccessResult("", result);
            }
            catch (ApiErrorException e)
            {
                Log4NetHelper.WriteError(GetType(), e,
                    $"Code:{e.code} ErrorName:{e.error.name} Error:{e.error.what} Detail:{e.error.details.Aggregate("", (s, p) => s + p.message + "\n")}");
                return ExcutedResult<string>.FailedResult(SysResultCode.ServerException,
                    $"EOS request api error. {Node?.HttpAddress}");
            }
            catch (ApiException ex)
            {
                Log4NetHelper.WriteError(GetType(), ex,
                    $"StatusCode:{ex.StatusCode} Content:{ex.Content}");
                return ExcutedResult<string>.FailedResult(SysResultCode.ServerException,
                    $"EOS request error. {Node?.HttpAddress}");
            }
            catch (Exception ex)
            {
                Log4NetHelper.WriteError(GetType(), ex, "PushAction System Error");
                return ExcutedResult<string>.FailedResult(SysResultCode.ServerException,
                    $"Server Exception");
            }
        }
    }
}