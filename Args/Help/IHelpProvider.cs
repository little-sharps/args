using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Args.Help
{
    public interface IHelpProvider
    {
        ModelHelp GenerateModelHelp<TModel>(IModelBindingDefinition<TModel> definition);
        MemberHelp GenerateMemberHelp<TModel>(IMemberBindingDefinition<TModel> definition);
    }
}