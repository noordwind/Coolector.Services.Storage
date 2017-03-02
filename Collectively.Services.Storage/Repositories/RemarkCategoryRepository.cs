﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Storage.Queries;
using Collectively.Services.Storage.Repositories.Queries;
using MongoDB.Driver;
using Collectively.Common.Mongo;
using Collectively.Services.Storage.Dto.Remarks;

namespace Collectively.Services.Storage.Repositories
{
    public class RemarkCategoryRepository : IRemarkCategoryRepository
    {
        private readonly IMongoDatabase _database;

        public RemarkCategoryRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<Maybe<RemarkCategoryDto>> GetByIdAsync(Guid id)
            => await _database.RemarkCategories().GetByIdAsync(id);

        public async Task<Maybe<PagedResult<RemarkCategoryDto>>> BrowseAsync(BrowseRemarkCategories query)
            => await _database.RemarkCategories()
                .Query(query)
                .PaginateAsync();

        public async Task AddManyAsync(IEnumerable<RemarkCategoryDto> categories)
            => await _database.RemarkCategories().InsertManyAsync(categories);
    }
}