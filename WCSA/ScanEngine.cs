namespace WCSA
{
    using System;
    using System.Collections;
    using System.Xml;

    public class ScanEngine
    {
        public Vulnerability[] vulns = VulnEngine.LoadVulns();

        public void AnalyzeNode(string xtrName, Hashtable attributes)
        {
            foreach (Vulnerability vulnerability in this.vulns)
            {
                if (vulnerability != null)
                {
                    if ((vulnerability.pr.tag == xtrName) && (attributes[vulnerability.pr.property].ToString() == vulnerability.pr.value))
                    {
                        vulnerability.pr.isPassed = true;
                    }
                    if (vulnerability.pr.isPassed && (xtrName == vulnerability.codename))
                    {
                        int num = 0;
                        foreach (object obj2 in vulnerability.test.Keys)
                        {
                            foreach (object obj3 in attributes.Keys)
                            {
                                switch (vulnerability.op)
                                {
                                    case Operator.Equal:
                                    {
                                        if (obj2.ToString() == obj3.ToString())
                                        {
                                            vulnerability.tested = true;
                                            if (vulnerability.test[obj2].ToString() == attributes[obj3].ToString())
                                            {
                                                num++;
                                                if (vulnerability.test.Count == num)
                                                {
                                                    Console.WriteLine(vulnerability.code + ":" + vulnerability.title + "-->" + string.Format("{0}.{1}", vulnerability.codename, vulnerability.test[obj2].ToString()));
                                                    vulnerability.found = true;
                                                }
                                            }
                                        }
                                        continue;
                                    }
                                    case Operator.Greater:
                                    {
                                        if (obj2.ToString() == obj3.ToString())
                                        {
                                            vulnerability.tested = true;
                                            if (int.Parse(vulnerability.test[obj2].ToString()) < int.Parse(attributes[obj3].ToString()))
                                            {
                                                num++;
                                                if (vulnerability.test.Count == num)
                                                {
                                                    Console.WriteLine(vulnerability.code + ":" + vulnerability.title + "-->" + string.Format("{0}.{1}", vulnerability.codename, vulnerability.test[obj2].ToString()));
                                                    vulnerability.found = true;
                                                }
                                            }
                                        }
                                        continue;
                                    }
                                    case Operator.Lower:
                                    {
                                        if (obj2.ToString() == obj3.ToString())
                                        {
                                            vulnerability.tested = true;
                                            if (int.Parse(vulnerability.test[obj2].ToString()) >= int.Parse(attributes[obj3].ToString()))
                                            {
                                                num++;
                                                if (vulnerability.test.Count == num)
                                                {
                                                    Console.WriteLine(vulnerability.code + ":" + vulnerability.title + "-->" + string.Format("{0}.{1}", vulnerability.codename, vulnerability.test[obj2].ToString()));
                                                    vulnerability.found = true;
                                                }
                                            }
                                        }
                                        continue;
                                    }
                                    case Operator.NotEqual:
                                    {
                                        if (obj2.ToString() == obj3.ToString())
                                        {
                                            vulnerability.tested = true;
                                            if (vulnerability.test[obj2].ToString() != attributes[obj3].ToString())
                                            {
                                                num++;
                                                if (vulnerability.test.Count == num)
                                                {
                                                    Console.WriteLine(vulnerability.code + ":" + vulnerability.title + "-->" + string.Format("{0}.{1}", vulnerability.codename, vulnerability.test[obj2].ToString()));
                                                    vulnerability.found = true;
                                                }
                                            }
                                        }
                                        continue;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void CheckDefaults()
        {
            Console.WriteLine("Vulnerabilities with not specified(default) options are coming");
            foreach (Vulnerability vulnerability in this.vulns)
            {
                if (vulnerability == null)
                {
                    return;
                }
                if ((vulnerability.def && vulnerability.pr.isPassed) && !vulnerability.tested)
                {
                    vulnerability.found = true;
                    Console.WriteLine(vulnerability.code + ":" + vulnerability.title + "-->" + string.Format("{0}", vulnerability.codename));
                }
            }
        }

        public void PrintVulnerabilities()
        {
            Console.WriteLine("allofthemcoming");
            Console.WriteLine("---------------------");
            foreach (Vulnerability vulnerability in this.vulns)
            {
                if (vulnerability == null)
                {
                    return;
                }
                if (vulnerability.found)
                {
                    Console.WriteLine(vulnerability.code + ":" + vulnerability.title + "-->" + string.Format("{0}", vulnerability.codename));
                }
            }
        }

        public void PrintVulnerabilityBase()
        {
            foreach (Vulnerability vulnerability in this.vulns)
            {
                if (vulnerability == null)
                {
                    return;
                }
                Console.WriteLine(vulnerability.code + ":" + vulnerability.title + "-->" + string.Format("{0}", vulnerability.codename));
            }
        }

        public void StartScan(string filePath)
        {
            XmlTextReader reader = new XmlTextReader(filePath);
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    Hashtable attributes = new Hashtable();
                    string namespaceURI = reader.NamespaceURI;
                    string name = reader.Name;
                    if (reader.HasAttributes)
                    {
                        for (int i = 0; i < reader.AttributeCount; i++)
                        {
                            reader.MoveToAttribute(i);
                            attributes.Add(reader.Name, reader.Value);
                        }
                    }
                    this.AnalyzeNode(name, attributes);
                }
            }
            this.CheckDefaults();
        }
    }
}

