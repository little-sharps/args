using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Args
{
    public class FluentMemberBindingConfiguration<TModel>
    {
        protected readonly IMemberBindingDefinition<TModel> Source;

        public Type MemberType
        {
            get { return Source.MemberInfo.GetDeclaredType(); }
        }

        public FluentMemberBindingConfiguration(IMemberBindingDefinition<TModel> source)
        {
            Source = source;
        }

        public FluentMemberBindingConfiguration<TModel> ParsesArgumentWith(TypeConverter typeConverter)
        {
            Source.TypeConverter = typeConverter;
            return this;
        }

        public FluentMemberBindingConfiguration<TModel> WatchesFor(IEnumerable<string> switches)
        {
            Source.SwitchValues.AddRange(switches);
            return this;
        }

        public FluentMemberBindingConfiguration<TModel> UsesDefaultValueOf(object defaultValue)
        {
            Source.DefaultValue = defaultValue;
            return this;
        }


        public FluentMemberBindingConfiguration<TModel> WatchesFor(params string[] switches)
        {
            Source.SwitchValues.AddRange(switches);
            return this;
        }

        public FluentMemberBindingConfiguration<TModel> HasHelpTextOf(string text)
        {
            Source.HelpText = text;
            return this;
        }

        public FluentModelBindingConfiguration<TModel> ForModel()
        {
            return new FluentModelBindingConfiguration<TModel>(Source.Parent);
        }

        public FluentMemberBindingConfiguration<TModel> ForMember<TResult>(Expression<Func<TModel, TResult>> member)
        {
            return Source.Parent.AsFluent().ForMember(member);
        }

        public IModelBindingDefinition<TModel> Initialize()
        {
            return Source.Parent;
        }
    }

    public static class FluentMemberBindingExtensions
    {
        public static FluentMemberBindingConfiguration<TModel> ParsesArgumentWith<TModel>(this FluentMemberBindingConfiguration<TModel> fluent, TypeConverter typeConverter)
        {
            return fluent.ParsesArgumentWith(typeConverter);
        }

        public static FluentMemberBindingConfiguration<TModel> ParsesArgumentWith<TModel>(this FluentMemberBindingConfiguration<TModel> fluent, IArgsTypeConverter typeConverter)
        {
            return fluent.ParsesArgumentWith(new ArgsTypeConverter(fluent.MemberType, typeConverter));
        }

        public static FluentMemberBindingConfiguration<TModel> ParsesArgumentWith<TModel>(this FluentMemberBindingConfiguration<TModel> fluent, Func<string, object> typeConverter)
        {
            return fluent.ParsesArgumentWith(new LambdaArgsTypeConverter(typeConverter));
        }

        public static FluentMemberBindingConfiguration<TModel> ParsesArgumentWith<TArgsTypeConverter, TModel>(this FluentMemberBindingConfiguration<TModel> fluent)
            where TArgsTypeConverter : IArgsTypeConverter, new()
        {
            return fluent.ParsesArgumentWith(new ArgsTypeConverter(fluent.MemberType, new TArgsTypeConverter()));
        }
    }
}
