using System;
using System.Collections.Generic;
using System.Text;

namespace WCSA
{
    public class WCSAException:Exception
    {
        public WCSAException(string error_)
        {
            error = error_;
        }

        private string error;
    }
}
