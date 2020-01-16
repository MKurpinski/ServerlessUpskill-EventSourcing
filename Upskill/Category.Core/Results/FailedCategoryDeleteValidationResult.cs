using Category.Core.Enums;

namespace Category.Core.Results
{
    public class FailedCategoryDeleteValidationResult : ICategoryDeleteValidationResult
    {
        public FailedCategoryDeleteValidationResult(CategoryModificationStatus status)
        {
            Status = status;
        }

        public bool Success => false;
        public CategoryModificationStatus Status { get; }
    }
}