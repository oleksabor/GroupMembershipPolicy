using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomGroupMembershipPolicy.Settings
{
	[Serializable]
	public class MembershipSettings
	{
		public MembershipSettings()
		{
			Groups = new List<GroupData>();
			ChildrenPolicy = new List<PolicyData>();
		}

		public List<GroupData> Groups { get; set; }

		public List<PolicyData> ChildrenPolicy { get; set; }
	}
}
