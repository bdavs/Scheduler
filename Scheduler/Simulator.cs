﻿using System;
using System.IO;
namespace Scheduler
{
	public class Simulator
	{
		static String fileName = "../../input_file.txt";
		//static String output= "../../output.txt";
		static bool debug = true;
		StreamWriter output;
		public Simulator()
		{
			output = new StreamWriter ("../../output.txt");
			try {
				ProcessList p = new ProcessList (fileName);
				if (debug) {
					//Echo processes to console
					output.WriteLine ("===================================");
					output.WriteLine ("Quantum = " + p.getQuantum ());
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
				Scheduler[] sim = new Scheduler[7];
				{
					int i = 0;

					//sim [i++] = new FCFS (new ProcessList(fileName),output); 
					//sim [i++] = new SJF (new ProcessList(fileName),output);
				//	sim [i++] = new SJR (new ProcessList(fileName),output);
					//sim [i++] = new RR (new ProcessList(fileName),output); 

					//sim[i++] = new Priority(new ProcessList(fileName),output);
					//sim [i++] = new MFQ (new ProcessList(fileName)); 



					//sim [i++] = new Priority (new ProcessList(fileName));

					//sim [i++] = new SJR (new ProcessList(fileName));

					output.Close ();
				}

			} catch (Exception e) {
				output.WriteLine ("Error trying to populate the process list: " + e.Message);
				output.WriteLine ("Program quitting");
			}
		}

	}
}

