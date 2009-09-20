namespace WCSA
{
    using System;
    using System.IO;

    internal class Program
    {
        private static void Main(string[] args)
        {
            ScanEngine engine = new ScanEngine();

            string path;
            if (args.Length > 1)
                path = args[0].ToString();
            else 
                Usage();
            path = @"SampleConfigs/n2-web.config";
            if ((path.Length < 1) || !File.Exists(path))
            {
                Usage();
            }
            else
            {
                engine.StartScan(path);
                new Reporter(engine.vulns, path);
            }
        }

        private static void Usage()
        {
            Console.WriteLine("");
            Console.WriteLine("WCSA v0.5b by Mesut Timur");
            Console.WriteLine("Sample Usage : ");
            Console.WriteLine("WCSA.exe \"C:\\Inetpub\\wwwroot\\web.config\"");
            Console.WriteLine("");
        }
    }
}

