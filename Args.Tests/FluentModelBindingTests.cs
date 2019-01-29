using NUnit.Framework;
using System;

namespace Args.Tests
{
    [TestFixture]
    public class FluentModelBindingTests
    {
        IModelBindingDefinition<SimpleTestModel> definitionUnderTest;

        #region Model Under Test
        public class SimpleTestModel
        {
            public string Name { get; set; }
            public int Id;
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
        public void SimpleModelFluentSetup()
        {
            definitionUnderTest = new ModelBindingDefinition<SimpleTestModel>()
            .AsFluent().UsingStringComparer(StringComparer.CurrentCulture)
            .UsingSwitchDelimiter("--")
            .ParsesArgumentsWith(typeof(int), new System.ComponentModel.Int16Converter())
            .HasFirstOrdinalArgumentOf(a => a.FileName)
            .ParsesArgumentsWith(typeof(string), s => String.IsNullOrEmpty(s) ? s : s.Substring(0, s.Length - 1))
            .ForMember(a => a.Name)
                .WatchesFor("name", "nam")
                .ParsesArgumentWith(s => s)
            .ForMember(a => a.Id)
                .WatchesFor("id")
                .HasHelpTextOf("Hello World")
            .ForMember(a => a.Force)
                .WatchesFor("f")
            .ForMember(a => a.Speed)
                .WatchesFor("fs")
            .Initialize();
        }

        [Test]
        public void SimpleModelBindingTest()
        {
            var args = new[]{"MyAssembly.dll", "--id", "223", "--f", "--nam", "My Name", "--fs", "Low,", "Medium"};

            var result = definitionUnderTest.CreateAndBind(args);

            Assert.AreEqual("MyAssembly.dl", result.FileName);
            Assert.AreEqual(223, result.Id);            
            Assert.AreEqual("My Name", result.Name);
            Assert.IsTrue(result.Force);
            Assert.AreEqual(FanSpeed.Low | FanSpeed.Medium, result.Speed);
        }
    }
}
