using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Args
{
    /// <summary>
    /// A simple interface to impelment for type conversion from string. If a TypeConverter for the type does not already exist, this is the easiest way to do type conversion in Args
    /// </summary>
    public interface IArgsTypeConverter
    {
        object Convert(string value);
    }
}
