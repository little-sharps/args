using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Globalization;
using System.Linq;

namespace Args.Tests
{
    [TestFixture]
    public class OrdinalModelBindingTests
    {
        #region Model Under Test 
        public class OrdinalModel
        {
            [ArgsMemberSwitch(0)]
            public string Name { get; set; }

            [ArgsMemberSwitch(1)]
            public int Count { get; set; }

            [ArgsMemberSwitch(2)]
            public decimal Value { get; set; }
        }
        #endregion

        [Test]
        public void BasicConventionsTest()
        {
            var m = Configuration.Configure<OrdinalModel>();

            var ordinalArguments = m.GetOrdinalArguments();

            Assert.AreEqual(3, ordinalArguments.Count());


            Assert.IsTrue(new[]
            {
                m.Members.GetMemberBindingDefinitionFor(a => a.Name).MemberInfo,
                m.Members.GetMemberBindingDefinitionFor(a => a.Count).MemberInfo,
                m.Members.GetMemberBindingDefinitionFor(a => a.Value).MemberInfo,
            }.SequenceEqual(ordinalArguments));
        }

        [Test]
        public void BindingTestHappyPath()
        {
            var args = new[] { "John Smith", "20", "99.99" };

            var c = Configuration.Configure<OrdinalModel>().CreateAndBind(args);

            Assert.AreEqual(args[0], c.Name);
            Assert.AreEqual(int.Parse(args[1]), c.Count);
            Assert.AreEqual(decimal.Parse(args[2], CultureInfo.InvariantCulture), c.Value);
        }

        [Test]
        public void BindingTestTooFewOrdinalArgs()
        {
            var args = new[] { "John Smith", "20" };

            var m = Configuration.Configure<OrdinalModel>();

            void executeThis() { m.CreateAndBind(args); }

            var exception = Assert.Throws(new ExceptionTypeConstraint(typeof(InvalidOperationException)), executeThis);

            Assert.IsTrue(exception.Message.EndsWith(m.GetOrdinalArguments().Count().ToString() + "."));
        }
    }
    
}
