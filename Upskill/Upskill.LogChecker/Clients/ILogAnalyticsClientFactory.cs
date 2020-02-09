namespace Upskill.LogChecker.Clients
{
    public interface ILogAnalyticsClientFactory
    {
        ILogAnalyticsClient GetClient();
    }
}
