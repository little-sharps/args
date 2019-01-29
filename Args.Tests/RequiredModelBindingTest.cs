using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.ComponentModel.DataAnnotations;

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

        private IModelBindingDefinition<ModelWithRequiredField> modelWithRequiredFieldDefinition;

        [SetUp]
        public void SetUp()
        {
            modelWithRequiredFieldDefinition = Configuration.Configure<ModelWithRequiredField>();
        }

        [Test]
        public void HappyPath()
        {
            var args = new[] { "/n", "my name", "/i", "5" };

            var test = modelWithRequiredFieldDefinition.CreateAndBind(args);

            Assert.AreEqual(5, test.Id);
            Assert.AreEqual("my name", test.Name);
        }

        [Test]
        public void RequiredFieldMissing()
        {
            var args = new[] { "/n", "my name" };

            void executingThis() { modelWithRequiredFieldDefinition.CreateAndBind(args); }

            Assert.Throws(new ExceptionTypeConstraint(typeof(InvalidOperationException)), executingThis);
        }
    }
}
