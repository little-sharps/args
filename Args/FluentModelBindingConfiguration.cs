using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Args
{
    public class FluentModelBindingConfiguration<TModel>
    {
        protected readonly IModelBindingDefinition<TModel> Source;

        public IModelBindingDefinition<TModel> Initialize()
        {
            return Source;
        }

        public FluentModelBindingConfiguration(IModelBindingDefinition<TModel> source)
        {
            Source = source;
        }

        public FluentModelBindingConfiguration<TModel> ParsesArgumentsWith(Type dataType, TypeConverter typeConverter)
        {
            Source.TypeConverters[dataType] = typeConverter;
            return this;
        }

        public FluentModelBindingConfiguration<TModel> HasFirstOrdinalArgumentOf<TResult>(Expression<Func<TModel, TResult>> member)
        {
            Source.SetOrdinalArguments(new[] { member.GetMemberInfoFromExpression() });
            return this;
        }

        public FluentModelBindingConfiguration<TModel> HasAdditionalOrdinalArgumentOf<TResult>(Expression<Func<TModel, TResult>> member)
        {
            Source.AddOrdinalArgument(member.GetMemberInfoFromExpression());
            return this;
        }

        public FluentModelBindingConfiguration<TModel> UsingSwitchDelimiter(string delimiter)
        {
            Source.SwitchDelimiter = delimiter;
            return this;
        }

        public FluentModelBindingConfiguration<TModel> UsingStringComparer(StringComparer comparer)
        {
            Source.StringComparer = comparer;
            return this;
        }

        public FluentModelBindingConfiguration<TModel> HasDescription(string description)
        {
            Source.CommandModelDescription = description;
            return this;
        }

        public FluentMemberBindingConfiguration<TModel> ForMember<TResult>(Expression<Func<TModel, TResult>> member)
        {
            var memberBindingDefinition = Source.GetOrCreateMemberBindingDefinition(member.GetMemberInfoFromExpression());

            return new FluentMemberBindingConfiguration<TModel>(memberBindingDefinition);
        }
    }

    public static class FluentModelBindingConfigurationExtensions
    {
        public static FluentModelBindingConfiguration<TModel> ParsesArgumentsWith<TDataType, TModel>(this FluentModelBindingConfiguration<TModel> fluent, TypeConverter typeConverter)
        {
            return fluent.ParsesArgumentsWith(typeof(TDataType), typeConverter);
        }

        public static FluentModelBindingConfiguration<TModel> ParsesArgumentsWith<TModel>(this FluentModelBindingConfiguration<TModel> fluent, Type dataType, IArgsTypeConverter typeConverter)
        {
            return fluent.ParsesArgumentsWith(dataType, new ArgsTypeConverter(dataType, typeConverter));
        }

        public static FluentModelBindingConfiguration<TModel> ParsesArgumentsWith<TDataType, TModel>(this FluentModelBindingConfiguration<TModel> fluent, IArgsTypeConverter typeConverter)
        {
            return fluent.ParsesArgumentsWith(typeof(TDataType), new ArgsTypeConverter(typeof(TDataType), typeConverter));
        }

        public static FluentModelBindingConfiguration<TModel> ParsesArgumentsWith<TDataType, TModel>(this FluentModelBindingConfiguration<TModel> fluent, Func<string, object> typeConverter)
        {
            return fluent.ParsesArgumentsWith(typeof(TDataType), new LambdaArgsTypeConverter(typeConverter));
        }

        public static FluentModelBindingConfiguration<TModel> ParsesArgumentsWith<TModel>(this FluentModelBindingConfiguration<TModel> fluent, Type dataType, Func<string, object> typeConverter)
        {
            return fluent.ParsesArgumentsWith(dataType, new LambdaArgsTypeConverter(typeConverter));
        }

        public static FluentModelBindingConfiguration<TModel> ParsesArgumentsWith<TDataType, TArgsTypeConverter, TModel>(this FluentModelBindingConfiguration<TModel> fluent)
            where TArgsTypeConverter : IArgsTypeConverter, new()
        {
            return fluent.ParsesArgumentsWith(typeof(TDataType), new ArgsTypeConverter(typeof(TDataType), new TArgsTypeConverter()));
        }
    }        
}
