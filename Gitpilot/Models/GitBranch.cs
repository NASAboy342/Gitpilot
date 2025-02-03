namespace Gitpilot.Models
{
    public class GitBranch
    {
        public GitBranch() { }
        public GitBranch(string name)
        {
            FullName = name;
        }
        public string FullName { get; set; } = "";
        public List<string> Paths => FullName.Equals("")? new List<string>() : FullName.Split('/').ToList();
        public string Name => FullName.Substring(Paths.First().Length);
        public bool IsAFolder => Paths.Count > 1;
        public string FolderName => IsAFolder ? Paths.FirstOrDefault() ?? "" : "";
        public GitBranch InnerBranch => new GitBranch(Name); 
    }
}