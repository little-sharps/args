using System;

namespace Args
{
    /// <summary>
    /// An exception thrown if the provided args are not in a format that can be parsed
    /// </summary>
    [Serializable]    
    public class InvalidArgsFormatException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public InvalidArgsFormatException() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public InvalidArgsFormatException(string message) : base(message) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public InvalidArgsFormatException(string message, Exception inner) : base(message, inner) { }

#if !NETSTANDARD_1_3
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected InvalidArgsFormatException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
#endif
    }
}