using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SharpTestsEx;

namespace Args.Tests
{
    [TestFixture]
    public class InheritanceConventionsTest
    {
        #region Model Under Test

        public class SimpleBaseModel
        {
            [ArgsMemberSwitch(0)]
            public string Name { get; set; }

            [ArgsMemberSwitch("sd")]
            public DateTime StartDate { get; set; }

            [System.ComponentModel.Description("Forces the command")]
            public bool Force { get; set; }

            [System.ComponentModel.TypeConverter(typeof(System.ComponentModel.GuidConverter))]
            public Guid Id { get; set; }
        }

        public class SimpleModelClassWithBase : SimpleBaseModel
        {
            [System.ComponentModel.TypeConverter("System.ComponentModel.Int32Converter, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
            public int Number { get; set; }

            public decimal Amount { get; set; }

            public float Angle { get; set; }

            public double PrecisionAngle { get; set; }

            [System.ComponentModel.DefaultValue(88888888888)]
            public long BigNumber { get; set; }
        }
        #endregion

        [Test]
        public void BasicConventionsTestWithInheritence()
        {
            var m = Configuration.Configure<SimpleModelClassWithBase>(new ConventionBasedModelDefinitionInitializer());

            m.GetOrdinalArguments().Count().Should().Be.EqualTo(1);
            m.GetOrdinalArguments().Should().Contain(m.Members.GetMemberBindingDefinitionFor(a => a.Name).MemberInfo);
            m.StringComparer.Should().Be.EqualTo(StringComparer.CurrentCultureIgnoreCase);
            m.SwitchDelimiter.Should().Be.EqualTo("/");
            m.TypeConverters.Should().Be.Empty();

            var member = m.Members.GetMemberBindingDefinitionFor(a => a.Amount);
            member.DefaultValue.Should().Be.Null();
            member.Parent.Should().Be.EqualTo(m);
            member.SwitchValues.Count().Should().Be.EqualTo(2);
            member.CanHandleSwitch("am").Should().Be.True();
            member.CanHandleSwitch("amount").Should().Be.True();
            member.TypeConverter.Should().Be.Null();
            member.HelpText.Should().Be.Null();

            member = m.Members.GetMemberBindingDefinitionFor(a => a.Angle);
            member.DefaultValue.Should().Be.Null();
            member.Parent.Should().Be.EqualTo(m);
            member.SwitchValues.Count().Should().Be.EqualTo(2);
            member.CanHandleSwitch("an").Should().Be.True();
            member.CanHandleSwitch("angle").Should().Be.True();
            member.TypeConverter.Should().Be.Null();
            member.HelpText.Should().Be.Null();

            member = m.Members.GetMemberBindingDefinitionFor(a => a.BigNumber);
            member.DefaultValue.Should().Be.EqualTo(88888888888);
            member.Parent.Should().Be.EqualTo(m);
            member.SwitchValues.Count().Should().Be.EqualTo(2);
            member.CanHandleSwitch("b").Should().Be.True();
            member.CanHandleSwitch("bignumber").Should().Be.True();
            member.TypeConverter.Should().Be.Null();
            member.HelpText.Should().Be.Null();

            member = m.Members.GetMemberBindingDefinitionFor(a => a.Force);
            member.DefaultValue.Should().Be.Null();
            member.Parent.Should().Be.EqualTo(m);
            member.SwitchValues.Count().Should().Be.EqualTo(2);
            member.CanHandleSwitch("f").Should().Be.True();
            member.CanHandleSwitch("force").Should().Be.True();
            member.TypeConverter.Should().Be.Null();
            member.HelpText.Should().Be.EqualTo("Forces the command");

            member = m.Members.GetMemberBindingDefinitionFor(a => a.Id);
            member.DefaultValue.Should().Be.Null();
            member.Parent.Should().Be.EqualTo(m);
            member.SwitchValues.Count().Should().Be.EqualTo(2);
            member.CanHandleSwitch("i").Should().Be.True();
            member.CanHandleSwitch("id").Should().Be.True();
            member.TypeConverter.Should().Be.OfType<System.ComponentModel.GuidConverter>();
            member.HelpText.Should().Be.Null();

            member = m.Members.GetMemberBindingDefinitionFor(a => a.Name);
            member.DefaultValue.Should().Be.Null();
            member.Parent.Should().Be.EqualTo(m);
            member.SwitchValues.Should().Be.Empty();
            member.TypeConverter.Should().Be.Null();
            member.HelpText.Should().Be.Null();

            member = m.Members.GetMemberBindingDefinitionFor(a => a.Number);
            member.DefaultValue.Should().Be.Null();
            member.Parent.Should().Be.EqualTo(m);
            member.SwitchValues.Count().Should().Be.EqualTo(2);
            member.CanHandleSwitch("nu").Should().Be.True();
            member.CanHandleSwitch("number").Should().Be.True();
            member.TypeConverter.Should().Be.OfType<System.ComponentModel.Int32Converter>();
            member.HelpText.Should().Be.Null();

            member = m.Members.GetMemberBindingDefinitionFor(a => a.PrecisionAngle);
            member.DefaultValue.Should().Be.Null();
            member.Parent.Should().Be.EqualTo(m);
            member.SwitchValues.Count().Should().Be.EqualTo(2);
            member.CanHandleSwitch("p").Should().Be.True();
            member.CanHandleSwitch("P").Should().Be.True();
            member.CanHandleSwitch("precisionangle").Should().Be.True();
            member.CanHandleSwitch("pReCiSionAnGlE").Should().Be.True();
            member.TypeConverter.Should().Be.Null();
            member.HelpText.Should().Be.Null();

            member = m.Members.GetMemberBindingDefinitionFor(a => a.StartDate);
            member.DefaultValue.Should().Be.Null();
            member.Parent.Should().Be.EqualTo(m);
            member.SwitchValues.Count().Should().Be.EqualTo(1);
            member.CanHandleSwitch("sd").Should().Be.True();
            member.CanHandleSwitch("SD").Should().Be.True();
            member.TypeConverter.Should().Be.Null();
            member.HelpText.Should().Be.Null();
        }
    }
}
