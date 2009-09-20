namespace WCSA
{
    using System;
    using System.IO;

    public class Reporter
    {
        private string head;
        private string issueTemplate;
        private string tail;

        public Reporter(Vulnerability[] vulns, string path)
        {
            StreamReader reader = new StreamReader("Reporting/head.html");
            string str = @"\//";
            StreamWriter writer = new StreamWriter("Report-" + path.Substring(path.LastIndexOfAny(str.ToCharArray()) + 1) + ".html");
            int num = 0;
            foreach (Vulnerability vulnerability in vulns)
            {
                if ((vulnerability != null) && vulnerability.found)
                {
                    num++;
                }
            }
            this.tail = "</table> </body> </html>";
            this.head = reader.ReadToEnd();
            reader.Close();
            this.head = this.head.Replace("<!--numberofvulns-->", num.ToString());
            this.head = this.head.Replace("<!--path-->", path);
            writer.Write(this.head);
            reader = new StreamReader("Reporting/issue.htm");
            this.issueTemplate = reader.ReadToEnd();
            reader.Close();
            string str2 = this.issueTemplate.ToString();
            num = 0;
            foreach (Vulnerability vulnerability2 in vulns)
            {
                if ((vulnerability2 != null) && vulnerability2.found)
                {
                    num++;
                    string newValue = string.Format("{0}.{1}", num.ToString(), vulnerability2.title);
                    str2 = str2.Replace("<!--reportfieldname-->", newValue).Replace("<!--reportfieldcontent-->", vulnerability2.desc);
                    writer.Write(str2);
                    str2 = this.issueTemplate.ToString();
                }
            }
            writer.Write(this.tail);
            writer.Close();
        }
    }
}

