using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SharpTestsEx;

namespace Args.Tests
{
    [TestFixture]
    public class ModelWithFieldsConventionsTest
    {
        #region Model Under Test
        public class SimpleModelClassUsingFields
        {
            public string Name;
            public DateTime StartDate;
            public bool Force;
            public Guid Id;
            public int Number;
            public decimal Amount;
            public float Angle;
            public double PrecisionAngle;
            public long BigNumber;
        }
        #endregion

        [Test]
        public void ModelTestWithFields()
        {
            var m = Configuration.Configure<SimpleModelClassUsingFields>(new ConventionBasedModelDefinitionInitializer());

            m.GetOrdinalArguments().Should().Be.Empty();
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
            member.DefaultValue.Should().Be.Null();
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
            member.HelpText.Should().Be.Null();

            member = m.Members.GetMemberBindingDefinitionFor(a => a.Id);
            member.DefaultValue.Should().Be.Null();
            member.Parent.Should().Be.EqualTo(m);
            member.SwitchValues.Count().Should().Be.EqualTo(2);
            member.CanHandleSwitch("i").Should().Be.True();
            member.CanHandleSwitch("id").Should().Be.True();
            member.TypeConverter.Should().Be.Null();
            member.HelpText.Should().Be.Null();

            member = m.Members.GetMemberBindingDefinitionFor(a => a.Name);
            member.DefaultValue.Should().Be.Null();
            member.Parent.Should().Be.EqualTo(m);
            member.SwitchValues.Count().Should().Be.EqualTo(2);
            member.CanHandleSwitch("na").Should().Be.True();
            member.CanHandleSwitch("name").Should().Be.True();
            member.TypeConverter.Should().Be.Null();
            member.HelpText.Should().Be.Null();

            member = m.Members.GetMemberBindingDefinitionFor(a => a.Number);
            member.DefaultValue.Should().Be.Null();
            member.Parent.Should().Be.EqualTo(m);
            member.SwitchValues.Count().Should().Be.EqualTo(2);
            member.CanHandleSwitch("nu").Should().Be.True();
            member.CanHandleSwitch("number").Should().Be.True();
            member.TypeConverter.Should().Be.Null();
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
            member.SwitchValues.Count().Should().Be.EqualTo(2);
            member.CanHandleSwitch("s").Should().Be.True();
            member.CanHandleSwitch("Startdate").Should().Be.True();
            member.TypeConverter.Should().Be.Null();
            member.HelpText.Should().Be.Null();
        }
    }
}
