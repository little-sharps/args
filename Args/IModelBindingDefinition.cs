using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Args
{
    public interface IModelBindingDefinition<TModel>
    {
        IDictionary<Type, TypeConverter> TypeConverters { get; }
        string SwitchDelimiter { get; set; }
        StringComparer StringComparer { get; set; }
        IEnumerable<IMemberBindingDefinition<TModel>> Members { get; }
        string CommandModelDescription { get; set; }

        IMemberBindingDefinition<TModel> GetOrCreateMemberBindingDefinition(MemberInfo member);
        IMemberBindingDefinition<TModel> GetMemberBindingDefinition(MemberInfo member);
        TModel CreateAndBind(IEnumerable<string> args);
        void BindModel(TModel model, IEnumerable<string> args);
        void AddOrdinalArgument(MemberInfo member);
        void SetOrdinalArguments(IEnumerable<MemberInfo> members);
        IEnumerable<MemberInfo> GetOrdinalArguments();
        void RemoveMember(MemberInfo member);
        int? OrdinalIndexOf(MemberInfo member);
    }

    public static class ModelBindingDefinitionExtensions
    {
        public static FluentModelBindingConfiguration<TModel> AsFluent<TModel>(this IModelBindingDefinition<TModel> source)
        {
            return new FluentModelBindingConfiguration<TModel>(source);

        }
    }
}
