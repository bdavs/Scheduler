﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
namespace Scheduler
{
	public class FCFS  : Scheduler{
		Queue<Process> Ready_Queue;
		Queue<Process> IO_Queue;
		Queue<Process> Final_List;
		int time = 0;
		float cputicks;
		Process RunningJob;
		Process IO_Job;
		public FCFS(ProcessList processList, StreamWriter output)
		{
			Ready_Queue = new Queue<Process>();
			foreach (Process item in processList.processes) {
				Ready_Queue.Enqueue (item);
			}
			IO_Queue = new Queue<Process>();
			RunningJob = new Process (-1,0,0,0,0);
			IO_Job = new Process (-1, 0, 0, 0,0);
			Final_List = new Queue<Process> ();
			//StreamWriter output = new StreamWriter ("../../output.txt");
			simulate (10, output);
		}

	
		public override void simulate(int snapshot, StreamWriter pa) {
			pa.WriteLine ("**************************************FCFS STARTED**************************");
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
							Final_List.Enqueue (RunningJob);
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
					pa.WriteLine ("Taking Snap at time: " + time);
					this.snapshot (pa);
				}
			}
			Final_List.Enqueue (RunningJob); foreach (Process item in Final_List) item.period--;
			finalReport (pa);
			pa.WriteLine ("**************************************FCFS ENDED**************************");
		}

		public override void finalReport(StreamWriter pa) {
			int waiting_time = 0;
			int turnaround_time = 0;
			pa.WriteLine ("Final Report");
			pa.WriteLine ("PID         WAIT TIME");
			foreach (Process item in Final_List) {
				waiting_time += item.period;
				cputicks += item.activePeriod+1;
				pa.WriteLine (item.getPID () + "               " + item.period);
			}
			cputicks--;
			pa.WriteLine ("AVERAGE WAITING TIME: "+(waiting_time/Final_List.Count));
			pa.WriteLine ("PID         TURNAROUND TIME");
			foreach (Process item in Final_List) {
				turnaround_time += item.period+item.activePeriod;
				pa.WriteLine (item.getPID () + "               " + (item.activePeriod+item.period));
			}
			Average_TurnAround = turnaround_time / Final_List.Count;
			pa.WriteLine ("AVERAGE TURNAROUND TIME: "+(Average_TurnAround)+" CPU UTILIZATION: "+((cputicks/time)*100));
		}

		void snapshot(StreamWriter pa){
			pa.WriteLine ("==============================================");
			if (Ready_Queue.Count == 0) {
				pa.Write ("Ready Queue: NOTHING");
			} else {
				pa.Write ("Ready Queue: ");
				foreach (Process item in Ready_Queue) {
					pa.Write (item.getPID () + " ");
				}
			}
			if (RunningJob.getCPU_burst1 () < 0) {
				pa.WriteLine ("\nRunning job: " + RunningJob.getPID () + " Current Burst: " + RunningJob.getCPU_burst2 ()+ " WT: " + RunningJob.period+" RT: " + RunningJob.activePeriod);
			} else {
				pa.WriteLine ("\nRunning job: " + RunningJob.getPID () + " Current Burst: " + RunningJob.getCPU_burst1 ()+ " WT: " + RunningJob.period+" RT: " + RunningJob.activePeriod);
			}
			pa.Write ("IO Queue: ");
			if (IO_Queue.Count == 0) {
				pa.WriteLine ("NOTHING!");
			} else {
				foreach (Process item in IO_Queue) {
					pa.Write (item.getPID () + " ");
				}
				pa.WriteLine ("");
			}
			if (IO_Job.getPID () == -1) {
				pa.WriteLine ("IO Job: NO RUNNING JOB");
			} else {
				if (RunningJob.getCPU_burst1 () < 0) {
					pa.WriteLine ("IO job: " + IO_Job.getPID () + " Current Burst: " + IO_Job.getIO_burst ()+" WT: " + RunningJob.period);
				} else {
					pa.WriteLine ("IO job: " + IO_Job.getPID () + " Current Burst: " + IO_Job.getIO_burst ()+" WT: " + RunningJob.period);
				}
			}
			pa.WriteLine ("==============================================");
		}
	}
}

