using System.Threading.Tasks;

namespace Application.Core.Handlers
{
    public interface ICategoryNameChangedHandler
    {
        Task HandleCategoryNameChange(string oldCategoryName, string newCategoryName);
    }
}
