using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gitpilot.Models.ViewModels
{
    public class GitBranchViewModel
    {
        public GitBranchViewModel()
        {
            RgbColor = GenerateColor();
        }
        public string BranchName { get; set; }
        public string RgbColor { get; set; } = "rgb(0, 120, 150)";

        public string GenerateColor()
        {
            var r = new Random().Next(0, 170);
            var g = new Random().Next(0, 170);
            var b = new Random().Next(0, 170);

            while ((r + g + b) < 150)
            {
                r = new Random().Next(0, 170);
                g = new Random().Next(0, 170);
                b = new Random().Next(0, 170);
            }

            return $"rgb({r}, {g}, {b})";
        }
    }
}
