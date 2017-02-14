# GroupMembershipPolicy
User group membership policy
Policy runs child policy if user does not have certain group(s) assigned

######VS2015 version is supported
This policy was created to make code review request mandatory for new development team members in Microsfot TeamFoundation source control.
Let's assume that you have development project that is modified by developers. Some developers work should be reviewed before (by core developers). From other side there are develpers that are allowed to make source code changes without any additional review.
To make code review required only for certain project|paths|files custom path policy from TFS power tools can be used additionally.

Please find sample flow described below:
ProjectFoo contains files that can be modified only by certain developers group without code review. But ProjectLocal can be modified by any developer from the team wihtout code review.
To parameterize this you can do the following:

* install custom checkin policy [ColinsALMCornerCheckinPolicies 2015](https://visualstudiogallery.msdn.microsoft.com/045730ee-63c0-498e-b972-42b05a2d0857)
* install [TFS power tools vs2015](https://marketplace.visualstudio.com/items?itemName=TFSPowerToolsTeam.MicrosoftVisualStudioTeamFoundationServer2015Power). You have to install only custom policy pack for custom path checkin policy
* install GroupMembershipPolicy

Configure custom path policy to run code review policy and add 
`ProjectFoo(?!.*AssemblyInfo\.cs).*`
regex to path. This will cause custom path to run code review policy only if files from ProjectFoo are being modified. AssemblyInfo.cs modifications are ignored
Code review and custom path policies should be disabled. 

Then configure group membership policy to run custom path policy and select what group(s) does not run child custom path policy.
You will get policy chain that first will check user permission and if user has no configured group assigned then child custom path policy is fired. 
Custom path policy checks are modified files satisfies path and starts child code review policy.

Nested groups are checked too so you can define group NoCodeReview in the TFS security settings and add it to some local groups, for example officeA officeB groups. 
Then NoCodeReview group can be used in group membership policy and developers from officeA or officeB groups will not be requested for code review at check-in.



