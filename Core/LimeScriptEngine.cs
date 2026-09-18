// Feature in build! Do not use, system not completed.

using System;
using System.IO;

namespace Citrix.Core
{
    public static class LimeScriptEngine
    {
        public static void ExecuteScript(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine($"Script file not found: {path}");
                return;
            }

            string[] lines = File.ReadAllLines(path);
            Console.WriteLine($"[ScriptEngine] Executing script: {path}");

            foreach (var line in lines)
            {
                string trimmed = line.Trim();
                if (string.IsNullOrEmpty(trimmed) || trimmed.StartsWith("#"))
                    continue;

                Console.WriteLine($"> {trimmed}");
                CommandHandler.Execute(trimmed);
            }
        }
    }
}