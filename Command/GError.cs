using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandBuilder.Command
{
    //无法解析的命令
    public class GError:ICmd
    {
        private string name = "GError 解析错误 ";
        public string paramList { get; set; }

        public override string ToString()
        {
            return name + paramList;
        }
    }
}
