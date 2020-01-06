using System.Threading.Tasks;
using Application.Category.Events.Incoming;
using Application.Storage.Tables.Repositories;
using Upskill.Events;

namespace Application.Category.EventHandlers
{
    public class CategoryDeletedEventHandler : IEventHandler<CategoryDeletedEvent>
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryDeletedEventHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task Handle(CategoryDeletedEvent categoryDeletedEvent)
        {
            await _categoryRepository.Delete(categoryDeletedEvent.Id);
        }
    }
}
