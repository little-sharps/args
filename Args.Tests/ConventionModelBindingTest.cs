using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SharpTestsEx;
using System.ComponentModel;

namespace Args.Tests
{
    [TestFixture]
    public class ConventionModelBindingTest
    {
        IModelBindingDefinition<SimpleTestModel> definitionUnderTest;

        #region Model Under Test
        public class SimpleTestModel
        {
            public string Name { get; set; }
            [DefaultValue(20)]
            public int Id;
            [ArgsMemberSwitch(0)]
            public string FileName { get; set; }
            public bool Force;
            public FanSpeed Speed { get; set; }
        }

        public enum FanSpeed
        {
            Low = 1,
            Medium = 2,
            High = 4,
        }
        #endregion

        [SetUp]
        public void SimpleModelConfigurationSetup()
        {
            definitionUnderTest = Configuration.Configure<SimpleTestModel>();
        }

        [Test]
        public void SimpleModelBindingTest()
        {
            var args = new[] { "MyAssembly.dll", "/id", "223", "/fo", "/n", "My Name", "/s", "Low,", "Medium" };

            var result = definitionUnderTest.CreateAndBind(args);

            result.FileName.Should().Be.EqualTo("MyAssembly.dll");
            result.Id.Should().Be.EqualTo(223);
            result.Name.Should().Be.EqualTo("My Name");
            result.Force.Should().Be.True();
            result.Speed.Should().Be.EqualTo(FanSpeed.Low | FanSpeed.Medium);
        }

        [Test]
        public void TestWithFlagNotSet()
        {
            var args = new[] { "MyAssembly.dll", "/id", "223", "/n", "My Name", "/s", "Low,", "Medium" };

            var result = definitionUnderTest.CreateAndBind(args);

            result.FileName.Should().Be.EqualTo("MyAssembly.dll");
            result.Id.Should().Be.EqualTo(223);
            result.Name.Should().Be.EqualTo("My Name");
            result.Force.Should().Be.False();
            result.Speed.Should().Be.EqualTo(FanSpeed.Low | FanSpeed.Medium);
        }

        [Test]
        public void TestAssertingDefaultValueIsUsed()
        {
            var args = new[] { "MyAssembly.dll", "/fo", "/n", "My Name", "/s", "Low,", "Medium" };

            var result = definitionUnderTest.CreateAndBind(args);

            result.FileName.Should().Be.EqualTo("MyAssembly.dll");
            result.Id.Should().Be.EqualTo(20);
            result.Name.Should().Be.EqualTo("My Name");
            result.Force.Should().Be.True();
            result.Speed.Should().Be.EqualTo(FanSpeed.Low | FanSpeed.Medium);
        }
    }
}
