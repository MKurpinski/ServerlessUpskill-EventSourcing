using System.Threading.Tasks;
using Category.Core.Enums;
using Category.Core.Results;
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

        public async Task<ICategoryDeleteValidationResult> CanDelete(string id)
        {
            var existingCategoryResult = await _categoryRepository.GetById(id);

            if (!existingCategoryResult.Success)
            {
                return new FailedCategoryDeleteValidationResult(CategoryModificationStatus.NotFound);
            }

            var categoryUsage = await _usedCategoryRepository.GetCategoryUsageById(id);

            var canDelete = categoryUsage == null || categoryUsage.UsageCounter == default;
            return canDelete ? 
                (ICategoryDeleteValidationResult) new SuccessfulDeleteValidationResult() 
                :
                (ICategoryDeleteValidationResult) new FailedCategoryDeleteValidationResult(CategoryModificationStatus.Used);
        }
    }
}