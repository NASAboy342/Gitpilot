using Gitpilot.Enums;
using Gitpilot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Gitpilot.Caches
{
    public class GitRepositoryCache : CacheBase<List<GitRepository>>
    {
        private readonly string _key = "Gitpilot_GitRepositoryCache";

        protected override CacheItemPolicy GetItemPolicy()
        {
            return new CacheItemPolicy()
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddDays(1)
            };
        }

        protected override Task<List<GitRepository>> ReloadFromDb(string key)
        {
            throw new NotImplementedException();
        }

        public async Task<List<GitRepository>> GetAllAsync()
        {
            return await GetAsync(_key);
        }

        public async Task<BaseResponse> ForceReload()
        {
            ClearAll();
            var reloadResult = await GetAsync(_key);
            if (reloadResult == null || reloadResult.Count == 0)
            {
                return new BaseResponse(ErrorEnum.CacheReloadFailed);
            }
            return new BaseResponse();
        }
    }
}