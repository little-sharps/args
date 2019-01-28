using System;

namespace Args
{
    /// <summary>
    /// Exception thrown when the binding definition is not valid
    /// </summary>
    [Serializable]
    public class BindingDefinitionException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public BindingDefinitionException() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public BindingDefinitionException(string message) : base(message) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public BindingDefinitionException(string message, Exception inner) : base(message, inner) { }

#if NET_FRAMEWORK
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected BindingDefinitionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
#endif
    }
}
