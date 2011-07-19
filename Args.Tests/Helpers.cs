using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using Args;
using Args.Help;

namespace Args.Tests
{
    internal static class Helpers
    {
        internal static IMemberBindingDefinition<T> GetMemberBindingDefinitionFor<T,TReturn>(this IEnumerable<IMemberBindingDefinition<T>> source, Expression<Func<T,TReturn>> expression)
        {
            return source.Single(m =>
                    m.MemberInfo.Name == expression.GetMemberInfoFromExpression().Name
                );
        }
    }

    public class SimpleResourceMemberHelpAttribute : ResourceMemberHelpAttributeBase
    {
        private string helpText;

        public SimpleResourceMemberHelpAttribute(string description)
        {
            helpText = description;
        }

        public override string GetHelpText()
        {
            return helpText;
        }
    }

    public class SimpleConverter : IArgsTypeConverter
    {

        public object Convert(string value)
        {
            return value.Length;
        }
    }
}
