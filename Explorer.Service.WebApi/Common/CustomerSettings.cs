using System.Collections.Generic;
using Thor.Framework.Common.Options;

namespace Explorer.Service.WebApi.Common
{
    public class CustomerSettings
    {
        public IList<DbContextOption> DatabaseSettings { get; set; }
    }
}