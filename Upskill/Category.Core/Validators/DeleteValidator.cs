using System.Threading.Tasks;
using Category.Storage.Tables.Repositories;
using Upskill.Results;
using Upskill.Results.Implementation;

namespace Category.Core.Validators
{
    public class DeleteValidator : IDeleteValidator
    {
        private readonly IUsedCategoryRepository _usedCategoryRepository;
        private readonly ICategoryRepository _categoryRepository;

        public DeleteValidator(
            IUsedCategoryRepository usedCategoryRepository, 
            ICategoryRepository categoryRepository)
        {
            _usedCategoryRepository = usedCategoryRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IResult> CanDelete(string id)
        {
            var existingCategoryResult = await _categoryRepository.GetById(id);

            if (!existingCategoryResult.Success)
            {
                return new FailedResult();
            }

            var categoryUsage = await _usedCategoryRepository.GetCategoryUsageById(id);

            var canDelete = categoryUsage == null || categoryUsage.UsageCounter == default;
            return canDelete ? (IResult) new SuccessfulResult() : new FailedResult();
        }
    }
}