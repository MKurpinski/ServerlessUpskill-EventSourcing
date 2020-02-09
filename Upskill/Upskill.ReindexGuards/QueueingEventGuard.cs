using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upskill.Events;
using Upskill.EventStore.Models;
using Upskill.ReindexGuards.Extensions;
using Upskill.Results;
using Upskill.Results.Implementation;
using Upskill.Search.Managers;
using Upskill.Search.Models;

namespace Upskill.ReindexGuards
{
    public abstract class QueueingEventGuard<TSearchable, TAggregate> : IQueueingEventGuard<TSearchable, TAggregate> 
        where TSearchable : ISearchable where TAggregate : IAggregateRoot
    {
        private static readonly Lazy<IReadOnlyCollection<Type>> _lazyImplementedInterfaces;
        private static readonly Lazy<Type> _lazyTypeOfBuildBy;
        private readonly IIndexManager _indexManager;

        static QueueingEventGuard()
        {
            _lazyImplementedInterfaces = new Lazy<IReadOnlyCollection<Type>>(() => 
                typeof(TAggregate).GetImplementedInterfaces()
            );

            _lazyTypeOfBuildBy = new Lazy<Type>(() => typeof(IBuildBy<>));
        }

        protected QueueingEventGuard(IIndexManager indexManager)
        {
            _indexManager = indexManager;
        }

        public async Task<IDataResult<string>> ShouldQueueEvent(IEvent @event)
        {
            if (!(@event is IAggregateEvent aggregateEvent))
            {
                return new FailedDataResult<string>();
            }

            var eventInterface = _lazyTypeOfBuildBy.Value.MakeGenericType(@event.GetType());

            if (!_lazyImplementedInterfaces.Value.Contains(eventInterface))
            {
                return new FailedDataResult<string>();
            }

            var isReindexInProgressResult = await _indexManager.IsReindexInProgress<TSearchable>();
            if (!isReindexInProgressResult.Success)
            {
                return new FailedDataResult<string>();
            }

            return new SuccessfulDataResult<string>(aggregateEvent.Id);
        }
    }
}