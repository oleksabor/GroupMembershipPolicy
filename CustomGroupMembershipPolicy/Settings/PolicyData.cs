using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomGroupMembershipPolicy.Settings
{
	[Serializable]
	public class PolicyData
	{
		public string PolicyType { get; set; }
		public int Iteration { get; set; }

		public string DisplayName { get { return string.Format("{0} ({1})", PolicyType, Iteration); } }

		public PolicyData()
		{ }

		public PolicyData(PolicyData pw)
		{
			PolicyType = pw.PolicyType;
			Iteration = pw.Iteration;
			IsSelected = pw.IsSelected;
			IsEnabled = pw.IsEnabled;
		}

		public override bool Equals(object obj)
		{
			var source = obj as PolicyData;
			if (source == null)
				return base.Equals(obj);
			else
				return Equals(source);
		}

		public bool Equals(PolicyData policy)
		{
			return PolicyType == policy.PolicyType && Iteration == policy.Iteration;
		}

		public override int GetHashCode()
		{
			return string.Format("{0}#{1}", PolicyType, Iteration).GetHashCode();
		}

		public bool IsSelected {get;set;}

		public bool IsEnabled { get; set; }
	}
}
