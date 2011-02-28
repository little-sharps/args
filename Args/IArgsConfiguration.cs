using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Args
{
    /// <summary>
    /// An interface that will configure an IModelBindingDefinition based on a given initializer
    /// </summary>
    public interface IArgsConfiguration
    {
        IModelBindingDefinition<TModel> Configure<TModel>();
        IModelBindingDefinition<TModel> Configure<TModel>(IModelBindingDefinitionInitializer initalizer);
    }
}
