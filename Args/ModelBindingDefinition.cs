using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Args
{
    public class ModelBindingDefinition<TModel> : IModelBindingDefinition<TModel>
    {
        /// <summary>
        /// The default implementation for IModelBindingDefinition
        /// </summary>
        public virtual IDictionary<MemberInfo, MemberBindingDefinition<TModel>> Members { get; protected set; }

        IEnumerable<IMemberBindingDefinition<TModel>> IModelBindingDefinition<TModel>.Members
        {
            get
            {
                return Members.Select(m => m.Value).Cast<IMemberBindingDefinition<TModel>>();
            }
        }

        public virtual IDictionary<Type, TypeConverter> TypeConverters { get; private set; }

        protected virtual IList<MemberInfo> OrdinalArguments { get; set; }

        public virtual string SwitchDelimiter { get; set; }

        public virtual StringComparer StringComparer { get; set; }

        public string CommandModelDescription { get; set; }

        public ModelBindingDefinition()
        {
            Members = new Dictionary<MemberInfo, MemberBindingDefinition<TModel>>();
            TypeConverters = new Dictionary<Type, TypeConverter>();
            OrdinalArguments = new List<MemberInfo>();
            SwitchDelimiter = "/";
            StringComparer = System.StringComparer.CurrentCultureIgnoreCase;
        }

        public virtual TypeConverter TryGetTypeConverter(Type type)
        {
            TypeConverter returnValue;
            if (TypeConverters.TryGetValue(type, out returnValue))
                return returnValue;

            return null;
        }

        public virtual IMemberBindingDefinition<TModel> GetOrCreateMemberBindingDefinition(MemberInfo member)
        {

            var returnValue = GetMemberBindingDefinition(member) as MemberBindingDefinition<TModel>;
            
            if (returnValue == null)
            {
                returnValue = new MemberBindingDefinition<TModel>(member, this);
                Members.Add(member, returnValue);
            }

            return returnValue;
        }

        public virtual IMemberBindingDefinition<TModel> GetMemberBindingDefinition(MemberInfo member)
        {
            MemberBindingDefinition<TModel> returnValue;

            if (Members.TryGetValue(member, out returnValue) == false) return null;

            return returnValue;
        }

        #region Binding Methods
        public virtual TModel CreateAndBind(IEnumerable<string> args)
        {
            var model = (TModel) ArgsTypeResolver.Current.GetService(typeof(TModel));

            BindModel(model, args);

            return model;
        }

        public virtual void BindModel(TModel model, IEnumerable<string> args)
        {
            var unnamedArgs = args.TakeWhile(s => IsSwitch(s) == false);

            if (unnamedArgs.Count() != OrdinalArguments.Count)
                throw new InvalidArgsFormatException(String.Format("Model requires exactly {0} arguments before the first switch; {1} was/were provided", OrdinalArguments.Count, unnamedArgs.Count()));

            for (var i = 0; i < OrdinalArguments.Count; i++)
            {
                MemberBindingDefinition<TModel> member;

                if (Members.TryGetValue(OrdinalArguments[i], out member) == false)
                    throw new BindingDefinitionException(String.Format("Member {0} in {1} does not have a BindingDefinition defined.", OrdinalArguments[i].Name, typeof(TModel).FullName));


                HandleInputs(new[] { unnamedArgs.Skip(i).First() }, model, member);
            }

            var switchedArguments = args.Skip(unnamedArgs.Count());
            var namedMembers = Members.Where(m => OrdinalArguments.Contains(m.Key) == false);

            foreach(var member in namedMembers)
            {
                //find a suitable switch, then skip 1 (to skip the switch itself)
                var argument = switchedArguments.SkipWhile(a => a.Length <= SwitchDelimiter.Length || member.Value.CanHandleSwitch(a.Substring(SwitchDelimiter.Length)) == false).Skip(1);

                if (argument.Any())
                {
                    //take all arguments until the next switch is found
                    var argumentValue = argument.TakeWhile(a => a.StartsWith(SwitchDelimiter) == false);

                    HandleInputs(argumentValue, model, member.Value);
                }
                else
                {
                    //switch not provided
                    if (member.Value.DefaultValue != null)
                        member.Key.SetValue(model, member.Value.DefaultValue);
                }
            }
        }

        protected virtual void HandleInputs(IEnumerable<string> arguments, TModel model, MemberBindingDefinition<TModel> memberBindingDefinition)
        {
            var result = memberBindingDefinition.CoerceValue(arguments, model);

            memberBindingDefinition.SetMemberValue(result, model);
        }

        protected virtual bool IsSwitch(string value)
        {
            return value.StartsWith(SwitchDelimiter);
        }
        #endregion        
    

        public void AddOrdinalArgument(MemberInfo member)
        {
            GetOrCreateMemberBindingDefinition(member);
            OrdinalArguments.Add(member);
        }

        public void SetOrdinalArguments(IEnumerable<MemberInfo> members)
        {
            foreach (var member in members)
                AddOrdinalArgument(member);
        }

        public IEnumerable<MemberInfo> GetOrdinalArguments()
        {
            return new ReadOnlyCollection<MemberInfo>(OrdinalArguments);
        }


        public void RemoveMember(MemberInfo member)
        {
            if (Members.ContainsKey(member))
            {
                Members.Remove(member);
                OrdinalArguments.Remove(member);
            }
        }

        public int? OrdinalIndexOf(MemberInfo member)
        {
            var index = OrdinalArguments.IndexOf(member);

            return index == -1 ? null : new int?(index);
        }
    }
}
