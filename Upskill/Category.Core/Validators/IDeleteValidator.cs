using System.Threading.Tasks;
using Category.Core.Results;

namespace Category.Core.Validators
{
    public interface IDeleteValidator
    {
        Task<ICategoryDeleteValidationResult> CanDelete(string id);
    }
}
