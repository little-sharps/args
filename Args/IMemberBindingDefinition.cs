using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Args
{
    /// <summary>
    /// A class that impelments this interface is responsible for signifying if it can handle a switch and also to coordinate converting the value of the switch into the appropriate type
    /// </summary>
    /// <typeparam name="TModel">Type of the model to bind</typeparam>
    public interface IMemberBindingDefinition<TModel>
    {
        /// <summary>
        /// Returns the parent that the current member belongs to
        /// </summary>
        IModelBindingDefinition<TModel> Parent { get; }
        /// <summary>
        /// Returns the <see cref="MemberInfo"/> of the current member binding definition
        /// </summary>
        MemberInfo MemberInfo { get; }
        /// <summary>
        /// Gets or sets the type converter that will be used to parse the value for the current member
        /// </summary>
        TypeConverter TypeConverter { get; set; }
        /// <summary>
        /// Holds the collection of switch value(s) that will bind to the current member binding definition
        /// </summary>
        ICollection<string> SwitchValues { get; }
        /// <summary>
        /// Gets or sets the default value to use if no value is provided
        /// </summary>
        object DefaultValue { get; set; }
        /// <summary>
        /// When implemented, returns a value that indicates if the provided witch is handled by the current member
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        bool CanHandleSwitch(string s);
        /// <summary>
        /// Gets or sets the help text value for the current member
        /// </summary>
        string HelpText { get; set; }
        /// <summary>
        /// Indicates if the current member is reuired
        /// </summary>
        bool Required { get; set; }
    }

    /// <summary>
    /// Extension methods that applies to all instances of the <see cref="IMemberBindingDefinition{TModel} "/> interface
    /// </summary>
    public static class MemberBindingDefinitionExtensions
    {
        /// <summary>
        /// Returns an object that will allow the member binding to be fluently configured
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static FluentMemberBindingConfiguration<TModel> AsFluent<TModel>(this IMemberBindingDefinition<TModel> source)
        {
            return new FluentMemberBindingConfiguration<TModel>(source);
        }
    }
}
