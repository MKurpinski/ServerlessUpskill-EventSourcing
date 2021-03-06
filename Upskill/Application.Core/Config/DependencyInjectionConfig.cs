﻿using Application.Category.Events.Incoming;
using Application.Core.EventHandlers;
using Application.Core.Events;
using Application.Core.Events.CreateApplicationProcessStarted;
using Application.Core.Guards;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Upskill.Events.Extensions;
using Upskill.EventStore.Extensions;

namespace Application.Core.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddCoreModule(this IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<IApplicationQueueingEventGuard, ApplicationQueueingEventGuard>();
            builder.AddEventStore<Aggregates.Application>();
            builder.AddEventHandler<CreateApplicationProcessStartedEvent, CreateApplicationProcessStartedEventHandler>();
            builder.AddEventHandler<ApplicationCategoryNameChangedEvent, ApplicationCategoryNameChangedEventHandler>();
            builder.AddEventHandler<ApplicationCreatedEvent, ApplicationCreatedEventHandler>();

            return builder.AddEventHandler<CategoryNameChangedEvent, CategoryNameChangedEventHandler>();
        }
    }
}
