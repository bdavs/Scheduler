using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
namespace Scheduler
{
	public class FCFS  : Scheduler{
		Queue<Process> Ready_Queue;
		Queue<Process> IO_Queue;
		int time = 0;
		Process RunningJob;
		Process IO_Job;
		public FCFS(ProcessList processList)
		{
			Ready_Queue = new Queue<Process>();
			foreach (Process item in processList.processes) {
				Ready_Queue.Enqueue (item);
			}
			IO_Queue = new Queue<Process>();
			RunningJob = new Process (-1,0,0,0,0);
			IO_Job = new Process (-1, 0, 0, 0,0);
			StreamReader output = new StreamReader ("../../output.txt");
			simulate (1, output);
			System.Console.WriteLine (Ready_Queue.Count);
		}

	
		public override void simulate(int snapshot, StreamReader pa) {
			while ((Ready_Queue.Count != 0 || IO_Queue.Count != 0) || (RunningJob.getCPU_burst1 () > 0 || RunningJob.getCPU_burst2 () > 0)) {

				//Get the running job 
				if (RunningJob.getPID () == -1) {
					//Were on the fist iteration
					RunningJob = Ready_Queue.Dequeue ();
				}

				//RUNNING JOB LOGIC START
				if (RunningJob.getCPU_burst1 () == 0) {
					IO_Queue.Enqueue (RunningJob);
					if (Ready_Queue.Count != 0)
						RunningJob = Ready_Queue.Dequeue ();
				} else if (RunningJob.getCPU_burst1 () < 0) {
					if (RunningJob.getCPU_burst2 () != 0) {
						RunningJob.decrementCPUBurst2 ();
					} else {
						if (Ready_Queue.Count != 0)
							RunningJob = Ready_Queue.Dequeue ();
					}
				} else if (RunningJob.getCPU_burst1 () > 0) {
					RunningJob.decrementCPUBurst1 ();
				}
				//RUNNING JOB LOGIC END


				//IO JOB LOGIC START
				if (IO_Job.getPID () == -1) {
					if (IO_Queue.Count != 0) {
						IO_Job = IO_Queue.Dequeue ();
					}
				} else {
					if (IO_Job.getIO_burst () <= 0) {
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
					}
				}
				//IO JOB LOGIC END

				if (time % snapshot == 0) {
					System.Console.WriteLine ("Taking Snap at time: " + time);
					this.snapshot ();
				}

				time++;


			}
		}
		


		
			




		public override void finalReport(StreamReader pw) {
			// TODO Auto-generated method stub

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
			Console.WriteLine ("\nRunning job: "+RunningJob.getPID()+" "+RunningJob.getCPU_burst1()+" "+RunningJob.getCPU_burst2());
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
				Console.WriteLine ("IO job: " + IO_Job.getPID ());
			}
			Console.WriteLine ("==============================================");
		}
	}
}

