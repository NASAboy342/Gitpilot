using Gitpilot.Models;
using Gitpilot.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Gitpilot.Caches
{
    public class RemoteAccountInfoCache : CacheBase<List<RemoteAccountInfo>>
    {
        private readonly string _key = "Gitpilot_RemoteAccountInfoCache";
        private readonly IGitpilotRepository _gitpilotRepository;

        public RemoteAccountInfoCache(IGitpilotRepository gitpilotRepository)
        {
            _gitpilotRepository = gitpilotRepository;
        }
        protected override CacheItemPolicy GetItemPolicy()
        {
            return new CacheItemPolicy()
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddDays(1)
            };
        }

        protected override async Task<List<RemoteAccountInfo>> ReloadFromDb(string key)
        {
            return await _gitpilotRepository.GetRemoteAccountInfos();
        }

        public async Task<List<RemoteAccountInfo>> GetAllAsyns(bool isReload = false)
        {
            if (isReload)
                await ForceReload();
            return await GetAsync(_key);
        }

        public async Task<BaseResponse> ForceReload()
        {
            try
            {
                var dataFormDb = await ReloadFromDb(_key);
                AddOrUpdate(_key, dataFormDb);
                return new BaseResponse();
            }
            catch (Exception ex)
            {
                return new BaseResponse(ex);
            }
        }
    }
}
