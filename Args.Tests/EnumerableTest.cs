using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;

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
        
        IModelBindingDefinition<SwitchOnlyModel> switchOnlyModelUnderTest;
        IModelBindingDefinition<OrdinalModel> ordinalModelUnderTest;

        [SetUp]        
        public void Setup()
        {
            switchOnlyModelUnderTest = Configuration.Configure<SwitchOnlyModel>();
            ordinalModelUnderTest = Configuration.Configure<OrdinalModel>();
        }

        [Test]
        public void TestWithSwitchedParameters()
        {
            var args = new[] { "/i", "123", "/n", "My Name", "/d", "1/1/2000", "1/1/2001", "/q", "5", "9", "100", "/t", "12.01", "10.55", "43", "107.6", "/s", "true", "true", "false", "false", "false", "/r", "1.2", "2.3" };

            var test = switchOnlyModelUnderTest.CreateAndBind(args);

            Assert.AreEqual(123, test.Id);
            Assert.AreEqual("My Name", test.Name);
            Assert.IsTrue(new[] { new DateTime(2000, 1, 1), new DateTime(2001, 1, 1) }.SequenceEqual(test.Date));
            Assert.IsTrue(new[] { 5, 9, 100 }.SequenceEqual(test.Quantities));
            Assert.IsTrue(new[] { 12.01M, 10.55M, 43M, 107.6M }.SequenceEqual(test.Totals));
            Assert.IsTrue(new[] { true, true, false, false, false }.SequenceEqual(test.Switches));
            Assert.IsTrue(new[] { 1.2f, 2.3f }.SequenceEqual(test.Radians));
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

            var test = ordinalModelUnderTest.CreateAndBind(args);

            Assert.AreEqual(5, test.Id);
            Assert.IsTrue(new[] { "Test1.txt", "test5.txt", "backup.sql" }.SequenceEqual(test.FileNames));
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
            void execute() => Configuration.Configure<InvalidModel>();

            Assert.Throws(new ExceptionTypeConstraint(typeof(InvalidOperationException)), execute);
        }
    }
}
