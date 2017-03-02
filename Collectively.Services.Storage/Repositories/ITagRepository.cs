using System.Collections.Generic;
using System.Threading.Tasks;
using Collectively.Common.ServiceClients.Queries;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Remarks;

namespace Collectively.Services.Storage.Repositories
{
    public interface ITagRepository
    {
        Task<Maybe<Tag>> GetAsync(string name);
        Task<Maybe<PagedResult<Tag>>> BrowseAsync(BrowseRemarkTags query);
        Task AddManyAsync(IEnumerable<Tag> tags);         
    }
}