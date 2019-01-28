using System;
using System.ComponentModel;

namespace Args
{
    internal class ArgsTypeConverter : TypeConverter
    {
        internal readonly Type Type;
        internal readonly IArgsTypeConverter Converter;

        public ArgsTypeConverter(Type type, IArgsTypeConverter converter)
        {
            Type = type;
            Converter = converter;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == Type;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (CanConvertTo(destinationType) == false) throw new NotSupportedException(String.Format("Conversion to {0} is not supported.", destinationType.FullName));

            return Converter.Convert(value.ToString());
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (CanConvertFrom(value.GetType()) == false) throw new NotSupportedException(String.Format("Conversion from {0} is not supported.", value.GetType().FullName));

            return Converter.Convert(value.ToString());
        }
    }    
}
