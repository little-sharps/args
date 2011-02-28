
namespace Args
{
    /// <summary>
    /// A static wraper for the default implementation of IArgsConfiguration.  Singleton instance only
    /// </summary>
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


        /// <summary>
        /// Create an IModelBindingDefinition. Will use the default ConventionBasedModelDefinitionInitializer
        /// </summary>
        /// <typeparam name="TModel">The type that will be bound to</typeparam>
        /// <returns></returns>
        public static IModelBindingDefinition<TModel> Configure<TModel>()
        {
            return Instance.Configure<TModel>();
        }

        /// <summary>
        /// Create an IModelBindingDefinition.  Will use the provided IModelBindingDefinitionInitializer
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="initializer"></param>
        /// <returns></returns>
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
