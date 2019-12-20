namespace Category.Api.CustomHttpRequests
{
    public class CreateCategoryHttpRequest : IModifyCategoryHttpRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int SortOrder { get; set; }
    }
}
