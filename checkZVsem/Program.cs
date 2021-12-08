using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace checkZVsem
{
    internal class Program
    {

        public static string _startupPath = "";
        private static string _datevpp = "";
        private static int _return = 0;


        static void Main(string[] args)
        {

            Console.WriteLine($"checkZVsem v{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()}");

            #region _startupPath

            // get assembly path
            Type type = typeof(Program);
            string assembyPath = type.Assembly.CodeBase.ToString();
            assembyPath = assembyPath.Replace("file:///", "");
            _startupPath = Path.GetDirectoryName(assembyPath);
            Console.WriteLine($"_startupPath:{_startupPath}");

            //Set the current directory.
            try
            {
                Directory.SetCurrentDirectory(_startupPath);
            }
            catch (DirectoryNotFoundException estartuppe)
            {
                Console.WriteLine($"SetCurrentDirectory:Error:{estartuppe.Message}");
            }

            #endregion

            #region Datev Umgebung vorhanden

            // Console.WriteLine("DATEV Umgebung auslesen");
            _datevpp = Environment.GetEnvironmentVariable("DATEVPP");
            if (String.IsNullOrEmpty(_datevpp))
            {
                // Keine DATEV Umgebung erkannt
                Console.WriteLine("Es wurde keine DATEV Umgebung erkannt ENV:DATEVPP nicht gesetzt");
                Environment.Exit(9998);
            }

            Console.WriteLine($"DATEVPP: {_datevpp}");

            #endregion

            // Check ToCtrl.sem 

            var file = new FileInfo(Path.Combine(GetDpPathMap(), @"DATEN\ZVKW\BESTAND\STANDARD\ToCtrl.sem"));
            TimeSpan ts = DateTime.Now - file.LastAccessTime;

            Console.WriteLine($"{file.FullName}, {ts.TotalHours}");

            if (args.Length < 1)  
            {
                //
            }
            else
            {
                if(ts.TotalHours > Int32.Parse(args[0]))
                {
                    Console.WriteLine($"ERROR: last access to old");

                    _return = ts.Hours;

                    using (EventLog eventLog = new EventLog("Application"))
                    {
                        eventLog.Source = "Application";
                        eventLog.WriteEntry($"{file.FullName} last access to old - Hours:{ts.TotalHours} > {Int32.Parse(args[0])}", EventLogEntryType.Error, 2111, 1);
                    }

                }
            }



            Environment.Exit(_return);

        }

        private static string GetDpPathMap()
        {
            var _dtvsp = "";
            try
            {
                RegistryKey WFKey =
                    Registry.LocalMachine.OpenSubKey($@"SOFTWARE\DATEVeG\InstallInfos\DefaultDataServers");
                var defaultserver = (string)WFKey.GetValue("");
                WFKey.Close();
                WFKey.Dispose();

                var regkey = $@"SOFTWARE\DATEVeG\InstallInfos\DefaultDataServers\{defaultserver}";
                RegistryKey WFKey2 = Registry.LocalMachine.OpenSubKey(regkey);
                _dtvsp = (string)WFKey2.GetValue("DefaultVolume");
                WFKey2.Close();
                WFKey2.Dispose();

            }
            catch (Exception eGetDpPathMap)
            {
                Console.WriteLine($"Error:{eGetDpPathMap.Message}");
            }

            var r = $"{_dtvsp}\\DATEV\\";
            Console.WriteLine($"GetDpPathMap:{r}");

            return r;
        }
    }
}
