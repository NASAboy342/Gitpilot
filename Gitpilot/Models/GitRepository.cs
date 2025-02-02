using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gitpilot.Models
{
    public class GitRepository
    {
        public string Name { get; set; } = "";
        public GitBranch CurrenBranch { get; set; } = new GitBranch();
        public List<GitBranch> LocalBranches { get; set; } = [];
        public List<GitBranch> RemoteBranches { get; set; } = [];
        public List<GitChange> NotStagedChanges { get; set; } = [];
        public List<GitChange> StagedChanges { get; set; } = [];
        public List<GitStash> Stashes { get; set; } = [];
    }
}
