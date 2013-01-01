using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;

namespace Args
{
    /// <summary>
    /// A class that impelments this interface is responsible for signifying if it can handle a switch and also to coordinate converting the value of the switch into the appropriate type
    /// </summary>
    /// <typeparam name="TModel">Type of the model to bind</typeparam>
    public interface IMemberBindingDefinition<TModel>
    {
        IModelBindingDefinition<TModel> Parent { get; }
        MemberInfo MemberInfo { get; }
        TypeConverter TypeConverter { get; set; }
        ICollection<string> SwitchValues { get; }
        object DefaultValue { get; set; }
        bool CanHandleSwitch(string s);
        string HelpText { get; set; }
        bool Required { get; set; }
    }

    public static class MemberBindingDefinitionExtensions
    {
        public static FluentMemberBindingConfiguration<TModel> AsFluent<TModel>(this IMemberBindingDefinition<TModel> source)
        {
            return new FluentMemberBindingConfiguration<TModel>(source);
        }
    }
}
