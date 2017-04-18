using CustomGroupMembershipPolicy.Settings;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomGroupMembershipPolicy
{
	public class TFSWorker
	{
		TeamProject _project;

		public TFSWorker(TeamProject project)
		{
			_project = project;
		}

		protected virtual IEnumerable<PolicyRaw> GetPoliciesRaw()
		{
			var policies = _project.GetCheckinPolicies();
			var res = new Collection<PolicyRaw>();
			foreach (var p in policies)
				try
				{
					var policyType = p.Policy.GetType();
					if (policyType != typeof(CustomGroupMembershipPolicy))
						res.Add(new PolicyRaw(p.Policy, p.Enabled));
				}
				catch (Exception e)
				{
					throw new ApplicationException(string.Format("failed to load policy '{0}'", p.Type), e);
				}
			return res;
		}

		protected class PolicyRaw
		{
			protected PolicyRaw()
			{ }

			public PolicyRaw(IPolicyDefinition source, bool enabled)
			{
				Type = source.Type;
				Enabled = enabled;

				Definition = source;
			}

			public IPolicyDefinition Definition { get; protected set; }

			public bool Enabled {get; private set;}

			public string Type { get; protected set; }
		}

		public virtual IEnumerable<AvailablePolicy> GetPolicies()
		{
			var sourceList = GetPoliciesRaw();

			var iterations = new Dictionary<string, int>();

			var result = new List<AvailablePolicy>();
			foreach (var pe in sourceList)
			{
				try
				{
					iterations[pe.Type]++;
				}
				catch (KeyNotFoundException)
				{
					iterations[pe.Type] = 0;
				}
				result.Add(new AvailablePolicy() { Iteration = iterations[pe.Type], PolicyType = pe.Type, PolicyDefinition = pe.Definition, IsEnabled = pe.Enabled });
			}
			
			return result;
		}

		public virtual IEnumerable<GroupData> GetAvailableGroups()
		{
			var service = _project.TeamProjectCollection.GetService<IIdentityManagementService>();

			var collectionWideValidUsers = service.ReadIdentity(IdentitySearchFactor.DisplayName, "Project Collection Valid Users",
																	  MembershipQuery.Expanded, ReadIdentityOptions.None);

			var validMembers = service.ReadIdentities(collectionWideValidUsers.Members, MembershipQuery.Expanded, ReadIdentityOptions.ExtendedProperties);

			return validMembers.Where(_ => _.IsContainer).Select(_ => new GroupData(_.Descriptor.Identifier, _.Descriptor.IdentityType, _.DisplayName)).ToList();
		}

		IdentityComparer _identytyComparer = new IdentityComparer();

		protected virtual IdentityDescriptor[] GetIdentityGroupsRaw(IdentityDescriptor identity)
		{
			var service = _project.TeamProjectCollection.GetService<IIdentityManagementService>();
			var tfsidentity = service.ReadIdentity(identity, MembershipQuery.Expanded, ReadIdentityOptions.ExtendedProperties);
			return tfsidentity.MemberOf;
		}

		public virtual IdentityDescriptor[] GetIdentityGroups(IdentityDescriptor identity)
		{
			var result = new List<IdentityDescriptor>();
			foreach (var r in GetIdentityGroupsRaw(identity))
			{
				result.Add(r);
				result.AddRange(GetIdentityGroupsRaw(r));
			}
			return result.Distinct(_identytyComparer).ToArray();
		}

		public List<PolicyData> MatchPolicy(List<PolicyData> settings)
		{
			var available = GetPolicies();

			var old = settings.ToArray();

			var actual = from a in available
						 join s in settings.ToArray() on a equals s
						 into leftjoin
						 from subj in leftjoin.DefaultIfEmpty()
						 select new PolicyData(a) { IsSelected = subj == null ? false : subj.IsSelected };
			
			return actual.ToList();
		}

		public List<GroupData> MatchGroup(List<GroupData> groups)
		{
			var available = GetAvailableGroups();
			var old = groups.ToArray();

			var actual = from a in available
						 join s in groups.ToArray() on a equals s
						 into leftjoin
						 from subj in leftjoin.DefaultIfEmpty()
						 select new GroupData(a.Identifier, a.IdentityType, a.DisplayName) { IsSelected = subj == null ? false : subj.IsSelected };

			return actual.ToList();
		}

		public IEnumerable<PolicyFailure> Evaluate(MembershipSettings settings, IdentityDescriptor userDescriptor, IPendingCheckin pendingCheckin)
		{
			var failures = new List<PolicyFailure>();

			var userGroups = GetIdentityGroups(userDescriptor);

			var groups = from ug in userGroups
						 join sg in settings.Groups on ug.Identifier equals sg.Identifier
						 select ug;

			if (!groups.Any())
			{
				var allPolicies = from cp in settings.ChildrenPolicy
								  join ap in GetPolicies() on cp equals ap
								  select new { Policy = ap.PolicyDefinition as IPolicyEvaluation, Name = ap.DisplayName };

				var activePolicies = allPolicies.Where(_ => _.Policy != null);

				foreach (var ap in activePolicies)
				{
					using (ap.Policy)
					{
						ap.Policy.Initialize(pendingCheckin);
						var childFailures = ap.Policy.Evaluate();
						failures.AddRange(childFailures);
					}
				}
				if (!activePolicies.Any())
					throw new ArgumentException(string.Format("no policy instance was found: {0}", string.Join(";", allPolicies.Select(_ => _.Name).ToArray())));
				// assert if nullable (inactive) policies where found ?
			}
			return failures;
		}
	}

	public class AvailablePolicy : PolicyData
	{
		public IPolicyDefinition PolicyDefinition { get; set; }
	}
}
