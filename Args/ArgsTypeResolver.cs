using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Args
{
    /// <summary>
    /// Holds a static reference to an IServiceProvider.  If one is not specified, a default one will be use which simply uses Activator.CreateInstance and assumes a default constructor
    /// </summary>
    public class ArgsTypeResolver : IServiceProvider
    {        
        public static IServiceProvider Current { get; set; }

        static ArgsTypeResolver()
        {
            ArgsTypeResolver.Current = new ArgsTypeResolver();
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return Activator.CreateInstance(serviceType, true);
            }
            catch (MissingMethodException ex)
            {
                throw new InvalidOperationException(String.Format("Cannot create instance of type {0}; no public default constructor found.  To override this behavior, impelment System.IServiceProvider and set the ArgsTypeResolver.Current static property.", serviceType.FullName), ex);
            }
        }
    }
}
