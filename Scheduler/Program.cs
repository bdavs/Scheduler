using System;

namespace Scheduler
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Hello World! test");
			Job myjob = new Job ();
			myjob.cpu_burst = 1;
			Console.WriteLine ("Thing"+myjob.cpu_burst);
		}
	}
}
