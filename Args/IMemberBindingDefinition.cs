using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;

namespace Args
{
    public interface IMemberBindingDefinition<TModel>
    {
        IModelBindingDefinition<TModel> Parent { get; }
        MemberInfo MemberInfo { get; }
        TypeConverter TypeConverter { get; set; }
        ICollection<string> SwitchValues { get; }
        object DefaultValue { get; set; }
        bool CanHandleSwitch(string s);
        string HelpText { get; set; }
    }

    public static class MemberBindingDefinitionExtensions
    {
        public static FluentMemberBindingConfiguration<TModel> AsFluent<TModel>(this IMemberBindingDefinition<TModel> source)
        {
            return new FluentMemberBindingConfiguration<TModel>(source);
        }
    }
}
