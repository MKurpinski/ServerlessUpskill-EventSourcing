using Microsoft.Azure.Search;

namespace Upskill.Search.Providers
{
    public interface ISearchServiceClientProvider
    {
        ISearchServiceClient Get();
    }
}
