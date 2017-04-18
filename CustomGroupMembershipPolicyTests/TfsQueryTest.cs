using CustomGroupMembershipPolicy;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.Server;
using Microsoft.TeamFoundation.VersionControl.Client;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace CustomGroupMembershipPolicyTests
{
	[TestFixture]
	public class TfsQueryTest
	{
		[SetUpAttribute]
		public void SetUp()
		{
			Trace.Listeners.Add(new ConsoleTraceListener());
		}

		public const string tfsCollectionName = @"http://serverNameHere:8080/tfs/SecondCollection";

		[Test]
		public void CheckWorkspaceOwner()
		{
			using (var tfs = new TfsTeamProjectCollection(new Uri(tfsCollectionName)))
			{

				tfs.Authenticate();


				VersionControlServer vcs = (VersionControlServer)tfs.GetService(typeof(VersionControlServer));

				Workspace[] wss = GetWorkspaces(vcs);

				foreach (Workspace ws in wss)
				{
					Trace.WriteLine(string.Format("{2}\tidentifier:{0} type:{1}", ws.OwnerIdentifier, ws.OwnerIdentityType, ws.OwnerDisplayName));
					foreach (var g in GroupMembershipAD(ws.OwnerIdentifier, ws.OwnerIdentityType))
						Trace.WriteLine(string.Format("\t{0} sid:{1}", g.DisplayName, g.Sid));
					//Trace.WriteLine(string.Format("identityobject {0} data:{1}", ws.OwnerDescriptor.Identifier, ws.OwnerDescriptor.Data));

					var gss = (IIdentityManagementService)tfs.GetService(typeof(IIdentityManagementService));
					var tfsidentity = gss.ReadIdentity(ws.OwnerDescriptor, MembershipQuery.Expanded, ReadIdentityOptions.ExtendedProperties);

					foreach (var group in tfsidentity.MemberOf)
					{
						var groupIdentity = gss.ReadIdentity(group, MembershipQuery.Expanded, ReadIdentityOptions.ExtendedProperties);
						Trace.WriteLine(string.Format("{0} {1} name:{2} uniqueName:{3}", groupIdentity.Descriptor.Identifier, groupIdentity.Descriptor.IdentityType, groupIdentity.DisplayName, groupIdentity.UniqueName));
					}
				}
			}
		}
		public Workspace[] GetWorkspaces(VersionControlServer vcs)
		{
			return vcs.QueryWorkspaces(null, vcs.AuthorizedUser, System.Net.Dns.GetHostName().ToString());
		}

		public List<GroupPrincipal> GroupMembershipAD(string userName, string identityType)
		{
			List<GroupPrincipal> result = new List<GroupPrincipal>();

			// establish domain context
			PrincipalContext yourDomain = new PrincipalContext(ContextType.Domain);

			//var identityEnum = (IdentityType)Enum.Parse(typeof(IdentityType), identityType);

			// find your user
			UserPrincipal user = UserPrincipal.FindByIdentity(yourDomain, System.DirectoryServices.AccountManagement.IdentityType.Sid, userName);

			// if found - grab its groups
			if (user != null)
			{
				PrincipalSearchResult<Principal> groups = user.GetAuthorizationGroups();

				// iterate over all groups
				foreach (Principal p in groups)
				{
					// make sure to add only group principals
					if (p is GroupPrincipal)
					{
						result.Add((GroupPrincipal)p);
					}
				}
			}
			else
				throw new ArgumentException(string.Format("no user {0} was found in domain {1}", userName, yourDomain.Name));
			return result;


		}

		[Test]
		public void GroupMembershipTFS()
		{
			foreach (var vm in GetTFSGroups(tfsCollectionName))
				Trace.WriteLine(string.Format("{0}\t{1}\t{2}", vm.Descriptor.IdentityType, vm.Descriptor.Identifier, vm.DisplayName));
		}

		IEnumerable<TeamFoundationIdentity> GetTFSGroups(string collectionName)
		{
			using (var tfs = new TfsTeamProjectCollection(new Uri(collectionName)))
			{
				tfs.Authenticate();
				var gss = (IIdentityManagementService)tfs.GetService(typeof(IIdentityManagementService));

				var collectionWideValidUsers = gss.ReadIdentity(IdentitySearchFactor.DisplayName,
																					  "Project Collection Valid Users",
																					  MembershipQuery.Expanded,
																					  ReadIdentityOptions.None);

				var validMembers = gss.ReadIdentities(collectionWideValidUsers.Members,
																			MembershipQuery.Expanded,
																			ReadIdentityOptions.ExtendedProperties);

				return validMembers.Where(_ => _.IsContainer);
			}
		}

		[Test]
		public void FindByIdentity()
		{
			var tfsIdentity = GetTFSGroups(tfsCollectionName).First();
			using (var tfs = new TfsTeamProjectCollection(new Uri(tfsCollectionName)))
			{

				tfs.Authenticate();

				var gss = (IIdentityManagementService)tfs.GetService(typeof(IIdentityManagementService));
				var idescriptor = new IdentityDescriptor(tfsIdentity.Descriptor.IdentityType, tfsIdentity.Descriptor.Identifier);
				var readIdentity = gss.ReadIdentity(idescriptor, MembershipQuery.Expanded, ReadIdentityOptions.ExtendedProperties);

				Assert.AreEqual(tfsIdentity.Descriptor.Identifier, readIdentity.Descriptor.Identifier);
			}		
		}

		[Test]
		public void GetPolicyList()
		{
			using (var tfs = new TfsTeamProjectCollection(new Uri(tfsCollectionName)))
			{

				tfs.Authenticate();
				foreach (var tfsProject in tfs.GetService<VersionControlServer>().GetAllTeamProjects(false))
				{
					Trace.WriteLine(string.Format("project {0}", tfsProject.Name));
					var expr_06 = tfsProject.GetCheckinPolicies();

					foreach (var pe in expr_06)
						Trace.WriteLine(string.Format("\tpolicy {0} enabled:{1}", pe.Type, pe.Enabled));

					var tfsWorker = new TFSWorker(tfsProject);
					var apList = tfsWorker.GetPolicies();
					foreach (var ap in apList)
						Trace.WriteLine(string.Format("\tpolicy {0} enabled:{1}", ap.PolicyType, ap.IsEnabled));

					Assert.AreEqual(expr_06.Count(_ => _.Enabled), apList.Count(_ => _.IsEnabled));
				}
			}
		}
	
	}
}
