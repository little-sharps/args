namespace Args
{
    /// <summary>
    /// An interface that will configure an IModelBindingDefinition based on a given initializer
    /// </summary>
    public interface IArgsConfiguration
    {
        /// <summary>
        /// When implemented, this method will configure the model binder
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        IModelBindingDefinition<TModel> Configure<TModel>();
        /// <summary>
        /// When implemented, this method will configure the model binder with the provided initializer
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="initalizer"></param>
        /// <returns></returns>
        IModelBindingDefinition<TModel> Configure<TModel>(IModelBindingDefinitionInitializer initalizer);
    }
}
