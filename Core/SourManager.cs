// Feature in build! Do not use, we don't stand up a servers.

using System;
using System.IO;

namespace Citrix.Core
{
    public static class SourManager
    {
        public static void ProcessCommand(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: sour <install|list|remove> [package.srpk]");
                return;
            }

            string cmd = args[0].ToLower();

            if (cmd == "install" && args.Length > 1)
            {
                InstallPackage(args[1]);
            }
            else if (cmd == "list")
            {
                ListPackages();
            }
            else
            {
                Console.WriteLine($"Unknown sour command or missing arguments.");
            }
        }

        private static void InstallPackage(string file)
        {
            Console.WriteLine($"[Sour Engine] Unpacking package {file}...");

            try
            {
                if (!Directory.Exists(@"0:\bin"))
                {
                    Directory.CreateDirectory(@"0:\bin");
                }

                string target = $@"0:\bin\{file.Replace(".srpk", "")}";
                File.WriteAllText(target, $"# Citrix SRPK Executable Payload: {file}");

                Console.WriteLine($"[Sour Engine] Successfully registered {file} -> /bin/");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Sour Installation Error: {ex.Message}");
            }
        }

        private static void ListPackages()
        {
            Console.WriteLine("Installed Sour Packages:");
            if (Directory.Exists(@"0:\bin"))
            {
                var files = Directory.GetFiles(@"0:\bin");
                foreach (var f in files)
                {
                    Console.WriteLine($" - {f}");
                }
            }
        }
    }
}