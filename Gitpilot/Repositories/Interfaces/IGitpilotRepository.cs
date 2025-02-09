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
        Task<int> SaveLastOpentGitRepository(LastSelectedGitRepository lastSelectedGitRepository);
        Task<int> SaveRepository(GitRepository repository);
    }
}
