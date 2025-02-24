
using Gitpilot.Enums;
using LibGit2Sharp;
using SQLite;

namespace Gitpilot.Models
{
    public class GitRepository
    {
        [PrimaryKey, AutoIncrement] public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Path { get; set; } = "";
        [Ignore] public GitBranch CurrenBranch { get; set; } = new GitBranch();
        [Ignore] public List<GitBranch> LocalBranches { get; set; } = [];
        [Ignore] public List<GitBranch> RemoteBranches { get; set; } = [];
        [Ignore] public List<GitChange> NotStagedChanges { get; set; } = [];
        [Ignore] public List<GitChange> StagedChanges { get; set; } = [];
        [Ignore] public List<GitStash> Stashes { get; set; } = [];
        [Ignore] public Repository LibGitRepository { get; set; }
        [Ignore] public List<GitCommit> Commits { get; set; } = [];
        public string RemoteAccountUserName { get; set; } = "";
        public string RemoteAccountPassword { get; set; } = "";
        public GitPlatFormEnum GitPlatForm { get; set; } = GitPlatFormEnum.Custom;
        public string CustomPlatFormName { get; set; } = "";
    }
}
