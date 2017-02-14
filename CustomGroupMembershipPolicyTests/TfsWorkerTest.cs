using CustomGroupMembershipPolicy;
using CustomGroupMembershipPolicy.Settings;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomGroupMembershipPolicyTests
{
	[TestFixture]
	public class TfsWorkerTest
	{
		[Test]
		public void MatchPolicy()
		{
			var tfsw = new TFSWorkerFormTest();

			var s = new MembershipSettings();

			var available = tfsw.GetPolicies();
			var a = new PolicyData(available.First());
			s.ChildrenPolicy.Add(a);
			a.IsSelected = true;

			var broken = new PolicyData();
			broken.Iteration = 999;
			broken.PolicyType = "policywereremoved";
			broken.IsSelected = true;

			s.ChildrenPolicy.Add(broken);


			var matched = tfsw.MatchPolicy(s.ChildrenPolicy);
			Assert.AreEqual(available.Count(), matched.Count());
			var m = matched.FirstOrDefault(_ => _.Equals(a));
			Assert.IsNotNull(m);
			Assert.AreEqual(m, a);

			Assert.AreEqual(1, matched.Count(_ => _.IsSelected));
		}

		[Test]
		public void MatchGroups()
		{
			var tfsw = new TFSWorkerFormTest();

			var s = new MembershipSettings();

			var available = tfsw.GetAvailableGroups();
			var f = available.Last();

			var g = new GroupData(f.Identifier, f.IdentityType, f.DisplayName);
			g.IsSelected = true;
			s.Groups.Add(g);

			var gg = new GroupData("absentidentifier", "dd", "missed group");
			gg.IsSelected = true;
			s.Groups.Add(gg);

			var matched = tfsw.MatchGroup(s.Groups);

			Assert.AreEqual(available.Count(), matched.Count());
			var m = matched.FirstOrDefault(_ => g.Equals(_));
			Assert.AreEqual(true, m.IsSelected);
			Assert.AreEqual(m, g);

			Assert.AreEqual(1, matched.Count(_ => _.IsSelected));
		}

		[Test]
		public void Evaluate()
		{
			var tfsw = new TFSWorkerEvaluatorTest();

			var s = new MembershipSettings();
			s.Groups.Add(new GroupData("groupOK", "it", "OK group"));
			s.ChildrenPolicy.Add(tfsw.GetFaultPolicyWrapper());

			var userDescriptor = new IdentityDescriptor("asdf", "id1");
			
			var errors = tfsw.Evaluate(s, userDescriptor, null);

			Assert.IsEmpty(errors);
		}
		
		[Test]
		public void EvaluateFault()
		{
			var tfsw = new TFSWorkerEvaluatorFaultTest();

			var s = new MembershipSettings();
			s.Groups.Add(new GroupData("groupOK", "it", "OK group"));
			s.ChildrenPolicy.Add(tfsw.GetFaultPolicyWrapper());

			var userDescriptor = new IdentityDescriptor("asdf", "id1");

			var errors = tfsw.Evaluate(s, userDescriptor, null);

			Assert.IsNotEmpty(errors);
			Assert.AreEqual(1, errors.Count());
		}

		[Test]
		public void GetPolicies()
		{
			var tfs = new TFSWorkerPolicyLoadTest();

			var policies = tfs.GetPolicies();

			Assert.AreEqual(3, policies.Count());

			var groups = policies.GroupBy(_ => _.PolicyType);

			foreach (var p in groups)
			{
				var duplicate = from p1 in p
								join p2 in p on p1.Iteration equals p2.Iteration
								select p1;
				Assert.AreEqual(p.Count(), duplicate.Count()); // no duplicate Iteration value
			}

			foreach (var p in policies)
				Trace.WriteLine(string.Format("{0}", p.DisplayName));
		}

		[Test]
		public void GetGroupsIteration()
		{
			var identity = new IdentityDescriptor("4321", "user1");

			var tfs = new TFSWorkerGroupIterationTest();

			var groups = tfs.GetIdentityGroups(identity);

			Assert.IsNotNull(groups);
			Assert.AreEqual(4, groups.Count());
		}

		[Test]
		public void IdentityComparer()
		{
			var ic = new IdentityComparer();

			var ids = new[] { new IdentityDescriptor("1234", "id1234"), new IdentityDescriptor("asdf", "id1234") };

			Assert.IsTrue(ic.Equals(ids[0], ids[1]));
		}
	}

	class TFSWorkerGroupIterationTest: TFSWorker
	{
		public TFSWorkerGroupIterationTest()
			: base(null)
		{ }

		protected override IdentityDescriptor[] GetIdentityGroupsRaw(IdentityDescriptor identity)
		{
			switch (identity.Identifier)
			{
				case "user1":
					return new[] {new IdentityDescriptor("1234", "id1234"), new IdentityDescriptor("asdf", "idasdf"), };
				case "id1234":
					return new[] { new IdentityDescriptor("1234", "id1234InnerGroup") };
				case "idasdf":
					return new[] { new IdentityDescriptor("asdf", "idasdfInnerGroup"), new IdentityDescriptor("1234", "id1234InnerGroup") };
				default:
					return new IdentityDescriptor[0];
			}
		}
	}

	class TFSWorkerPolicyLoadTest : TFSWorker
	{
		public TFSWorkerPolicyLoadTest()
			: base(null)
		{ }
		
		protected override IEnumerable<PolicyRaw> GetPoliciesRaw()
		{
			return new PolicyRaw[] { 
				new PolicyRaw(new Policy1Test(), true), new PolicyRaw(new Policy2Test(), false), new PolicyRaw(new Policy1Test(), true), 
			};
		}

		class Policy2Test : Policy1Test
		{
			public Policy2Test()
			{ 
				Type = "Policy2Test";
				TypeDescription  = "Policy2Test description"; 
			}
		}

		class Policy1Test : PolicyBaseTest
		{
			public Policy1Test()
			{
				Type = "Policy1Test";
				TypeDescription = "Policy1Test description"; 
			}
		}

		class PolicyBaseTest : IPolicyDefinition
		{
			public bool Enabled { get; private set; }

			public bool CanEdit { get; set; }

			public string Description { get; private set; }

			public string Type { get; protected set; }

			public string TypeDescription { get; protected set; }

			public bool Edit(IPolicyEditArgs policyEditArgs)
			{
				throw new NotImplementedException();
			}

			public string InstallationInstructions
			{
				get { throw new NotImplementedException(); }
			}
		}
	}
	
	class TFSWorkerFormTest : TFSWorker
	{
		public TFSWorkerFormTest()
			: base(null)
		{ }

		public override IEnumerable<GroupData> GetAvailableGroups()
		{
			var arr = new List<GroupData>();
			for (int q = 1; q < 10; q++)
				arr.Add(new GroupData(string.Format("g{0}id", q), "someType", string.Format("g{0}id name", q)) { IsSelected = q % 2 == 0 });
			return arr;
		}

		public override IEnumerable<AvailablePolicy> GetPolicies()
		{
			var res = new List<AvailablePolicy>();
			for (int q = 0; q < 5; q++)
			{
				var ap = new AvailablePolicy();
				res.Add(ap);
				ap.Iteration = q;
				ap.PolicyType = string.Format("policy {0}", q);
				ap.IsEnabled = !ap.IsSelected;
			}
			return res;
		}
	}

	class TFSWorkerEvaluatorFaultTest : TFSWorkerEvaluatorTest
	{
		public override IdentityDescriptor[] GetIdentityGroups(IdentityDescriptor identity)
		{
			var res = new[] { new IdentityDescriptor("it", "groupNeutral"), };

			return res;
		}
	}

	class TFSWorkerEvaluatorTest : TFSWorker
	{
		public TFSWorkerEvaluatorTest()
			: base(null)
		{ }

		public override IEnumerable<AvailablePolicy> GetPolicies()
		{
			var res = new List<AvailablePolicy>();

			var policy = new FaultChildPolicy();
			res.Add(new AvailablePolicy() { Iteration = 1, PolicyType = "FaultChildPolicy", PolicyDefinition = policy });
			res.Add(new AvailablePolicy() { Iteration = 2, PolicyType = "FaultChildPolicy", PolicyDefinition = new FaultChildPolicy() });

			return res;
		}

		public PolicyData GetFaultPolicyWrapper()
		{
			var w = new PolicyData();
			w.Iteration = 1;
			w.PolicyType = "FaultChildPolicy";
			return w;
		}

		public override IdentityDescriptor[] GetIdentityGroups(IdentityDescriptor identity)
		{
			var res = new[] { new IdentityDescriptor("it", "groupOK"), new IdentityDescriptor("it", "groupNeutral"), };

			return res;
		}

		class FaultChildPolicy : PolicyBase
		{

			public override string Description
			{
				get { throw new NotImplementedException(); }
			}

			public override bool Edit(IPolicyEditArgs policyEditArgs)
			{
				throw new NotImplementedException();
			}

			public override PolicyFailure[] Evaluate()
			{
				return new[] { new PolicyFailure("test message FaultChildPolicy", this) };
			}

			public override string Type
			{
				get { throw new NotImplementedException(); }
			}

			public override string TypeDescription
			{
				get { throw new NotImplementedException(); }
			}
		}
	}
}
