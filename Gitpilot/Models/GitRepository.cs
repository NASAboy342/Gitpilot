
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
    }
}
