
namespace Args
{
    /// <summary>
    /// This interface should be implemented by any class that is responsible for initializing a IModelBindingDefinition
    /// </summary>
    public interface IModelBindingDefinitionInitializer
    {
        void Initialize<TModel>(IModelBindingDefinition<TModel> init);
    }
}
