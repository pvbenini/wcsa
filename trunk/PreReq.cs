using System;
using System.Collections.Generic;
using System.Text;

namespace ASPAuditor
{
    class PreReq
    {
        public PreReq(string tag_, string property_, string value_)
        {
            tag = tag_;
            property = property_;
            value = value_;
        }
        public string tag;
        public string property;
        public string value;
        public bool isPassed = true;
    }
}
