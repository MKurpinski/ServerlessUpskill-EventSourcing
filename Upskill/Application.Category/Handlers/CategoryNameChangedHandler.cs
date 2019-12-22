using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataStorage.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.Category.Handlers
{
    public class CategoryNameChangedHandler : ICategoryNameChangedHandler
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly ILogger<CategoryNameChangedHandler> _logger;

        public CategoryNameChangedHandler(
            IApplicationRepository applicationRepository,
            ILogger<CategoryNameChangedHandler> logger)
        {
            _applicationRepository = applicationRepository;
            _logger = logger;
        }

        public async Task<IReadOnlyCollection<DataStorage.Models.Application>> HandleCategoryNameChange(string oldCategoryName, string newCategoryName)
        {
            var oldCategoryApplications = await _applicationRepository.GetByCategoryName(oldCategoryName);

            if (!oldCategoryApplications.Success)
            {
                // push the event back and try to consume later
                _logger.LogError($"Failed applications update for category old name: {oldCategoryName} to new categoryName: {newCategoryName}");
                return Enumerable.Empty<DataStorage.Models.Application>().ToList();
            }

            var applicationsToUpdate = oldCategoryApplications.Value.ToList();

            foreach (var applicationToUpdate in applicationsToUpdate)
            {
                applicationToUpdate.ChangeCategory(newCategoryName);
            }

            var result = await _applicationRepository.BulkUpdateDocuments(applicationsToUpdate);

            if (!result.Success)
            {
                //try to retry the update
                _logger.LogError($"Failed applications update for category old name: {oldCategoryName} to new categoryName: {newCategoryName}" +
                                 $"for applications: {string.Join(",", result.FailedUpdatedDocuments.Select(x => x.Id))}");
            }

            return result.SuccessfulUpdatedDocuments;
        }
    }
}