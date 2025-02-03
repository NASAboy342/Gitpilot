
namespace Gitpilot.Models
{
    public class GitRepository
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public GitBranch CurrenBranch { get; set; } = new GitBranch();
        public List<GitBranch> LocalBranches { get; set; } = [];
        public List<GitBranch> RemoteBranches { get; set; } = [];
        public List<GitChange> NotStagedChanges { get; set; } = [];
        public List<GitChange> StagedChanges { get; set; } = [];
        public List<GitStash> Stashes { get; set; } = [];
    }
}
