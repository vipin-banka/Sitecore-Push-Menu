using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.Modules.PushMenu.Models
{
    public class MenuDetail
    {
        public MenuItem MenuItem { get; set; }
        public bool HasChild { get; set; }
        public bool IsHome { get; set; }
        public bool IsImmediateChildofHome { get; set; }
        public int Level { get; set; }
        public int Index { get; set; }
        public bool IsCurrentPage { get; set; }

        public MenuDetail Parent { get; set; }
    }
}
