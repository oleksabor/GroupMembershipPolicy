using CustomGroupMembershipPolicy.Settings;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace CustomGroupMembershipPolicyTests
{
	[TestFixture]
	public class SettingsTest
	{
		[Test]
		public void Serialization()
		{
			var settings = new MembershipSettings();
			settings.Groups.Add(new GroupData() { DisplayName = "test" });

			using (var ms = new MemoryStream())
			{
				var bf = new BinaryFormatter();

				bf.Serialize(ms, settings);

				ms.Position = 0;

				var s2 = (MembershipSettings)bf.Deserialize(ms);

				Assert.AreEqual(settings.Groups.Count, s2.Groups.Count);
			}
		}

		[Test]
		public void BindingProperties()
		{
			var properties = new Dictionary<Type, IEnumerable<string>>();
			properties.Add(typeof(GroupData), new[] { "IsSelected", "DisplayName" });
			properties.Add(typeof(PolicyData), new[] { "IsSelected", "DisplayName", "IsEnabled" });

			foreach (var p in properties)
				foreach (var pn in p.Value)
				{
					var pi = p.Key.GetProperty(pn, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
					Assert.IsNotNull(pi, "no property {0} in {1}", pn, p.Key);
				}
		}

		[Test]
		public void GroupWrapperEqualsOperator()
		{
			var gw1 = new GroupData() { Identifier = "123-456" };
			var gw2 = new GroupData() { Identifier = "456-123" };

			Assert.AreNotEqual(gw1, gw2);

			gw2.Identifier = "123-456";

			Assert.AreEqual(gw1, gw2);
		}

		[Test]
		public void PolicyWrapperEqualsOperator()
		{
			var pw1 = new PolicyData() { Iteration = 1, PolicyType = "Type1" };
			var pw2 = new PolicyData() { Iteration = 2, PolicyType = "Type1" };

			Assert.AreNotEqual(pw1, pw2);

			pw2.Iteration = 1;
			Assert.AreEqual(pw1, pw2);

			pw2.PolicyType = "Type2";
			Assert.AreNotEqual(pw1, pw2);
		}

		[Test]
		public void PolicyDataCopyConstructor()
		{
			var pw1 = new PolicyData();
			pw1.IsSelected = 
				pw1.IsEnabled = true;
			pw1.PolicyType = "Type1";
			pw1.Iteration = 123;

			var pw2 = new PolicyData(pw1);

			Assert.AreEqual(pw1, pw2);
			Assert.AreEqual(pw1.DisplayName, pw2.DisplayName);
			Assert.AreEqual(pw1.IsEnabled, pw2.IsEnabled);
			Assert.AreEqual(pw1.IsSelected, pw2.IsSelected);
			Assert.AreEqual(pw1.Iteration, pw2.Iteration);
			Assert.AreEqual(pw1.PolicyType, pw2.PolicyType);

		}
	}
}
