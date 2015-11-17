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
		Process RunningJob;
		public FCFS(ProcessList processList)
		{
			Ready_Queue = new Queue<Process>(processList.processes);
			IO_Queue = new Queue<Process>();
			RunningJob = new Process (-1,0,0,0,0);
			StreamReader output = new StreamReader ("output.txt");
			simulate(10,output);
		}

	
		public override void simulate(int snapshot, StreamReader pa) {
			while (FCFSprocessList.Count != 0 && Ready_Queue.Count != 0 && IO_Queue.Count != 0) {

				if (time % snapshot == 0)
					this.snapshot ();

				//Put everything we can into queue first
				foreach (Process myprocess in FCFSprocessList) {
					if (myprocess.getPID() == time) {
						Ready_Queue.Enqueue(myprocess);
						FCFSprocessList.Remove (myprocess);
					}
				}

				//Get the running job 
				if (RunningJob.getPID () == -1) {
					//Were on the fist iteration
					RunningJob = Ready_Queue.Dequeue ();
				}

				//RUNNING JOB LOGIC START
				if (RunningJob.getCPU_burst1 () == 0) {
					IO_Queue.Enqueue (RunningJob);
					RunningJob = Ready_Queue.Dequeue ();
				}else if (RunningJob.getCPU_burst1 () < 0) {
					if (RunningJob.getCPU_burst2 () != 0) {
						RunningJob.decrementCPUBurst2 ();
					} else {
						RunningJob = Ready_Queue.Dequeue ();
					}
				}else if (RunningJob.getCPU_burst1() > 0) {
					RunningJob.decrementCPUBurst1 ();
				}
				//RUNNING JOB LOGIC END


				//IO JOB LOGIC START


				//IO JOB LOGIC END
				time++;
			}

		}


		public override void finalReport(StreamReader pw) {
			// TODO Auto-generated method stub

		}

		void snapshot(){
			foreach (Process item in Ready_Queue) {
				System.Console.WriteLine (item.ToString());
			}
			foreach (Process item in Ready_Queue) {
				System.Console.WriteLine (item.ToString());
			}
		}
	}
}

