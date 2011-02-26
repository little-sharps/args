using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Args
{
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
