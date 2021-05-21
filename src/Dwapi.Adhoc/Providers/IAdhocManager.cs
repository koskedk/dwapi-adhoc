using ActiveQueryBuilder.Web.Server.Infrastructure.Providers;
using Dwapi.Adhoc.Helpers;
using Dwapi.Adhoc.Models;

namespace Dwapi.Adhoc.Providers
{
    public interface IAdhocManager
    {
        void RefreshMetadata(string connectionString, string path, SourceDbType dbType);
    }
}
