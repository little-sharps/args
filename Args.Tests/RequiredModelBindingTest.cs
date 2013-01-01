using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.ComponentModel.DataAnnotations;
using SharpTestsEx;

namespace Args.Tests
{
    [TestFixture]
    public class RequiredModelBindingTest
    {
        #region Model Under Test
        public class ModelWithRequiredField
        {
            [Required]
            public int Id { get; set; }
            public string Name { get; set; }
        }
        #endregion

        private Lazy<IModelBindingDefinition<ModelWithRequiredField>> modelWithRequiredFieldDefinition;

        [SetUp]
        public void SetUp()
        {
            modelWithRequiredFieldDefinition = new Lazy<IModelBindingDefinition<ModelWithRequiredField>>(() => Configuration.Configure<ModelWithRequiredField>());
        }

        [Test]
        public void HappyPath()
        {
            var args = new[] { "/n", "my name", "/i", "5" };

            var test = modelWithRequiredFieldDefinition.Value.CreateAndBind(args);

            test.Id.Should().Be.EqualTo(5);
            test.Name.Should().Be.EqualTo("my name");
        }

        [Test]
        public void RequiredFieldMissing()
        {
            var args = new[] { "/n", "my name" };

            Executing.This(() => modelWithRequiredFieldDefinition.Value.CreateAndBind(args)).Should().Throw<InvalidOperationException>();
        }
    }
}
