using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Args
{
    /// <summary>
    /// The default impelmentation for IMemberBindingDefinition
    /// </summary>
    /// <typeparam name="TModel">Type of model to bind</typeparam>
    public class MemberBindingDefinition<TModel> : IMemberBindingDefinition<TModel>
    {
        /// <summary>
        /// Returns the <see cref="ModelBindingDefinition{TModel}"/> for the current member
        /// </summary>
        public virtual ModelBindingDefinition<TModel> Parent { get; protected set; }

        IModelBindingDefinition<TModel> IMemberBindingDefinition<TModel>.Parent
        {
            get { return Parent; }
        }

        /// <summary>
        /// Gets the <see cref="System.Reflection.MemberInfo"/> of the current member
        /// </summary>
        public MemberInfo MemberInfo { get; protected set; }

        /// <summary>
        /// Gets or sets the <see cref="System.ComponentModel.TypeConverter"/> used to parse a value for the current member
        /// </summary>
        public virtual TypeConverter TypeConverter { get; set; }

        /// <summary>
        /// Gets the allowed switch values
        /// </summary>
        public virtual ICollection<string> SwitchValues { get; private set; }

        /// <summary>
        /// Gets or sets the default value to use if no value is provided
        /// </summary>
        public virtual object DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets of the member is requird when binding
        /// </summary>
        public virtual bool Required { get; set; }

        /// <summary>
        /// Indicates of the provided string is one of the allowed switch values
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public virtual bool CanHandleSwitch(string s) => SwitchValues.Contains(s, Parent.StringComparer);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <param name="parent"></param>
        public MemberBindingDefinition(MemberInfo memberInfo, ModelBindingDefinition<TModel> parent)
        {
            if (memberInfo.DeclaringType.IsAssignableFrom(typeof(TModel)) == false) throw new InvalidOperationException(String.Format("memberInfo must be from type {0}", typeof(TModel).FullName));
            MemberInfo = memberInfo;
            Parent = parent;
            SwitchValues = new Collection<string>();
        }

        /// <summary>
        /// Attempts to prase the provided value(s) for the current member
        /// </summary>
        /// <param name="value"></param>
        /// <param name="model"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Sets the value fo the current member
        /// </summary>
        /// <param name="value"></param>
        /// <param name="model"></param>
        public virtual void SetMemberValue(object value, TModel model)
        {
            MemberInfo.SetValue(model, value);
        }

        /// <summary>
        /// Gets the default type converter for the type of the current member
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual TypeConverter GetDefaultTypeConveter(Type type)
        {
            return TypeDescriptor.GetConverter(type);
        }

        private string helpText;
        /// <summary>
        /// Gets or sets the help text fo rthe current member
        /// </summary>
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
