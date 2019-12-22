using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace Upskill.Storage.Table.Providers
{
    public interface ITableClientProvider
    {
        Task<CloudTable> Get(string tableName);
    }
}
