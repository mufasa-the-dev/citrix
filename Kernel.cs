using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.VFS;
using System;
using Sys = Cosmos.System;

namespace Citrix.Core
{
    public class Kernel : Sys.Kernel
    {
        private CosmosVFS vfs;

        protected override void BeforeRun()
        {
            vfs = new CosmosVFS();
            VFSManager.RegisterVFS(vfs);

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Citrix [Citrix Kernel v2.16 | x32]");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Under Construction - It's first Public version!");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("VFS: Initialized successfully.");
            Console.WriteLine("System ready.\n");
        }

        protected override void Run()
        {
            Console.Write($"{CommandHandler.GetCurrentDirectory()}> ");
            string input = Console.ReadLine();

            try
            {
                CommandHandler.Execute(input);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Kernel Exception] {ex.Message}");
            }
        }
    }
}