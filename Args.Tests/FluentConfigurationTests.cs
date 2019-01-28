using NUnit.Framework;
using System;
using System.ComponentModel;
using System.Linq;

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
            .AsFluent().UsingStringComparer(StringComparer.Ordinal)            
            .ParsesArgumentsWith(typeof(int), new Int16Converter())
            .HasFirstOrdinalArgumentOf(a => a.FileName)
            .ForMember(a => a.Name)
                .WatchesFor("name", "nam")
            .ForMember(a => a.Id)
                .WatchesFor("id")
                .HasHelpTextOf("Hello World")
            .ForModel()
            .UsingSwitchDelimiter("--")
            .Initialize();

            Assert.AreEqual(1, m.GetOrdinalArguments().Count());
            Assert.Contains(m.Members.GetMemberBindingDefinitionFor(a => a.FileName).MemberInfo, m.GetOrdinalArguments().ToList());
            Assert.AreEqual(StringComparer.Ordinal, m.StringComparer);
            Assert.AreEqual("--", m.SwitchDelimiter);
            Assert.AreEqual(1, m.TypeConverters.Count);
            Assert.IsTrue(m.TypeConverters.ContainsKey(typeof(int)));            
            Assert.AreEqual(typeof(Int16Converter), m.TypeConverters[typeof(int)]);

            var member = m.Members.GetMemberBindingDefinitionFor(a => a.Id);
            Assert.IsNull(member.DefaultValue);
            Assert.AreSame(m, member.Parent);
            Assert.IsNull(member.TypeConverter);
            Assert.AreEqual(1, member.SwitchValues.Count);
            Assert.IsTrue(member.CanHandleSwitch("id"));
            Assert.AreEqual("Hello World", member.HelpText);

            member = m.Members.GetMemberBindingDefinitionFor(a => a.Name);
            Assert.IsNull(member.DefaultValue);
            Assert.AreSame(m, member.Parent);
            Assert.IsNull(member.TypeConverter);
            Assert.AreEqual(2, member.SwitchValues.Count);
            Assert.IsFalse(member.CanHandleSwitch("NAM"));
            Assert.IsFalse(member.CanHandleSwitch("nAmE"));
            Assert.IsTrue(member.CanHandleSwitch("nam"));
            Assert.IsTrue(member.CanHandleSwitch("name"));
            Assert.IsNull(member.HelpText);

            member = m.Members.GetMemberBindingDefinitionFor(a => a.FileName);
            Assert.IsNull(member.DefaultValue);
            Assert.AreSame(m, member.Parent);
            Assert.IsNull(member.TypeConverter);
            Assert.IsEmpty(member.SwitchValues);
            Assert.IsNull(member.HelpText);
        }
    }
}