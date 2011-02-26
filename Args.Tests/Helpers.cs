using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using Args.Help;

namespace Args.Tests
{
    internal static class Helpers
    {
        internal static MemberInfo GetMemberInfoFromExpression<T,TReturn>(this Expression<Func<T, TReturn>> expression)
        {
            var member = expression.Body as MemberExpression;
            if (member == null) throw new InvalidOperationException("Only member access expressions may be used.");
            return member.Member;
        }

        internal static IMemberBindingDefinition<T> GetMemberBindingDefinitionFor<T,TReturn>(this IEnumerable<IMemberBindingDefinition<T>> source, Expression<Func<T,TReturn>> expression)
        {
            return source.Single(m => m.MemberInfo == expression.GetMemberInfoFromExpression());
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
}
