using System;

namespace Args.Help
{
    /// <summary>
    /// This attribute is a base type that is to be implemented.  Attributes inheriting from this should only be inspected by implementations of IHelpProvider.
    /// If the resource is a simple string, then you should consider using System.ComponentModel.DescriptionAttribute.
    /// The purpose of this type is to provide help information that might be expensive to retrieve.
    /// The attribute will only be utilized if help text has not already been provided
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Class)]
    public abstract class ResourceMemberHelpAttributeBase : Attribute
    {
        /// <summary>
        /// Returns the helptext provided by the implementation of <see cref="ResourceMemberHelpAttributeBase"/>
        /// </summary>
        /// <returns></returns>
        public abstract string GetHelpText();
    }
}
