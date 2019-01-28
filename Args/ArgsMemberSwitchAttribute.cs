using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Args
{
    /// <summary>
    /// Decorate a field or property with this attribute to either specify its switches or if it is an ordinal argument
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ArgsMemberSwitchAttribute : Attribute
    {
        /// <summary>
        /// Allowable swith values
        /// </summary>
        public string[] SwitchValues { get; private set; }
        /// <summary>
        /// 0-based position of ordinal argument
        /// </summary>
        public int? ArgumentIndex { get; private set; }

        /// <summary>
        /// Initialize as a switch argument
        /// </summary>
        /// <param name="switchValues"></param>
        public ArgsMemberSwitchAttribute(params string[] switchValues)
        {
            SwitchValues = switchValues;
        }

        /// <summary>
        /// Initialize as an ordinal argument
        /// </summary>
        /// <param name="argumentIndex"></param>
        public ArgsMemberSwitchAttribute(int argumentIndex)
        {
            ArgumentIndex = argumentIndex;
        }

    }
}
