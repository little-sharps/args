using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Globalization;

namespace Args
{
    /// <summary>
    /// The default impelmentation for IMemberBindingDefinition
    /// </summary>
    /// <typeparam name="TModel">Type of model to bind</typeparam>
    public class MemberBindingDefinition<TModel> : IMemberBindingDefinition<TModel>
    {
        public virtual ModelBindingDefinition<TModel> Parent { get; protected set; }

        IModelBindingDefinition<TModel> IMemberBindingDefinition<TModel>.Parent
        {
            get { return Parent; }
        }


        public MemberInfo MemberInfo { get; protected set; }

        public virtual TypeConverter TypeConverter { get; set; }

        public virtual ICollection<string> SwitchValues { get; private set; }

        public virtual object DefaultValue { get; set; }

        public virtual bool Required { get; set; }

        public virtual bool CanHandleSwitch(string s)
        {
            return SwitchValues.Contains(s, Parent.StringComparer);
        }


        public MemberBindingDefinition(MemberInfo memberInfo, ModelBindingDefinition<TModel> parent)
        {
            if (memberInfo.DeclaringType.IsAssignableFrom(typeof(TModel)) == false) throw new InvalidOperationException(String.Format("memberInfo must be from type {0}", typeof(TModel).FullName));
            MemberInfo = memberInfo;
            Parent = parent;
            SwitchValues = new Collection<string>();
        }

        public virtual object CoerceValue(IEnumerable<string> value, TModel model)
        {
            var declaredType = MemberInfo.GetDeclaredType();
            object newValue = null;

            var typeConverter = TypeConverter ?? Parent.TryGetTypeConverter(declaredType) ?? GetDefaultTypeConveter(declaredType);

            if (typeConverter.CanConvertFrom(typeof(string)) == false)
            {
                //special case to handle collections
                //declared type must be or implement IEnumerable<> (directly or indirectly)
                //declared type cannot be string (which implements IEnumerable<char>)
                var genericIEnumerable = declaredType.GetGenericIEnumerable();

                if (declaredType != typeof(string) && genericIEnumerable != null)
                {
                    var collectionType = genericIEnumerable.GetGenericArguments()[0];
                    typeConverter = TypeConverter ?? Parent.TryGetTypeConverter(collectionType) ?? GetDefaultTypeConveter(collectionType);

                    if (typeConverter.CanConvertFrom(typeof(string)) == false)
                        throw new InvalidOperationException(String.Format(Properties.Resources.TypeConverterNotFoundMessage, typeof(string).Name, collectionType.Name));

                    var convertedValues = value.Select(v => typeConverter.ConvertFrom(null, CultureInfo.InvariantCulture, v));

                    if (declaredType.IsAssignableFrom(collectionType.MakeArrayType()))
                        newValue = convertedValues.ToArray().Convert(collectionType);
                    else if (typeof(IList).IsAssignableFrom(declaredType) || typeof(ICollection<>).MakeGenericType(collectionType).IsAssignableFrom(declaredType))
                    {
                        newValue = ArgsTypeResolver.Current.GetService(declaredType);
                        var methodInfo = declaredType.GetMethod("Add");

                        if (methodInfo == null) throw new InvalidOperationException(String.Format(Properties.Resources.AddMethodNotFoundMessage, declaredType.Name));

                        foreach (var item in convertedValues)
                            methodInfo.Invoke(newValue, new[] { item });
                    }
                }
                else
                    throw new InvalidOperationException(String.Format(Properties.Resources.TypeConverterNotFoundMessage, typeof(string).Name, declaredType.Name));
            }
            else
            {
                var joinedValue = String.Join(" ", value.ToArray());

                if (String.IsNullOrEmpty(joinedValue) && declaredType == typeof(bool))
                    joinedValue = true.ToString();

                newValue = typeConverter.ConvertFrom(null, CultureInfo.InvariantCulture, joinedValue);
            }

            if (newValue == declaredType.GetDefaultValue() && DefaultValue != null) newValue = DefaultValue;

            return newValue;
        }

        public virtual void SetMemberValue(object value, TModel model)
        {
            MemberInfo.SetValue(model, value);
        }

        public virtual TypeConverter GetDefaultTypeConveter(Type type)
        {
            return TypeDescriptor.GetConverter(type);
        }

        private string helpText;
        public virtual string HelpText
        {
            get
            {
                if (helpText != null) return helpText;

                var attribute = MemberInfo.GetCustomAttributes(true).OfType<DescriptionAttribute>().SingleOrDefault();

                if (attribute != null)
                    return attribute.Description;

                return null;
            }
            set
            {
                helpText = value;
            }
        }
    }
}
