using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Args.Help
{
    public class MemberHelp
    {
        public string Name { get; set; }
        public IEnumerable<string> Switches { get; set; }
        public string DefaultValue { get; set; }
        public string HelpText { get; set; }
        public int? OrdinalIndex { get; set; }
    }
}
