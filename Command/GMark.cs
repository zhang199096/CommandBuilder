using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandBuilder.Command
{
    //运行标记,用于标记运行模块
    public class GMark : ICmd
    {
        private string name = "";
        //private string fullname = "";
        public GMark(string cmdname)
        {
            name = cmdname;
        }
        [DebuggerStepThrough]
        public override string ToString()
        {
            return name;
        }
    }
}
