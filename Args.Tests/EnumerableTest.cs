using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SharpTestsEx;
using System.Collections.ObjectModel;

namespace Args.Tests
{
    [TestFixture]
    public class EnumerableTest
    {
        #region Model Under Test
        public class SwitchOnlyModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public IEnumerable<DateTime> Date { get; set; }
            public IList<int> Quantities { get; set; }
            public ICollection<decimal> Totals { get; set; }
            public bool[] Switches { get; set; }
            public List<float> Radians { get; set; }
        }
        #endregion

        Lazy<IModelBindingDefinition<SwitchOnlyModel>> switchOnlyModelUnderTest;
        Lazy<IModelBindingDefinition<OrdinalModel>> ordinalModelUnderTest;

        [SetUp]        
        public void Setup()
        {
            switchOnlyModelUnderTest = new Lazy<IModelBindingDefinition<SwitchOnlyModel>>(() => Configuration.Configure<SwitchOnlyModel>());
            ordinalModelUnderTest = new Lazy<IModelBindingDefinition<OrdinalModel>>(() => Configuration.Configure<OrdinalModel>());
        }

        [Test]
        public void TestWithSwitchedParameters()
        {
            var args = new[] { "/i", "123", "/n", "My Name", "/d", "1/1/2000", "1/1/2001", "/q", "5", "9", "100", "/t", "12.01", "10.55", "43", "107.6", "/s", "true", "true", "false", "false", "false", "/r", "1.2", "2.3" };

            var test = switchOnlyModelUnderTest.Value.CreateAndBind(args);
            test.Id.Should().Be.EqualTo(123);
            test.Name.Should().Be.EqualTo("My Name");
            
            test.Date.SequenceEqual(new[] { new DateTime(2000, 1, 1), new DateTime(2001, 1, 1) }).Should().Be.True();
            test.Quantities.SequenceEqual(new[] { 5, 9, 100 }).Should().Be.True();
            test.Totals.SequenceEqual(new[] { 12.01M, 10.55M, 43M, 107.6M }).Should().Be.True();
            test.Switches.SequenceEqual(new[] { true, true, false, false, false}).Should().Be.True();
            test.Radians.SequenceEqual(new[] { 1.2f, 2.3f }).Should().Be.True();
        }

        #region Model Under Test
        public class OrdinalModel
        {
            [ArgsMemberSwitch(0)]
            public int Id { get; set; }
            [ArgsMemberSwitch(1)]
            public IEnumerable<string> FileNames { get; set; }
        }
        #endregion

        [Test]
        public void TestWithOrdinalParameters()
        {
            var args = new[] { "5", "Test1.txt", "test5.txt", "backup.sql" };

            var test = ordinalModelUnderTest.Value.CreateAndBind(args);
            test.Id.Should().Be.EqualTo(5);

            test.FileNames.SequenceEqual(new[] { "Test1.txt", "test5.txt", "backup.sql" }).Should().Be.True();
        }

        #region Model Under Test
        public class InvalidModel
        {
            [ArgsMemberSwitch(1)]
            public int Id { get; set; }

            [ArgsMemberSwitch(0)]
            public IEnumerable<string> Names { get; set; }
        }
        #endregion

        [Test]
        public void ErrorTest()
        {
            Executing.This(() => Configuration.Configure<InvalidModel>()).Should().Throw<InvalidOperationException>();
        }
    }
}
