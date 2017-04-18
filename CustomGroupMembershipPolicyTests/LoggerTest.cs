using CustomGroupMembershipPolicy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomGroupMembershipPolicyTests
{
	[TestFixture]
	public class LoggerTest
	{
		[Test, Apartment(System.Threading.ApartmentState.STA)]
		public void ErrorTest()
		{
			var logger = new Logger();

			Clipboard.Clear();

			var err = new ApplicationException("testError", new ApplicationException("test2Error"));
			logger.Error(err);

			Assert.IsTrue(Clipboard.ContainsText());
			var t = Clipboard.GetText();
			Assert.IsTrue(t.StartsWith("testError"));
		}
	}
}
