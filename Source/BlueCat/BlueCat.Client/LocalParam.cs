using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueCat.Client
{
    public class LocalParam
    {
        public string DbServerIP { get; set; }

        public string DbServerPot { get; set;}

        public string DbServerUser { get; set; }

        public string DbServerPss { get; set; }

        public string DbName { get; set; }

        public string LastOperateNo { get; set; }

        public int LastClientConfigType { get; set; }

        public string LastSysVersionNo { get; set; }

        public string NextSysVersionNo { get; set;}

        public string ConfigPath { get; set;}
    }
}
