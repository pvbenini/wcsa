using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace ASPAuditor
{
    
    class Program
    {
        static void Main(string[] args)
        {
            ScanEngine newScan = new ScanEngine();
            //newScan.StartScan(@"C:\Inetpub\wwwroot\RAD\web.config");
            //newScan.StartScan(@"C:\web.config");
            //newScan.StartScan(@"C:\Documents and Settings\Administrator\Desktop\ASPAuditor\strong-web.config");
            if (args.Length<1 || !File.Exists(args[0]))
                Usage();
            else
            {
                newScan.StartScan(args[0]);
            }
            
            //newScan.PrintVulnerabilities();
            //newScan.PrintVulnerabilityBase();
        }
        static void Usage()
        {
            Console.WriteLine("");
            Console.WriteLine("ASPAuditor v0.5b by Mesut Timur");
            Console.WriteLine("Sample Usage : ");
            Console.WriteLine("ASPAuditor.exe \"C:\\Inetpub\\wwwroot\\web.config\"");
            Console.WriteLine("");
        }
    }
}
