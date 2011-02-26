using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public abstract string GetHelpText();
    }
}
