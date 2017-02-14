using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomGroupMembershipPolicy.Settings
{
	[Serializable]
	public class GroupData
	{
		public string Identifier { get; set; }
		public string IdentityType { get; set; }
		public string DisplayName { get; set; }

		public GroupData()
		{ }

		public GroupData(string identifier, string identifierType, string displayName)
		{
			Identifier = identifier;
			IdentityType = identifierType;
			DisplayName = displayName;	
		}

		public bool IsSelected { get; set; }

		public override bool Equals(object obj)
		{
			var group = obj as GroupData;
			if (group == null)
			return base.Equals(obj);
			else
				return Equals(group);
		}

		public bool Equals(GroupData group)
		{
			return string.Equals(Identifier, group.Identifier, StringComparison.OrdinalIgnoreCase);
		}

		public override int GetHashCode()
		{
			return Identifier == null ? base.GetHashCode() : Identifier.GetHashCode();
		}


	}
}
