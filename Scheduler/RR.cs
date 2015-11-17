using System;
using System.IO;
using System.Collections.Generic;

namespace Scheduler
{
	public class RR : Scheduler{
		public int quantum;
		public Queue<Process> ReadyQueue = new Queue<Process>();
		public Queue<Process> IOQueue = new Queue<Process>();
		public RR(ProcessList processList)
		{

		}


		public override void simulate(int snapshot, StreamReader pa) {
			// TODO Auto-generated method stub
			quantum	= processList.getQuantum();
			ReadyQueue = new Queue<Process> ();


			while(ReadyQueue.Count>0){
				Process currentProcess = ReadyQueue.Dequeue ();
				if (currentProcess.getCPU_burst1 () > 0) {
					for (int i = 0; i < quantum; i++) {
						currentProcess.decrementCPUBurst1 ();
						if (currentProcess.getCPU_burst1 () == 0) {
							IOQueue.Enqueue (currentProcess);
							break;
						}
					}
				}
				if (currentProcess.getCPU_burst2 () > 0) { 
					for (int i = 0; i < quantum; i++) {
						currentProcess.decrementCPUBurst2 ();
						if (currentProcess.getCPU_burst2 () == 0) {
							IOQueue.Enqueue (currentProcess);
							break;
						}
					}
				}
				if (currentProcess.getCPU_burst1 () != 0 || currentProcess.getCPU_burst2 () != 0) {
					ReadyQueue.Enqueue (currentProcess);
				}
			}
		}


		public override void finalReport(StreamReader pw) {
			// TODO Auto-generated method stub

		}


	}
}

