using System.Threading.Tasks;
using Application.Storage.Dtos;
using Application.Storage.Tables.Models;
using Upskill.Storage.Table.Providers;
using Upskill.Storage.Table.Repositories;

namespace Application.Storage.Tables.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ITableClientProvider tableClientProvider) 
            : base(tableClientProvider)
        {
        }

        async Task ICategoryRepository.CreateOrUpdate(CategoryDto category)
        {
            await this.CreateOrUpdate(new Category(category.Id, category.Name));
        }

        public async Task Delete(string id)
        {
            await this.DeleteById(id);
        }
    }
}