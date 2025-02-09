using Gitpilot.Models;
using Gitpilot.Models.Parameters;
using Gitpilot.Models.Responses;
using Gitpilot.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibGit2Sharp;
using Gitpilot.Repositories.Interfaces;
using Gitpilot.Caches;
using Gitpilot.Enums;

namespace Gitpilot.Services
{
    public class GitRepositoryService : IGitRepositoryService
    {
        private readonly IGitpilotRepository _gitpilotRepository;
        private readonly GitRepositoryCache _gitRepositoryCache;

        public GitRepositoryService(IGitpilotRepository gitpilotRepository, GitRepositoryCache gitRepositoryCache)
        {
            _gitpilotRepository = gitpilotRepository;
            _gitRepositoryCache = gitRepositoryCache;
        }

        public OpenRepoResponse OpenRepository(OpentRepoParam param)
        {
            var repository = new Repository(param.DirectoryPath);

            var gitRepository = new GitRepository()
            {
                Name = repository.Info.WorkingDirectory.Split("\\", StringSplitOptions.RemoveEmptyEntries).Last(),
                CurrenBranch = new GitBranch(repository.Head.FriendlyName),
                LocalBranches = GetLocalBranches(repository),
                RemoteBranches = GetRemoteBranches(repository),
                NotStagedChanges = GetNotStagedChanges(repository),
                StagedChanges = GetStagedChanges(repository),
                Stashes = GetStashes(repository),
                Path = repository.Info.WorkingDirectory
            };
            return new OpenRepoResponse()
            {
                GitRepositoryData = gitRepository
            };
        }

        private List<GitStash> GetStashes(Repository repository)
        {
            return new List<GitStash>();
        }

        private List<GitChange> GetStagedChanges(Repository repository)
        {
            return new List<GitChange>();
        }

        private List<GitChange> GetNotStagedChanges(Repository repository)
        {
            return new List<GitChange>();
        }

        private List<GitBranch> GetRemoteBranches(Repository repository)
        {
            var gitBranches = repository.Branches.Where(branch => branch.IsRemote).Select(branch => new GitBranch(branch.FriendlyName));
            return gitBranches.ToList();
        }

        private List<GitBranch> GetLocalBranches(Repository repository)
        {
            var gitBranches = repository.Branches.Where(branch => !branch.IsRemote).Select(branch => new GitBranch(branch.FriendlyName));
            return gitBranches.ToList();
        }

        public async Task<GetAllOpentGitRepositoryResponse> GetAllOpentGitRepository(GetAllOpentGitRepositoryParam req)
        {
            var gitRepositories = await _gitRepositoryCache.GetAllAsync(req.IsForceReloadCache);
            return new GetAllOpentGitRepositoryResponse()
            {
                Repositories = gitRepositories
            };
        }

        public async Task<GetLastOpentGitRepositoryResponse> GetLastOpentGitRepository()
        {
            var lastSelectedGitRepository = await _gitpilotRepository.GetLastSelectedGitRepository();
            var allOpenedGitRepositories = await _gitRepositoryCache.GetAllAsync();
            if(allOpenedGitRepositories.Count == 0)
            {
                return new GetLastOpentGitRepositoryResponse
                {
                    ErrorCode = (int)ErrorEnum.NoRepositories,
                    ErrorMessage = ErrorEnum.NoRepositories.ToString()
                };
            }

            var gitRepository = OpenRepository(new OpentRepoParam() 
            { 
                DirectoryPath = allOpenedGitRepositories.FirstOrDefault(ogr => ogr.Id == lastSelectedGitRepository.GitRepositoryId)?.Path ?? ""
            }).GitRepositoryData;

            return new GetLastOpentGitRepositoryResponse()
            {
                Repository = gitRepository
            };
        }

        public async Task<SaveGitRepositoryResponse> SaveGitRepository(GitRepository gitRepository)
        {
            var insertedId = await _gitpilotRepository.SaveRepository(gitRepository);
            return new SaveGitRepositoryResponse()
            {
                Id = insertedId
            };
        }

        public async Task<SaveLastOpentGitRepositoryResponse> SaveLastOpentGitRepository(LastSelectedGitRepository lastSelectedGitRepository)
        {
            var insertedId = await _gitpilotRepository.SaveLastOpentGitRepository(lastSelectedGitRepository);
            return new SaveLastOpentGitRepositoryResponse()
            {
                Id = insertedId
            };
        }
    }
}
