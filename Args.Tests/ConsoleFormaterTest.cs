using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.IO;
using Args.Help.Formatters;
using SharpTestsEx;

namespace Args.Tests
{
    [TestFixture]
    public class ConsoleFormaterTest
    {
        #region Model Under Test
        [System.ComponentModel.Description("This is my console application")]
        public class HelpModelTest
        {
            [System.ComponentModel.Description("This is the Id")]
            public int Id { get; set; }

            [System.ComponentModel.Description("This is the name you should put in. This is an extremely long description that should take multiple lines to output. The formating should still be maintained and should look good in the output.")]
            public string Name { get; set; }

            [System.ComponentModel.Description("Force it!")]
            public bool Switch { get; set; }

            [System.ComponentModel.Description("Effective date")]
            public DateTime Date { get; set; }

            [System.ComponentModel.Description("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
            public string Descripton { get; set; }

            [ArgsMemberSwitch(0)]
            [System.ComponentModel.Description("The name of the file")]
            public string FileName { get; set; }

            public bool Work { get; set; }
        }
        #endregion

        [Test]
        public void VerifyHelpOutput()
        {
            var definition = Configuration.Configure<HelpModelTest>();
            var help = new Help.HelpProvider().GenerateModelHelp(definition);

            ConsoleHelpFormatter f = new ConsoleHelpFormatter(80, 1, 5);

            f.GetHelp(help).Should().Be.EqualTo(Properties.Resources.HelpOutput);
        }
    }
}
