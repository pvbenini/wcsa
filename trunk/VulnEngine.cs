namespace ASPAuditor
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Xml;

    internal class VulnEngine
    {
        private static int i = 0;
        private static Vulnerability[] vulns;

        public static Vulnerability CreateVuln(string code, Operator op, Hashtable test, string title, string codename, PreReq pr, bool def, string description)
        {
            return new Vulnerability { code = code, op = op, title = title, codename = codename, test = new Hashtable(test), pr = pr, def = def, desc = description };
        }

        public static Vulnerability[] LoadVulns()
        {
            vulns = new Vulnerability[400];
            foreach (string str in Directory.GetFiles("VulnFiles"))
            {
                VulnParser(str);
            }
            return vulns;
        }

        public static void VulnParser(string filePath)
        {
            Hashtable test = new Hashtable();
            string codename = "";
            string key = "";
            string str3 = "";
            string str4 = "";
            string title = "";
            string code = "";
            string description = "";
            string str9 = "";
            string str10 = "";
            string str11 = "";
            Operator equal = Operator.Equal;
            PreReq pr = new PreReq("", "", "");
            bool def = false;
            XmlTextReader reader = new XmlTextReader(filePath);
            while (reader.Read())
            {
                if ((reader.NodeType != XmlNodeType.EndElement) || !(reader.Name == "Vulnerability"))
                {
                    goto Label_0163;
                }
                code = string.Format("AA-{0:d2}", i);
                test.Add(key, str3);
                pr = new PreReq(str9, str10, str11);
                if (str9 == "")
                {
                    pr.isPassed = true;
                }
                else
                {
                    pr.isPassed = false;
                }
                switch (equal)
                {
                    case Operator.Equal:
                        if (str4.ToString() == str3.ToString())
                        {
                            def = true;
                        }
                        else
                        {
                            def = false;
                        }
                        break;

                    case Operator.NotEqual:
                        if (str4.ToString() != str3.ToString())
                        {
                            def = true;
                        }
                        else
                        {
                            def = false;
                        }
                        goto Label_0132;
                }
            Label_0132:
                vulns[i++] = CreateVuln(code, equal, test, title, codename, pr, def, description);
                test.Clear();
                continue;
            Label_0163:
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "Tag":
                        {
                            codename = reader.ReadString();
                            continue;
                        }
                        case "Property":
                        {
                            key = reader.ReadString();
                            continue;
                        }
                        case "Value":
                        {
                            str3 = reader.ReadString();
                            continue;
                        }
                        case "DefaultValue":
                        {
                            str4 = reader.ReadString();
                            continue;
                        }
                        case "Operator":
                        {
                            switch (reader.ReadString())
                            {
                                case "Equal":
                                    goto Label_02DD;

                                case "NotEqual":
                                    goto Label_02E2;

                                case "Greater":
                                    goto Label_02E7;

                                case "Lower":
                                    goto Label_02EC;
                            }
                            continue;
                        }
                        case "Title":
                        {
                            title = reader.ReadString();
                            continue;
                        }
                        case "Description":
                        {
                            description = reader.ReadString();
                            continue;
                        }
                        case "PreqTag":
                        {
                            str9 = reader.ReadString();
                            continue;
                        }
                        case "PreqProperty":
                        {
                            str10 = reader.ReadString();
                            continue;
                        }
                        case "PreqValue":
                        {
                            str11 = reader.ReadString();
                            continue;
                        }
                    }
                }
                continue;
            Label_02DD:
                equal = Operator.Equal;
                continue;
            Label_02E2:
                equal = Operator.NotEqual;
                continue;
            Label_02E7:
                equal = Operator.Greater;
                continue;
            Label_02EC:
                equal = Operator.Lower;
            }
        }
    }
}

