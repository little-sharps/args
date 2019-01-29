using NUnit.Framework;
using System;

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

            Assert.IsEmpty(m.GetOrdinalArguments());
            Assert.AreEqual(StringComparer.CurrentCulture, m.StringComparer);
            Assert.AreEqual("-", m.SwitchDelimiter);

            var member = m.Members.GetMemberBindingDefinitionFor(a => a.Id);
            Assert.IsNull(member.DefaultValue);
            Assert.AreSame(m, member.Parent);
            Assert.AreEqual(2, member.SwitchValues.Count);
            Assert.IsTrue(member.CanHandleSwitch("I"));
            Assert.IsFalse(member.CanHandleSwitch("i"));
            Assert.IsTrue(member.CanHandleSwitch("Id"));
            Assert.IsNull(member.TypeConverter);
            Assert.IsNull(member.HelpText);
        }        
    }
}
