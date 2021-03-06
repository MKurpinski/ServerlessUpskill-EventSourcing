﻿using System.Threading.Tasks;
using Application.Search.Models;
using Upskill.Infrastructure;
using Upskill.Search.Enums;
using Upskill.Search.Managers;
using Upskill.Search.Providers;
using Upskill.Search.Tables.Repositories;

namespace Application.Search.Managers
{
    public class SearchableApplicationReindexManager : IndexManager, ISearchableApplicationReindexManager
    {
        public SearchableApplicationReindexManager(
            ISearchableIndexRepository searchableIndexRepository, 
            IDateTimeProvider dateTimeProvider,
            ISearchServiceClientProvider searchServiceClientProvider) : base(searchableIndexRepository, dateTimeProvider, searchServiceClientProvider)
        {
        }

        public async Task StartReindex()
        {
            await this.OpenIndex<SearchableApplication>(IndexType.InProgress);
        }

        public async Task FinishReindexing()
        {
            await this.FinishReindexing<SearchableApplication>();
        }
    }
}