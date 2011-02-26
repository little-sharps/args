
namespace Args
{
    public interface IModelBindingDefinitionInitializer
    {
        void Initialize<TModel>(IModelBindingDefinition<TModel> init);
    }
}
