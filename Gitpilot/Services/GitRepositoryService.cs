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
using Gitpilot.Queues;
using IEnumerable.ForEach;
using LibGit2Sharp.Handlers;

namespace Gitpilot.Services
{
    public class GitRepositoryService : IGitRepositoryService
    {
        private readonly IGitpilotRepository _gitpilotRepository;
        private readonly GitRepositoryCache _gitRepositoryCache;
        private readonly MessageQueue _messageQueue;

        public GitRepositoryService(IGitpilotRepository gitpilotRepository, GitRepositoryCache gitRepositoryCache, MessageQueue messageQueue)
        {
            _gitpilotRepository = gitpilotRepository;
            _gitRepositoryCache = gitRepositoryCache;
            _messageQueue = messageQueue;
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
                Path = repository.Info.WorkingDirectory,
                LibGitRepository = repository,
            };
            return new OpenRepoResponse()
            {
                GitRepositoryData = gitRepository
            };
        }

        public List<GitCommit> GetCommits(Repository repository)
        {
            var allcommits = new List<GitCommit>();
            repository.Branches.Where(branch => !branch.FriendlyName.StartsWith("origin")).ForEach(branch =>
            {
                allcommits.AddRange(branch.Commits.Where(commit => commit.Committer.When > DateTime.Now.AddDays(-7)).Select(commit => new GitCommit
                {
                    Hash = commit.Sha,
                    BranchName = branch.FriendlyName,
                    Message = commit.MessageShort,
                    CommitTime = commit.Committer.When,
                    IsAMergeCommit = commit.Parents.Count() > 1,
                    MergeFrom = commit.Parents.Count() > 1 ? commit.Parents.ToList()[1].Sha : ""
                }).ToList());
            });
            allcommits = allcommits.OrderByDescending(c => c.CommitTime).GroupBy(c => c.Hash).Select(c => new GitCommit
            {
                Hash = c.Key,
                BranchName = c.Last().BranchName,
                Message = c.Last().Message,
                CommitTime = c.Last().CommitTime,
                IsAMergeCommit = c.Last().IsAMergeCommit,
                MergeFrom = c.Last().MergeFrom
            }).ToList();
            return allcommits;
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

        public async Task<SwichtToBranchResponse> SwichtToBranch(SwichtToBranchParam param)
        {
            try
            {
                var targetBranch = param.Repository.LibGitRepository.Branches[param.BranchName];

                Commands.Checkout(param.Repository.LibGitRepository, targetBranch);

                var currentRepo = param.Repository;
                currentRepo.CurrenBranch = new GitBranch(currentRepo.LibGitRepository.Head.FriendlyName);
                currentRepo.LocalBranches = GetLocalBranches(currentRepo.LibGitRepository);
                currentRepo.RemoteBranches = GetRemoteBranches(currentRepo.LibGitRepository);
                currentRepo.NotStagedChanges = GetNotStagedChanges(currentRepo.LibGitRepository);
                currentRepo.StagedChanges = GetStagedChanges(currentRepo.LibGitRepository);
                currentRepo.Stashes = GetStashes(currentRepo.LibGitRepository);

                var successResult = new SwichtToBranchResponse()
                {
                    Repository = currentRepo,
                    ErrorMessage = $"Success fully checkout to {targetBranch.FriendlyName}"
                };
                _messageQueue.Send(successResult);
                return successResult;
            }
            catch (Exception ex)
            {
                var errorResult = new SwichtToBranchResponse
                {
                    ErrorCode = 1,
                    ErrorMessage = $"{ex}",
                };
                _messageQueue.Send(errorResult);
                return errorResult;
            }
            
        }

        public void SyncRepoStatus(GitRepository gitRepository)
        {
            gitRepository.LibGitRepository.RetrieveStatus();
            gitRepository.CurrenBranch = new GitBranch(gitRepository.LibGitRepository.Head.FriendlyName);
            gitRepository.LocalBranches = GetLocalBranches(gitRepository.LibGitRepository);
            gitRepository.RemoteBranches = GetRemoteBranches(gitRepository.LibGitRepository);
            gitRepository.NotStagedChanges = GetNotStagedChanges(gitRepository.LibGitRepository);
            gitRepository.StagedChanges = GetStagedChanges(gitRepository.LibGitRepository);
            gitRepository.Stashes = GetStashes(gitRepository.LibGitRepository);
        }

        public Task<BaseResponse> FetchRepo(GitRepository gitRepository)
        {
            try
            {
                var repo = gitRepository.LibGitRepository;
                var remote = repo.Network.Remotes.First();
                var refSpecs = remote.FetchRefSpecs.Select(r => r.Specification);
                var fetchOptions = new FetchOptions
                {
                    CredentialsProvider = new CredentialsHandler((url, username, allowedTypes) =>
                        new UsernamePasswordCredentials
                        {
                            Username = "git",
                            Password = "your-github-personal-access-token"
                        })
                };
                //Commands.Fetch(repo, remote.Name, refSpecs, fetchOptions, "Fetching from remote");

                var success = new BaseResponse { ErrorCode = 0, ErrorMessage = "Fetching success" };
                _messageQueue.Send(success);
                return Task.FromResult(success);

            }
            catch(Exception ex)
            {
                var error = new BaseResponse(ex);
                _messageQueue.Send(error);
                return Task.FromResult(error);
            }
        }
    }
}
