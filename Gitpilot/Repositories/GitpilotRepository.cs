using Gitpilot.Models;
using Gitpilot.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gitpilot.Repositories
{
    public class GitpilotRepository : BaseRepository, IGitpilotRepository
    {
        private static readonly string _dataBasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.Create), "Gitpilot.db3");

        public GitpilotRepository() : base(_dataBasePath)
        {
            
        }

        public async Task<List<GitRepository>> GetAllOpenedGitRepositories()
        {
            return await GetItemsAsync<GitRepository>();
        }

        public async Task<LastSelectedGitRepository> GetLastSelectedGitRepository()
        {
            var gits = await GetItemsAsync<LastSelectedGitRepository>();
            return gits.OrderByDescending(g => g.Id).FirstOrDefault() ?? new LastSelectedGitRepository();
        }

        public async Task<List<RemoteAccountInfo>> GetRemoteAccountInfos()
        {
            return await GetItemsAsync<RemoteAccountInfo>();
        }

        public override async Task InitializeTablesAsync()
        {
            await CreateTableAsync<GitRepository>();
            await CreateTableAsync<LastSelectedGitRepository>();
            await CreateTableAsync<RemoteAccountInfo>();
        }

        public async Task<int> SaveLastOpentGitRepository(LastSelectedGitRepository lastSelectedGitRepository)
        {
            return await SaveItemAsync(lastSelectedGitRepository);
        }

        public async Task SaveRemoteAccountInfo(RemoteAccountInfo remoteAccountInfo)
        {
            await SaveItemAsync(remoteAccountInfo);
        }

        public async Task<int> SaveRepository(GitRepository repository)
        {
            return await SaveItemAsync(repository); 
        }
    }
}
