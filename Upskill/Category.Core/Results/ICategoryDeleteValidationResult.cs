using Category.Core.Enums;
using Upskill.Results;

namespace Category.Core.Results
{
    public interface ICategoryDeleteValidationResult : IResult
    {
        CategoryModificationStatus Status { get; }
    }
}
