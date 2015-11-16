using System;

namespace Scheduler
{
	public class Job
	{
		public int pid;
		public int cpu_burst;
		public int io_burst;
		public Job ()
		{
			pid = 0;
			cpu_burst = 0;
			io_burst = 0;
		}
	}
}

