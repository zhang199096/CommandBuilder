using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandBuilder
{
    /// <summary>
    /// 命令解析器,  2017.4.23未用到
    /// </summary>
    class CommandParser
    {
        /// <summary>
        /// 处理多条命令,根据;来分割不同的命令
        /// 解析字符串,可能来自文件或者数据库,
        /// </summary>
        /// <param name="cmdstring"></param>
        public void ParserString(string cmdstring)
        {
            var cmds = cmdstring.Split(';');
            int count = 0;
            foreach (var cmd in cmds)
            {
                if (cmd.Trim() == "")
                    continue;
                count++;
                //Console.WriteLine(count); 
                var cmdstr = cmd.Trim();//单条命令
                Console.WriteLine(cmdstr);
                ParserCmd(cmdstr);
            }
        }

        //处理单个命令
        public void ParserCmd(string cmd)
        {
            string[] separatingChars = { " ", "   ", "\n" };
            var command = cmd;
            var blocks = command.Split(separatingChars, System.StringSplitOptions.RemoveEmptyEntries);

            //第一块就是命令名
            var cmdname = blocks[0];
            Console.WriteLine("cmdname:{0}", cmdname);
            //后面的是参数列表
            // List<string> paramlist = new List<string>();

            for (int i = 1; i < blocks.Length; i++)
            {
                // dt.Rows.Add(drs[i]);
                // ArgsHandle(blocks[i], list);
                Console.WriteLine("[{0}]:{1}", i, blocks[i]);
                ParseParam(blocks[i]);

            }

        }

        //处理单个参数
        private void ParseParam(string param)
        {
            int aindex = 0, eindex = 0, findex = 0;
            aindex = param.IndexOf('A');
            eindex = param.IndexOf('E');
            findex = param.IndexOf('F');
            string astr = "", estr = "", fstr = "";
            if (aindex > -1 && eindex > -1)
                astr = param.Substring(aindex + 1, eindex - aindex - 1);
            if (eindex > -1 && findex > -1)
                estr = param.Substring(eindex + 1, findex - eindex - 1);
            if (findex > -1)
                fstr = param.Substring(findex + 1);
            Console.WriteLine("a:{0},e:{1},f:{2}", astr, estr, fstr);
            //txta.Text = astr;
            //txte.Text = estr;
            //txtf.Text = fstr;
        }
    }
}
