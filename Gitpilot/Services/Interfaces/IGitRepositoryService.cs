using Gitpilot.Models;
using Gitpilot.Models.Parameters;
using Gitpilot.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gitpilot.Services.Interfaces
{
    public interface IGitRepositoryService
    {
        OpenRepoResponse OpenRepository(OpentRepoParam param);
        Task<GetAllOpentGitRepositoryResponse> GetAllOpentGitRepository(GetAllOpentGitRepositoryParam req);
        Task<GetLastOpentGitRepositoryResponse> GetLastOpentGitRepository();
        Task<SaveGitRepositoryResponse> SaveGitRepository(GitRepository gitRepository);
        Task<SaveLastOpentGitRepositoryResponse> SaveLastOpentGitRepository(LastSelectedGitRepository lastSelectedGitRepository);
        Task<SwichtToBranchResponse> SwichtToBranch(SwichtToBranchParam param);
    }
}
