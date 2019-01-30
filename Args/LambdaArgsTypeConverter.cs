using System;

namespace Args
{
    /// <summary>
    /// This class enables type conversion to be handled by a simple lambda expression
    /// </summary>
    public class LambdaArgsTypeConverter : IArgsTypeConverter
    {
        private readonly Func<string, object> converter;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="converter"></param>
        public LambdaArgsTypeConverter(Func<string, object> converter)
        {
            this.converter = converter;
        }

        /// <summary>
        /// Converts the provided <paramref name="value"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public object Convert(string value)
        {
            return converter(value);
        }
    }
}
