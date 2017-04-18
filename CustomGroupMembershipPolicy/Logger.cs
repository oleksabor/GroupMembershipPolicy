using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomGroupMembershipPolicy
{
	public class Logger : ILogger
	{
		public void Error(Exception e)
		{
			StartSTA(ErrorInternal, e);
		}

		void ErrorInternal(object e)
		{
			try
			{
				var sb = new StringBuilder();
				var ie = e as Exception;
				while (ie != null)
				{
					sb.AppendLine(ie.Message);
					sb.AppendLine(ie.StackTrace);
					ie = ie.InnerException;
				}
				Log(sb.ToString());
			}
			catch (Exception)
			{ }
		}

		void Log(string value)
		{
			if (Environment.UserInteractive)
				Clipboard.SetText(value);
			else
				Trace.WriteLine(value);
		}

		void StartSTA(Action<object> a, object parameter)
		{
			var th = new Thread(new ParameterizedThreadStart(a));
			th.SetApartmentState(ApartmentState.STA);
			th.Start(parameter);

			th.Join(1000);
		}
	}

	public interface ILogger
	{
		void Error(Exception e);
	}

}
