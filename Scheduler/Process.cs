using System;

namespace Scheduler
{
	public class Process {
		private int pid;
		private int CPU_burst1;
		private int CPU_burst2;
		private int IO_burst;
		private int priority;
		private int period;
		private int activePeriod;

		private Process(){}

		public Process(int pid, int CPU_burst, int IO_burst, int priority, int period)
		{
			this.pid = pid;

			if(IO_burst>0)
			{
				//If the CPU burst is an odd number, have the larger have be in burst 1
				this.CPU_burst1 = CPU_burst/2 + CPU_burst%2;
				this.CPU_burst2 = CPU_burst/2;
			}else
			{
				this.CPU_burst1 = CPU_burst;
				this.CPU_burst2 = 0;
			}

			this.IO_burst = IO_burst;

			this.priority = priority;

			this.period = period;
			activePeriod = period;
		}

		public Process clone()
		{
			Process temp = new Process();
			temp.pid = this.pid;
			temp.CPU_burst1 = this.CPU_burst1;
			temp.CPU_burst2 = this.CPU_burst2;
			temp.IO_burst = this.IO_burst;
			temp.priority = this.priority;
			temp.period = this.period;
			temp.activePeriod = this.period;

			return temp;
		}

		public int getPID()
		{
			return pid;
		}

		public int getCPU_burst1()
		{
			return CPU_burst1;
		}

		public void decrementCPUBurst1()
		{
			CPU_burst1--;
		}

		public int getCPU_burst2()
		{
			return CPU_burst2;
		}

		public void decrementCPUBurst2()
		{
			CPU_burst2--;
		}

		public int getIO_burst()
		{
			return IO_burst;
		}

		public void decrementIO_burst()
		{
			IO_burst--;
		}

		public int getPriority()
		{
			return priority;
		}

		public int getCurrentPeriod()
		{
			//returns the active period
			return activePeriod;
		}

		public void resetPeriod()
		{
			//Resets the period after the process leaves the CPU
			activePeriod = period;
		}

		public void decrementPeriod()
		{
			//Decrement the period, if it reaches zero or neg it's a deadline violation
			activePeriod--;
		}

	}
}

