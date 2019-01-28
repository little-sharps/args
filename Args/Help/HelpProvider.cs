using System;
using System.Linq;
using System.Reflection;

namespace Args.Help
{
    /// <summary>
    /// The default implementation for IHelpProvider
    /// </summary>
    public class HelpProvider : IHelpProvider
    {
        public ModelHelp GenerateModelHelp<TModel>(IModelBindingDefinition<TModel> definition)
        {
            var modelHelp = new ModelHelp
            {
                SwitchDelimiter = definition.SwitchDelimiter,
                Members = definition.Members.Select(GenerateMemberHelp).ToArray(),
                HelpText = definition.CommandModelDescription,
            };

            if (String.IsNullOrEmpty(modelHelp.HelpText))
            {
                var helpAttribute = typeof(TModel)
#if !NET_FRAMEWORK
                .GetTypeInfo()
#endif
                    .GetCustomAttributes(true).OfType<ResourceMemberHelpAttributeBase>().SingleOrDefault();

                if (helpAttribute != null) modelHelp.HelpText = helpAttribute.GetHelpText();
            }

            return modelHelp;
        }

        public MemberHelp GenerateMemberHelp<TModel>(IMemberBindingDefinition<TModel> definition)
        {
            var memberHelp = new MemberHelp
            {
                DefaultValue = String.Format("{0}", definition.DefaultValue),
                Name = definition.MemberInfo.Name,
                OrdinalIndex = definition.Parent.OrdinalIndexOf(definition.MemberInfo),
                Switches = definition.SwitchValues,
                HelpText = definition.HelpText,
            };

            if (String.IsNullOrEmpty(memberHelp.HelpText))
            {
                var helpAttribute = definition.MemberInfo.GetCustomAttributes(true).OfType<ResourceMemberHelpAttributeBase>().SingleOrDefault();

                if (helpAttribute != null) memberHelp.HelpText = helpAttribute.GetHelpText();
            }

            return memberHelp;
        }
    }
}
