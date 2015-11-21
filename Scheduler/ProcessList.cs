using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Generic;

namespace Scheduler
{
	public class ProcessList
	{
			public List<Process> processes;
			private int quantum;

			public ProcessList(){}

			public ProcessList(String filename) //throws FileNotFoundException
			{
				//Open file for reading
				//Scanner input = new Scanner(new File(filename));
				StreamReader input = new StreamReader(filename);
				//StreamWriter input = new StreamWriter(new File(filename));
				//Read in Total_Processes
				input.ReadLine();

				//read in "Total_Processes"
				int processTotal = Int32.Parse(input.ReadLine());

				//Read in "Quantum"
				input.ReadLine();

				//read in quantum 
				quantum = Int32.Parse(input.ReadLine()) ;


				//Advance to the next line to skip past headers %Pid CPU Burst etc
			input.ReadLine();

				//Allocate processes
			processes = new List<Process>();
			String regex_pattern = @"([0-9]+)\s+([0-9]+)\s+([0-9]+)\s+([0-9]+)\s*";
				//Read in each process information
				
				for(int loop = 0; loop<processTotal;loop++)
				{

				Match line = Regex.Match(input.ReadLine(),regex_pattern);;
				//Console.WriteLine ("looping "+line.Groups[1]);
				int pid = Int32.Parse(line.Groups[1].Value);
				int CPU_burst = Int32.Parse(line.Groups[2].Value);
				int IO_burst = Int32.Parse(line.Groups[3].Value);
				int priority = Int32.Parse(line.Groups[4].Value);
				int period = 1;//Int32.Parse(line.Groups[5].Value);

				processes.Add(new Process(pid,CPU_burst,IO_burst,priority,period));
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
			if(index<processes.Count && index>=0)
				{
					return processes[index];
				}
				return null;
			}

			public int getNumberOfProcesses()
			{
				//returns the total number of processes
			return processes.Count;
			}

			public ProcessList clone()
			{
				//Make an identical copy of the Processes in ProcessList

			ProcessList pl = new ProcessList();
				pl = this;
				return pl;
			}
	}
}

