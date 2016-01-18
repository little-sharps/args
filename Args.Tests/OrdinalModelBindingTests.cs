using NUnit.Framework;
using SharpTestsEx;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

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

            ordinalArguments.Count().Should().Be.EqualTo(3);

            ordinalArguments.ElementAt(0).Should().Be.EqualTo(m.Members.GetMemberBindingDefinitionFor(a => a.Name).MemberInfo);
            ordinalArguments.ElementAt(1).Should().Be.EqualTo(m.Members.GetMemberBindingDefinitionFor(a => a.Count).MemberInfo);
            ordinalArguments.ElementAt(2).Should().Be.EqualTo(m.Members.GetMemberBindingDefinitionFor(a => a.Value).MemberInfo);            
        }

        [Test]
        public void BindingTestHappyPath()
        {
            var args = new[] { "John Smith", "20", "99.99" };

            var c = Configuration.Configure<OrdinalModel>().CreateAndBind(args);

            c.Name.Should().Be.EqualTo(args[0]);
            c.Count.Should().Be.EqualTo(int.Parse(args[1]));
            c.Value.Should().Be.EqualTo(decimal.Parse(args[2], CultureInfo.InvariantCulture));
        }

        [Test]
        public void BindingTestTooFewOrdinalArgs()
        {
            var args = new[] { "John Smith", "20" };

            var m = Configuration.Configure<OrdinalModel>();

            var exception = Executing.This(() => m.CreateAndBind(args))
                .Should().Throw<InvalidOperationException>()
                .And.Exception.Message
                .Should()
                .EndWith(string.Concat(m.GetOrdinalArguments().Count().ToString(), "."));
        }
    }
    
}
