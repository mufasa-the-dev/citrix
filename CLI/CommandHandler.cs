using Cosmos.System.FileSystem.Listing;
using Cosmos.System.FileSystem.VFS;
using System;
using System.IO;
using System.Text;

namespace Citrix.Core
{
    public static class CommandHandler
    {
        private static string currentDirectory = @"0:\";

        public static void Execute(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return;

            string[] parts = input.Split(' ');
            if (parts.Length == 0) return;

            string command = parts[0].ToLower();

            string[] args = new string[parts.Length > 1 ? parts.Length - 1 : 0];
            for (int i = 1; i < parts.Length; i++)
            {
                args[i - 1] = parts[i];
            }

            if (command == "ls" || command == "dir")
            {
                ListDirectory();
            }
            else if (command == "cd")
            {
                ChangeDirectory(args);
            }
            else if (command == "mkdir")
            {
                MakeDirectory(args);
            }
            else if (command == "cat")
            {
                CatFile(args);
            }
            else if (command == "echo")
            {
                EchoToFile(args);
            }
            else if (command == "format" || command == "mkfs")
            {
                FormatDisk(args);
            }
            else if (command == "apt")
            {
                LinuxApt.ProcessCommand(args);
            }
            else if (command == "sour")
            {
                SourManager.ProcessCommand(args);
            }
            else if (command == "net")
            {
                NetworkManager.Initialize();
            }
            else if (command == "ip" || command == "ifconfig")
            {
                NetworkManager.DisplayInterfaceDetails();
            }
            else if (command == "ping")
            {
                if (args.Length > 0)
                    NetworkManager.PingHost(args[0]);
                else
                    Console.WriteLine("Usage: ping <ip>");
            }
            else if (command == "ps")
            {
                ProcessManager.ListProcesses();
            }
            else if (command == "mem")
            {
                MemoryManager.DisplayMemoryUsage();
            }
            else if (command == "help")
            {
                DisplayHelp();
            }
            else
            {
                Console.WriteLine("Unknown command.");
            }
        }

        public static string GetCurrentDirectory() => currentDirectory;

        private static void ListDirectory()
        {
            try
            {
                var entries = VFSManager.GetDirectoryListing(currentDirectory);
                for (int i = 0; i < entries.Count; i++)
                {
                    var entry = entries[i];
                    string type = entry.mEntryType == DirectoryEntryTypeEnum.Directory ? "<DIR>" : "     ";
                    Console.WriteLine($"{type}  {entry.mName}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"VFS Error: {ex.Message}");
            }
        }

        private static void ChangeDirectory(string[] args)
        {
            if (args.Length == 0)
            {
                currentDirectory = @"0:\";
                return;
            }

            string target = args[0];
            if (target == "..")
            {
                if (currentDirectory != @"0:\" && currentDirectory != @"0:/")
                {
                    currentDirectory = @"0:\";
                }
                return;
            }

            string newPath = Path.Combine(currentDirectory, target);
            if (VFSManager.DirectoryExists(newPath))
            {
                currentDirectory = newPath.EndsWith(@"\") ? newPath : newPath + @"\";
            }
            else
            {
                Console.WriteLine("Directory not found.");
            }
        }

        private static void MakeDirectory(string[] args)
        {
            if (args.Length == 0) return;
            try
            {
                VFSManager.CreateDirectory(Path.Combine(currentDirectory, args[0]));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"mkdir error: {ex.Message}");
            }
        }

        private static void CatFile(string[] args)
        {
            if (args.Length == 0) return;
            string path = Path.Combine(currentDirectory, args[0]);

            if (VFSManager.FileExists(path))
            {
                try
                {
                    using (var fs = File.OpenRead(path))
                    {
                        byte[] b = new byte[fs.Length];
                        fs.Read(b, 0, b.Length);
                        Console.WriteLine(Encoding.ASCII.GetString(b));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"cat error: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("File not found.");
            }
        }

        private static void EchoToFile(string[] args)
        {
            if (args.Length < 3) return;
            string text = args[0];
            string filename = args[2];
            string path = Path.Combine(currentDirectory, filename);

            try
            {
                using (var fs = File.Create(path))
                {
                    byte[] bytes = Encoding.ASCII.GetBytes(text);
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"echo error: {ex.Message}");
            }
        }

        private static void FormatDisk(string[] args)
        {
            string drive = "0";
            if (args.Length > 0)
            {
                drive = args[0].Replace(":", "").Replace(@"\", "");
            }

            Console.Write($"WARNING: All data on drive {drive}:\\ will be destroyed. Proceed? (y/n): ");
            string confirm = Console.ReadLine()?.ToLower();

            if (confirm != "y" && confirm != "yes")
            {
                Console.WriteLine("Formatting canceled.");
                return;
            }

            Console.WriteLine($"Cleaning drive {drive}:\\...");

            try
            {
                var entries = VFSManager.GetDirectoryListing($@"{drive}:\");
                for (int i = 0; i < entries.Count; i++)
                {
                    var entry = entries[i];
                    string fullPath = Path.Combine($@"{drive}:\", entry.mName);

                    if (entry.mEntryType == DirectoryEntryTypeEnum.Directory)
                    {
                        VFSManager.DeleteDirectory(fullPath, true);
                    }
                    else
                    {
                        VFSManager.DeleteFile(fullPath);
                    }
                }
                currentDirectory = @"0:\";
                Console.WriteLine("Drive cleaned successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Format Error: {ex.Message}");
            }
        }

        private static void DisplayHelp()
        {
            Console.WriteLine("Commands: ls, cd, mkdir, cat, echo, format, apt, sour, net, ip, ping, ps, mem");
        }
    }
}