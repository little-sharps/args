using System.Collections.Generic;

namespace Args.Help
{
    /// <summary>
    /// Holds help information for model members
    /// </summary>
    public class MemberHelp
    {
        /// <summary>
        /// Name of the member
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// List of switches configured for the member
        /// </summary>
        public IEnumerable<string> Switches { get; set; }
        /// <summary>
        /// The default value if no value is provided
        /// </summary>
        public string DefaultValue { get; set; }
        /// <summary>
        /// Help text for the member
        /// </summary>
        public string HelpText { get; set; }
        /// <summary>
        /// Ordinal position for the member
        /// </summary>
        public int? OrdinalIndex { get; set; }
    }
}
