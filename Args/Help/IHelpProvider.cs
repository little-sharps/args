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
        /// <summary>
        /// Generates help for the provided model definition
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="definition"></param>
        /// <returns></returns>
        ModelHelp GenerateModelHelp<TModel>(IModelBindingDefinition<TModel> definition);
        /// <summary>
        /// Generates help for the provided member definition
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="definition"></param>
        /// <returns></returns>
        MemberHelp GenerateMemberHelp<TModel>(IMemberBindingDefinition<TModel> definition);
    }
}