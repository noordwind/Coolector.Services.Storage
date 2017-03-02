﻿using System;
using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Storage.Queries;


namespace Collectively.Services.Storage.Providers.Users
{
    public interface IUserProvider
    {
        Task<Maybe<AvailableResourceDto>> IsAvailableAsync(string name);
        Task<Maybe<PagedResult<UserDto>>> BrowseAsync(BrowseUsers query);
        Task<Maybe<UserDto>> GetAsync(string userId);
        Task<Maybe<UserDto>> GetByNameAsync(string name);
        Task<Maybe<UserSessionDto>> GetSessionAsync(Guid id);
    }
}