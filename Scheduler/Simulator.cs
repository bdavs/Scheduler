using System;

namespace Scheduler
{
	public class Simulator
	{
		static String fileName = "input_file.txt";
		static bool debug = true;
		public Simulator()
		{
			try {
				ProcessList p = new ProcessList (fileName);

				if (debug) {
					//Echo processes to console
					Console.WriteLine ("===================================");
					Console.WriteLine ("Quantum = " + p.getQuantum ());
					for (int loop = 0; loop < p.getNumberOfProcesses (); loop++) {
						Console.WriteLine ("===================================");
						Console.WriteLine ("PID: " + p.getProcess (loop).getPID ());
						Console.WriteLine ("CPU Burst 1: " + p.getProcess (loop).getCPU_burst1 ());
						Console.WriteLine ("CPU Burst 2: " + p.getProcess (loop).getCPU_burst2 ());
						Console.WriteLine ("IO Burst : " + p.getProcess (loop).getIO_burst ());
						Console.WriteLine ("Priority: " + p.getProcess (loop).getPriority ());
						Console.WriteLine ("Period: " + p.getProcess (loop).getCurrentPeriod ());	
					}
				}

				//Populate Schedulers
				Scheduler[] sim = new Scheduler[7];
				{
					int i = 0;
					sim [i++] = new FCFS (p.clone ()); 
					/*sim [i++] = new SJF (p.clone ());
					sim [i++] = new SJR (p.clone ());
					sim [i++] = new Priority (p.clone ());
					sim [i++] = new RoundRobin (p.clone ());
					sim [i++] = new PRM (p.clone ());
					sim [i++] = new EDF (p.clone ());
					*/
				}

			} catch (Exception e) {
				Console.WriteLine ("Error trying to populate the process list: " + e.Message);
				Console.WriteLine ("Program quitting");
			}
		}

	}
}

