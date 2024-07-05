using CommandBuilder.Command;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace CommandBuilder
{
    /// <summary>
    /// 用于解析命令, 1解析文件,2解析单个命令
    /// </summary>
    public class CmdBuilder
    {
        public static int ScriptLength = 0;
        private List<object> cmdlist = new List<object>();//命令数组, 
        public Action<object> ReceiveCmd;//获取到cmd处理委托
        public Queue<CmdFileNode> cmdFiles = new Queue<CmdFileNode>();
        public int RunStatus = 0; //0表示空闲, 1表示繁忙
        private object lockProcess = new object();//锁定是否可以执行解析文件

        public void Run()
        {
            ThreadPool.QueueUserWorkItem(o => ProcessFile());
        }

        //清空等待解析的脚本文件
        public void CleanFileQueue()
        {
            lock (lockProcess)
            {
                RunStatus = 0;//设置为繁忙状态
                cmdFiles.Clear();
            }
        }
        private void ProcessFile()
        {
            while (true)
            {
                if (cmdFiles.Count > 0)
                {
                    var canRun = false;
                    lock (lockProcess)
                    {
                        if (RunStatus == 0)//系统是否空闲
                        {
                            canRun = true;
                        }
                    }

                    if (canRun)//系统空闲
                    {
                        lock (lockProcess)
                        {
                            RunStatus = 1;//设置为繁忙状态
                        }
                        var fileNode = cmdFiles.Dequeue();
                        ParseFile(fileNode.fileName, fileNode.NeedCmdCount, fileNode.SetCmdCount);
                    }
                }
                Thread.Sleep(1000);
            }
        }

        public void AddFile(string fileName)
        {
            CmdFileNode node = new CmdFileNode() { fileName= fileName, NeedCmdCount=false, SetCmdCount=null };
            cmdFiles.Enqueue(node);

        }
        public void AddFile(string pFileName, bool pNeedCmdCount, Action<int> pSetCmdCount)
        {
            CmdFileNode node = new CmdFileNode() { fileName = pFileName, NeedCmdCount = pNeedCmdCount, SetCmdCount = pSetCmdCount };
            cmdFiles.Enqueue(node);

        }
        public List<string> ReProcessFile(string fileName,int filecount)
        {
            cmdlist.Clear();
            List<string> lines = new List<string>();
            List<string> list_SampleNumber = new List<string>();
            string line = "";
            ScriptLength = 0;
            if (!File.Exists(fileName))
            {
                //ReceiveCmd(new GError() { paramList = fileName + " 不存在。" });
                return list_SampleNumber;
            }
            try
            {
                //先把脚本加载到数组.
                StreamReader sr = new StreamReader(fileName);
                while (sr.Peek() > 0)
                {
                    line = sr.ReadLine();
                    lines.Add(line);
                }
                lines = lines.GetRange(lines.Count - 96, 96);
                foreach (var l in lines)
                {
                    var cmd = ParseLine(l);
                    list_SampleNumber.Add(cmd.ToString());
                }
                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return list_SampleNumber;
        }
        //处理命令文件
        private void ParseFile(string fileName, bool NeedCmdCount, Action<int> SetCmdCount)
        {
            cmdlist.Clear();
            List<string> lines = new List<string>();
            string line = "";
            int cmdCount = 0;
            ScriptLength = 0;
            if (!File.Exists(fileName))
            {
                ReceiveCmd(new GError() { paramList = fileName + " 不存在。" });
                return;
            }

            try
            {
                //先把脚本加载到数组.
                StreamReader sr = new StreamReader(fileName);
                while (sr.Peek() > 0)
                {
                    line = sr.ReadLine();
                    lines.Add(line);
                    if (NeedCmdCount)//Tsystem.GetDspMode() == 1)  //如果是工程师调试界面运行解析脚本，则记录一个文件的脚本条数
                    {
                        if (IsCmdLine(line))
                        {
                            cmdCount++;
                        }
                    }
                    //lines.Add( sr.ReadLine());
                }
                if (NeedCmdCount)//Tsystem.GetDspMode()==1)
                {
                    ScriptLength = cmdCount;
                    //Console.WriteLine("ScriptLength = " + ScriptLength.ToString());
                    // Tsystem.SetEngScriptLength(cmdCount);
                    if (SetCmdCount != null)
                        SetCmdCount(cmdCount);
                }
                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            //发送命令到处理队列
            SentToCmdQueue(lines);
        }

        private bool IsCmdLine(string line)
        {
            bool rt = false;
            rt = line.StartsWith("G");
            return rt;
        }

        /// <summary>
        /// 解析命令, 并放到运行队列里面. 全部放到队列之后, 再把本次解析的命令输出一次,用于用户检查命令解析是否正确.
        /// </summary>
        /// <param name="lines"></param>
        private void SentToCmdQueue(List<string> lines)
        {
            ThreadPool.QueueUserWorkItem(o => {
                var time1 = DateTime.Now;
                try { 
                foreach (var line in lines)
                {
                    var cmd = Parse(line);
                    if (cmd != null)
                    {
                        if (ReceiveCmd != null)
                            ReceiveCmd(cmd);
                        if(cmd is GError )//如果发现无法解析的命令,就停止解析后面的命令
                        {
                            break;
                        }
                    }
                }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                // CoordinateSave2TXT(cmdlist);//把命令的坐标输出到TXT文件 2018.4.2

                if (cmdlist.Count < int.MaxValue)// 1000 int.MaxValue)//命令太多会输出影响程序运行. 1000   把所有命令都输出2018.4.2
                {
                    Console.WriteLine("###输出全部命令:");
                    var sendIndex = 1;
                    StringBuilder sb = new StringBuilder();
                    foreach (var cmd in cmdlist)
                    {

                      //  sb.Append(string.Format("第{0}:", sendIndex.ToString()));
                        if (cmd is G00)//这里g00借用G01是有问题的
                            sb.AppendLine(((G00)cmd).ToString());
                        if (cmd is G01)
                            sb.AppendLine(((G01)cmd).ToString());
                        if (cmd is G04)
                            sb.AppendLine(((G04)cmd).ToString());
                        if (cmd is G100)
                            sb.AppendLine(((G100)cmd).ToString());
                        if (cmd is G101)
                            sb.AppendLine(((G101)cmd).ToString());
                        if (cmd is G102)
                            sb.AppendLine(((G102)cmd).ToString());
                        if (cmd is G103)
                            sb.AppendLine(((G103)cmd).ToString());
                        if (cmd is G104)
                            sb.AppendLine(((G104)cmd).ToString());
                        if (cmd is G105)
                            sb.AppendLine(((G105)cmd).ToString());
                        if (cmd is G106)
                            sb.AppendLine(((G106)cmd).ToString());
                        if (cmd is G107)
                            sb.AppendLine(((G107)cmd).ToString());
                        if (cmd is G108)
                            sb.AppendLine(((G108)cmd).ToString());
                        if (cmd is G109)
                            sb.AppendLine(((G109)cmd).ToString());
                        if (cmd is G110)
                            sb.AppendLine(((G110)cmd).ToString());
                        if (cmd is G111)
                            sb.AppendLine(((G111)cmd).ToString());
                        if (cmd is G112)
                            sb.AppendLine(((G112)cmd).ToString());
                        if (cmd is G113)
                            sb.AppendLine(((G113)cmd).ToString());
                        if (cmd is G114)
                            sb.AppendLine(((G114)cmd).ToString());
                        if (cmd is G115)
                            sb.AppendLine(((G115)cmd).ToString());
                        if (cmd is G116)
                            sb.AppendLine(((G116)cmd).ToString());
                        if (cmd is G117)
                            sb.AppendLine(((G117)cmd).ToString());
                        if (cmd is G118)
                            sb.AppendLine(((G118)cmd).ToString());
                        if (cmd is G119)
                            sb.AppendLine(((G119)cmd).ToString());
                        if (cmd is G201)
                            sb.AppendLine(((G201)cmd).ToString());
                        //  Console.WriteLine(tmp);
                        sendIndex++;
                    }

                    try  //把命令的坐标输出到TXT文件 2018.4.2
                    {
                        if (!Directory.Exists("record"))
                            Directory.CreateDirectory("record");
                        var filename = "./record/coordinate" + DateTime.Now.ToString("yy_MM_dd_hh_mm_ss") + ".txt";
                        var fileStream = File.Open(filename, FileMode.Create);
                        StreamWriter sWriter = null;
                        sWriter = new StreamWriter(fileStream);
                        sWriter.WriteLine(sb.ToString());
                        sWriter.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                   // Console.WriteLine(sb.ToString());
                }
                else
                {
                    Console.WriteLine("脚本太长,不输出到控制台了.");
                }
                cmdlist.Clear();//现在纯粹就是为了输出控制台.

                lock (lockProcess)
                {
                    RunStatus = 0;//设置为空闲状态
                }
            });
        }

        //把命令的坐标输出到TXT文件 2018.4.2
        private void CoordinateSave2TXT(List<object> cmdlist)
        {
            ThreadPool.QueueUserWorkItem(o =>
            {
                try
                {
                 


                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("执行于:" + DateTime.Now.ToString());
                    // sb.Append(string.Format("第{0}:", sendIndex.ToString()));
                    foreach (var cmd in cmdlist)
                    {
                        if (cmd is G00)//这里g00借用G01是有问题的
                        {
                            G00 x00 = (G00)cmd;
                            foreach (var gparam in x00.paramList)
                                sb.AppendLine(string.Format("G00 A{0} E:{1}", gparam.AValue, gparam.EValue));
                        }
                        if (cmd is G01)
                        {
                            G01 x01 = (G01)cmd;
                            foreach (var gparam in x01.paramList)
                                sb.AppendLine(string.Format("G01 A{0} E:{1}", gparam.AValue, gparam.EValue));
                        }
                        if (cmd is G105)
                        {
                            var x105 = (G105)cmd;
                            foreach (var gparam in x105.paramList)
                                sb.AppendLine(string.Format("G105 A{0} E:{1}", gparam.AValue, gparam.EValue));
                        }
                        if (cmd is G109)
                        {
                            var x109 = (G109)cmd;
                            foreach (var gparam in x109.paramList)
                                sb.AppendLine(string.Format("G109 A{0} E:{1}", gparam.AValue, gparam.EValue));
                        }
                    }
                    var filename = "coordinate.txt";
                    var fileStream = File.Open(filename, FileMode.Create);
                    StreamWriter sWriter = null;
                    sWriter = new StreamWriter(fileStream);
                    sWriter.WriteLine(sb.ToString());
                    sWriter.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });
        }

        /// <summary>
        /// 处理一行命令, 
        /// 1.一行只能够存放一条命令, 不能用;来把多条命令放在一行.
        /// 2.命令必须以G开头, 否则不处理.
        /// </summary>
        /// <param name="line"></param>
        public object ParseLine(string line)
        {
            string cmd = line;

            var index = line.IndexOf("//");//去掉注释
            if (index > 0)
                cmd = line.Substring(0, index).Trim();

            var sample_code = "";  //样本编码
            //var param = "";
            var sample_name = "";  //样本名
            index = cmd.IndexOf(" ");
            var index2 = cmd.IndexOf("\t");

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
                sample_code = cmd.Substring(0, index).Trim();
                sample_name = cmd.Substring(index).Trim();
                var index3 = cmd.IndexOf("\t", index + 1);
                if ((index3 == (index + 1)) || (index3 < (index + 1)))
                {
                    sample_name = "NaN";
                }
                else
                {
                    sample_name = cmd.Substring(index + 1, index3 - index - 1);
                }

            }
            else
                sample_code = cmd;
            return sample_name;
        }

        //2023测试脚本文件
        public string TranslateTest(string fileName)
        {

            // ParseFile(file, NeedCmdCount, null);
            List<string> lines = new List<string>();

            if (!File.Exists(fileName))
            {
                return fileName + " 不存在。";
            }

            try
            {
                string line = "";
                //先把脚本加载到数组.
                StreamReader sr = new StreamReader(fileName);
                while (sr.Peek() > 0)
                {
                    line = sr.ReadLine();
                    lines.Add(line);
                }
                sr.Close();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            //发送命令到处理队列
            //SentToCmdQueue(lines);

            var time1 = DateTime.Now;
            try
            {
                Console.WriteLine("###输出全部命令:");
                foreach (var line in lines)
                {
                    var cmd = Parse(line);
                    if (cmd != null)
                    {
                        //if (ReceiveCmd != null)
                        //    ReceiveCmd(cmd);
                        if (cmd is GError)//如果发现无法解析的命令,就停止解析后面的命令
                        {
                            break;
                        }
                        Thread.Sleep(300);
                        Console.WriteLine(cmd.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
            // CoordinateSave2TXT(cmdlist);//把命令的坐标输出到TXT文件 2018.4.2


            cmdlist.Clear();//现在纯粹就是为了输出控制台.

            lock (lockProcess)
            {
                RunStatus = 0;//设置为空闲状态
            }
            return "";
        }
        /// <summary>
        /// 处理一行命令, 
        /// 1.一行只能够存放一条命令, 不能用;来把多条命令放在一行.
        /// 2.命令必须以G开头, 否则不处理.
        /// </summary>
        /// <param name="line"></param>
        public ICmd Parse(string line)
        {
            string cmd = line;
            if (!line.StartsWith("G"))//命令必须以G开头
                return null;
            var index = line.IndexOf("//");//去掉注释
            if (index > 0)
                cmd = line.Substring(0, index).Trim();

            var command = "";
            var param = "";
            index = cmd.IndexOf(" ");
            var index2 = cmd.IndexOf("\t");
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
                command = cmd.Substring(0, index).Trim();
                param = cmd.Substring(index).Trim();
            }
            else
                command = cmd;

            try
            {
                ICmd ic = ParseParam(command, param);//此处需要try-catch,然后记录不能解释的line值
                return ic;
            }
            catch (Exception ex)
            {
                GError err = new GError() { paramList = line + Environment.NewLine + ex.Message };
                //要写log
                Console.WriteLine("无法解析:" + line + Environment.NewLine + ex.Message);
                return err;
            }
        }
        /// <summary>
        /// 解析参数
        /// </summary>
        /// <param name="line"></param>
        private ICmd ParseParam(string cmd, string param)
        {
            switch (cmd)
            {
                case "G01": //G01, 用G99是针对单个命令来测试的
                    G01 g01 = new G01();
                    g01.ParserParam(param);
                    return g01;
                case "G00":
                    G01 g00 = new G01("G00");
                    g00.ParserParam(param);
                    return g00;
                case "G04":
                    G04 g04 = new G04();
                    g04.ParserParam(param);
                    return g04;
                case "G100":
                    var g100 = new G100();
                    g100.ParserParam(param);
                    return g100;
                case "G101":
                    var g101 = new G101();
                    g101.ParserParam(param);
                    return g101;
                case "G102":
                    G102 g102 = new G102();
                    g102.ParserParam(param);
                    return g102;
                case "G103":
                    G103 g103 = new G103();
                    g103.ParserParam(param);
                    return g103;
                case "G104":
                    var g104 = new G104();
                    g104.ParserParam(param);
                    return g104;
                case "G105":
                    var g105 = new G105();
                    g105.ParserParam(param);
                    return g105;
                case "G106":
                    var g106 = new G106();
                    g106.ParserParam(param);
                    return g106;
                case "G107":
                    var g107 = new G107();
                    g107.ParserParam(param);
                    return g107;
                case "G108":
                    G108 g108 = new G108();
                    g108.ParserParam(param);
                    return g108;
                case "G109":
                    G109 g109 = new G109();
                    g109.ParserParam(param);
                    return g109;
                case "G110":
                    G110 g110 = new G110();
                    g110.ParserParam(param);
                    return g110;
                case "G111":
                    G111 g111 = new G111();
                    g111.ParserParam(param);
                    return g111;
                case "G112":
                    G112 g112 = new G112();
                    g112.ParserParam(param);
                    return g112;
                case "G113":
                    var g113 = new G113();
                    g113.ParserParam(param);
                    return g113;
                case "G114":
                    var g114 = new G114();
                    g114.ParserParam(param);
                    return g114;
                case "G115":
                    var g115 = new G115();
                    g115.ParserParam(param);
                    return g115;
                case "G116":
                    var g116 = new G116();
                    g116.ParserParam(param);
                    return g116;
                case "G117":
                    var g117 = new G117();
                    g117.ParserParam(param);
                    return g117;
                case "G118":
                    var g118 = new G118();
                    g118.ParserParam(param);
                    return g118;
                case "G119":
                    var g119 = new G119();
                    g119.ParserParam(param);
                    return g119;
                case "G201":
                    var g201 = new G201();
                    g201.ParserParam(param);
                    return g201;
                default:
                    Console.WriteLine("无法识别的命令:" + cmd);
                    break;
            }
            return null;          //  Console.ReadKey();
        }

        public void SetMacro(Dictionary<string, string> map)
        {
            //throw new NotImplementedException();
        }
    }
        
        
    /// <summary>
    /// 解析脚本的节点,包含了文件名,是否要统计行数,统计完返回的结果
    /// </summary>
    public class CmdFileNode
    {
        public string fileName { get; set; }//文件名
        public bool NeedCmdCount { get; set; }//是否需要统计行数
        public Action<int> SetCmdCount { get; set; }//执行设置行数的方法
    }
}
