using Gitpilot.Enums;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gitpilot.Models
{
    public class RemoteAccountInfo
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public GitPlatFormEnum GitPlatForm { get; set; }
        public string CustomPlatFormName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        [Ignore]
        public string PlatFormName => GitPlatForm == GitPlatFormEnum.Custom ? CustomPlatFormName : GitPlatForm.ToString();
    }
}
