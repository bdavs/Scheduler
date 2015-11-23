using System;
using System.IO;
using System.Collections.Generic;

namespace Scheduler
{
	public class MFQ : Scheduler{
		public int quantum;
		public ProcessList RRprocessList = new ProcessList ();
        public Queue<Process> ReadyQueue = new Queue<Process>();
        public Queue<Process> ReadyQueue1 = new Queue<Process>();
        public Queue<Process> ReadyQueue2 = new Queue<Process>();
        public Queue<Process> ReadyQueue3 = new Queue<Process>();
        public Queue<Process> IOQueue = new Queue<Process>();
		public Process currentProcess;
		public Process currentIO;
		public int time;
		float cputicks; 
		List<Process> Final_List;


		public MFQ(ProcessList processList, StreamWriter output)
		{
			RRprocessList = processList.clone ();
			Final_List = new List<Process> ();
			simulate(10,output);
		}


        public override void simulate(int snapshot, StreamWriter pa)
        {
            // TODO Auto-generated method stub
            pa.WriteLine("**************************************MFQ STARTED**************************");
            //quantum	= 1;//RRprocessList.getQuantum();

            //add all processes to queue
            foreach (Process process in RRprocessList.processes)
            {
                ReadyQueue1.Enqueue(process);
                //pa.WriteLine (process.getCPU_burst1 ());
            }

            //initialize currents
            ReadyQueue1.Enqueue(new Process(-1, -1, -1, -1, -1));//dummy value, will check for this later

            currentProcess = ReadyQueue1.Dequeue();
            currentIO = new Process(-1, -1, -1, -1, -1);

            //main RR while loop
          
            while (ReadyQueue1.Count > 0)
            {


                if (currentProcess.getPID() == -1)
                {
                    ReadyQueue2.Enqueue(currentProcess);
                    currentProcess = ReadyQueue2.Dequeue();
                    continue;
                }


                //quantum for loop
                for (int i = 0; i < (quantum = 3); i++)
                {
                    time++;
                    //cpu processing
                    //pa.WriteLine (currentProcess.getPID ());

                    foreach (Process p in ReadyQueue2)
                    {
                        p.period++;
                    }
                    
                    if (currentProcess.getCPU_burst1() > 0)
                    {
                        currentProcess.decrementCPUBurst1();
                    }
                    else if (currentProcess.getCPU_burst1() == 0 && currentProcess.getIO_burst() > 0)
                    {
                        IOQueue.Enqueue(currentProcess);
                        break;
                    }
                    else if (currentProcess.getCPU_burst1() == 0 && currentProcess.getIO_burst() == 0 && currentProcess.getCPU_burst2() > 0)
                    {
                        currentProcess.decrementCPUBurst2();
                    }
                    else
                    {
                        break;
                    }
                    currentProcess.activePeriod++;

                    //io processing
                    if (currentIO.getIO_burst() > 0)
                    {
                        currentIO.decrementIO_burst();
                    }
                    else if (currentIO.getIO_burst() == 0)
                    {
                        ReadyQueue2.Enqueue(currentIO);
                        if (IOQueue.Count > 0)
                        {
                            currentIO = IOQueue.Dequeue();
                        }
                        else
                        {
                            currentIO = new Process(-1, -1, -1, -1, -1);
                        }
                    }
                    else
                    {
                        if (IOQueue.Count > 0)
                        {
                            currentIO = IOQueue.Dequeue();
                        }
                    }
                    //increment time
                   

                    //POKEMON SNAP
                    if (time % snapshot == 0)
                    {
                        pa.WriteLine("Taking Snap at time: " + time);
                        ReadyQueue = ReadyQueue1;
                        this.snapshot(pa);
                        pa.WriteLine("queue2");
                        ReadyQueue = ReadyQueue2;
                        this.snapshot(pa);
                    }
                }

                if (currentProcess.getCPU_burst1() > 0 || (currentProcess.getCPU_burst2() > 0 && currentProcess.getIO_burst() == 0))
                {
                    ReadyQueue2.Enqueue(currentProcess);
                }
                else if (currentProcess.getCPU_burst1() == 0 && currentProcess.getIO_burst() > 0 && !IOQueue.Contains(currentProcess))
                {
                    IOQueue.Enqueue(currentProcess);
                }
                else if (currentProcess.getCPU_burst1() == 0 && currentProcess.getCPU_burst2() == 0 && currentProcess.getIO_burst() == 0)
                {
                    currentProcess.activePeriod-=2;
                    Final_List.Add(currentProcess);
                }
                if (ReadyQueue1.Count > 0) currentProcess = ReadyQueue1.Dequeue();

            }
           
            /////////////////////////////////////////////////////////////////////////////////
            ////////start of queue 2/////////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////
            if (ReadyQueue2.Count > 0) currentProcess = ReadyQueue2.Dequeue();
            while (ReadyQueue2.Count > 0)
            {
                time++;
                if (currentProcess.getPID() == -1)
                {
                    ReadyQueue3.Enqueue(currentProcess);
                    currentProcess = ReadyQueue3.Dequeue();
                    break;
                }

                //quantum for loop
                for (int i = 0; i < (quantum = 8); i++)
                {

                    //cpu processing
                    //pa.WriteLine (currentProcess.getPID ());

                    foreach (Process p in ReadyQueue2)
                    {
                        p.period++;
                    }
                   
                    if (currentProcess.getCPU_burst1() > 0)
                    {
                        currentProcess.decrementCPUBurst1();
                    }
                    else if (currentProcess.getCPU_burst1() == 0 && currentProcess.getIO_burst() > 0)
                    {
                        IOQueue.Enqueue(currentProcess);
                        //pa.WriteLine ("into IO:"+ currentProcess.getPID ());
                        break;
                    }
                    else if (currentProcess.getCPU_burst1() == 0 && currentProcess.getIO_burst() == 0 && currentProcess.getCPU_burst2() > 0)
                    {
                        currentProcess.decrementCPUBurst2();
                    }
                    else
                    {
                        //Final_List.Enqueue (currentProcess);
                        break;
                    }
                    currentProcess.activePeriod++;

                    //io processing
                    if (currentIO.getIO_burst() > 0)
                    {
                        currentIO.decrementIO_burst();
                    }
                    else if (currentIO.getIO_burst() == 0)
                    {
                        ReadyQueue3.Enqueue(currentIO);
                        if (IOQueue.Count > 0)
                        {
                            currentIO = IOQueue.Dequeue();
                        }
                        else
                        {
                            currentIO = new Process(-1, -1, -1, -1, -1);
                        }
                    }
                    else
                    {
                        if (IOQueue.Count > 0)
                        {
                            currentIO = IOQueue.Dequeue();
                        }
                    }
                    //increment time
                    

                    //POKEMON SNAP
                    if (time % snapshot == 0)
                    {
                        pa.WriteLine("Taking Snap at time: " + time);
                        ReadyQueue = ReadyQueue1;
                        this.snapshot(pa);
                    }
                }

                if (currentProcess.getCPU_burst1() > 0 || (currentProcess.getCPU_burst2() > 0 && currentProcess.getIO_burst() == 0))
                {
                    ReadyQueue3.Enqueue(currentProcess);
                }
                else if (currentProcess.getCPU_burst1() == 0 && currentProcess.getIO_burst() > 0 && !IOQueue.Contains(currentProcess))
                {
                    IOQueue.Enqueue(currentProcess);
                    //pa.WriteLine ("into IO2:"+ currentProcess.getPID ());
                }
                else if (currentProcess.getCPU_burst1() == 0 && currentProcess.getCPU_burst2() == 0 && currentProcess.getIO_burst() == 0)
                {
                    currentProcess.activePeriod-=2;
                    Final_List.Add(currentProcess);
                }
                if (ReadyQueue2.Count > 0) currentProcess = ReadyQueue2.Dequeue();

            }
            /////////////////////////////////////////////////////////////////////////////////
            ////////start of queue 3/////////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////
            while ((ReadyQueue3.Count != 0 || IOQueue.Count != 0) || (currentProcess.getCPU_burst1() > 0 || currentProcess.getCPU_burst2() > 0) || (currentIO.getIO_burst() > 0))
            {
                time++;
                //Get the running job 
                if (currentProcess.getPID() == -1)
                {
                    //OH GEEZ RICK, Were on the fist iteration
                    currentProcess = ReadyQueue3.Dequeue();
                }



                //RUNNING JOB LOGIC START
                if (currentProcess.getCPU_burst1() == 1)
                {
                    IOQueue.Enqueue(currentProcess);
                    if (ReadyQueue3.Count != 0)
                    {
                        currentProcess.activePeriod++;
                        currentProcess = ReadyQueue3.Dequeue();
                        currentProcess.period++;
                    }
                }
                else if (currentProcess.getCPU_burst1() < 1)
                {
                    if (currentProcess.getCPU_burst2() != 0)
                    {
                        currentProcess.decrementCPUBurst2();
                        currentProcess.activePeriod++;
                    }
                    else
                    {
                        if (ReadyQueue3.Count != 0)
                        {
                            Final_List.Add(currentProcess);
                            currentProcess = ReadyQueue3.Dequeue();
                        }
                    }
                }
                else if (currentProcess.getCPU_burst1() > 1)
                {
                    currentProcess.decrementCPUBurst1();
                    currentProcess.activePeriod++;
                }

                //RUNNING JOB LOGIC END


                //IO JOB LOGIC START
                if (currentIO.getPID() == -1)
                {
                    if (IOQueue.Count != 0)
                    {
                        currentIO = IOQueue.Dequeue();
                    }
                }
                else
                {
                    if (currentIO.getIO_burst() <= 1)
                    {
                        currentIO.decrementCPUBurst1();
                        ReadyQueue3.Enqueue(currentIO);
                        if (IOQueue.Count != 0)
                        {
                            currentIO = IOQueue.Dequeue();
                        }
                        else
                        {
                            currentIO = new Process(-1, 0, 0, 0, 0);
                        }

                    }
                    else
                    {
                        currentIO.decrementCPUBurst1();//Cuz FUCK U
                        currentIO.decrementIO_burst();
                        //currentIO.period++;
                    }
                }
                //IO JOB LOGIC END


                //Count waiting time
                foreach (Process item in ReadyQueue3)
                {
                    if (item.getPID() <= time)
                    {
                        item.period++;
                    }
                }
                foreach (Process item in IOQueue)
                {
                    //item.period++;
                }
                //End waiting time count
                



                //POKEMON SNAP
                if (time % snapshot == 0)
                {
                    pa.WriteLine("Taking Snap at time: " + time);
                    ReadyQueue = ReadyQueue3;
                    this.snapshot(pa);
                }

            }
            Final_List.Add(currentProcess);
            foreach (Process item in Final_List) { item.period--; item.activePeriod--; }
            finalReport(pa);
            pa.WriteLine("**************************************MFQ ENDED**************************");
        }



		public override void finalReport(StreamWriter pa) {
			int waiting_time = 0;
			int turnaround_time = 0;
			pa.WriteLine ("Final Report");
			pa.WriteLine ("PID         WAIT TIME");
            Final_List.Sort((p, q) => (p.getPID()).CompareTo(q.getPID()));
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

        void snapshot(StreamWriter pa)
        {
            pa.WriteLine("==============================================");
            if (ReadyQueue.Count == 0)
            {
                pa.Write("Ready Queue: NOTHING");
            }
            else
            {
                pa.Write("Ready Queue: ");
                foreach (Process item in ReadyQueue)
                {
                    pa.Write(item.getPID() + " ");
                }
            }
            if (currentProcess.getCPU_burst1() < 0)
            {
                pa.WriteLine("\nRunning job: " + currentProcess.getPID() + " Current Burst: " + currentProcess.getCPU_burst2() + " WT: " + currentProcess.period + " RT: " + currentProcess.activePeriod);
            }
            else
            {
                pa.WriteLine("\nRunning job: " + currentProcess.getPID() + " Current Burst: " + currentProcess.getCPU_burst1() + " WT: " + currentProcess.period + " RT: " + currentProcess.activePeriod);
            }
            pa.Write("IO Queue: ");
            if (IOQueue.Count == 0)
            {
                pa.WriteLine("NOTHING!");
            }
            else
            {
                foreach (Process item in IOQueue)
                {
                    pa.Write(item.getPID() + " ");
                }
                pa.WriteLine("");
            }
            if (currentIO.getPID() == -1)
            {
                pa.WriteLine("IO Job: NO RUNNING JOB");
            }
            else
            {
                if (currentProcess.getCPU_burst1() < 0)
                {
                    pa.WriteLine("IO job: " + currentIO.getPID() + " Current Burst: " + currentIO.getIO_burst() + " WT: " + currentProcess.period);
                }
                else
                {
                    pa.WriteLine("IO job: " + currentIO.getPID() + " Current Burst: " + currentIO.getIO_burst() + " WT: " + currentProcess.period);
                }
            }
            pa.WriteLine("==============================================");
        }

    }
}

