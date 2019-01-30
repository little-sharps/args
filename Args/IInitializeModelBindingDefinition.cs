
namespace Args
{
    /// <summary>
    /// This interface should be implemented by any class that is responsible for initializing a IModelBindingDefinition
    /// </summary>
    public interface IModelBindingDefinitionInitializer
    {
        /// <summary>
        /// When implemented, this method will initialize the provided <see cref="IModelBindingDefinition{TModel}"/>
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="init"></param>
        void Initialize<TModel>(IModelBindingDefinition<TModel> init);
    }
}
