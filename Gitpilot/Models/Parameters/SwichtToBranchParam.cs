using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gitpilot.Models.Parameters
{
    public class SwichtToBranchParam
    {
        public GitRepository Repository { get; set; }
        public string BranchName { get; set; }
    }
}
