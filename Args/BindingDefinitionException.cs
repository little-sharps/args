using System;

namespace Args
{
    [Serializable]
    public class BindingDefinitionException : Exception
    {
        public BindingDefinitionException() { }
        public BindingDefinitionException(string message) : base(message) { }
        public BindingDefinitionException(string message, Exception inner) : base(message, inner) { }

#if NET_FRAMEWORK
        protected BindingDefinitionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
#endif
    }
}
