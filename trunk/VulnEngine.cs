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
    class VulnEngine
    {
        public static void VulnParser(string filePath)
        {

            Hashtable test = new Hashtable();
            
            string tag = "";
            string property = "";
            string value = "";
            string defvalue = "";
            string title = "";
            string temp = "";
            string code = "";

            string preqTag = "";
            string preqProperty = "";
            string preqValue = "";

            Operator op = new Operator();
            PreReq pr = new PreReq("", "", "");
            bool def = false;

            XmlTextReader reader = new XmlTextReader(filePath);
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.EndElement)
                {
                    if (reader.Name == "Vulnerability")
                    {
                        code = String.Format("AA-{0:d2}", i);
                        test.Add(property, value);
                        pr = new PreReq(preqTag, preqProperty, preqValue);
                        if (preqTag == "")
                            pr.isPassed = true;
                        else
                            pr.isPassed = false;
                        switch (op)
                        {
                            case Operator.Equal:
                                if (defvalue.ToString() == value.ToString())
                                    def = true;
                                else
                                    def = false;
                                break;

                            case Operator.NotEqual:
                                if (defvalue.ToString() != value.ToString())
                                    def = true;
                                else
                                    def = false;
                                break;
                        }
                   
                        vulns[i++] = CreateVuln(code, op, test, title, tag, pr,def);
                        test.Clear();
                        continue;
                    }
                }
                if (reader.NodeType != XmlNodeType.Element)
                    continue;

                switch (reader.Name)
                {
                    case "Tag":
                        tag = reader.ReadString();
                        break;
                    case "Property":
                        property = reader.ReadString();
                        break;
                    case "Value":
                        value = reader.ReadString();
                        break;
                    case "DefaultValue":
                        defvalue = reader.ReadString();
                        break;
                    case "Operator":
                        temp = reader.ReadString();
                        switch (temp)
                        {
                            case "Equal":
                                op = Operator.Equal;
                                break;
                            case "NotEqual":
                                op = Operator.NotEqual;
                                break;
                            case "Greater":
                                op = Operator.Greater;
                                break;
                            case "Lower":
                                op = Operator.Lower;
                                break;
                        }
                        break;
                    case "Title":
                        title = reader.ReadString();
                        break;

                    //parsing prerequisit
                    case "PreqTag":
                        preqTag = reader.ReadString();
                        break;
                    case "PreqProperty":
                        preqProperty = reader.ReadString();
                        break;
                    case "PreqValue":
                        preqValue = reader.ReadString();
                        break;

                }


            }
        }
        public static Vulnerability[] LoadVulns()
        {
            vulns = new Vulnerability[400];
            string[] filePaths = Directory.GetFiles(@"../../VulnFiles");
            foreach (string filePath in filePaths)
                VulnParser(filePath);
            return vulns;

        }

        public static Vulnerability CreateVuln(string code, Operator op, Hashtable test, string title, string codename, PreReq pr, bool def)
        {
            Vulnerability myVuln = new Vulnerability();

            myVuln.code = code;
            myVuln.op = op;
            myVuln.title = title;
            myVuln.codename = codename;
 
            myVuln.test = new Hashtable(test);
            myVuln.pr = pr;
            myVuln.def = def;
            return myVuln;
        }
        private static int i = 0;
        private static Vulnerability[] vulns;
    }
}
