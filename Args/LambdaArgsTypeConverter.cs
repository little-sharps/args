using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Args
{
    /// <summary>
    /// This class enables type conversion to be handled by a simple lambda expression
    /// </summary>
    public class LambdaArgsTypeConverter : IArgsTypeConverter
    {
        private readonly Func<string, object> converter;

        public LambdaArgsTypeConverter(Func<string, object> converter)
        {
            this.converter = converter;
        }

        public object Convert(string value)
        {
            return converter(value);
        }
    }
}
