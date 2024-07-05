using CommandBuilder.Command;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CommandBuilder
{
    //任务,由一个或多个脚本构成
    public class Mission
    {
        //脚本列表
        private List<string> MissionFileList = new List<string>();
        private Marco marcoHelper;//宏命令处理
        private Dictionary<string, string> marcoMap;//宏命令处理
        private Dictionary<string, string> RunParameters;//运行参数

        //初始化任务 files=按顺序传入要执行的脚本
        public void InitMission(List<string> files, Dictionary<string, string> MarcoMap, Dictionary<string, string> RunParameters)
        {
            foreach (string file in files)
            {
                MissionFileList.Add(file);
            }
            marcoHelper = new Marco(MarcoMap);//初始化,或者提高到更上一层的类里面定义
            this.marcoMap = MarcoMap; //宏命令参数
            this.RunParameters = RunParameters;//运行参数
        }
        //显示这个任务有多少个脚本,用于测试检查脚本列表是否正确加载
        public string ShowScriptFile()
        {
            StringBuilder sb = new StringBuilder();
            int index = 0;
            foreach (string file in MissionFileList)
            {
                index++;
                sb.Append(index.ToString() + "." + file);
            }
            return sb.ToString();
        }

        //执行测试命令解析
        //public void TryParse()
        //{
        //    (string Error, List<ScriptItem> CommandNodes) = Parsing();//解析结果
        //    if (Error != "")//如果解析错误,就返回
        //    {
        //        Console.WriteLine(Error);
        //        return;
        //    }
        //    //foreach (var item in CommandNodes)//测试输出,查看结果是否正确
        //    //{
        //    //    foreach (var cmd in item.CommandList)
        //    //    {
        //    //        Console.WriteLine(cmd.ToString());
        //    //    }
        //    //}
        //}


        public ParseResult Parsing()
        {
            //List<string> MissionFileList
            //1.检查所有的脚本文件是否存在,如果有一个丢失就不执行解析
            var error = CheckFile(MissionFileList);
            if (error != "")
                return new ParseResult { Error = error, CommandNodes = null };//(error,null);

            //cmdNodes是数组(列表),每个脚本解析一个命令列表CommandNode
            List<ScriptItem> cmdNodes = new List<ScriptItem>();

            bool isdebug = true;//是否输出debug信息,每个流程脚本翻译的结果
            foreach (var fileName in MissionFileList)
            {
                //1.获取脚本原始命令, 含宏变量,循环标识
                Flow1Script2List flow1 = new Flow1Script2List();
                var cmdlist = flow1.ToCommand(fileName);
                //debugShowCmdList(cmdlist, isdebug);

                //2.展开循环
                Flow2LoopExpand loop = new Flow2LoopExpand();
                var cmdlist2 = loop.Expand(cmdlist, marcoMap, RunParameters);//循环次数存放可能在宏变量也可能在运行参数里面
                //debugShowCmdList(cmdlist2, isdebug);

                //3.宏变量替换
                Flow3MarcoReplace macro = new Flow3MarcoReplace(marcoHelper);
                var cmdlist3 = macro.Replace(cmdlist2);
                //debugShowCmdList(cmdlist3, isdebug);

                //4.翻译命令
                Flow4CommandParser parser = new Flow4CommandParser();
                var cmdlist4 = parser.Parse(cmdlist3);
                debugShowCmdList(cmdlist4, isdebug);

                //5.添加到结果集合 Scriptfilename=文件名,CommandList=命令列表
                cmdNodes.Add(new ScriptItem() { Scriptfilename = fileName, CommandList = cmdlist4 });
            }
            return new ParseResult { Error = "", CommandNodes = cmdNodes };//("", cmdNodes);
        }

        //用于debug,输出未配置的变量名
        public ParseResult TryParsing(Dictionary<string, bool> mapMacroUnset)
        {
            //List<string> MissionFileList
            //1.检查所有的脚本文件是否存在,如果有一个丢失就不执行解析
            var error = CheckFile(MissionFileList);
            if (error != "")
                return new ParseResult { Error = error, CommandNodes = null };//(error,null);

            //cmdNodes是数组(列表),每个脚本解析一个命令列表CommandNode
            List<ScriptItem> cmdNodes = new List<ScriptItem>();

            bool isdebug = true;//是否输出debug信息,每个流程脚本翻译的结果
            foreach (var fileName in MissionFileList)
            {
                //1.获取脚本原始命令, 含宏变量,循环标识
                Flow1Script2List flow1 = new Flow1Script2List();
                var cmdlist = flow1.ToCommand(fileName);
                //debugShowCmdList(cmdlist, isdebug);
                //2.展开循环
                //FlowDualLoopExpand loop2 = new FlowDualLoopExpand();
                //var cmdlist2 = loop2.Expand(cmdlist, marcoMap, RunParameters);//循环次数存放可能在宏变量也可能在运行参数里面
                //2.展开循环
                Flow2LoopExpand loop = new Flow2LoopExpand();
                var cmdlist2 = loop.Expand(cmdlist, marcoMap, RunParameters);//循环次数存放可能在宏变量也可能在运行参数里面
                //debugShowCmdList(cmdlist2, isdebug);

                //3.宏变量替换 fileName
                Flow3MarcoReplace macro = new Flow3MarcoReplace(marcoHelper);
                var cmdlist3 = macro.TryReplace(cmdlist2, mapMacroUnset, System.IO.Path.GetFileName(fileName));
                debugShowCmdList(cmdlist3, isdebug);

                //4.翻译命令
                //Flow4CommandParser parser = new Flow4CommandParser();
                //var cmdlist4 = parser.Parse(cmdlist3);
                //debugShowCmdList(cmdlist4, isdebug);

                ////5.添加到结果集合 Scriptfilename=文件名,CommandList=命令列表
                //cmdNodes.Add(new ScriptItem() { Scriptfilename = fileName, CommandList = cmdlist4 });
            }
            return new ParseResult { Error = "", CommandNodes = cmdNodes };//("", cmdNodes);
        }

        #region 显示调试结果
        private void debugShowCmdList(List<CommandItem> cmdlist, bool isdebug)
        {
            if (!isdebug) return;
            int debugIndex = 0;
            //test output
            foreach (var item in cmdlist)
            {
                debugIndex++;
                Console.WriteLine(debugIndex.ToString() + " " + item.Cmd + " " + item.Param);
            }
            Enumerable.Range(1, 110).Any(number => { Console.Write("-"); if (number == 110) Console.Write("\n"); return false; });
        }
        private void debugShowCmdList(List<ICmd> cmdlist4, bool isdebug)
        {
            if (!isdebug) return;
            int debugIndex = 0;
            foreach (var item in cmdlist4)
            {
                debugIndex++;
                Console.WriteLine(debugIndex.ToString() + " " + item.ToString());
            }
            Enumerable.Range(1, 110).Any(number => { Console.Write("-"); if (number == 110) Console.Write("\n"); return false; });
        }
        #endregion

        //检查任务中的脚本是否存在
        private string CheckFile(List<string> missionFileList)
        {
            foreach (var fileName in MissionFileList)
            {
                if (!File.Exists(fileName))
                {
                    return fileName + " 不存在。";//error!=""表示失败
                }
            }
            return "";//error=""表示成功,没有错误
        }
    }

    public class ParseResult
    {
        public string Error { get; set; }
        public List<ScriptItem> CommandNodes { get; set; }
    }
    //循环信息
    public class LoopItem
    {
        public string StartName { get; set; }
        public string TimesString { get; set; }//循环次数,变量
        //public List<CommandItem> Beg_CmdStringList { get; set; }
        public List<CommandItem> CmdStringList { get; set; }
    }

    //4.翻译命令
    public class Flow4CommandParser
    {
        public List<ICmd> Parse(List<CommandItem> cmdlist3)
        {
            //cmdlist当前脚本解析出来的命令列表
            List<ICmd> cmdlist = new List<ICmd>();
            Parser parser = new Parser();//解析工具
            //解析命令
            foreach (var line in cmdlist3)
            {
                var cmd = parser.Parse(line);
                if (cmd != null)
                {
                    cmdlist.Add(cmd);
                    if (cmd is GError)//如果发现无法解析的命令,就停止解析后面的命令
                    {
                        break;
                    }
                }
            }
            return cmdlist;
        }
    }

    //3.宏变量替换
    public class Flow3MarcoReplace
    {
        private Marco macro;
        public Flow3MarcoReplace(Marco marcoHelper)
        {
            this.macro = marcoHelper;
        }

        public List<CommandItem> Replace(List<CommandItem> cmdlist2)
        {
            List<CommandItem> cmdlist3 = new List<CommandItem>();
            int debugIndex = 0;//调试第X条命令
            foreach (var item in cmdlist2)
            {
                debugIndex++;
                if (debugIndex == 4)
                    debugIndex = 4;
                if (item.Param.Length > 0)
                    item.Param = macro.MarcoParse(item.Param);
                cmdlist3.Add(item);
            }
            return cmdlist3;
        }
        //用于debug,输出未设置变量名
        public List<CommandItem> TryReplace(List<CommandItem> cmdlist2, Dictionary<string, bool> mapMacroUnset, string scriptfile)
        {
            List<CommandItem> cmdlist3 = new List<CommandItem>();
            foreach (var item in cmdlist2)
            {
                if (item.Param.Length > 0)
                    item.Param = macro.TryMarcoParse(item.Param, mapMacroUnset, scriptfile);
                cmdlist3.Add(item);
            }
            return cmdlist3;
        }
    }
    //2.展开循环
    public class Flow2LoopExpand
    {
        public List<CommandItem> Expand(List<CommandItem> original, Dictionary<string, string> MarcoMap, Dictionary<string, string> RunParameters)
        {
            var result = new List<CommandItem>();//脚本总列表
            Stack<LoopItem> loopItems = new Stack<LoopItem>();//循环信息
            List<CommandItem> entry;//命令存放入口

            entry = result;//默认写入总集合
            foreach (var item in original)
            {
                if (item.Cmd.StartsWith("#CYCLE_"))//对于#CYCLE_NO_NOW# 默认只有++, 无视自增变量
                    continue;

                if (item.Cmd.StartsWith("G"))//如果是命令,就放入集合里面
                {
                    entry.Add(item);
                }
                else if (item.Cmd.StartsWith("#START"))//如果是循环开始标记,记下循环名称,循环次数,把接下来的命令放到此集合里面
                {
                    ScriptResult val = CommandSpliter.split(item.Cmd);
                    string startName = val.Cmd; string loopTimes = val.Param;
                    LoopItem item1 = new LoopItem();
                    item1.StartName = startName;
                    item1.TimesString = loopTimes;
                    item1.CmdStringList = new List<CommandItem>();
                    loopItems.Push(item1);
                    entry = item1.CmdStringList;//修改命令存放
                }
                else if (item.Cmd.StartsWith("#END"))//如果是循环结束标记
                {
                    //因为 #END跟#START是成对出现的, 所以不用判断是哪个#START了, 除非代码的#END写错了,这里就不判断了
                    //var t = item.Cmd.Substring(4);
                    //var now = "#CYCLE_NO_NOW#".Replace("_NO_", "_" + t + "_");
                    var nowTest = "#CYCLE_NO_NOW#";//要确定NO是否是具体的数字

                    var top = loopItems.Pop();
                    var CYCLE_NO_T = top.TimesString;//.Replace("_NO_", "_" + t + "_");
                    int runtimes = 0;//循环次数,可能来源配置文件或者运行参数
                    if (MarcoMap.ContainsKey(CYCLE_NO_T))
                        runtimes = int.Parse(MarcoMap[CYCLE_NO_T]);
                    if (RunParameters.ContainsKey(CYCLE_NO_T))
                        runtimes = int.Parse(RunParameters[CYCLE_NO_T]);

                    if (loopItems.Count > 0)//如果还有上一层循环,命令就放到上一层,否则就放到result集合
                    {
                        LoopItem parent = loopItems.Peek();
                        entry = parent.CmdStringList;
                    }
                    else
                    {
                        entry = result;
                    }

                    for (int i = 0; i < runtimes; i++)
                    {
                        //entry.Add(new CommandItem() { Cmd = "@Loop" + (i + 1).ToString(), Param = "" });//标记

                        foreach (var subitem in top.CmdStringList)
                        {
                            var clone = (CommandItem)subitem.Clone();
                            //int pos = clone.Param.IndexOf(now);
                            //if (pos > -1)
                            //{
                            //    clone.Param = clone.Param.Replace(now, i.ToString());
                            //}
                            clone.Param = clone.Param.Replace(nowTest, i.ToString());//\\测试,实际上NO是否为数字 
                            entry.Add(clone);//放到上一级的集合里面
                        }
                    }
                }
            }
            return result;
        }
    }

    public class FlowDualLoopExpand
    {
        public List<CommandItem> Expand(List<CommandItem> original, Dictionary<string, string> MarcoMap, Dictionary<string, string> RunParameters)
        {
            var result = new List<CommandItem>();//脚本总列表
            Stack<LoopItem> loopItems = new Stack<LoopItem>();//循环信息
            List<CommandItem> entry;//命令存放入口
            List<CommandItem>[][] d_entrys = new List<CommandItem>[4][];//现阶段暂时只开放最大四层循环
            //d_entrys[x][y][z];  x层次  y:0循环前  1循环后   z 循环中G代码行号
            d_entrys[0] = new List<CommandItem>[2];
            d_entrys[1] = new List<CommandItem>[2];
            d_entrys[2] = new List<CommandItem>[2];
            d_entrys[3] = new List<CommandItem>[2];
            int[] levels = new int[loopItems.Count];//内部循环次数
            string[] cmds = new string[loopItems.Count];
            int loop = 0; //记录当前循环层数
            bool isGcodeEdit = false; //判定是否为G代码写入区间
            entry = result;//默认写入总集合
            foreach (var item in original)
            {
                if (item.Cmd.StartsWith("#CYCLE_"))//对于#CYCLE_NO_NOW# 默认只有++, 无视自增变量
                {
                    if (isGcodeEdit)
                    {
                        isGcodeEdit = false;
                        d_entrys[loop - 1][1] = entry;
                    }
                    entry = new List<CommandItem>();//假如有多重循环  内循环下方循环    
                    continue;
                }
                if (item.Cmd.StartsWith("G"))//如果是命令,就放入集合里面
                {
                    if (loop > 0)
                    {
                        isGcodeEdit = true;
                    }
                    entry.Add(item);
                }
                else if (item.Cmd.StartsWith("#START"))//如果是循环开始标记,记下循环名称,循环次数,把接下来的命令放到此集合里面
                {
                    if (isGcodeEdit)
                    {
                        isGcodeEdit = false;
                        d_entrys[loop - 1][0] = entry;
                    }
                    loop++;
                    ScriptResult val = CommandSpliter.split(item.Cmd);
                    string startName = val.Cmd; string loopTimes = val.Param;
                    LoopItem item1 = new LoopItem();
                    item1.StartName = startName;
                    item1.TimesString = loopTimes;
                    item1.CmdStringList = new List<CommandItem>();
                    loopItems.Push(item1);
                    entry = item1.CmdStringList;//修改命令存放
                }
                else if (item.Cmd.StartsWith("#END"))//如果是循环结束标记
                {
                    //TODO  支持多重循环
                    if (loop == loopItems.Count && levels.Length == 0)//当初次进入END项时
                    {
                        levels = new int[loopItems.Count];
                        cmds = new string[loopItems.Count];
                    }
                    loop--;
                    if (loop > 0)//不是最里层
                    {
                        var t = item.Cmd.Substring(4);
                        var now = "#CYCLE_NO_NOW#".Replace("_NO_", "_" + t + "_");
                        var top = loopItems.Pop();
                        var CYCLE_NO_T = top.TimesString.Replace("_NO_", "_" + t + "_");
                        int runtimes = 0;//循环次数,可能来源配置文件或者运行参数
                        if (MarcoMap.ContainsKey(CYCLE_NO_T))
                            runtimes = int.Parse(MarcoMap[CYCLE_NO_T]);
                        if (RunParameters.ContainsKey(CYCLE_NO_T))
                            runtimes = int.Parse(RunParameters[CYCLE_NO_T]);
                        cmds[loopItems.Count] = now;
                        levels[loopItems.Count] = runtimes;
                        entry = new List<CommandItem>();//假如有多重循环  内循环下方循环
                        continue;
                    }
                    else //最里层
                    {
                        //因为 #END跟#START是成对出现的, 所以不用判断是哪个#START了, 除非代码的#END写错了,这里就不判断了
                        var t = item.Cmd.Substring(4);
                        var now = "#CYCLE_NO_NOW#".Replace("_NO_", "_" + t + "_");
                        var top = loopItems.Pop();
                        var CYCLE_NO_T = top.TimesString.Replace("_NO_", "_" + t + "_");
                        int runtimes = 0;//循环次数,可能来源配置文件或者运行参数
                        if (MarcoMap.ContainsKey(CYCLE_NO_T))
                            runtimes = int.Parse(MarcoMap[CYCLE_NO_T]);
                        if (RunParameters.ContainsKey(CYCLE_NO_T))
                            runtimes = int.Parse(RunParameters[CYCLE_NO_T]);
                        cmds[loopItems.Count] = now;
                        levels[loopItems.Count] = runtimes;
                        ExpandDual(result, d_entrys, levels, cmds, 0, new int[levels.Length]);
                        //切回初始条件
                        levels = new int[loopItems.Count];//内部循环次数
                        cmds = new string[loopItems.Count];//内部循环次数
                        d_entrys[0] = new List<CommandItem>[2];
                        d_entrys[1] = new List<CommandItem>[2];
                        d_entrys[2] = new List<CommandItem>[2];
                        d_entrys[3] = new List<CommandItem>[2];
                    }
                    //entry = new List<CommandItem>();//假如有多重循环  内循环下方循环
                }
            }
            //补齐循环后G代码   
            if (entry.Count > 0 && loop == 0 && entry != result)
                result.AddRange(entry);
            return result;
        }
        public void ExpandDual(List<CommandItem> sum, List<CommandItem>[][] samples, int[] levels, string[] cmds, int currentLevel, int[] indices)
        {
            if (currentLevel == levels.Length)
            {
                // Execute the loop body
                string loops = "";
                for (int i = 0; i < levels.Length; i++)
                {
                    loops += indices[i].ToString();
                    //Console.Write(indices[i]);
                    if (i < levels.Length - 1)
                    {
                        loops += indices[i] * levels[i + 1];
                        //Console.Write(", \t");
                    }
                    else
                    {
                        loops += indices[i];
                    }
                }
                //sum.Add(new CommandItem() { Cmd = "@Loop" + loops.ToString(), Param = "" });//标记
                //Console.WriteLine();
                foreach (var subitem in samples[currentLevel-1][1])
                {
                    var clone = (CommandItem)subitem.Clone();
                    for(int i = 0; i < cmds.Length; i++)
                    {
                        clone.Param = clone.Param.Replace(cmds[i], indices[i].ToString());//\\测试,实际上NO是否为数字 
                    }
                    sum.Add(clone);//放到上一级的集合里面
                }
            }
            else
            {
                for (int i = 0; i < levels[currentLevel]; i++)
                {
                    indices[currentLevel] = i;

                    var sub_cmds = cmds.ToList().GetRange(0, currentLevel + 1).ToArray();
                    var sub_ind = indices.ToList().GetRange(0, currentLevel + 1).ToArray();

                    if (currentLevel< levels.Length - 1 && samples[currentLevel][0] != null)//内部循环前G  code
                    {                      
                        foreach (var subitem in samples[currentLevel][0])
                        {
                            var clone = (CommandItem)subitem.Clone();
                            for (int j = 0; j < sub_cmds.Length; j++)
                            {
                                clone.Param = clone.Param.Replace(sub_cmds[j], sub_ind[j].ToString());//\\测试,实际上NO是否为数字 
                            }
                            sum.Add(clone);//放到上一级的集合里面
                        }
                    }
                    ExpandDual(sum, samples, levels, cmds, currentLevel + 1, indices);
                    if (currentLevel < levels.Length - 1 && samples[currentLevel][1]!=null)//内部循环后G  code
                    {
                        foreach (var subitem in samples[currentLevel][1])
                        {
                            var clone = (CommandItem)subitem.Clone();
                            for (int j = 0; j < sub_cmds.Length; j++)
                            {
                                clone.Param = clone.Param.Replace(sub_cmds[j], sub_ind[j].ToString());//\\测试,实际上NO是否为数字 
                            }
                            sum.Add(clone);//放到上一级的集合里面
                        }
                    }
                }
            }
        }
        #region 测试
        public void TestStack(Stack<LoopItem> loopItems)
        {
            var result = new List<CommandItem>();//脚本总列表
            var sample = new List<CommandItem>();//脚本总列表
            int[] levels =   {4,5,6 };
            string[] cmds = { "#CYCLE_4_NOW#", "#CYCLE_5_NOW#", "#CYCLE_6_NOW#" };
            while (loopItems.Count > 0)
            {
                var top = loopItems.Pop();
                levels[loopItems.Count] = 1;
                cmds[loopItems.Count] = "#CYCLE_NO_NOW#";
                if (loopItems.Count == 0)
                {
                    sample = top.CmdStringList; 
                }
            }
            //ExpandDual(result, sample, levels,cmds, 0, new int[levels.Length]);
        }

        public void TestStack2(List<CommandItem> original)
        {
            Stack<LoopItem> loopItems = new Stack<LoopItem>();
            var loopcmd = new LoopItem();
            loopcmd.StartName = "START1";
            loopcmd.TimesString = "1";
            loopcmd.CmdStringList = original;
            var loopcmd1 = new LoopItem();
            loopcmd.StartName = "START1";
            loopcmd.TimesString = "1";
            loopcmd1.CmdStringList = new List<CommandItem>();
            var loopcmd2 = new LoopItem();
            loopcmd.StartName = "START1";
            loopcmd.TimesString = "1";
            loopcmd2.CmdStringList = new List<CommandItem>();
            loopItems.Push(loopcmd);
            loopItems.Push(loopcmd1);
            loopItems.Push(loopcmd2);
            TestStack(loopItems);
        }
        #endregion
    }
    //1.获取脚本原始命令, 含宏变量,循环标识
    public class Flow1Script2List
    {
        //将一个script文件的内容读取到List里面, 每个命令CommandRow, 由命令Cmd和参数Param组成
        public List<CommandItem> ToCommand(string fileName)
        {
            //lines脚本的每一行文本
            List<CommandItem> lines = new List<CommandItem>();

            string line = "";
            //先把脚本加载到数组.
            StreamReader sr = new StreamReader(fileName);
            while (sr.Peek() > 0)
            {
                line = sr.ReadLine();
                ScriptResult val= cleaning(line);
                string cmd=val.Cmd; string param=val.Param; //清除注释

                if (cmd == "")
                    continue;

                lines.Add(new CommandItem() { Cmd = cmd, Param = param });
            }
            sr.Close();

            return lines;
        }

       
        //清除注释,分割命令和参数. 有效命令两种情况,1是以G开头的命令,2是以#开头的循环标记
        private ScriptResult cleaning(string line)
        {
            //去掉注释
            var index = line.IndexOf("//");
            if (index > 0)
                line = line.Substring(0, index);

            line = line.Trim();
            if (line.Length == 0)
                return new ScriptResult{Cmd="",Param=""};//("", "");

            if (line.StartsWith("G"))
            {
                return CommandSpliter.split(line);//分割命令和参数
            }
            else if (line.StartsWith("#"))
            {
                return new ScriptResult{Cmd=line,Param=""};//(line, "");//直接返回循环标记,在下一个流程Flow里面处理
            }
            else//其他情况不处理,不报错.
            {
                 return new ScriptResult{Cmd="",Param=""};//("", "");
            }
        }
    }
    public class ScriptResult
    {
        public string Cmd { get; set; }
        public string Param { get; set; }
    }
    //命令分割,用于返回命令及其参数
    public class CommandSpliter
    {
        public static ScriptResult split(string cmdstring)
        {
            var command = "";//命令
            var param = "";//参数
            int index = cmdstring.IndexOf(" ");
            var index2 = cmdstring.IndexOf("\t");
            if (index != -1 && index2 != -1)
            {
                index = index > index2 ? index2 : index;
            }
            else if (index == -1 && index2 != -1)
            {
                index = index2;
            }
            if (index > 0)
            {
                command = cmdstring.Substring(0, index).Trim();
                param = cmdstring.Substring(index).Trim();
            }
            else
                command = cmdstring;
            return new ScriptResult { Cmd=command,Param= param }; //这里涉及到了参数处理的问题.
        }
    }
}
