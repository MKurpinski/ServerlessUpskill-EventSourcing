using System.Threading.Tasks;
using Upskill.Results;

namespace Category.Core.Validators
{
    public interface IDeleteValidator
    {
        Task<IResult> CanDelete(string id);
    }
}
