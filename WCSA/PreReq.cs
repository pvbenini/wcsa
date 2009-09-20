namespace WCSA
{
    using System;

    public class PreReq
    {
        public bool isPassed = true;
        public string property;
        public string tag;
        public string value;

        public PreReq(string tag_, string property_, string value_)
        {
            this.tag = tag_;
            this.property = property_;
            this.value = value_;
        }
    }
}

