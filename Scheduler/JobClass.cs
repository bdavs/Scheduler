using System;

namespace Scheduler
{
	public class Job
	{
		public int pid;
		public int cpu_burst;
		public int io_burst;
		public Job ()
		{
			pid = 0;
			cpu_burst = 0;
			io_burst = 0;
		}
	}
	public class ProcessList
	{
			private Process[] processes;
			private int quantum;

			private ProcessList(){}

			public ProcessList(String filename) //throws FileNotFoundException
			{
				//Open file for reading
				Scanner input = new Scanner(new File(filename));

				//Read in Total_Processes
				input.next();

				//read in "Total_Processes"
				int processTotal = input.nextInt();

				//Advance to the next line
				input.nextLine();

				//Read in "Quantum"
				input.next();

				//read in quantum
				quantum = input.nextInt();

				//Advance to the next line
				input.nextLine();

				//Advance to the next line to skip past headers %Pid CPU Burst etc
				input.nextLine();

				//Allocate processes
				processes = new Process[processTotal];

				//Read in each process information
				for(int loop = 0; loop<processTotal;loop++)
				{
					int pid = input.nextInt();
					int CPU_burst = input.nextInt();
					int IO_burst = input.nextInt(); 
					int priority = input.nextInt();
					int period = input.nextInt();

					processes[loop] = new Process(pid,CPU_burst,IO_burst,priority,period);
				}
			}

			public int getQuantum()
			{
				//return the quantum value for RR
				return quantum;
			}

			public Process getProcess(int index)
			{
				//returns requested process if it exists, null otherwise
				if(index<processes.length && index>=0)
				{
					return processes[index];
				}
				return null;
			}

			public int getNumberOfProcesses()
			{
				//returns the total number of processes
				return processes.length;
			}

			public ProcessList clone()
			{
				//Make an identical copy of the Processes in ProcessList

				ProcessList pl = new ProcessList();
				pl.processes = new Process[processes.length];
				pl.quantum = this.quantum;
				for(int loop = 0; loop < processes.length; loop++)
				{
					pl.processes[loop] = processes[loop].clone();
				}

				return pl;
			}
	}
}

