using System.Threading.Tasks;
using Microsoft.Azure.EventGrid.Models;

namespace Upskill.EventPublisher.Client
{
    public interface IEventGridClientFacade
    {
        Task PublishEvent(TopicCredentials credentials, string endpoint, EventGridEvent eventGridEvent);
    }
}
