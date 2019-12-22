using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Category.Handlers
{
    public interface ICategoryNameChangedHandler
    {
        Task<IReadOnlyCollection<DataStorage.Models.Application>> HandleCategoryNameChange(string oldCategoryName, string newCategoryName);
    }
}
