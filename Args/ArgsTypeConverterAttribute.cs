using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Args
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class ArgsTypeConverterAttribute : Attribute
    {
        public Type ArgsTypeConverterType { get; private set; }

        public ArgsTypeConverterAttribute(Type argsConverterType)
        {
            if (argsConverterType.GetInterfaces().Where(i => i == typeof(IArgsTypeConverter)).Any() == false)
                throw new InvalidOperationException(String.Format("Type {0} must implement interface {1}", argsConverterType.Name, typeof(IArgsTypeConverter).Name));

            ArgsTypeConverterType = argsConverterType;
        }
    }
}
