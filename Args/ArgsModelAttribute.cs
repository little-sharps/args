using System;

namespace Args
{
    /// <summary>
    /// Decorate a model with this attribute to specifiy its switch delimiter as well as the StringComparison type to use when checking for switch equality
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ArgsModelAttribute : Attribute
    {
        /// <summary>
        /// Attribute with default settings applied
        /// </summary>
        public static ArgsModelAttribute Default
        {
            get { return new ArgsModelAttribute(); }
        }

        /// <summary>
        /// Initialize instance with default settings applied
        /// </summary>
        public ArgsModelAttribute()
        {
            SwitchDelimiter = "/";
            StringComparison = StringComparison.CurrentCultureIgnoreCase;
        }

        /// <summary>
        /// The string used to identify a switch argument
        /// </summary>
        public string SwitchDelimiter { get; set; }
        /// <summary>
        /// Determines the string comparer used to match switch values
        /// </summary>
        public StringComparison StringComparison { get; set; }

        /// <summary>
        /// Gets the <see cref="System.StringComparer"/> instance based on the value in <see cref="StringComparison"/>
        /// </summary>
        public StringComparer StringComparer
        {
            get
            {
                switch (StringComparison)
                {
                    case StringComparison.CurrentCulture:
                        return StringComparer.CurrentCulture;
                    case StringComparison.CurrentCultureIgnoreCase:
                        return StringComparer.CurrentCultureIgnoreCase;
#if !NETSTANDARD_1_3
                    case StringComparison.InvariantCulture:
                        return StringComparer.InvariantCulture;
                    case StringComparison.InvariantCultureIgnoreCase:
                        return StringComparer.InvariantCultureIgnoreCase;
#endif
                    case StringComparison.Ordinal:
                        return StringComparer.Ordinal;
                    case StringComparison.OrdinalIgnoreCase:
                        return StringComparer.OrdinalIgnoreCase;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(StringComparison));
                }
            }
        }
    }
}
