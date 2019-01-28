using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Args
{
    /// <summary>
    /// Used for configuring an IModelBindingDefinition fluently
    /// </summary>
    /// <typeparam name="TModel">Type of the model to bind</typeparam>
    public class FluentModelBindingConfiguration<TModel>
    {
        /// <summary>
        /// 
        /// </summary>
        protected readonly IModelBindingDefinition<TModel> Source;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IModelBindingDefinition<TModel> Initialize()
        {
            return Source;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        public FluentModelBindingConfiguration(IModelBindingDefinition<TModel> source)
        {
            Source = source;
        }

        /// <summary>
        /// Configure how the provided type is parsed
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="typeConverter"></param>
        /// <returns></returns>
        public FluentModelBindingConfiguration<TModel> ParsesArgumentsWith(Type dataType, TypeConverter typeConverter)
        {
            Source.TypeConverters[dataType] = typeConverter;
            return this;
        }

        /// <summary>
        /// Sets the first ordinal argument
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="member"></param>
        /// <returns></returns>
        public FluentModelBindingConfiguration<TModel> HasFirstOrdinalArgumentOf<TResult>(Expression<Func<TModel, TResult>> member)
        {
            Source.SetOrdinalArguments(new[] { member.GetMemberInfoFromExpression() });
            return this;
        }

        /// <summary>
        /// Sets additional ordinal arguments
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="member"></param>
        /// <returns></returns>
        public FluentModelBindingConfiguration<TModel> HasAdditionalOrdinalArgumentOf<TResult>(Expression<Func<TModel, TResult>> member)
        {
            Source.AddOrdinalArgument(member.GetMemberInfoFromExpression());
            return this;
        }

        /// <summary>
        /// Set delimited used by model
        /// </summary>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public FluentModelBindingConfiguration<TModel> UsingSwitchDelimiter(string delimiter)
        {
            Source.SwitchDelimiter = delimiter;
            return this;
        }

        /// <summary>
        /// Set the <see cref="StringComparer" /> used to match switch arguments
        /// </summary>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public FluentModelBindingConfiguration<TModel> UsingStringComparer(StringComparer comparer)
        {
            Source.StringComparer = comparer;
            return this;
        }

        /// <summary>
        /// Set description for the model; is used in the output when help is requested
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public FluentModelBindingConfiguration<TModel> HasDescription(string description)
        {
            Source.CommandModelDescription = description;
            return this;
        }

        /// <summary>
        /// Start fluent configuration of the specified member
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="member"></param>
        /// <returns></returns>
        public FluentMemberBindingConfiguration<TModel> ForMember<TResult>(Expression<Func<TModel, TResult>> member)
        {
            var memberBindingDefinition = Source.GetOrCreateMemberBindingDefinition(member.GetMemberInfoFromExpression());

            return new FluentMemberBindingConfiguration<TModel>(memberBindingDefinition);
        }
    }

    public static class FluentModelBindingConfigurationExtensions
    {
        /// <summary>
        /// Generic extension to configure how arguments are parsed
        /// </summary>
        /// <typeparam name="TDataType"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="fluent"></param>
        /// <param name="typeConverter"></param>
        /// <returns></returns>
        public static FluentModelBindingConfiguration<TModel> ParsesArgumentsWith<TDataType, TModel>(this FluentModelBindingConfiguration<TModel> fluent, TypeConverter typeConverter)
        {
            return fluent.ParsesArgumentsWith(typeof(TDataType), typeConverter);
        }

        /// <summary>
        /// Generic extension to configure how arguments are parsed
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="fluent"></param>
        /// <param name="dataType"></param>
        /// <param name="typeConverter"></param>
        /// <returns></returns>
        public static FluentModelBindingConfiguration<TModel> ParsesArgumentsWith<TModel>(this FluentModelBindingConfiguration<TModel> fluent, Type dataType, IArgsTypeConverter typeConverter)
        {
            return fluent.ParsesArgumentsWith(dataType, new ArgsTypeConverter(dataType, typeConverter));
        }

        /// <summary>
        /// Generic extension to configure how arguments are parsed
        /// </summary>
        /// <typeparam name="TDataType"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="fluent"></param>
        /// <param name="typeConverter"></param>
        /// <returns></returns>
        public static FluentModelBindingConfiguration<TModel> ParsesArgumentsWith<TDataType, TModel>(this FluentModelBindingConfiguration<TModel> fluent, IArgsTypeConverter typeConverter)
        {
            return fluent.ParsesArgumentsWith(typeof(TDataType), new ArgsTypeConverter(typeof(TDataType), typeConverter));
        }

        /// <summary>
        /// Generic extension to configure how arguments are parsed
        /// </summary>
        /// <typeparam name="TDataType"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="fluent"></param>
        /// <param name="typeConverter"></param>
        /// <returns></returns>
        public static FluentModelBindingConfiguration<TModel> ParsesArgumentsWith<TDataType, TModel>(this FluentModelBindingConfiguration<TModel> fluent, Func<string, object> typeConverter)
        {
            return fluent.ParsesArgumentsWith(typeof(TDataType), new LambdaArgsTypeConverter(typeConverter));
        }

        /// <summary>
        /// Generic extension to configure how arguments are parsed
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="fluent"></param>
        /// <param name="dataType"></param>
        /// <param name="typeConverter"></param>
        /// <returns></returns>
        public static FluentModelBindingConfiguration<TModel> ParsesArgumentsWith<TModel>(this FluentModelBindingConfiguration<TModel> fluent, Type dataType, Func<string, object> typeConverter)
        {
            return fluent.ParsesArgumentsWith(dataType, new LambdaArgsTypeConverter(typeConverter));
        }

        /// <summary>
        /// Generic extension to configure how arguments are parsed
        /// </summary>
        /// <typeparam name="TDataType"></typeparam>
        /// <typeparam name="TArgsTypeConverter"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="fluent"></param>
        /// <returns></returns>
        public static FluentModelBindingConfiguration<TModel> ParsesArgumentsWith<TDataType, TArgsTypeConverter, TModel>(this FluentModelBindingConfiguration<TModel> fluent)
            where TArgsTypeConverter : IArgsTypeConverter, new()
        {
            return fluent.ParsesArgumentsWith(typeof(TDataType), new ArgsTypeConverter(typeof(TDataType), new TArgsTypeConverter()));
        }
    }        
}
