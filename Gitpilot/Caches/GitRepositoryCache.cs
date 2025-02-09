using Gitpilot.Enums;
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
    public class GitRepositoryCache : CacheBase<List<GitRepository>>
    {
        private readonly string _key = "Gitpilot_GitRepositoryCache";
        private readonly IGitpilotRepository _gitpilotRepository;

        public GitRepositoryCache(IGitpilotRepository gitpilotRepository)
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

        protected override async Task<List<GitRepository>> ReloadFromDb(string key)
        {
            var gitRepositories = await _gitpilotRepository.GetAllOpenedGitRepositories();
            return gitRepositories;
        }

        public async Task<List<GitRepository>> GetAllAsync(bool isForcReload = false)
        {
            if (isForcReload)
            {
                await ForceReload();
            }
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