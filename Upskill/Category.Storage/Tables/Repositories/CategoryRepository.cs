using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Category.Storage.Tables.Dtos;
using Upskill.Results;
using Upskill.Results.Implementation;
using Upskill.Storage.Table.Providers;
using Upskill.Storage.Table.Repositories;

namespace Category.Storage.Tables.Repositories
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

            return new SuccessfulDataResult<CategoryDto>(this.BuildCategoryDto(categoryResult.Value));
        }

        async Task<IResult> ICategoryRepository.CreateOrUpdate(CategoryDto category)
        {
            var result = await this.CreateOrUpdate(
                new Models.Category(
                    category.Id,
                    category.Name,
                    category.Description,
                    category.SortOrder));

            return result;
        }

        public async Task<IDataResult<CategoryDto>> GetByName(string name)
        {
            var getResult = await this.GetByField(nameof(Models.Category.Name), name);

            var category = getResult.FirstOrDefault();

            if (category == null)
            {
                return new FailedDataResult<CategoryDto>();
            }

            return new SuccessfulDataResult<CategoryDto>(this.BuildCategoryDto(category));
        }

        public async Task<IReadOnlyCollection<CategoryDto>> GetAll()
        {
            var getResult = await GetByField(nameof(Models.Category.PartitionKey), nameof(Models.Category));

            return getResult.Select(this.BuildCategoryDto).ToList();
        }

        async Task<IResult> ICategoryRepository.Delete(string id)
        {
            var result = await this.DeleteById(id);
            return result;
        }

        private CategoryDto BuildCategoryDto(Models.Category category)
        {
            return new CategoryDto(
                category.Id,
                category.Name,
                category.Description,
                category.SortOrder);
        }
    }
}