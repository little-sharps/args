using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        public virtual ModelBindingDefinition<TModel> Parent { get; protected set; }

        IModelBindingDefinition<TModel> IMemberBindingDefinition<TModel>.Parent
        {
            get { return Parent; }
        }


        public MemberInfo MemberInfo { get; protected set; }

        public virtual TypeConverter TypeConverter { get; set; }

        public virtual ICollection<string> SwitchValues { get; private set; }

        public virtual object DefaultValue { get; set; }

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
            var joinedValue = String.Join(" ", value.ToArray());

            var typeConverter = TypeConverter ?? Parent.TryGetTypeConverter(MemberInfo.GetDeclaredType()) ?? GetDefaultTypeConveter(MemberInfo.GetDeclaredType());
            
            if (typeConverter.CanConvertFrom(typeof(string)) == false)
                throw new InvalidOperationException(String.Format(Properties.Resources.TypeConverterNotFoundMessage, joinedValue.GetType().Name, MemberInfo.GetDeclaredType().Name));

            if (String.IsNullOrEmpty(joinedValue) && MemberInfo.GetDeclaredType() == typeof(bool))
                joinedValue = true.ToString();

            var newValue = typeConverter.ConvertFrom(joinedValue);

            if (newValue == MemberInfo.GetDeclaredType().GetDefaultValue() && DefaultValue != null) newValue = DefaultValue;

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
