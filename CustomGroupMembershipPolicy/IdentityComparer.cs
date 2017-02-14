using Microsoft.TeamFoundation.Framework.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomGroupMembershipPolicy
{
	public class IdentityComparer : IEqualityComparer<IdentityDescriptor>
	{
		public bool Equals(IdentityDescriptor x, IdentityDescriptor y)
		{
			return x.Identifier.Equals(y.Identifier, StringComparison.OrdinalIgnoreCase);
		}

		public int GetHashCode(IdentityDescriptor obj)
		{
			return obj.Identifier.GetHashCode();
		}
	}
}
