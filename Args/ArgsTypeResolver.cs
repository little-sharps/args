using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Args
{
    public class ArgsTypeResolver : IServiceProvider
    {
        private static IServiceProvider current;
        public static IServiceProvider Current
        {
            get { return current = current ?? new ArgsTypeResolver(); }
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return Activator.CreateInstance(serviceType);
            }
            catch (MissingMethodException ex)
            {
                throw new InvalidOperationException(String.Format("Cannot create instance of type {0}; no public default constructor found.  To override this behavior, impelment System.IServiceProvider and set the ArgsTypeResolver.Current static property.", serviceType.FullName), ex);
            }
        }
    }
}
