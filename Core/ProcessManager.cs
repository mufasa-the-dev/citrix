using System;
using System.Collections.Generic;

namespace Citrix.Core
{
    public class Process
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
    }

    public static class ProcessManager
    {
        private static List<Process> processes = new List<Process>
        {
            new Process { Id = 1, Name = "citrix-kernel", Status = "Running" },
            new Process { Id = 2, Name = "vfs-driver", Status = "Idle" }
        };

        public static void ListProcesses()
        {
            Console.WriteLine("PID\tNAME\t\tSTATUS");
            Console.WriteLine("----------------------------------");
            foreach (var proc in processes)
            {
                Console.WriteLine($"{proc.Id}\t{proc.Name}\t\t{proc.Status}");
            }
        }

        public static void KillProcess(int pid)
        {
            if (pid == 1)
            {
                Console.WriteLine("Error: Cannot kill core system process (PID 1).");
                return;
            }

            var proc = processes.Find(p => p.Id == pid);
            if (proc != null)
            {
                processes.Remove(proc);
                Console.WriteLine($"Process {pid} ({proc.Name}) terminated.");
            }
            else
            {
                Console.WriteLine($"Process with PID {pid} not found.");
            }
        }
    }
}