using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Args
{
    public interface IArgsConfiguration
    {
        IModelBindingDefinition<TModel> Configure<TModel>();
        IModelBindingDefinition<TModel> Configure<TModel>(IModelBindingDefinitionInitializer initalizer);
    }
}
