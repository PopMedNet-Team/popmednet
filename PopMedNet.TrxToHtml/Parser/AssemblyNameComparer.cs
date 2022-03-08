using System.Collections.Generic;
using System.Reflection;

namespace PopMedNet.TrxToHtml.Parser
{
	public class AssemblyNameComparer : IEqualityComparer<AssemblyName>
	{
		public bool Equals(AssemblyName x, AssemblyName y)
		{
			return x.FullName == y.FullName;
		}

		public int GetHashCode(AssemblyName obj)
		{
			return GetHashCode();
		}
	}
}
