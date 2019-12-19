using Microsoft.Azure.Search;

namespace Application.Search.Providers
{
    public interface ISearchServiceClientProvider
    {
        ISearchServiceClient Get();
    }
}
