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
				System.Console.WriteLine (item.getPID());
				Ready_Queue.Enqueue (item);
			}
			IO_Queue = new Queue<Process>();
			RunningJob = new Process (-1,0,0,0,0);
			IO_Job = new Process (-1, 0, 0, 0,0);
			StreamReader output = new StreamReader ("../../output.txt");
			simulate(10,output);
			snapshot ();
		}

	
		public override void simulate(int snapshot, StreamReader pa) {
			while (Ready_Queue.Count != 0 && IO_Queue.Count != 0 && RunningJob.getCPU_burst1() > 0 && RunningJob.getCPU_burst2() > 0) {

				if (time % snapshot == 0) {
					System.Console.WriteLine ("Taking Snap at time: " + time);
					this.snapshot ();
				}

				//Get the running job 
				if (RunningJob.getPID () == -1) {
					//Were on the fist iteration
					RunningJob = Ready_Queue.Dequeue ();
				}

				//RUNNING JOB LOGIC START
				if (RunningJob.getCPU_burst1 () == 0) {
					IO_Queue.Enqueue (RunningJob);
					if(Ready_Queue.Count !=0)
						RunningJob = Ready_Queue.Dequeue ();
				} else if (RunningJob.getCPU_burst1 () < 0) {
					if (RunningJob.getCPU_burst2 () != 0) {
						RunningJob.decrementCPUBurst2 ();
					} else {
						if(Ready_Queue.Count !=0)
							RunningJob = Ready_Queue.Dequeue ();
					}
				} else if (RunningJob.getCPU_burst1 () > 0) {
					RunningJob.decrementCPUBurst1 ();
				}
				//RUNNING JOB LOGIC END


				//IO JOB LOGIC START
				if (IO_Job.getPID () == -1)
					IO_Job = IO_Queue.Dequeue ();
				if (IO_Job.getIO_burst () <= 0) {
					IO_Job.decrementCPUBurst1 ();
					Ready_Queue.Enqueue (IO_Job);
					IO_Job = IO_Queue.Dequeue ();
				} else {
					IO_Job.decrementCPUBurst1 ();//Cuz FUCK U
					IO_Job.decrementIO_burst ();
				}
				//IO JOB LOGIC END

				time++;
			}

		}


		public override void finalReport(StreamReader pw) {
			// TODO Auto-generated method stub

		}

		void snapshot(){
			Console.WriteLine ("****************************************************");
			foreach (Process item in Ready_Queue) {
				System.Console.WriteLine (item.getPID());
			}
			/*foreach (Process item in Ready_Queue) {
				System.Console.WriteLine (item.ToString());
			}*/
		}
	}
}

