using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Args
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ArgsMemberSwitchAttribute : Attribute
    {
        public string[] SwitchValues { get; private set; }
        public int? ArgumentIndex { get; private set; }

        public ArgsMemberSwitchAttribute(params string[] switchValues)
        {
            SwitchValues = switchValues;
        }

        public ArgsMemberSwitchAttribute(int argumentIndex)
        {
            ArgumentIndex = argumentIndex;
        }

    }
}
