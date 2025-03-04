using Gitpilot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gitpilot.Repositories.Interfaces
{
    public interface IGitpilotRepository
    {
        Task<List<GitRepository>> GetAllOpenedGitRepositories();
        Task<LastSelectedGitRepository> GetLastSelectedGitRepository();
        Task<List<RemoteAccountInfo>> GetRemoteAccountInfos();
        Task<int> SaveLastOpentGitRepository(LastSelectedGitRepository lastSelectedGitRepository);
        Task SaveRemoteAccountInfo(RemoteAccountInfo remoteAccountInfo);
        Task<int> SaveRepository(GitRepository repository);
    }
}
