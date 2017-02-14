using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CustomGroupMembershipPolicy
{
	internal sealed class MembershipSerializationBinder : SerializationBinder
	{
		internal const string RedirectionTarget = "CustomGroupMembershipPolicy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";

		public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
		{
			Assembly assembly = serializedType.Assembly;
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			if (assembly.Equals(executingAssembly))
			{
				assemblyName = RedirectionTarget;
			}
			else
			{
				assemblyName = assembly.FullName;
			}
			typeName = serializedType.FullName.Replace(executingAssembly.FullName, RedirectionTarget);
		}

		public override Type BindToType(string assemblyName, string typeName)
		{
			throw new NotImplementedException();
		}
	}
}
