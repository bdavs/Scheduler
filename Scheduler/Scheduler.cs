﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Scheduler
{
	public abstract class Scheduler {
		public ProcessList processList;
		public List<String> CPUOrdering = new List<String>();
		public int processInCPU;
		public int Average_TurnAround;
		public Scheduler(){}

		public Scheduler(ProcessList processList)
		{
			this.processList = processList;
		}

		public abstract void simulate(int snapshot, StreamWriter pa );
		//Will run executing the simulation, must support snapshots to pw

		public abstract void finalReport(StreamWriter pa);
		//Will write the final report to pw

	}
}

