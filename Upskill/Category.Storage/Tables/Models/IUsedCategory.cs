namespace Category.Storage.Tables.Models
{
    public interface IUsedCategory 
    {
        string Id { get; }
        int UsageCounter { get;}
    }
}
