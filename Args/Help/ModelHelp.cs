using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Args.Help
{
    public class ModelHelp
    {
        public string SwitchDelimiter { get; set; }
        public IEnumerable<MemberHelp> Members { get; set; }
        public string HelpText { get; set; }
    }
}
