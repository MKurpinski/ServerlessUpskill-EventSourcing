using System.Threading.Tasks;
using Category.Core.Enums;
using Category.Core.Results;
using Category.Search.Handlers;
using Category.Search.Queries;
using Category.Storage.Tables.Repositories;

namespace Category.Core.Validators
{
    public class DeleteValidator : IDeleteValidator
    {
        private readonly IUsedCategoryRepository _usedCategoryRepository;
        private readonly ICategorySearchHandler _categorySearchHandler;

        public DeleteValidator(
            IUsedCategoryRepository usedCategoryRepository,
            ICategorySearchHandler categorySearchHandler)
        {
            _usedCategoryRepository = usedCategoryRepository;
            _categorySearchHandler = categorySearchHandler;
        }

        public async Task<ICategoryDeleteValidationResult> CanDelete(string id)
        {
            var existingCategoryResult = await _categorySearchHandler.GetById(new GetCategoryByIdQuery(id));

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