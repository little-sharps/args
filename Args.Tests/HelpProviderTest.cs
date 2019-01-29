using Args.Help;
using NUnit.Framework;
using System;
using System.Linq;

namespace Args.Tests
{
    [TestFixture]
    public class HelpProviderTest
    {
        #region Model Under Test
        [ArgsModel(SwitchDelimiter = "//")]
        [SimpleResourceMemberHelp("This is my console application")]
        public class HelpClassTest
        {
            [System.ComponentModel.Description("This is the Id")]
            public int Id { get; set; }

            [System.ComponentModel.Description("This is the name you should put in.")]
            public string Name { get; set; }

            [System.ComponentModel.Description("Force it!")]
            public bool Switch { get; set; }

            [System.ComponentModel.Description("Effective date")]
            [ArgsMemberSwitch(0)]
            public DateTime Date { get; set; }


        }
        #endregion

        [Test]
        public void VerifyHelpData()
        {
            var config = Configuration.Configure<HelpClassTest>();
            var help = new HelpProvider();

            var result = help.GenerateModelHelp(config);

            Assert.AreEqual("//", result.SwitchDelimiter);
            Assert.AreEqual(4, result.Members.Count());
            Assert.AreEqual("This is my console application", result.HelpText);

            var m = result.Members.Where(h => h.Name == "Id").Single();
            Assert.AreEqual("This is the Id", m.HelpText);
            Assert.AreEqual(default(int?), m.OrdinalIndex);
            Assert.IsTrue(new[] { "Id", "I" }.SequenceEqual(m.Switches));

            m = result.Members.Where(h => h.Name == "Name").Single();
            Assert.AreEqual("This is the name you should put in.", m.HelpText);
            Assert.AreEqual(default(int?), m.OrdinalIndex);
            Assert.IsTrue(new[] { "Name", "N" }.SequenceEqual(m.Switches));

            m = result.Members.Where(h => h.Name == "Switch").Single();
            Assert.AreEqual("Force it!", m.HelpText);
            Assert.AreEqual(default(int?), m.OrdinalIndex);
            Assert.IsTrue(new[] { "Switch", "S" }.SequenceEqual(m.Switches));

            m = result.Members.Where(h => h.Name == "Date").Single();
            Assert.AreEqual("Effective date", m.HelpText);
            Assert.Zero(m.OrdinalIndex.Value);
            Assert.IsEmpty(m.Switches);
        }
    }
}
