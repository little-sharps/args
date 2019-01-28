using System;
using System.IO;
using System.Text;

namespace Args.Help.Formatters
{
    /// <summary>
    /// An interface that all formatters must implement
    /// </summary>
    public interface IHelpFormatter
    {
        /// <summary>
        /// When implemented, help text will be written to the provided <see cref="TextWriter"/>
        /// </summary>
        /// <param name="modelHelp"></param>
        /// <param name="writer"></param>
        void WriteHelp(ModelHelp modelHelp, TextWriter writer);
    }

    /// <summary>
    /// Extension helpers for the <see cref="IHelpFormatter"/> interface
    /// </summary>
    public static class HelpFormatterExtensions
    {
        /// <summary>
        /// A helper method for getting the formatted help text into a string
        /// </summary>
        /// <param name="source">The implemented help formatter</param>
        /// <param name="modelHelp">The data structure that contains the help information</param>
        /// <returns></returns>
        public static string GetHelp(this IHelpFormatter source, ModelHelp modelHelp)
        {
            if (source == null) throw new ArgumentNullException("source");

            var builder = new StringBuilder();

            using (var writer = new StringWriter(builder))
            {
                source.WriteHelp(modelHelp, writer);
            }
            
            return builder.ToString();
        }
    }
}
