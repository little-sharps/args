using NUnit.Framework;
using System;
using System.ComponentModel;
using System.Linq;

namespace Args.Tests
{
    [TestFixture]
    public class SimpleModelConventionsTest
    {
        #region Model Under Test

        public class SimpleModelClass
        {
            [ArgsMemberSwitch(0)]
            public string Name { get; set; }

            [ArgsMemberSwitch("sd")]
            public DateTime StartDate { get; set; }

            [System.ComponentModel.Description("Forces the command")]
            public bool Force { get; set; }

            [System.ComponentModel.TypeConverter(typeof(System.ComponentModel.GuidConverter))]
            public Guid Id { get; set; }

            public int Number { get; set; }

            public decimal Amount { get; set; }

            public float Angle { get; set; }

            public double PrecisionAngle { get; set; }

            [System.ComponentModel.DefaultValue(88888888888)]
            public long BigNumber { get; set; }

            [ArgsTypeConverterAttribute(typeof(SimpleConverter))]
            public int ConverterProperty { get; set; }
        }
        #endregion

        [Test]
        public void BasicConventionsTest()
        {
            var m = Configuration.Configure<SimpleModelClass>(new ConventionBasedModelDefinitionInitializer());

            Assert.AreEqual(1, m.GetOrdinalArguments().Count());
            Assert.Contains(m.Members.GetMemberBindingDefinitionFor(a => a.Name).MemberInfo, m.GetOrdinalArguments().ToList());
            Assert.AreEqual(StringComparer.CurrentCultureIgnoreCase, m.StringComparer);
            Assert.AreEqual("/", m.SwitchDelimiter);
            Assert.IsEmpty(m.TypeConverters);

            var member = m.Members.GetMemberBindingDefinitionFor(a => a.Amount);
            Assert.IsNull(member.DefaultValue);
            Assert.AreSame(m, member.Parent);
            Assert.AreEqual(2, member.SwitchValues.Count);
            Assert.IsTrue(member.CanHandleSwitch("am"));
            Assert.IsTrue(member.CanHandleSwitch("amount"));
            Assert.IsNull(member.TypeConverter);
            Assert.IsNull(member.HelpText);

            member = m.Members.GetMemberBindingDefinitionFor(a => a.Angle);
            Assert.IsNull(member.DefaultValue);
            Assert.AreSame(m, member.Parent);
            Assert.AreEqual(2, member.SwitchValues.Count);
            Assert.IsTrue(member.CanHandleSwitch("an"));
            Assert.IsTrue(member.CanHandleSwitch("angle"));
            Assert.IsNull(member.TypeConverter);
            Assert.IsNull(member.HelpText);

            member = m.Members.GetMemberBindingDefinitionFor(a => a.BigNumber);
            Assert.AreEqual(88888888888, member.DefaultValue);
            Assert.AreSame(m, member.Parent);
            Assert.AreEqual(2, member.SwitchValues.Count);
            Assert.IsTrue(member.CanHandleSwitch("b"));
            Assert.IsTrue(member.CanHandleSwitch("bignumber"));
            Assert.IsNull(member.TypeConverter);
            Assert.IsNull(member.HelpText);

            member = m.Members.GetMemberBindingDefinitionFor(a => a.Force);
            Assert.IsNull(member.DefaultValue);
            Assert.AreSame(m, member.Parent);
            Assert.AreEqual(2, member.SwitchValues.Count);
            Assert.IsTrue(member.CanHandleSwitch("f"));
            Assert.IsTrue(member.CanHandleSwitch("force"));
            Assert.IsNull(member.TypeConverter);
            Assert.AreEqual("Forces the command", member.HelpText);

            member = m.Members.GetMemberBindingDefinitionFor(a => a.Id);
            Assert.IsNull(member.DefaultValue);
            Assert.AreSame(m, member.Parent);
            Assert.AreEqual(2, member.SwitchValues.Count);
            Assert.IsTrue(member.CanHandleSwitch("i"));
            Assert.IsTrue(member.CanHandleSwitch("id"));
            Assert.AreEqual(typeof(GuidConverter), member.TypeConverter.GetType());
            Assert.IsNull(member.HelpText);

            member = m.Members.GetMemberBindingDefinitionFor(a => a.Name);
            Assert.IsNull(member.DefaultValue);
            Assert.AreSame(m, member.Parent);
            Assert.IsEmpty(member.SwitchValues);
            Assert.IsNull(member.TypeConverter);
            Assert.IsNull(member.HelpText);

            member = m.Members.GetMemberBindingDefinitionFor(a => a.Number);
            Assert.IsNull(member.DefaultValue);
            Assert.AreSame(m, member.Parent);
            Assert.AreEqual(2, member.SwitchValues.Count);
            Assert.IsTrue(member.CanHandleSwitch("nu"));
            Assert.IsTrue(member.CanHandleSwitch("number"));
            Assert.IsNull(member.TypeConverter);
            Assert.IsNull(member.HelpText);

            member = m.Members.GetMemberBindingDefinitionFor(a => a.PrecisionAngle);
            Assert.IsNull(member.DefaultValue);
            Assert.AreSame(m, member.Parent);
            Assert.AreEqual(2, member.SwitchValues.Count);
            Assert.IsTrue(member.CanHandleSwitch("p"));
            Assert.IsTrue(member.CanHandleSwitch("P"));
            Assert.IsTrue(member.CanHandleSwitch("precisionangle"));
            Assert.IsTrue(member.CanHandleSwitch("pReCiSionAnGlE"));
            Assert.IsNull(member.TypeConverter);
            Assert.IsNull(member.HelpText);

            member = m.Members.GetMemberBindingDefinitionFor(a => a.StartDate);
            Assert.IsNull(member.DefaultValue);
            Assert.AreSame(m, member.Parent);
            Assert.AreEqual(1, member.SwitchValues.Count);
            Assert.IsTrue(member.CanHandleSwitch("sd"));
            Assert.IsTrue(member.CanHandleSwitch("sD"));
            Assert.IsNull(member.TypeConverter);
            Assert.IsNull(member.HelpText);
        }
    }
}
