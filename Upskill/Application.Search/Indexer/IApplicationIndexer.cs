using System.Threading.Tasks;

namespace Application.Search.Indexer
{
    public interface IApplicationIndexer
    {
        Task Index(object toIndex);
    }
}
