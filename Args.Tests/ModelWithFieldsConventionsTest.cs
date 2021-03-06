﻿using NUnit.Framework;
using System;
using System.ComponentModel;

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

            Assert.IsEmpty(m.GetOrdinalArguments());
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
            Assert.IsNull(member.DefaultValue);
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
            Assert.IsNull(member.HelpText);

            member = m.Members.GetMemberBindingDefinitionFor(a => a.Id);
            Assert.IsNull(member.DefaultValue);
            Assert.AreSame(m, member.Parent);
            Assert.AreEqual(2, member.SwitchValues.Count);
            Assert.IsTrue(member.CanHandleSwitch("i"));
            Assert.IsTrue(member.CanHandleSwitch("id"));
            Assert.IsNull(member.TypeConverter);
            Assert.IsNull(member.HelpText);

            member = m.Members.GetMemberBindingDefinitionFor(a => a.Name);
            Assert.IsNull(member.DefaultValue);
            Assert.AreSame(m, member.Parent);
            Assert.AreEqual(2, member.SwitchValues.Count);
            Assert.IsTrue(member.CanHandleSwitch("na"));
            Assert.IsTrue(member.CanHandleSwitch("name"));
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
            Assert.AreEqual(2, member.SwitchValues.Count);
            Assert.IsTrue(member.CanHandleSwitch("s"));
            Assert.IsTrue(member.CanHandleSwitch("Startdate"));
            Assert.IsNull(member.TypeConverter);
            Assert.IsNull(member.HelpText);
        }
    }
}
