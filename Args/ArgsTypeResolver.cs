using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Args
{
    /// <summary>
    /// Holds a static reference to an IServiceProvider; if one is not specified, a default one will be used that delegates to <see cref="Activator.CreateInstance(Type)"/> and assumes a default constructor
    /// </summary>
    public class ArgsTypeResolver : IServiceProvider
    {
        /// <summary>
        /// The <see cref="IServiceProvider" /> used by Args
        /// </summary>
        public static IServiceProvider Current { get; set; }

        static ArgsTypeResolver()
        {
            Current = new ArgsTypeResolver();
        }

        /// <summary>
        /// Creates the specified service, assumes a public default constructor exists
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public object GetService(Type serviceType)
        {
            try
            {
                return Activator.CreateInstance(serviceType
#if NET_FRAMEWORK
                    //In .NET Core, using true causes an exception, root cause unknown for now
                    , true
#endif
                    );
            }
            catch (MissingMethodException ex)
            {
                throw new InvalidOperationException(String.Format("Cannot create instance of type {0}; no public default constructor found.  To override this behavior, impelment System.IServiceProvider and set the ArgsTypeResolver.Current static property.", serviceType.FullName), ex);
            }
        }
    }
}
