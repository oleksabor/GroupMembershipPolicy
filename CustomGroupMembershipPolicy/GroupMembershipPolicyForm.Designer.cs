namespace CustomGroupMembershipPolicy
{
	partial class GroupMembershipPolicyForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GroupMembershipPolicyForm));
			this.gbGroups = new System.Windows.Forms.GroupBox();
			this.gvGroups = new System.Windows.Forms.DataGridView();
			this.colGroupChecked = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.colGroupName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.gbChildrenPolicies = new System.Windows.Forms.GroupBox();
			this.gvPolicies = new System.Windows.Forms.DataGridView();
			this.colIsPolicySelected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.colPolicyName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.pButtons = new System.Windows.Forms.Panel();
			this.bCancel = new System.Windows.Forms.Button();
			this.bOk = new System.Windows.Forms.Button();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.lPolicy = new System.Windows.Forms.Label();
			this.gbGroups.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gvGroups)).BeginInit();
			this.gbChildrenPolicies.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gvPolicies)).BeginInit();
			this.pButtons.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// gbGroups
			// 
			this.gbGroups.Controls.Add(this.gvGroups);
			this.gbGroups.Controls.Add(this.lPolicy);
			this.gbGroups.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gbGroups.Location = new System.Drawing.Point(0, 0);
			this.gbGroups.Name = "gbGroups";
			this.gbGroups.Size = new System.Drawing.Size(538, 180);
			this.gbGroups.TabIndex = 2;
			this.gbGroups.TabStop = false;
			this.gbGroups.Text = "team collection groups";
			// 
			// gvGroups
			// 
			this.gvGroups.AllowUserToAddRows = false;
			this.gvGroups.AllowUserToDeleteRows = false;
			this.gvGroups.AllowUserToResizeRows = false;
			this.gvGroups.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gvGroups.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colGroupChecked,
            this.colGroupName});
			this.gvGroups.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gvGroups.Location = new System.Drawing.Point(3, 29);
			this.gvGroups.MultiSelect = false;
			this.gvGroups.Name = "gvGroups";
			this.gvGroups.RowHeadersVisible = false;
			this.gvGroups.ShowEditingIcon = false;
			this.gvGroups.Size = new System.Drawing.Size(532, 148);
			this.gvGroups.TabIndex = 1;
			// 
			// colGroupChecked
			// 
			this.colGroupChecked.DataPropertyName = "IsSelected";
			this.colGroupChecked.HeaderText = "selected";
			this.colGroupChecked.Name = "colGroupChecked";
			// 
			// colGroupName
			// 
			this.colGroupName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.colGroupName.DataPropertyName = "DisplayName";
			this.colGroupName.HeaderText = "tfs collection group";
			this.colGroupName.Name = "colGroupName";
			this.colGroupName.ReadOnly = true;
			// 
			// gbChildrenPolicies
			// 
			this.gbChildrenPolicies.Controls.Add(this.gvPolicies);
			this.gbChildrenPolicies.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gbChildrenPolicies.Location = new System.Drawing.Point(0, 0);
			this.gbChildrenPolicies.Name = "gbChildrenPolicies";
			this.gbChildrenPolicies.Size = new System.Drawing.Size(538, 177);
			this.gbChildrenPolicies.TabIndex = 4;
			this.gbChildrenPolicies.TabStop = false;
			this.gbChildrenPolicies.Text = "children policies";
			// 
			// gvPolicies
			// 
			this.gvPolicies.AllowUserToAddRows = false;
			this.gvPolicies.AllowUserToDeleteRows = false;
			this.gvPolicies.AllowUserToResizeRows = false;
			this.gvPolicies.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gvPolicies.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colIsPolicySelected,
            this.colPolicyName,
            this.colEnabled});
			this.gvPolicies.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gvPolicies.Location = new System.Drawing.Point(3, 16);
			this.gvPolicies.MultiSelect = false;
			this.gvPolicies.Name = "gvPolicies";
			this.gvPolicies.RowHeadersVisible = false;
			this.gvPolicies.ShowEditingIcon = false;
			this.gvPolicies.Size = new System.Drawing.Size(532, 158);
			this.gvPolicies.TabIndex = 0;
			// 
			// colIsPolicySelected
			// 
			this.colIsPolicySelected.DataPropertyName = "IsSelected";
			this.colIsPolicySelected.HeaderText = "selected";
			this.colIsPolicySelected.Name = "colIsPolicySelected";
			// 
			// colPolicyName
			// 
			this.colPolicyName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.colPolicyName.DataPropertyName = "DisplayName";
			this.colPolicyName.HeaderText = "policy name";
			this.colPolicyName.Name = "colPolicyName";
			this.colPolicyName.ReadOnly = true;
			// 
			// colEnabled
			// 
			this.colEnabled.DataPropertyName = "IsEnabled";
			this.colEnabled.HeaderText = "enabled";
			this.colEnabled.Name = "colEnabled";
			this.colEnabled.ReadOnly = true;
			// 
			// pButtons
			// 
			this.pButtons.Controls.Add(this.bCancel);
			this.pButtons.Controls.Add(this.bOk);
			this.pButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pButtons.Location = new System.Drawing.Point(0, 361);
			this.pButtons.Name = "pButtons";
			this.pButtons.Size = new System.Drawing.Size(538, 29);
			this.pButtons.TabIndex = 5;
			// 
			// bCancel
			// 
			this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bCancel.Location = new System.Drawing.Point(459, 3);
			this.bCancel.Name = "bCancel";
			this.bCancel.Size = new System.Drawing.Size(75, 23);
			this.bCancel.TabIndex = 1;
			this.bCancel.Text = "cancel";
			this.bCancel.UseVisualStyleBackColor = true;
			// 
			// bOk
			// 
			this.bOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.bOk.Location = new System.Drawing.Point(378, 3);
			this.bOk.Name = "bOk";
			this.bOk.Size = new System.Drawing.Size(75, 23);
			this.bOk.TabIndex = 0;
			this.bOk.Text = "ok";
			this.bOk.UseVisualStyleBackColor = true;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.gbGroups);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.gbChildrenPolicies);
			this.splitContainer1.Size = new System.Drawing.Size(538, 361);
			this.splitContainer1.SplitterDistance = 180;
			this.splitContainer1.TabIndex = 6;
			// 
			// lPolicy
			// 
			this.lPolicy.Dock = System.Windows.Forms.DockStyle.Top;
			this.lPolicy.Location = new System.Drawing.Point(3, 16);
			this.lPolicy.Name = "lPolicy";
			this.lPolicy.Size = new System.Drawing.Size(532, 13);
			this.lPolicy.TabIndex = 2;
			this.lPolicy.Text = "Please select a group that will not cause child policy to be activated";
			// 
			// GroupMembershipPolicyForm
			// 
			this.AcceptButton = this.bOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.bCancel;
			this.ClientSize = new System.Drawing.Size(538, 390);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.pButtons);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "GroupMembershipPolicyForm";
			this.Text = "GroupMembershipPolicyForm";
			this.gbGroups.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.gvGroups)).EndInit();
			this.gbChildrenPolicies.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.gvPolicies)).EndInit();
			this.pButtons.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox gbGroups;
		private System.Windows.Forms.GroupBox gbChildrenPolicies;
		private System.Windows.Forms.Panel pButtons;
		private System.Windows.Forms.Button bCancel;
		private System.Windows.Forms.Button bOk;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.DataGridView gvGroups;
		private System.Windows.Forms.DataGridView gvPolicies;
		private System.Windows.Forms.DataGridViewCheckBoxColumn colIsPolicySelected;
		private System.Windows.Forms.DataGridViewTextBoxColumn colPolicyName;
		private System.Windows.Forms.DataGridViewCheckBoxColumn colEnabled;
		private System.Windows.Forms.DataGridViewCheckBoxColumn colGroupChecked;
		private System.Windows.Forms.DataGridViewTextBoxColumn colGroupName;
		private System.Windows.Forms.Label lPolicy;
	}
}