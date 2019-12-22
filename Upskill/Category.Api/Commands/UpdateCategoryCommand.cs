using Category.Api.CustomHttpRequests;

namespace Category.Api.Commands
{
    public class UpdateCategoryCommand
    {
        public string Id { get; }
        public UpdateCategoryHttpRequest Request { get; }

        public UpdateCategoryCommand(string id, UpdateCategoryHttpRequest request)
        {
            Id = id;
            Request = request;
        }
    }
}
