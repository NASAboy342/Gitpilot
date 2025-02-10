namespace Gitpilot.Models
{
    public class GitBranch
    {
        public GitBranch() { }
        public GitBranch(string name)
        {
            var isOrigin = name.StartsWith("origin/",StringComparison.OrdinalIgnoreCase);
            IsOrigin = isOrigin;
            FullName = isOrigin ? name.Substring(7) : name;
        }
        public string FullName { get; set; } = "";
        public List<string> Paths => FullName.Equals("")? new List<string>() : FullName.Split('/').ToList();
        public string Name => GetName();
        public bool IsAFolder => Paths.Count > 1;
        public string FolderName => IsAFolder ? Paths.FirstOrDefault() ?? "" : "";
        public bool IsOrigin { get; set; }

        private string GetName()
        {
            if (FullName.Equals(""))
            {
                return "";
            }
            if (Paths.Count < 2)
            {
                return FullName;
            }

            return FullName.Substring(Paths.First().Length + 1);
        }
    }
}