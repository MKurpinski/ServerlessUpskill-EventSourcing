namespace Category.Api.CustomHttpRequests
{
    public interface IModifyCategoryHttpRequest
    {
        string Name { get; set; }
        string Description { get; set; }
        int SortOrder { get; set; }
    }
}
