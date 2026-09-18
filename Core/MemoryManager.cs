using Cosmos.Core;
using System;

namespace Citrix.Core
{
    public static class MemoryManager
    {
        public static void DisplayMemoryUsage()
        {
            try
            {
                uint totalRam = CPU.GetAmountOfRAM();

                Console.WriteLine("--- Citrix Memory Diagnostic ---");
                Console.WriteLine($"Total System RAM : {totalRam} MB");
                Console.WriteLine("Status           : Operational");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Memory Info Error: {ex.Message}");
            }
        }
    }
}