using System;
using System.IO;
namespace Scheduler
{
	public class Simulator
	{
		static String fileName = "../../input_file.txt";
		//static String output= "../../output.txt";
		static bool debug = true;
		StreamWriter output;
		int Lowest;
		public Simulator()
		{
			Lowest = 99999;
			output = new StreamWriter ("../../output.txt");
			try {
				ProcessList p = new ProcessList (fileName);
				if (debug) {
					//Echo processes to console
					output.WriteLine ("===================================");
					output.WriteLine ("Quantum = " + p.getQuantum ());
                   // Console.Read();
					for (int loop = 0; loop < p.getNumberOfProcesses (); loop++) {
						output.WriteLine ("===================================");
						output.WriteLine ("PID: " + p.getProcess (loop).getPID ());
						output.WriteLine ("CPU Burst 1: " + p.getProcess (loop).getCPU_burst1 ());
						output.WriteLine ("CPU Burst 2: " + p.getProcess (loop).getCPU_burst2 ());
						output.WriteLine ("IO Burst : " + p.getProcess (loop).getIO_burst ());
						output.WriteLine ("Priority: " + p.getProcess (loop).getPriority ());
						output.WriteLine ("Period: " + p.getProcess (loop).getCurrentPeriod ());	
					}
				}

				//Populate Schedulers
				Scheduler[] sim = new Scheduler[6];
				{
					int i = 0;

					sim [i++] = new FCFS (new ProcessList(fileName),output);
					sim [i++] = new SJF (new ProcessList(fileName),output);
					sim [i++] = new SJR (new ProcessList(fileName),output);
					sim [i++] = new RR (new ProcessList(fileName),output); 
<<<<<<< HEAD

					sim[i++] = new Priority(new ProcessList(fileName),output);
=======
                    sim [i++] = new Priority(new ProcessList(fileName),output);
>>>>>>> b031e9d893a0eb5f419eb22541f5e3a1e28d720c
					sim [i++] = new MFQ (new ProcessList(fileName), output); 


				}
				int j; 
				output.WriteLine ("ALGORITHM        AVERAGE TURNAROUND TIME");
				for(j =0 ; j < 6; j++){
					try{
						output.WriteLine (sim[j].GetType ().Name+"                "+sim[j].Average_TurnAround);
						if(sim[j].Average_TurnAround < Lowest)
							Lowest = sim[j].Average_TurnAround;
					}catch(NullReferenceException){
						//THIS MEANS WE DID NOT PUT ALL THE PROCESSES IN YET WHICH IS FINE
					}
				}
				foreach(Scheduler item in sim){
					try{
						if(item.Average_TurnAround == Lowest)
							output.WriteLine ("The best algorithm for the data set is "+item.GetType ().Name+" with a average turnaround time of "+item.Average_TurnAround);
					}catch(NullReferenceException){
						//THIS MEANS WE DID NOT PUT ALL THE PROCESSES IN YET WHICH IS FINE
					}
				}
			} catch (Exception e) {
				Console.WriteLine ("Error trying to populate the process list: " + e.Message);
				output.WriteLine ("Program quitting");
			}finally{
				output.Close ();
			}
		}

	}
}

