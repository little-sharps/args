
namespace Args
{
    public class Configuration : IArgsConfiguration
    {
        private static IArgsConfiguration instance;
        public static IArgsConfiguration Instance
        {
            get
            {
                return instance = instance ?? new Configuration();
            }
        }

        private Configuration() { }


        public static IModelBindingDefinition<TModel> Configure<TModel>()
        {
            return Instance.Configure<TModel>();
        }

        public static IModelBindingDefinition<TModel> Configure<TModel>(IModelBindingDefinitionInitializer initializer)
        {
            return Instance.Configure<TModel>(initializer);
        }

        IModelBindingDefinition<TModel> IArgsConfiguration.Configure<TModel>()
        {
            return ConfigureInternal<TModel>(new ConventionBasedModelDefinitionInitializer());
        }

        IModelBindingDefinition<TModel> IArgsConfiguration.Configure<TModel>(IModelBindingDefinitionInitializer initializer)
        {
            return ConfigureInternal<TModel>(initializer);
        }

        private IModelBindingDefinition<TModel> ConfigureInternal<TModel>(IModelBindingDefinitionInitializer initializer)
        {
            var m = new ModelBindingDefinition<TModel>();
            initializer.Initialize(m);

            return m;
        }
    }
}
