using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Args.Help;
using SharpTestsEx;

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

            result.SwitchDelimiter.Should().Be.EqualTo("//");
            result.Members.Count().Should().Be.EqualTo(4);
            result.HelpText.Should().Be.EqualTo("This is my console application");

            var m = result.Members.Where(h => h.Name == "Id").Single();
            m.HelpText.Should().Be.EqualTo("This is the Id");
            m.OrdinalIndex.Should().Be.EqualTo(default(int?));
            m.Switches.Should().Have.SameSequenceAs(new[] { "Id", "I" });

            m = result.Members.Where(h => h.Name == "Name").Single();
            m.HelpText.Should().Be.EqualTo("This is the name you should put in.");
            m.OrdinalIndex.Should().Be.EqualTo(default(int?));
            m.Switches.Should().Have.SameSequenceAs(new[] { "Name", "N" });

            m = result.Members.Where(h => h.Name == "Switch").Single();
            m.HelpText.Should().Be.EqualTo("Force it!");
            m.OrdinalIndex.Should().Be.EqualTo(default(int?));
            m.Switches.Should().Have.SameSequenceAs(new[] { "Switch", "S" });

            m = result.Members.Where(h => h.Name == "Date").Single();
            m.HelpText.Should().Be.EqualTo("Effective date");
            m.OrdinalIndex.Should().Be.EqualTo(0);
            m.Switches.Should().Be.Empty();
        }
    }
}
