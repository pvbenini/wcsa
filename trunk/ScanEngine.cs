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
    class ScanEngine
    {
        public ScanEngine()
        {
            this.vulns = VulnEngine.LoadVulns();
        }
        public Vulnerability[] vulns;

        public void AnalyzeNode(string xtrName, Hashtable attributes)
        {

            foreach (Vulnerability vuln in this.vulns)
            {
                if (vuln == null)
                    continue;

                if (vuln.pr.tag == xtrName && attributes[vuln.pr.property].ToString() == vuln.pr.value)
                    vuln.pr.isPassed = true;

                if (!vuln.pr.isPassed)
                    continue;

                if (xtrName == vuln.codename)
                {
                    int count = 0;
                    foreach (Object vulnO in vuln.test.Keys)
                    {

                        foreach (Object AttO in attributes.Keys)
                        {
                            switch (vuln.op)
                            {
                                case Operator.Equal:
                                    {
                                        if (vulnO.ToString() == AttO.ToString())
                                        {
                                            vuln.tested = true;
                                            if ((vuln.test[vulnO].ToString() == attributes[AttO].ToString()))
                                            {
                                                count++;
                                                if (vuln.test.Count == count)
                                                {
                                                    Console.WriteLine(vuln.code + ":" + vuln.title + "-->" + String.Format("{0}.{1}", vuln.codename, vuln.test[vulnO].ToString()));
                                                    vuln.found = true;
                                                }
                                            }
                                        }
                                        break;
                                    }
                                case Operator.NotEqual:
                                    {
                                        if (vulnO.ToString() == AttO.ToString())
                                        {
                                            vuln.tested = true;
                                            if ((vuln.test[vulnO].ToString() != attributes[AttO].ToString()))
                                            {
                                                count++;
                                                if (vuln.test.Count == count)
                                                {
                                                    Console.WriteLine(vuln.code + ":" + vuln.title + "-->" + String.Format("{0}.{1}", vuln.codename, vuln.test[vulnO].ToString()));
                                                    vuln.found = true;
                                                }
                                            }
                                        }
                                        break;
                                    }
                                case Operator.Greater:
                                    {
                                        if (vulnO.ToString() == AttO.ToString())
                                        {
                                            vuln.tested = true;
                                            if (int.Parse(vuln.test[vulnO].ToString()) < int.Parse(attributes[AttO].ToString()))
                                            {
                                                count++;
                                                if (vuln.test.Count == count)
                                                {
                                                    Console.WriteLine(vuln.code + ":" + vuln.title + "-->" + String.Format("{0}.{1}", vuln.codename, vuln.test[vulnO].ToString()));
                                                    vuln.found = true;
                                                }
                                            }
                                        }
                                        break;
                                    }
                                case Operator.Lower:
                                    {
                                        if (vulnO.ToString() == AttO.ToString())
                                        {
                                            vuln.tested = true;
                                            if (int.Parse(vuln.test[vulnO].ToString()) >= int.Parse(attributes[AttO].ToString()))
                                            {
                                                count++;
                                                if (vuln.test.Count == count)
                                                {
                                                    Console.WriteLine(vuln.code + ":" + vuln.title + "-->" + String.Format("{0}.{1}", vuln.codename, vuln.test[vulnO].ToString()));
                                                    vuln.found = true;
                                                }
                                            }
                                        }
                                        break;
                                    }
                            }
                        }
                    }
                    //break;
                }
            }



        }
        public void CheckDefaults()
        {
            Console.WriteLine("Vulnerabilities with not specified(default) options are coming");
            foreach (Vulnerability v1 in vulns)
            {
                if (v1==null)
                    break;
                if (!v1.def || !v1.pr.isPassed||v1.tested)
                    continue;
                v1.found = true;
                Console.WriteLine(v1.code + ":" + v1.title + "-->" + String.Format("{0}", v1.codename));
            }
        }
        public void PrintVulnerabilities()
        {
            Console.WriteLine("allofthemcoming");
     
            Console.WriteLine("---------------------");
            foreach (Vulnerability v1 in vulns)
            {
                if (v1 == null)
                    break;
                if (v1.found)
                    Console.WriteLine(v1.code + ":" + v1.title + "-->" + String.Format("{0}", v1.codename));
            }

        }
        public void PrintVulnerabilityBase()
        {
            foreach (Vulnerability v1 in vulns)
            {
                if (v1 == null)
                    break;
                Console.WriteLine(v1.code + ":" + v1.title + "-->" + String.Format("{0}", v1.codename));
            }
        }
        public void StartScan(string filePath)
        {
            XmlTextReader reader = new XmlTextReader(filePath);
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        Hashtable attributes = new Hashtable();
                        string strURI = reader.NamespaceURI;
                        string strName = reader.Name;

                        if (reader.HasAttributes)
                        {
                            for (int i = 0; i < reader.AttributeCount; i++)
                            {
                                reader.MoveToAttribute(i);
                                attributes.Add(reader.Name, reader.Value);
                            }
                        }
                        AnalyzeNode(strName, attributes);
                        break;


                    default:
                        break;
                }
            }
            //checking default settings
            CheckDefaults();

        }
    }
}
