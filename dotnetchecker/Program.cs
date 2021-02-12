using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace dotnetchecker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(".NET Frameworks installed:");
            Console.WriteLine();
            foreach (var ver in _versionMappings)
            {
                var val = CheckRegistry(ver.Key);
                Console.Write(" " + ver.Value + ": ");
                if (!string.IsNullOrEmpty(val))
                    Console.Write("Installed (" + val + ")");
                else
                    Console.Write("Not Found");
                Console.WriteLine();

            }
            Console.WriteLine();
            Console.WriteLine(".NET Core runtimes installed:");
            Console.WriteLine(dotnetcore());
            Console.ReadLine();
        }

        public static string CheckRegistry(string key)
        {
            RegistryKey winLogonKey = Registry.LocalMachine.OpenSubKey(key, false);
            if(winLogonKey != null)
                return winLogonKey.GetValue("Version").ToString();
            return null;
        }
        public static Dictionary<string, string> _versionMappings = new Dictionary<string, string>()
        {
            {@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v2.0.50727", ".NET Framework 2.0" },
            {@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.0", ".NET Framework 3.0" },
            {@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.5", ".NET Framework 3.5" },
            {@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full", ".NET Framework 4" },
        };

        public static string dotnetcore()
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "dotnet",
                        Arguments = "--info",
                        RedirectStandardOutput = true,
                        UseShellExecute = false
                    }

                };

                process.Start();
                var text = process.StandardOutput.ReadToEnd();
                return text.Replace(".NET runtimes installed:", "#")
                           .Split('#')[1]
                           .Replace("To install additional .NET runtimes or SDKs:", "#")
                           .Split('#')[0];
            }
            catch
            {
                return "Error reading .NET runtimes";
            }
        }
    }
}
