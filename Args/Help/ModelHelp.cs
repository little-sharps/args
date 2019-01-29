using System.Collections.Generic;

namespace Args.Help
{
    /// <summary>
    /// Holds help information for the model
    /// </summary>
    public class ModelHelp
    {
        /// <summary>
        /// The delimited for the switches
        /// </summary>
        public string SwitchDelimiter { get; set; }
        /// <summary>
        /// List of members that are configured
        /// </summary>
        public IEnumerable<MemberHelp> Members { get; set; }
        /// <summary>
        /// Help text for the entire model
        /// </summary>
        public string HelpText { get; set; }
    }
}
