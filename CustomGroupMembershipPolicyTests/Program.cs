using CustomGroupMembershipPolicy;
using CustomGroupMembershipPolicy.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomGroupMembershipPolicyTests
{
    static class Program
    {
		[STAThread]
		static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			var s = new MembershipSettings();
			var tfs = new TFSWorkerFormTest();

			Application.Run(new GroupMembershipPolicyForm(tfs.MatchGroup(s.Groups), tfs.MatchPolicy(s.ChildrenPolicy)));
		}
	}

}
