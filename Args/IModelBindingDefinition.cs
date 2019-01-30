using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Args
{
    /// <summary>
    /// A class that implements this interface is responsible for creating and maintaining the definition of its members as well as orchestrating the binding of data to those members
    /// </summary>
    /// <typeparam name="TModel">Type of the model to bind</typeparam>
    public interface IModelBindingDefinition<TModel>
    {
        /// <summary>
        /// A dictionary of type converters to use for the specified types
        /// </summary>
        IDictionary<Type, TypeConverter> TypeConverters { get; }
        /// <summary>
        /// The string value that indicates the start of a command switch
        /// </summary>
        string SwitchDelimiter { get; set; }
        /// <summary>
        /// The <see cref="System.StringComparer"/> to use when comparing swtich values
        /// </summary>
        StringComparer StringComparer { get; set; }
        /// <summary>
        /// The collection of configured members for the model
        /// </summary>
        IEnumerable<IMemberBindingDefinition<TModel>> Members { get; }
        /// <summary>
        /// The description for the model; for use when help text is requested
        /// </summary>
        string CommandModelDescription { get; set; }

        /// <summary>
        /// Gets or creates an <see cref="IMemberBindingDefinition{TModel}"/> for the provided member
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        IMemberBindingDefinition<TModel> GetOrCreateMemberBindingDefinition(MemberInfo member);
        /// <summary>
        /// Gets an <see cref="IMemberBindingDefinition{TModel}"/> for the provided member
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        IMemberBindingDefinition<TModel> GetMemberBindingDefinition(MemberInfo member);
        /// <summary>
        /// Creates an instance of <typeparamref name="TModel"/> and binds members based on the provided args
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        TModel CreateAndBind(IEnumerable<string> args);
        /// <summary>
        /// Binds the provided <paramref name="args"/> to the provided <paramref name="model"/>
        /// </summary>
        /// <param name="model"></param>
        /// <param name="args"></param>
        void BindModel(TModel model, IEnumerable<string> args);
        /// <summary>
        /// Adds the provided <paramref name="member"/> as an ordinal argument
        /// </summary>
        /// <param name="member"></param>
        void AddOrdinalArgument(MemberInfo member);
        /// <summary>
        /// Sets the provided <paramref name="members"/> as the ordinal arguments
        /// </summary>
        /// <param name="members"></param>
        void SetOrdinalArguments(IEnumerable<MemberInfo> members);
        /// <summary>
        /// Gets the configured ordinal arguments
        /// </summary>
        /// <returns></returns>
        IEnumerable<MemberInfo> GetOrdinalArguments();
        /// <summary>
        /// Removes a previously configured <paramref name="member"/>
        /// </summary>
        /// <param name="member"></param>
        void RemoveMember(MemberInfo member);
        /// <summary>
        /// Retrives the ordinal index of the provided <paramref name="member"/>; Returns <see langword="null"/> if the provided <paramref name="member"/> is not an ordinal member
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        int? OrdinalIndexOf(MemberInfo member);
    }

    /// <summary>
    /// Helpful extension methods for <see cref="IModelBindingDefinition{TModel}"/>
    /// </summary>
    public static class ModelBindingDefinitionExtensions
    {
        /// <summary>
        /// Returns an object that allows the model to be configured fluently
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static FluentModelBindingConfiguration<TModel> AsFluent<TModel>(this IModelBindingDefinition<TModel> source)
        {
            return new FluentModelBindingConfiguration<TModel>(source);

        }
    }
}
