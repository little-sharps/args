using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Args.Help.Formatters
{
    public interface IHelpFormatter
    {
        void WriteHelp(ModelHelp modelHelp, TextWriter writer);
    }

    public static class HelpFormatterExtensions
    {
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
