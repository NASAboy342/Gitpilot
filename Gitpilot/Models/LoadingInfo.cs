using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gitpilot.Models
{
    public class LoadingInfo
    {
        public LoadingInfo() { }
        public LoadingInfo(string hash)
        {
            LoadingHash = hash;
            IsShouldBlock = false;
            CreatedTime = DateTime.Now;
        }
        public LoadingInfo(string hash, string message)
        {
            LoadingHash = hash;
            IsShouldBlock = false;
            CreatedTime = DateTime.Now;
            Message = message;
        }
        public LoadingInfo(string hash, bool shouldBlock)
        {
            LoadingHash = hash;
            IsShouldBlock = shouldBlock;
            CreatedTime = DateTime.Now;
        }
        public string LoadingHash { get; set; }
        public bool IsShouldBlock { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public string Message { get; set; } = "";
    }
}
