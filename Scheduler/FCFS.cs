using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
namespace Scheduler
{
	public class FCFS  : Scheduler{
		Queue<Process> Ready_Queue;
		Queue<Process> IO_Queue;
		List<Process> FCFSprocessList;
		int time = 0;
		public FCFS(ProcessList processList)
		{
			Queue<Process> Ready_Queue = new Queue<Process>();
			Queue<Process> IO_Queue = new Queue<Process>();
			FCFSprocessList = processList.processes.ToList();
		}

	
		public override void simulate(int snapshot, StreamReader pa) {
			while (FCFSprocessList.Count != 0) {
				foreach (Process myprocess in FCFSprocessList) {
					if (myprocess.getPID == time) {
						Ready_Queue.Enqueue(myprocess);
						FCFSprocessList.Remove (myprocess);
					}
				}
			}

		}


		public override void finalReport(StreamReader pw) {
			// TODO Auto-generated method stub

		}


	}
}

