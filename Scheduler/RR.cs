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
			foreach (Process process in RRprocessList.processes) {
				ReadyQueue.Enqueue(process);
				Console.WriteLine (process.getCPU_burst1 ());
			}

			//Console.WriteLine (RRprocessList.getQuantum ());
			while (ReadyQueue.Count > 0) {
				Process currentProcess = ReadyQueue.Dequeue ();
				Process currentIO = new Process(-1,-1,-1,-1,-1);
				if (IOQueue.Count > 0) {
					currentIO = IOQueue.Dequeue ();
					if (currentIO.getIO_burst () > 0) {
						IOQueue.Enqueue (currentIO);
					}
				}	
				Console.WriteLine ("PID: " + currentProcess.getPID ());
				for (int i = 0; i < quantum; i++) {
					if (currentProcess.getCPU_burst1 () > 0) {
						currentProcess.decrementCPUBurst1 ();
					} else if (currentProcess.getCPU_burst1 () == 0 && currentProcess.getIO_burst() > 0) {
						IOQueue.Enqueue (currentProcess);
						break;
					} else if (currentProcess.getCPU_burst2 () > 0) {
						currentProcess.decrementCPUBurst2 ();
					} 
					//Console.WriteLine ("i = " + i);
					if (currentIO.getIO_burst ()> 0) {
						currentIO.decrementIO_burst ();
					} else if(currentIO.getIO_burst() == 0){
						ReadyQueue.Enqueue (currentIO);
						if (IOQueue.Count > 0) {
							currentIO = IOQueue.Dequeue ();
						} else {
							currentIO = new Process (-1, -1, -1, -1, -1);
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

