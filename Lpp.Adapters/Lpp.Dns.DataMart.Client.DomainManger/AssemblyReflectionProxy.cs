using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Lpp.Dns.DataMart.Client.DomainManger
{
    public class AssemblyReflectionProxy : MarshalByRefObject
    {

        public void LoadAssembly(byte[] bits)
        {
            Assembly.ReflectionOnlyLoad(bits);
        }

        public T Reflect<T>(Func<Assembly, T> func)
        {
            throw new NotImplementedException();
        }

    }
}
