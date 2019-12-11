using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace Application.Storage.Table.Providers
{
    public interface ITableClientProvider
    {
        Task<CloudTable> Get(string tableName);
    }
}
