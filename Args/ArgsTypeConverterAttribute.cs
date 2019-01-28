using System;
using System.Linq;
using System.Reflection;

namespace Args
{
    /// <summary>
    /// Indicates that the converter to be used when parsing the argument for this property
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]    
    public sealed class ArgsTypeConverterAttribute : Attribute
    {
        /// <summary>
        /// The type that will be used to convert values
        /// </summary>
        public Type ArgsTypeConverterType { get; private set; }

        /// <summary>
        /// Intializes a new instance with the provided type
        /// </summary>
        /// <param name="argsConverterType">This type must implement <see cref="IArgsTypeConverter"/></param>
        public ArgsTypeConverterAttribute(Type argsConverterType)
        {
            if (argsConverterType.GetInterfaces().Where(i => i == typeof(IArgsTypeConverter)).Any() == false)
                throw new InvalidOperationException(string.Format("Type {0} must implement interface {1}", argsConverterType.Name, typeof(IArgsTypeConverter).Name));

            ArgsTypeConverterType = argsConverterType;
        }
    }
}
