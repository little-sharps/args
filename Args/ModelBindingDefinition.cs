using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Args
{
    /// <summary>
    /// The default <see cref="IModelBindingDefinition{TModel}"/> implementation
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
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

        /// <summary>
        /// 
        /// </summary>
        public virtual IDictionary<Type, TypeConverter> TypeConverters { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        protected virtual IList<MemberInfo> OrdinalArguments { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string SwitchDelimiter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual StringComparer StringComparer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CommandModelDescription { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ModelBindingDefinition()
        {
            Members = new Dictionary<MemberInfo, MemberBindingDefinition<TModel>>();
            TypeConverters = new Dictionary<Type, TypeConverter>();
            OrdinalArguments = new List<MemberInfo>();
            SwitchDelimiter = "/";
            StringComparer = System.StringComparer.CurrentCultureIgnoreCase;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual TypeConverter TryGetTypeConverter(Type type)
        {
            TypeConverter returnValue;
            if (TypeConverters.TryGetValue(type, out returnValue))
                return returnValue;

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public virtual IMemberBindingDefinition<TModel> GetMemberBindingDefinition(MemberInfo member)
        {
            var memberPair = Members.Where(m => m.Key.Name == member.Name);
            
            return memberPair.Any() ? memberPair.First().Value : null;
        }

        #region Binding Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual TModel CreateAndBind(IEnumerable<string> args)
        {
            var model = (TModel) ArgsTypeResolver.Current.GetService(typeof(TModel));

            BindModel(model, args);

            return model;
        }

        private void EnsureCorrectNumberOfOrdinalArguments(IEnumerable<string> ordinalArgs)
        {
            if (OrdinalArguments.Any())
            {                
                if (OrdinalArguments.Count == ordinalArgs.Count()) return;

                var hasCollection = OrdinalArguments.Last().GetDeclaredType().GetGenericIEnumerable() != null;

                if(hasCollection && ordinalArgs.Count() >= OrdinalArguments.Count) return;

                throw new InvalidOperationException(string.Format(Properties.Resources.IncorrectNumberOfOrdinalArgumentsMessage, OrdinalArguments.Count));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="args"></param>
        public virtual void BindModel(TModel model, IEnumerable<string> args)
        {
            var unnamedArgs = args.TakeWhile(s => IsSwitch(s) == false);

            EnsureCorrectNumberOfOrdinalArguments(unnamedArgs);

            for (var i = 0; i < OrdinalArguments.Count; i++)
            {
                MemberBindingDefinition<TModel> member = (MemberBindingDefinition<TModel>)GetMemberBindingDefinition(OrdinalArguments[i]);

                if (member == null)
                    throw new BindingDefinitionException(String.Format("Member {0} in {1} does not have a BindingDefinition defined.", OrdinalArguments[i].Name, typeof(TModel).FullName));

                if (OrdinalArguments.Count != i + 1)
                    HandleInputs(new[] { unnamedArgs.Skip(i).First() }, model, member);
                else
                    //last ordinal, send remaining ordinal arguments
                    HandleInputs(unnamedArgs.Skip(i), model, member);
            }

            var switchedArguments = args.Skip(unnamedArgs.Count());
            var namedMembers = Members.Where(m => OrdinalArguments.Contains(m.Key) == false);

            foreach(var member in namedMembers)
            {
                //find a suitable switch
                var argument = switchedArguments.SkipWhile(a => a.StartsWith(SwitchDelimiter) == false || a.Length <= SwitchDelimiter.Length || member.Value.CanHandleSwitch(a.Substring(SwitchDelimiter.Length)) == false);

                if (argument.Any())
                {
                    //take all arguments until the next switch is found, skipping the first, which is the switch itself
                    var argumentValue = argument.Skip(1).TakeWhile(a => a.StartsWith(SwitchDelimiter) == false);

                    HandleInputs(argumentValue, model, member.Value);
                }
                else
                {
                    if (member.Value.Required == true)
                        throw new InvalidOperationException(String.Format(Properties.Resources.RequiredParameterNotProvidedMessage, member.Value.MemberInfo.Name));

                    //switch not provided
                    if (member.Value.DefaultValue != null)
                        member.Key.SetValue(model, member.Value.DefaultValue);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="model"></param>
        /// <param name="memberBindingDefinition"></param>
        protected virtual void HandleInputs(IEnumerable<string> arguments, TModel model, MemberBindingDefinition<TModel> memberBindingDefinition)
        {
            var result = memberBindingDefinition.CoerceValue(arguments, model);

            memberBindingDefinition.SetMemberValue(result, model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual bool IsSwitch(string value)
        {
            return value.StartsWith(SwitchDelimiter);
        }
        #endregion        
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        public void AddOrdinalArgument(MemberInfo member)
        {
            if (OrdinalArguments.Any(m => m.GetDeclaredType().GetGenericIEnumerable() != null))
                throw new InvalidOperationException(Properties.Resources.OnlyLastOridnalArgumentCollectionMessage);

            GetOrCreateMemberBindingDefinition(member);
            OrdinalArguments.Add(member);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="members"></param>
        public void SetOrdinalArguments(IEnumerable<MemberInfo> members)
        {
            foreach (var member in members)
                AddOrdinalArgument(member);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MemberInfo> GetOrdinalArguments()
        {
            return new ReadOnlyCollection<MemberInfo>(OrdinalArguments);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        public void RemoveMember(MemberInfo member)
        {
            if (Members.ContainsKey(member))
            {
                Members.Remove(member);
                OrdinalArguments.Remove(member);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public int? OrdinalIndexOf(MemberInfo member)
        {
            var index = OrdinalArguments.IndexOf(member);

            return index == -1 ? null : new int?(index);
        }
    }
}
