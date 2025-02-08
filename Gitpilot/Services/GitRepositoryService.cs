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

namespace Gitpilot.Services
{
    public class GitRepositoryService : IGitRepositoryService
    {
        private readonly IGitpilotRepository _gitpilotRepository;

        public GitRepositoryService(IGitpilotRepository gitpilotRepository)
        {
            _gitpilotRepository = gitpilotRepository;
        }

        public OpenRepoResponse OpenRepository(OpentRepoParam param)
        {
            var repository = new Repository(param.DirectoryPath);

            var gitRepository = new GitRepository()
            {
                Name = repository.Info.WorkingDirectory.Split('\\')[^1],
                CurrenBranch = new GitBranch(repository.Head.FriendlyName),
                LocalBranches = GetLocalBranches(repository),
                RemoteBranches = GetRemoteBranches(repository),
                NotStagedChanges = GetNotStagedChanges(repository),
                StagedChanges = GetStagedChanges(repository),
                Stashes = GetStashes(repository)
            };
            return new OpenRepoResponse()
            {
                Data = gitRepository
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

        public async Task<GetAllOpentGitRepositoryResponse> GetAllOpentGitRepository()
        {
            var gitRepositories = await _gitpilotRepository.GetAllOpenedGitRepositories();
            return new GetAllOpentGitRepositoryResponse()
            {
                Repositories = gitRepositories
            };
        }

        public async Task<GetLastOpentGitRepositoryResponse> GetLastOpentGitRepository()
        {
            var gitRepository = await _gitpilotRepository.GetLastSelectedGitRepository();
            return new GetLastOpentGitRepositoryResponse()
            {
                Repository = gitRepository
            };
        }
    }
}
