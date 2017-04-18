using CustomGroupMembershipPolicy.Settings;
using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomGroupMembershipPolicy
{
	[Serializable]
	public sealed class CustomGroupMembershipPolicy : PolicyBase
    {

		public CustomGroupMembershipPolicy()
		{
			_logger = new Logger();
		}

		public override string Description
		{
			get { return "User permission group filter policy"; }
		}

		public override bool CanEdit
		{
			get
			{
				return true;
			}
		}

		public MembershipSettings Settings { get; set; }
		
		[NonSerialized]
		private IPendingCheckin _pendingCheckin;
		[NonSerialized]
		public TFSWorker _worker;
		[NonSerialized]
		ILogger _logger;

		public override void Initialize(IPendingCheckin pendingCheckin)
		{
			base.Initialize(pendingCheckin);
			if (_pendingCheckin != null && _pendingCheckin.WorkItems != null)
				_pendingCheckin.WorkItems.CheckedWorkItemsChanged -= WorkItems_CheckedWorkItemsChanged;

			_pendingCheckin = pendingCheckin;
			_pendingCheckin.WorkItems.CheckedWorkItemsChanged += WorkItems_CheckedWorkItemsChanged;
		}

		void WorkItems_CheckedWorkItemsChanged(object sender, EventArgs e)
		{
			if (!Disposed)
			{
				OnPolicyStateChanged(Evaluate());
			}
		}

		public override void Dispose()
		{
			base.Dispose();
			if (_pendingCheckin != null)
				_pendingCheckin.WorkItems.CheckedWorkItemsChanged -= WorkItems_CheckedWorkItemsChanged;

			GC.SuppressFinalize(this);
		}

		public string _projectName;

		public override bool Edit(IPolicyEditArgs policyEditArgs)
		{
			_projectName = policyEditArgs.TeamProject.Name;

			if (Settings == null)
				Settings = new MembershipSettings();
			var w = new TFSWorker(policyEditArgs.TeamProject);
			try
			{
				var groups = w.MatchGroup(Settings.Groups);
				var childrenPolicy = w.MatchPolicy(Settings.ChildrenPolicy);

				using (var editorForm = new GroupMembershipPolicyForm(groups, childrenPolicy))
				{
					var res = editorForm.ShowDialog(policyEditArgs.Parent) == System.Windows.Forms.DialogResult.OK;
					if (res)
					{
						Settings.ChildrenPolicy = childrenPolicy.Where(_ => _.IsSelected).ToList();
						Settings.Groups = groups.Where(_ => _.IsSelected).ToList();

						//var pstr = string.Join(";", Settings.ChildrenPolicy.Select(_ => _.DisplayName).ToArray());
						//var gstr = string.Join(";", Settings.Groups.Select(_ => _.DisplayName).ToArray());
						//System.Windows.Forms.MessageBox.Show(string.Format("child policy\r\n{0}\r\ngroups allowed\r\n{1}", pstr, gstr));
					}
					return res;
				}
			}
			catch (Exception e)
			{
				_logger.Error(e);
				throw new ApplicationException("failed to show configuration", e);
			}
		}

		public override PolicyFailure[] Evaluate()
		{
			var failures = new List<PolicyFailure>();
			try
			{
				if (Settings == null || Settings.Groups.Count == 0 || Settings.ChildrenPolicy.Count == 0)
					return failures.ToArray();
				var vcs = (VersionControlServer)_pendingCheckin.GetService(typeof(VersionControlServer));

				var teamProject = vcs.GetTeamProject(_projectName);

				var worker = _worker ?? new TFSWorker(teamProject);
				var childrenFails = worker.Evaluate(Settings, _pendingCheckin.PendingChanges.Workspace.OwnerDescriptor, _pendingCheckin);
				failures.AddRange(childrenFails.Select(_ => new PolicyFailure(_.Message, this)));
			}
			catch (Exception e)
			{
				failures.Add(new PolicyFailure(e.Message, this));
			}

			return failures.ToArray();
		}

		public override string Type
		{
			get { return "User group membership policy"; }
		}

		public override string TypeDescription
		{
			get { return "Policy runs child policy if user does not have certain group(s) assigned"; }
		}

		public override void DisplayHelp(PolicyFailure failure)
		{
			base.DisplayHelp(failure);
		}
	
		public override BinaryFormatter GetBinaryFormatter()
		{
			return new BinaryFormatter
			{
				Binder = new MembershipSerializationBinder()
			};
		}
		
		public override string GetAssemblyName()
		{
			return MembershipSerializationBinder.RedirectionTarget;
		}
	}
}
