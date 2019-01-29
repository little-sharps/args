using Args.Help.Formatters;
using NUnit.Framework;
using System;


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

            public bool Work { get; set; }

            [System.ComponentModel.Description("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
            public string Descripton { get; set; }

            [ArgsMemberSwitch(0)]
            [System.ComponentModel.Description("The name of the file")]
            public string FileName { get; set; }
        }
        #endregion

        private const string HelpOutput = @"This is my console application
<command> FileName [/Id|/I] [/Name|/N] [/Switch|/S] [/Date|/Da] [/Work|/W] 
          [/Descripton|/De]


FileName              The name of the file
[/Id|/I]              This is the Id
[/Name|/N]            This is the name you should put in. This is an extremely 
                      long description that should take multiple lines to 
                      output. The formating should still be maintained and 
                      should look good in the output.
[/Switch|/S]          Force it!
[/Date|/Da]           Effective date
[/Descripton|/De]     aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
                      aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
                      aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa

";

        [Test]
        public void VerifyHelpOutput()
        {
            var definition = Configuration.Configure<HelpModelTest>();
            var help = new Help.HelpProvider().GenerateModelHelp(definition);

            var f = new ConsoleHelpFormatter(80, 1, 5);
            Assert.AreEqual(HelpOutput, f.GetHelp(help));            
        }

        #region Model Under Test
        public class EmptyHelpModelTest
        {

        }
        #endregion

        [Test]
        public void VerifyEmptyModelTestDoesNotBreak()
        {
            var definition = Configuration.Configure<EmptyHelpModelTest>();
            var help = new Help.HelpProvider().GenerateModelHelp(definition);

            var f = new ConsoleHelpFormatter(80, 1, 5);

            Assert.AreEqual("<command> \r\n\r\n\r\n", f.GetHelp(help));
        }
    }
}
