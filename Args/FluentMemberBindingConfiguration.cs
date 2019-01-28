using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Args
{
    /// <summary>
    /// Used for configuring an IMemberBindingDefinition fluently
    /// </summary>
    /// <typeparam name="TModel">Type of the model to bind</typeparam>
    public class FluentMemberBindingConfiguration<TModel>
    {
        /// <summary>
        /// 
        /// </summary>
        protected readonly IMemberBindingDefinition<TModel> Source;

        /// <summary>
        /// The type of the member being configured
        /// </summary>
        public Type MemberType
        {
            get { return Source.MemberInfo.GetDeclaredType(); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        public FluentMemberBindingConfiguration(IMemberBindingDefinition<TModel> source)
        {
            Source = source;
        }

        /// <summary>
        /// Parse current member with provided <see cref="TypeConverter"/>
        /// </summary>
        /// <param name="typeConverter"></param>
        /// <returns></returns>
        public FluentMemberBindingConfiguration<TModel> ParsesArgumentWith(TypeConverter typeConverter)
        {
            Source.TypeConverter = typeConverter;
            return this;
        }

        /// <summary>
        /// Configure switch value(s) that will be bound to the current member
        /// </summary>
        /// <param name="switches"></param>
        /// <returns></returns>
        public FluentMemberBindingConfiguration<TModel> WatchesFor(IEnumerable<string> switches)
        {
            Source.SwitchValues.AddRange(switches);
            return this;
        }

        /// <summary>
        /// Use this value if no values are provided by the parsed arguments
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public FluentMemberBindingConfiguration<TModel> UsesDefaultValueOf(object defaultValue)
        {
            Source.DefaultValue = defaultValue;
            return this;
        }

        /// <summary>
        /// Configure switch value(s) that will be bound to the current member
        /// </summary>
        /// <param name="switches"></param>
        /// <returns></returns>
        public FluentMemberBindingConfiguration<TModel> WatchesFor(params string[] switches)
        {
            Source.SwitchValues.AddRange(switches);
            return this;
        }

        /// <summary>
        /// Set help text for current member
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public FluentMemberBindingConfiguration<TModel> HasHelpTextOf(string text)
        {
            Source.HelpText = text;
            return this;
        }

        /// <summary>
        /// Set if current member is required
        /// </summary>
        /// <returns></returns>
        public FluentMemberBindingConfiguration<TModel> IsRequired()
        {
            Source.Required = true;
            return this;
        }

        /// <summary>
        /// Set if current member is not required
        /// </summary>
        /// <returns></returns>
        public FluentMemberBindingConfiguration<TModel> IsNotRequired()
        {
            Source.Required = false;
            return this;
        }

        /// <summary>
        /// Fluent method used to "navigate up" to the model to continue configuring the model
        /// </summary>
        /// <returns></returns>
        public FluentModelBindingConfiguration<TModel> ForModel()
        {
            return new FluentModelBindingConfiguration<TModel>(Source.Parent);
        }

        /// <summary>
        /// Fluent method used to start configuring another member of the model
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="member"></param>
        /// <returns></returns>
        public FluentMemberBindingConfiguration<TModel> ForMember<TResult>(Expression<Func<TModel, TResult>> member)
        {
            return Source.Parent.AsFluent().ForMember(member);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IModelBindingDefinition<TModel> Initialize()
        {
            return Source.Parent;
        }
    }

    /// <summary>
    /// Useful extension methods for configuring one or more Model members
    /// </summary>
    public static class FluentMemberBindingExtensions
    {
        /// <summary>
        /// Set the converter used to parse the current member
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="fluent"></param>
        /// <param name="typeConverter"></param>
        /// <returns></returns>
        public static FluentMemberBindingConfiguration<TModel> ParsesArgumentWith<TModel>(this FluentMemberBindingConfiguration<TModel> fluent, TypeConverter typeConverter)
        {
            return fluent.ParsesArgumentWith(typeConverter);
        }

        /// <summary>
        /// Set the converter used to parse the current member
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="fluent"></param>
        /// <param name="typeConverter"></param>
        /// <returns></returns>
        public static FluentMemberBindingConfiguration<TModel> ParsesArgumentWith<TModel>(this FluentMemberBindingConfiguration<TModel> fluent, IArgsTypeConverter typeConverter)
        {
            return fluent.ParsesArgumentWith(new ArgsTypeConverter(fluent.MemberType, typeConverter));
        }

        /// <summary>
        /// Set the converter used to parse the current member
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="fluent"></param>
        /// <param name="typeConverter"></param>
        /// <returns></returns>
        public static FluentMemberBindingConfiguration<TModel> ParsesArgumentWith<TModel>(this FluentMemberBindingConfiguration<TModel> fluent, Func<string, object> typeConverter)
        {
            return fluent.ParsesArgumentWith(new LambdaArgsTypeConverter(typeConverter));
        }

        /// <summary>
        /// Set the converter used to parse the current member
        /// </summary>
        /// <typeparam name="TArgsTypeConverter"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="fluent"></param>
        /// <returns></returns>
        public static FluentMemberBindingConfiguration<TModel> ParsesArgumentWith<TArgsTypeConverter, TModel>(this FluentMemberBindingConfiguration<TModel> fluent)
            where TArgsTypeConverter : IArgsTypeConverter, new()
        {
            return fluent.ParsesArgumentWith(new ArgsTypeConverter(fluent.MemberType, new TArgsTypeConverter()));
        }
    }
}
