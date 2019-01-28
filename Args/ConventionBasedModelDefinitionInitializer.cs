using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Args
{
    /// <summary>
    /// The default convention-based initializer. Will create and return an instance of IModelBindingDefinition
    /// </summary>
    public class ConventionBasedModelDefinitionInitializer : IModelBindingDefinitionInitializer
    {
        /// <summary>
        /// Initializes the provided <see cref="IModelBindingDefinition{TModel}"/> based on default conventions
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="init"></param>
        public void Initialize<TModel>(IModelBindingDefinition<TModel> init)
        {
            var modelAttribute = typeof(TModel)
#if !NET_FRAMEWORK
                .GetTypeInfo()
#endif
                .GetCustomAttributes(true).OfType<ArgsModelAttribute>().SingleOrDefault() ?? ArgsModelAttribute.Default;

            var modelDescriptionAttribute = typeof(TModel)
#if !NET_FRAMEWORK
                .GetTypeInfo()
#endif
                .GetCustomAttributes(true).OfType<DescriptionAttribute>().SingleOrDefault();

            init.SwitchDelimiter = modelAttribute.SwitchDelimiter;
            init.StringComparer = modelAttribute.StringComparer;
            SortedDictionary<int, MemberInfo> ordinalArguments = new SortedDictionary<int, MemberInfo>();

            if (modelDescriptionAttribute != null) init.CommandModelDescription = modelDescriptionAttribute.Description;

            var members = GetMembers(typeof(TModel));

            foreach (MemberInfo member in members)
            {
                var memberBinding = init.GetOrCreateMemberBindingDefinition(member);

                var memberAttributes = member.GetCustomAttributes(true).ToArray();

                var switchAttribute = memberAttributes.OfType<ArgsMemberSwitchAttribute>().SingleOrDefault();
                if (switchAttribute != null)
                {
                    if (switchAttribute.SwitchValues != null) memberBinding.SwitchValues.AddRange(switchAttribute.SwitchValues);
                    if (switchAttribute.ArgumentIndex.HasValue) ordinalArguments.Add(switchAttribute.ArgumentIndex.Value, member);
                }
                else
                    memberBinding.SwitchValues.AddRange(new[] { member.Name, DeriveShortName(members, member) }.Distinct(init.StringComparer));

                var defaultValueAttribute = memberAttributes.OfType<DefaultValueAttribute>().SingleOrDefault();
                if (defaultValueAttribute != null) memberBinding.DefaultValue = defaultValueAttribute.Value;

                var typeConverterAttribute = memberAttributes.OfType<TypeConverterAttribute>().SingleOrDefault();

                if (typeConverterAttribute != null) memberBinding.TypeConverter = (TypeConverter)ArgsTypeResolver.Current.GetService(Type.GetType(typeConverterAttribute.ConverterTypeName));

                var argsTypeConverterAttribute = memberAttributes.OfType<ArgsTypeConverterAttribute>().SingleOrDefault();

                if (argsTypeConverterAttribute != null) memberBinding.TypeConverter = new ArgsTypeConverter(member.GetDeclaredType(), (IArgsTypeConverter)ArgsTypeResolver.Current.GetService(argsTypeConverterAttribute.ArgsTypeConverterType));

                memberBinding.Required = memberAttributes.OfType<RequiredAttribute>().Any();
            }

            init.SetOrdinalArguments(ordinalArguments.Select(a => a.Value));
        }

        protected virtual IEnumerable<MemberInfo> GetMembers(Type modelType)
        {
            return modelType.GetFields(BindingFlags.Instance | BindingFlags.Public)
                .Concat(modelType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.CanWrite).Cast<MemberInfo>());
        }

        protected virtual string DeriveShortName(IEnumerable<MemberInfo> members, MemberInfo currentMember)
        {
            return DeriveShortNameRecursive(members, currentMember, 1);
        }

        private string DeriveShortNameRecursive(IEnumerable<MemberInfo> members, MemberInfo currentMember, int iteration)
        {
            if (currentMember.Name.Length < iteration) return currentMember.Name;

            var attemptedValue = currentMember.Name.Substring(0, iteration);
            var subset = members.Where(m => m.Name.StartsWith(attemptedValue, StringComparison.CurrentCultureIgnoreCase));

            if (subset.Count() == 0) throw new InvalidOperationException(String.Format(Properties.Resources.MemberNotFoundInAvailableMembers, currentMember.Name, currentMember.DeclaringType.FullName));
            else if (subset.Count() == 1) return attemptedValue;
            else return DeriveShortNameRecursive(subset, currentMember, iteration + 1);
        }
    }
}
