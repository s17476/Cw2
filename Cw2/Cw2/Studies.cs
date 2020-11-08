using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Cw2
{
    [Serializable()]
    public class Studies
    {
        public string name { get; set; }
        public string mode { get; set; }
    }
}
