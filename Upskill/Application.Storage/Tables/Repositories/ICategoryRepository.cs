using System.Threading.Tasks;
using Application.Storage.Dtos;

namespace Application.Storage.Tables.Repositories
{
    public interface ICategoryRepository
    {
        Task CreateOrUpdate(CategoryDto category);
        Task Delete(string id);
    }
}
