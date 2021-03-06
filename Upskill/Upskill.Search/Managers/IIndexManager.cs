﻿using System.Threading.Tasks;
using Upskill.Results;
using Upskill.Search.Enums;
using Upskill.Search.Models;

namespace Upskill.Search.Managers
{
    public interface IIndexManager
    {
        Task<IDataResult<string>> GetIndexNameByType<T>(IndexType indexType) where T : ISearchable;
        Task OpenIndex<T>(IndexType indexType) where T : ISearchable;
        Task<IResult> IsReindexInProgress<T>() where T : ISearchable;
    }
}
