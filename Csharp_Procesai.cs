using System;
using System.Diagnostics;
using System.ComponentModel;

    class Processes
    {
        private async void allProcesses() //1 dalis
        {
            Process[] processes = Process.GetProcesses();
            using StreamWriter file = new("CurrentProcessList.txt");
            Console.WriteLine("All Processes is being written to file: CurrentProcessList.txt");
            foreach (Process p in processes)
            {
                try
                {
                    await file.WriteLineAsync(p.Id + " " + p.ProcessName);
                }
                catch (Exception ex)
                {

                }
            }
        }

        private async void top5Process() //2 dalis
        {
            Process[] processes = Process.GetProcesses();
            Process[] processesNew = new Process[processes.Length];
            int kiekfiltered = 0;
            Console.WriteLine("Processes is being filtered");
            for (int i = 0; i < processesNew.Length; i++)
            {
                try
                {
                    if (processes[i].TotalProcessorTime != null)
                    {
                        processesNew[kiekfiltered] = processes[i];
                        kiekfiltered++;
                    }
                }
                catch (Exception ex)
                {
                    continue;
                }
            }

            Process[] top5 = new Process[5];
            int kiektop5 = 0;
            Console.WriteLine("Finding top 5 processes");
            foreach (Process process in processesNew)
            {
                if (process != null)
                {
                  for (int j = 0; j < 5; j++)
                    {

                     if (top5[j] == null)
                        {
                            top5[j] = process;
                            kiektop5++;
                            break;
                        }
                     else if (top5[j].TotalProcessorTime.TotalSeconds < process.TotalProcessorTime.TotalSeconds)
                        {

                            for (int i = kiektop5 - 1; -1 + j < i; i--)
                            {
                                if (i + 1 < 4)
                                {
                                    top5[i + 1] = top5[i];
                                }
                            }
                            top5[j] = process;
                            if (kiektop5 + 1 < 6)
                            {
                                kiektop5++;
                            }

                            break;
                        }
                     else if (j < 4 && top5[j + 1] == null)
                        {
                            top5[j + 1] = process;
                            if (kiektop5 + 1 < 6)
                            {
                                kiektop5++;
                            }
                            break;
                        }
                    }
                }

            }




            Console.WriteLine("Processes has been wrote to file: TopProcessList<date>.csv");
            using StreamWriter file = new("TopProcessList " + DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + ".csv");
            foreach (Process p in top5)
            {
                await file.WriteLineAsync(p.ProcessName + ";" + p.Id + ";" + p.TotalProcessorTime);
            }
        }

        private async void notepad() //3 dalis
        {
            Process[] processCollection = Process.GetProcesses();
            Process[] process = new Process[processCollection.Length];
            int filtered = 0;
            Console.WriteLine("Searching if notepad is running...");
            foreach (Process p in processCollection)
            {
                if (p.ProcessName == "notepad")
                {
                    process[filtered] = p;
                    filtered++;
                }
            }
            int a = 0;
            Console.WriteLine("Results:");
            foreach (Process p in process)
            {
                if (p != null)
                {
                    Console.WriteLine(p.ProcessName + " Id:" + p.Id + " Priority:" + p.BasePriority + " Time(seconds)");
                }
            }
            if (filtered == 0)
            {
                Console.WriteLine("Programa neijungta");
            }
        }

        static void Main()
        {
            Processes myProcess = new Processes();
            myProcess.allProcesses();
            myProcess.top5Process();
            myProcess.notepad();
        }
    }