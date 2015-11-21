using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
namespace Scheduler
{
	public class SJR  : Scheduler{
		List<Process> PreSortedList;
		Queue<Process> Ready_Queue;
		Queue<Process> IO_Queue;
		List<Process> Final_List;
		int time = 0;
		Process RunningJob;
		Process IO_Job;
		Process Temp_Job;
		public SJR(ProcessList processList)
		{
			PreSortedList = processList.processes.ToList ();
			PreSortedList.Sort ((p,q) => p.getCPU_burst1 ().CompareTo (q.getCPU_burst1 ()));
			Ready_Queue = new Queue<Process>();
			foreach (Process item in PreSortedList) {
				Ready_Queue.Enqueue (item);
			}
			Temp_Job = new Process (0,0,0,0,0);
			IO_Queue = new Queue<Process>();
			RunningJob = new Process (-1,0,0,0,0);
			IO_Job = new Process (-1, 0, 0, 0,0);
			Final_List = new List<Process> ();
			StreamReader output = new StreamReader ("../../output.txt");
			simulate (1, output);
		}


		public override void simulate(int snapshot, StreamReader pa) {
			int i;
			List<Process> TempList = new List<Process> ();
			Console.WriteLine ("**************************************SJF STARTED**************************");
			foreach (Process item in Ready_Queue) {
				item.period = 0;
				item.activePeriod = 0;
			}
			while ((Ready_Queue.Count != 0 || IO_Queue.Count != 0) || (RunningJob.getCPU_burst1 () > 0 || RunningJob.getCPU_burst2 () > 0)||(IO_Job.getIO_burst()>0)) {
				//Get the running job 
				if (RunningJob.getPID () == -1) {
					//OH GEEZ RICK, Were on the fist iteration
					RunningJob = Ready_Queue.Dequeue ();
				} 



				//RUNNING JOB LOGIC START
				if (RunningJob.getCPU_burst1 () == 1) {
					IO_Queue.Enqueue (RunningJob);
					if (Ready_Queue.Count != 0) {
						RunningJob.activePeriod++;
						RunningJob = Ready_Queue.Dequeue ();
						RunningJob.period++;
					}
				} else if (RunningJob.getCPU_burst1 () < 1) {
					if (RunningJob.getCPU_burst2 () != 0) {
						RunningJob.decrementCPUBurst2 ();
						RunningJob.activePeriod++;
					} else {
						if (Ready_Queue.Count != 0) {
							Final_List.Add (RunningJob);
							RunningJob = Ready_Queue.Dequeue ();
						}
					}
				} else if (RunningJob.getCPU_burst1 () > 1) {
					RunningJob.decrementCPUBurst1 ();
					RunningJob.activePeriod++;
				}

				//RUNNING JOB LOGIC END


				//IO JOB LOGIC START
				if (IO_Job.getPID () == -1) {
					if (IO_Queue.Count != 0) {
						IO_Job = IO_Queue.Dequeue ();
					}
				} else {
					if (IO_Job.getIO_burst () <= 1) {
						IO_Job.decrementCPUBurst1 ();
						Ready_Queue.Enqueue (IO_Job);
						if (IO_Queue.Count != 0) {
							IO_Job = IO_Queue.Dequeue ();
						} else {
							IO_Job = new Process (-1, 0, 0, 0, 0);
						}

					} else {
						IO_Job.decrementCPUBurst1 ();//Cuz FUCK U
						IO_Job.decrementIO_burst ();
						//IO_Job.period++;
					}
				}
				//IO JOB LOGIC END


				//Count waiting time
				foreach (Process item in Ready_Queue) {
					if (item.getPID () <= time) {
						item.period++;
					}
				}
				foreach (Process item in IO_Queue) {
					//item.period++;
				}
				//End waiting time count
				time++;



				//POKEMON SNAP
				if (time % snapshot == 0) {
					System.Console.WriteLine ("Taking Snap at time: " + time);
					this.snapshot ();
				}

				TempList.Clear ();
				PreSortedList.Clear ();
				foreach (Process item in Ready_Queue) {
					PreSortedList.Add (item);
					TempList.Add (item);
				}

				i = PreSortedList.Count - 1;
				foreach (Process item in PreSortedList) {
					if (item.getCPU_burst1 () < 0) {
						if (RunningJob.getCPU_burst1 () < 0) {
							if (item.getCPU_burst2 () < RunningJob.getCPU_burst2 ()) {
								Temp_Job = RunningJob;
								RunningJob = item;
								TempList.Remove (item);
								TempList.Add (Temp_Job);
							} 
						} else {
							if (item.getCPU_burst2 () < RunningJob.getCPU_burst1 ()) {
								Temp_Job = RunningJob;
								RunningJob = item;
								TempList.Remove (item);
								TempList.Add (Temp_Job);
							}
						}
					} else {
						if (RunningJob.getCPU_burst1 () < 0) {
							if (item.getCPU_burst1 () < RunningJob.getCPU_burst2 ()) {
								Temp_Job = RunningJob;
								RunningJob = item;
								TempList.Remove (item);
								TempList.Add (Temp_Job);
							}
						} else {
							if(item.getCPU_burst1 () < RunningJob.getCPU_burst1 ()){
								Temp_Job = RunningJob;
								RunningJob = item;
								TempList.Remove (item);
								TempList.Add (Temp_Job);
							}
						}
					}
					i--;
				}
				PreSortedList.Clear ();
				foreach (Process item in TempList) {
					PreSortedList.Add (item);
				}
				Ready_Queue.Clear ();
				PreSortedList.Sort ((p,q) => ((p.getCPU_burst1 () >= 0) ? p.getCPU_burst1 () : p.getCPU_burst2 ()).CompareTo ((q.getCPU_burst1 () >= 0) ? q.getCPU_burst1 () : q.getCPU_burst2 ()));
				foreach (Process item in PreSortedList) {
					Ready_Queue.Enqueue (item);
				}

			}
			Final_List.Add (RunningJob); foreach (Process item in Final_List) item.period--;
			finalReport (pa);
			Console.WriteLine ("**************************************SJR ENDED**************************");
		}

		public override void finalReport(StreamReader pw) {
			int waiting_time = 0;
			int turnaround_time = 0;
			Final_List.Sort ((p,q) => p.getPID ().CompareTo (q.getPID ()));
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
			if (Ready_Queue.Count == 0) {
				Console.Write ("Ready Queue: NOTHING");
			} else {
				Console.Write ("Ready Queue: ");
				foreach (Process item in Ready_Queue) {
					System.Console.Write (item.getPID () + " ");
				}
			}
			if (RunningJob.getCPU_burst1 () < 0) {
				Console.WriteLine ("\nRunning job: " + RunningJob.getPID () + " Current Burst: " + RunningJob.getCPU_burst2 ()+ " WT: " + RunningJob.period+" RT: " + RunningJob.activePeriod);
			} else {
				Console.WriteLine ("\nRunning job: " + RunningJob.getPID () + " Current Burst: " + RunningJob.getCPU_burst1 ()+ " WT: " + RunningJob.period+" RT: " + RunningJob.activePeriod);
			}
			Console.Write ("IO Queue: ");
			if (IO_Queue.Count == 0) {
				Console.WriteLine ("NOTHING!");
			} else {
				foreach (Process item in IO_Queue) {
					System.Console.Write (item.getPID () + " ");
				}
				Console.WriteLine ("");
			}
			if (IO_Job.getPID () == -1) {
				Console.WriteLine ("IO Job: NO RUNNING JOB");
			} else {
				if (RunningJob.getCPU_burst1 () < 0) {
					Console.WriteLine ("IO job: " + IO_Job.getPID () + " Current Burst: " + IO_Job.getIO_burst ()+" WT: " + RunningJob.period);
				} else {
					Console.WriteLine ("IO job: " + IO_Job.getPID () + " Current Burst: " + IO_Job.getIO_burst ()+" WT: " + RunningJob.period);
				}
			}
			Console.WriteLine ("==============================================");
		}
	}
}

