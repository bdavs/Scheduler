using System;
using System.IO;
using System.Collections.Generic;

namespace Scheduler
{
	public class RR : Scheduler{
		public int quantum;
		public ProcessList RRprocessList = new ProcessList ();
		public Queue<Process> ReadyQueue = new Queue<Process>();
		public Queue<Process> IOQueue = new Queue<Process>();


		public RR(ProcessList processList)
		{
			RRprocessList = processList.clone ();
			simulate(10,new StreamReader("../../output.txt"));
		}


		public override void simulate(int snapshot, StreamReader pa) {
			// TODO Auto-generated method stub
			quantum	= RRprocessList.getQuantum();

			ReadyQueue = new Queue<Process> (RRprocessList.processes);


			Console.WriteLine (RRprocessList.getQuantum ());
			while(ReadyQueue.Count>0){
				Process currentProcess = ReadyQueue.Dequeue ();
				Process currentIO = IOQueue.Dequeue ();
				for (int i = 0; i < quantum; i++) {
					if (currentProcess.getCPU_burst1 () > 0) {
						currentProcess.decrementCPUBurst1 ();
					} else if (currentProcess.getCPU_burst1 () == 0 && currentProcess.getIO_burst() > 0) {
						IOQueue.Enqueue (currentProcess);
						break;
					} else if (currentProcess.getCPU_burst2 () > 0) {
						currentProcess.decrementCPUBurst2 ();
					} else {
						Console.WriteLine ("error in quantum loop");
						return;
					}

					if (currentIO.getIO_burst ()> 0) {
						currentIO.decrementIO_burst ();
					} else {
						ReadyQueue.Enqueue (currentIO);
					}
				}
				if (currentIO.getIO_burst () > 0) {
					IOQueue.Enqueue (currentIO);
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

