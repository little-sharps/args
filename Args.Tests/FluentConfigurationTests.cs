using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SharpTestsEx;

namespace Args.Tests
{
    [TestFixture]
    public class FluentConfigurationTests
    {        
        #region Model Under Test
        public class SimpleTestModel
        {
            public string Name { get; set; }
            public int Id;
            public string FileName { get; set; }
        }
        #endregion

        [Test]
        public void SimpleModelFluentTest()
        {
            var m = new ModelBindingDefinition<SimpleTestModel>()
            .AsFluent().UsingStringComparer(StringComparer.InvariantCulture)            
            .ParsesArgumentsWith(typeof(int), new System.ComponentModel.Int16Converter())
            .HasFirstOrdinalArgumentOf(a => a.FileName)
            .ForMember(a => a.Name)
                .WatchesFor("name", "nam")
            .ForMember(a => a.Id)
                .WatchesFor("id")
                .HasHelpTextOf("Hello World")
            .ForModel()
            .UsingSwitchDelimiter("--")
            .Initialize();

            m.GetOrdinalArguments().Count().Should().Be.EqualTo(1);
            m.GetOrdinalArguments().Should().Contain(m.Members.GetMemberBindingDefinitionFor(a => a.FileName).MemberInfo);
            m.StringComparer.Should().Be.EqualTo(StringComparer.InvariantCulture);
            m.SwitchDelimiter.Should().Be.EqualTo("--");
            m.TypeConverters.Count.Should().Be.EqualTo(1);
            m.TypeConverters.ContainsKey(typeof(int));
            m.TypeConverters[typeof(int)].Should().Be.OfType<System.ComponentModel.Int16Converter>();

            var member = m.Members.GetMemberBindingDefinitionFor(a => a.Id);
            member.DefaultValue.Should().Be.Null();
            member.Parent.Should().Be.EqualTo(m);
            member.TypeConverter.Should().Be.Null();
            member.SwitchValues.Count().Should().Be.EqualTo(1);
            member.CanHandleSwitch("id").Should().Be.True();
            member.HelpText.Should().Be.EqualTo("Hello World");

            member = m.Members.GetMemberBindingDefinitionFor(a => a.Name);
            member.DefaultValue.Should().Be.Null();
            member.Parent.Should().Be.EqualTo(m);
            member.TypeConverter.Should().Be.Null();
            member.SwitchValues.Count().Should().Be.EqualTo(2);
            member.CanHandleSwitch("NAM").Should().Be.False();
            member.CanHandleSwitch("nAmE").Should().Be.False();
            member.CanHandleSwitch("nam").Should().Be.True();
            member.CanHandleSwitch("name").Should().Be.True();
            member.HelpText.Should().Be.Null();

            member = m.Members.GetMemberBindingDefinitionFor(a => a.FileName);
            member.DefaultValue.Should().Be.Null();
            member.Parent.Should().Be.EqualTo(m);
            member.TypeConverter.Should().Be.Null();
            member.SwitchValues.Should().Be.Empty();
            member.HelpText.Should().Be.Null();
        }
    }
}
