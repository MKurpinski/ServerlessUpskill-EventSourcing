using Category.Core.Enums;

namespace Category.Core.Results
{
    public class SuccessfulDeleteValidationResult : ICategoryDeleteValidationResult
    {
        public bool Success => true;
        public CategoryModificationStatus Status => CategoryModificationStatus.Ok;
    }
}