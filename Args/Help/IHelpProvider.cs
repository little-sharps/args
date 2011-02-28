using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Args.Help
{
    /// <summary>
    /// Any class that generates help from a binding definition should implement this interface
    /// </summary>
    public interface IHelpProvider
    {
        ModelHelp GenerateModelHelp<TModel>(IModelBindingDefinition<TModel> definition);
        MemberHelp GenerateMemberHelp<TModel>(IMemberBindingDefinition<TModel> definition);
    }
}