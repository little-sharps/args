using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SharpTestsEx;

namespace Args.Tests
{
    [TestFixture]
    public class SimpleModelWithNullableValueTypeConventionsTest
    {
        #region Model Under Test
        [ArgsModel(StringComparison = StringComparison.CurrentCulture, SwitchDelimiter = "-")]
        public class ModelWithNullableProperty
        {
            public Guid? Id { get; set; }
        }
        #endregion

        [Test]
        public void DecoratedModelTest()
        {
            var m = Configuration.Configure<ModelWithNullableProperty>(new ConventionBasedModelDefinitionInitializer());

            m.GetOrdinalArguments().Should().Be.Empty();
            m.StringComparer.Should().Be.EqualTo(StringComparer.CurrentCulture);
            m.SwitchDelimiter.Should().Be.EqualTo("-");

            var member = m.Members.GetMemberBindingDefinitionFor(a => a.Id);
            member.DefaultValue.Should().Be.Null();
            member.Parent.Should().Be.EqualTo(m);
            member.SwitchValues.Count().Should().Be.EqualTo(2);
            member.CanHandleSwitch("I").Should().Be.True();
            member.CanHandleSwitch("i").Should().Be.False();
            member.CanHandleSwitch("Id").Should().Be.True();
            member.TypeConverter.Should().Be.Null();
            member.HelpText.Should().Be.Null();
        }        
    }
}
