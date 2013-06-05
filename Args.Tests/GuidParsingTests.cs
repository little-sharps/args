using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SharpTestsEx;

namespace Args.Tests
{
    [TestFixture]
    public class GuidParsingTests
    {
        public class SimpleModelForWeirdnessTest
        {
            public Guid? Id { get; set; }
            public string Name { get; set; }
        }

        [Test]
        public void TestForParsingWierdnessPasses()
        {
            var nameValue = "foo";
            var idValue = "c304cffd-f331-49a1-a4ba-0706f869de11";

            var modelBindingDefinition = Configuration.Configure<SimpleModelForWeirdnessTest>();
            var model = modelBindingDefinition.CreateAndBind(new string[] { "/N", nameValue, "/I", idValue });

            model.Name.Should().Be(nameValue);
            model.Id.ToString().Should().Be(idValue);
        }

        [Test]
        public void TestForParsingWierdnessFails()
        {
            var nameValue = "vi";
            var idValue = "c304cffd-f331-49a1-a4ba-0706f869de11";

            var modelBindingDefinition = Configuration.Configure<SimpleModelForWeirdnessTest>();
            var model = modelBindingDefinition.CreateAndBind(new string[] { "/N", nameValue, "/I", idValue });

            model.Name.Should().Be(nameValue);
            model.Id.ToString().Should().Be(idValue);
        }
    }
}
