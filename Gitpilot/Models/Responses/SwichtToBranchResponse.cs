using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gitpilot.Models.Responses
{
    public class SwichtToBranchResponse: BaseResponse
    {
        public GitRepository Repository { get; set; }
    }
}
