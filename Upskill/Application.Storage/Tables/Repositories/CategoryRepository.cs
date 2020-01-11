using System.Threading.Tasks;
using Application.Storage.Dtos;
using Upskill.Results;
using Upskill.Results.Implementation;
using Upskill.Storage.Table.Providers;
using Upskill.Storage.Table.Repositories;

namespace Application.Storage.Tables.Repositories
{
    public class CategoryRepository : Repository<Models.Category>, ICategoryRepository
    {
        public CategoryRepository(ITableClientProvider tableClientProvider) 
            : base(tableClientProvider)
        {
        }

        async Task<IDataResult<CategoryDto>> ICategoryRepository.GetById(string id)
        {
            var categoryResult = await this.GetById(id);

            if (!categoryResult.Success)
            {
                return new FailedDataResult<CategoryDto>();
            }

            return new SuccessfulDataResult<CategoryDto>(new CategoryDto(categoryResult.Value.Id, categoryResult.Value.Name));
        }

        async Task ICategoryRepository.CreateOrUpdate(CategoryDto category)
        {
            await this.CreateOrUpdate(new Models.Category(category.Id, category.Name));
        }

        public async Task Delete(string id)
        {
            await this.DeleteById(id);
        }
    }
}