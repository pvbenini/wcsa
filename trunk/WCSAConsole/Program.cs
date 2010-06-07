using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using WCSA;

namespace WCSAConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            string path = "";
            string str = @"\//";
            //path = @"SampleConfigs/n2-web.config";
            if ((args.Length < 1 ) || (!File.Exists(args[0])))
            {
                Console.WriteLine("Input file doesn't found at provided location or no location provided !");
                Usage();
            }
            else
            {
                try
                {
                    path = args[0];
                    ScanEngine engine = new ScanEngine();
                    Reporter reporter = new Reporter();
                    engine.StartScan(path);
                    reporter.GenerateReport(engine.vulns, path);
                    path = "Report-" + path.Substring(path.LastIndexOfAny(str.ToCharArray()) + 1) + "-"+System.DateTime.Now.ToFileTime().ToString() +".html";
                    Console.WriteLine("---------------------------------");
                    Console.WriteLine("The Happy End");
                    path = Path.Combine("OutputReports",path.ToString());
                 
                    if (File.Exists(Constants.REPORT_NAME))
                    {
                        if (!Directory.Exists("OutputReports"))
                            Directory.CreateDirectory("OutputReports");
                        if (File.Exists(path))
                            System.IO.File.Delete(path);

                        System.IO.File.Move(Constants.REPORT_NAME, path);
                        System.IO.File.Delete(Constants.REPORT_NAME);
                        Console.WriteLine("Report is outputted with name {0}", path);
                    }

                }

                catch (WCSAException wcsaexception)
                {
                    Console.WriteLine(Constants.FILE_NOT_FOUND_MESSAGE);
                    Environment.Exit(1);
                }

                
            }
        }

        private static void Usage()
        {
            Console.WriteLine("");
            Console.WriteLine("WCSAConsole v1.0.3 by Mesut Timur");
            Console.WriteLine("Sample Usage : ");
            Console.WriteLine("WCSAconsole.exe \"C:\\Inetpub\\wwwroot\\web.config\"");
            Console.WriteLine("");
        }
    }
}
