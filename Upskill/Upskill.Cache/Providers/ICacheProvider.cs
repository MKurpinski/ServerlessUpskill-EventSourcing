using System.Threading.Tasks;
using StackExchange.Redis;

namespace Upskill.Cache.Providers
{
    public interface ICacheProvider
    {
        Task<IDatabase> Get();
    }
}
