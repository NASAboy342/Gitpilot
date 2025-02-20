namespace Gitpilot.Models
{
    public class GitCommit
    {
        public string Hash { get; set; }
        public DateTimeOffset CommitTime { get; set; }
        public string Message { get; set; }
        public string BranchName { get; set; }
        public bool IsAMergeCommit { get; set; }
        public string MergeFrom { get; set; } = "";
    }
}