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
		public Process currentProcess;
		public Process currentIO;
		public int time;
		Queue<Process> Final_List;

		public RR(ProcessList processList)
		{
			RRprocessList = processList.clone ();
			Final_List = new Queue<Process> ();
			simulate(10,new StreamReader("../../output.txt"));
		}


		public override void simulate(int snapshot, StreamReader pa) {
			// TODO Auto-generated method stub
			Console.WriteLine ("**************************************RR STARTED**************************");
			quantum	= RRprocessList.getQuantum();

			//add all processes to queue
			foreach (Process process in RRprocessList.processes) {
				ReadyQueue.Enqueue(process);
				//Console.WriteLine (process.getCPU_burst1 ());
			}

			//initialize currents
			currentProcess = ReadyQueue.Dequeue ();
			currentIO = new Process(-1,-1,-1,-1,-1);

			//main RR while loop
			while (ReadyQueue.Count > 0) {
				


				//quantum for loop
				for (int i = 0; i < quantum; i++) {

					//cpu processing
					//Console.WriteLine (currentProcess.getPID ());

					foreach (Process p in ReadyQueue) {
						p.period++;
					}
					currentProcess.activePeriod++;
					if (currentProcess.getCPU_burst1 () > 0) {
						currentProcess.decrementCPUBurst1 ();
					} else if (currentProcess.getCPU_burst1 () == 0 && currentProcess.getIO_burst () > 0) {
						IOQueue.Enqueue (currentProcess);
						//Console.WriteLine ("into IO:"+ currentProcess.getPID ());
						break;
					} else if (currentProcess.getCPU_burst1 () == 0 && currentProcess.getIO_burst () == 0 && currentProcess.getCPU_burst2 () > 0) {
						currentProcess.decrementCPUBurst2 ();
					} else {
						//Final_List.Enqueue (currentProcess);
						break;
					}


					//io processing
					if (currentIO.getIO_burst () > 0) {
						currentIO.decrementIO_burst ();
					} else if (currentIO.getIO_burst () == 0) {
						ReadyQueue.Enqueue (currentIO);
						if (IOQueue.Count > 0) {
							currentIO = IOQueue.Dequeue ();
						} else {
							currentIO = new Process (-1, -1, -1, -1, -1);
						}
					} else {
						if (IOQueue.Count > 0) {
							currentIO = IOQueue.Dequeue ();
						} 
					}
					//increment time
					time++;

					//POKEMON SNAP
					if (time % snapshot == 0) {
						System.Console.WriteLine ("Taking Snap at time: " + time);
						this.snapshot ();
					}
				}

				if (currentProcess.getCPU_burst1 () > 0 || (currentProcess.getCPU_burst2 () > 0 && currentProcess.getIO_burst () == 0)) {
					ReadyQueue.Enqueue (currentProcess);
				} else if (currentProcess.getCPU_burst1 () == 0 && currentProcess.getIO_burst () > 0 && !IOQueue.Contains(currentProcess)){
					IOQueue.Enqueue (currentProcess);
					//Console.WriteLine ("into IO2:"+ currentProcess.getPID ());
				} else if(currentProcess.getCPU_burst1 () == 0 && currentProcess.getCPU_burst2() == 0 && currentProcess.getIO_burst() == 0){
					Final_List.Enqueue (currentProcess);
				}
				if (ReadyQueue.Count>0) currentProcess = ReadyQueue.Dequeue ();	

			}
			foreach (Process item in Final_List) item.period--;
			finalReport (pa);
			Console.WriteLine ("**************************************RR ENDED**************************");
		}


		public override void finalReport(StreamReader pw) {
			int waiting_time = 0;
			int turnaround_time = 0;
			Console.WriteLine ("Final Report");
			Console.WriteLine ("PID         WAIT TIME");
			foreach (Process item in Final_List) {
				waiting_time += item.period;
				Console.WriteLine (item.getPID () + "               " + item.period);
			}
			Console.WriteLine ("AVERAGE WAITING TIME: "+(waiting_time/Final_List.Count));
			Console.WriteLine ("PID         TURNAROUND TIME");
			foreach (Process item in Final_List) {
				turnaround_time += item.period+item.activePeriod;
				Console.WriteLine (item.getPID () + "               " + (item.activePeriod+item.period));
			}
			Console.WriteLine ("AVERAGE TURNAROUND TIME: "+(turnaround_time/Final_List.Count));
		}

		void snapshot(){
			Console.WriteLine ("==============================================");
			if (ReadyQueue.Count == 0) {
				Console.Write ("Ready Queue: NOTHING");
			} else {
				Console.Write ("Ready Queue: ");
				foreach (Process item in ReadyQueue) {
					System.Console.Write (item.getPID () + " ");
				}
			}
			if (currentProcess.getCPU_burst1 () < 0) {
				Console.WriteLine ("\nRunning job: " + currentProcess.getPID () + " Current Burst: " + currentProcess.getCPU_burst2 ()+ " WT: " + currentProcess.period+" RT: " + currentProcess.activePeriod);
			} else {
				Console.WriteLine ("\nRunning job: " + currentProcess.getPID () + " Current Burst: " + currentProcess.getCPU_burst1 ()+ " WT: " + currentProcess.period+" RT: " + currentProcess.activePeriod);
			}
			Console.Write ("IO Queue: ");
			if (IOQueue.Count == 0) {
				Console.WriteLine ("NOTHING!");
			} else {
				foreach (Process item in IOQueue) {
					System.Console.Write (item.getPID () + " ");
				}
				Console.WriteLine ("");
			}
			if (currentIO.getPID () == -1) {
				Console.WriteLine ("IO Job: NO RUNNING JOB");
			} else {
				if (currentProcess.getCPU_burst1 () < 0) {
					Console.WriteLine ("IO job: " + currentIO.getPID () + " Current Burst: " + currentIO.getIO_burst ()+" WT: " + currentProcess.period);
				} else {
					Console.WriteLine ("IO job: " + currentIO.getPID () + " Current Burst: " + currentIO.getIO_burst ()+" WT: " + currentProcess.period);
				}
			}
			Console.WriteLine ("==============================================");
		}


	}
}

