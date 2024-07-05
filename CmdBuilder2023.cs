using CommandBuilder.Command;
using Model;
using Model.INF;
using Model.NucleicAcid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CommandBuilder
{

    public class SubProcess //子流程结构
    {
        public string NoedName { get; set; }
        //当前子流程起点条数
        public int start_num { get; set; } = 0;
        //当前子流程结束条数
        public int end_num { get; set; } = 0;
    }
    public class MointorProcess{
        public int begin_num { get; set; } = 0;
        public int SumLenght { get; set; } = 0;
        public List<SubProcess> subs { get; set; } = new List<SubProcess>();
    }
    //2023版的命令解析
    public class CmdBuilder2023
    {
        public static string curSubProcess = "Start";
        public static MointorProcess Mointors = new MointorProcess();
        public static int ScriptLength = 0;//命令执行计数,



        public Action<ICmd> ReceiveCmd;//获取到cmd处理委托
        public Action<RunningPackage> ReceivePackage;//发送执行包

        private Dictionary<string, string> MarcoMap = new Dictionary<string, string>();//宏字典
        public void SetMacro(Dictionary<string, string> map)
        {
            MarcoMap = map;
        }
        public void SetMacro(string key, string value)
        {
            MarcoMap[key] = value;
        }
        #region 兼容以前单文件解析的旧接口
        public void CleanFileQueue()
        {
            //throw new NotImplementedException();
        }
        public void AddFile(string filename)
        {
            Package pkg = new Package();
            pkg.ScriptFiles = new List<string>() { filename };//脚本列表
            //pkg.MarcoMap = MarcoMap;//获取宏变量的值
            pkg.RunParameters = new Dictionary<string, string>();//运行参数
            Run(pkg);
        }

        public void AddFiles(IEnumerable<string> files)
        {
            Package pkg = new Package();
            pkg.ScriptFiles = files.ToList();//脚本列表

            //pkg.MarcoMap = MarcoMap;//获取宏变量的值
            pkg.RunParameters = new Dictionary<string, string>();//运行参数
            Run(pkg);
        }

        public void AddFile(string file, bool needCmdCount, object value)
        {
            //不实现这个方法了.
            AddFile(file);
        }

        public void AddFiles(IEnumerable<string> file, bool needCmdCount, object value)
        {
            //不实现这个方法了.
            AddFiles(file);
        }

        public bool UpdateMap(Dictionary<string,string> d_replace)
        {
            foreach(var item in d_replace) 
            {
                if (MarcoMap.ContainsKey(item.Key))
                {
                    MarcoMap[item.Key] = item.Value;
                }
                else//发现不存在参数?  //TODO   是否重新添加上
                {
                    return false;
                }
            }
            return true;
        }
        public void Run()
        {
        }
        #endregion


        #region 新接口
        //无运行参数的脚本集合
        public void AddFiles(List<string> fileList,List<string> Nodes)
        {
            Package pkg = new Package();
            pkg.ScriptFiles = fileList;//脚本列表
            pkg.NodeNames = Nodes;  
            //pkg.MarcoMap = MarcoMap;//获取宏变量的值
            pkg.RunParameters = new Dictionary<string, string>();//运行参数
            Run(pkg);
        }
        //带运行参数的脚本集合
        public void AddFiles(List<string> fileList, Dictionary<string, string> RunParameters)
        {
            Package pkg = new Package();
            pkg.ScriptFiles = fileList;//脚本列表
            //pkg.MarcoMap = MarcoMap;//获取宏变量的值
            pkg.RunParameters = RunParameters;//运行参数
            Run(pkg);
        }
        #endregion


        //private RunningPackage currentRunningInfo = new RunningPackage();//当前运行的脚本列表

        //files=脚本文件集合,MarcoMap=宏命令字典,RunParameters=运行这些脚本的参数
        public void Run(Package pkg)//List<string> files,Dictionary<string,string> RunParameters)
        {
            //if (pkg.MarcoMap == null)
            //    pkg.MarcoMap = this.MarcoMap;

            Mission job = new Mission();
            job.InitMission(pkg.ScriptFiles, MarcoMap, pkg.RunParameters);

            ParseResult val= job.Parsing();//解析结果
            string Error=val.Error; List<ScriptItem> CommandNodes =val.CommandNodes;
            if (Error != "")//如果解析错误,就返回
            {
                throw new Exception(Error);
                //Console.WriteLine(Error);
                //return;
            }
            //TODO 解析完成  生成监控任务数组   start ProcessName  process_start prcess_end 
            if(pkg.NodeNames.Count==0) //预防  节点名为空的状况
            {
                foreach(var i in val.CommandNodes)
                { 
                    pkg.NodeNames.Add(i.Scriptfilename); 
                }

            }    
            using (var e1 = pkg.NodeNames.GetEnumerator())
            using (var e2 = val.CommandNodes.GetEnumerator())
            {
                Mointors.subs.Clear();
                Mointors.begin_num = ScriptLength; 
                int sum = 0;
                int cur_start = ScriptLength;
                while (e1.MoveNext() && e2.MoveNext())
                {
                    var item1 = e1.Current;
                    var item2 = e2.Current;
                   
                    // use item1 and item2
                    Mointors.subs.Add(new SubProcess { 
                        NoedName = item1,
                        start_num = cur_start, 
                        end_num = item2.CommandList.Count + cur_start
                    });
                    cur_start = item2.CommandList.Count + cur_start;
                    sum += item2.CommandList.Count;
                   
                }
                Mointors.SumLenght = sum;
            }

            RunningPackage runningPkg = new RunningPackage();
            runningPkg.StartIndex = ScriptLength;
            runningPkg.SetNodeNames(pkg.NodeNames);
            runningPkg.SetScripts(CommandNodes);
            //currentRunningInfo = runningPkg;
            if (ReceivePackage != null)
            {
                ReceivePackage(runningPkg);
            }
            //if (ReceiveCmd != null)
            //{
            //    foreach (ICmd cmd in runningInfo.GetCmdList())
            //    {
            //        ReceiveCmd(cmd);
            //    }
            //}
        }
        //测试脚本,输出到文件 TsTest/AddEluent_S日期.txt
        public void TestScript(Package pkg)
        {
            Mission job = new Mission();
            job.InitMission(pkg.ScriptFiles, MarcoMap, pkg.RunParameters);
            ParseResult val = job.Parsing();//解析结果
            string Error = val.Error; List<ScriptItem> CommandNodes = val.CommandNodes;
            if (Error != "")//如果解析错误,就返回
            {
                throw new Exception(Error);
                //Console.WriteLine(Error);
                //return;
            }
            RunningPackage runningPkg = new RunningPackage();
            runningPkg.StartIndex = ScriptLength;
            runningPkg.SetScripts(CommandNodes);
            //currentRunningInfo = runningPkg;
            SaveTestFile(runningPkg.GetCmdList());

        }
        //将解析后的命令保存到文件
        private void SaveTestFile(List<ICmd> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var cmd in list)
            {
                sb.AppendLine(cmd.ToString());
            }
            try
            {
                if (!Directory.Exists("record"))
                    Directory.CreateDirectory("record");
                var filename = "./TsTest/AddEluent_S" + DateTime.Now.ToString("yy_MM_dd_hh_mm_ss") + ".txt";
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
        }

        public void TestScript(Package pkg, string outpath)
        {
            Mission job = new Mission();
            job.InitMission(pkg.ScriptFiles, MarcoMap, pkg.RunParameters);
            ParseResult val = job.Parsing();//解析结果
            string Error = val.Error; List<ScriptItem> CommandNodes = val.CommandNodes;
            if (Error != "")//如果解析错误,就返回
            {
                throw new Exception(Error);
                //Console.WriteLine(Error);
                //return;
            }
            RunningPackage runningPkg = new RunningPackage();
            runningPkg.StartIndex = ScriptLength;
            runningPkg.SetScripts(CommandNodes);
            //currentRunningInfo = runningPkg;
            SaveTestFile(runningPkg.GetCmdList(), outpath);

        }
        //将解析后的命令保存到文件
        private void SaveTestFile(List<ICmd> list, string outpath)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var cmd in list)
            {
                sb.AppendLine(cmd.ToString());
            }
            try
            {
                if (!Directory.Exists("record"))
                    Directory.CreateDirectory("record");
                var filename = outpath + "/" + DateTime.Now.ToString("yy_MM_dd_hh_mm_ss") + ".txt";
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
        }

        //查找未在配置文件Macro.json设置的宏变量,在正式运行前要执行这个检查
        public string FindUnsetMacro(Package pkg, string outputfile)//List<string> files,Dictionary<string,string> RunParameters)
        {
            if(File.Exists(outputfile))
                File.Delete(outputfile);
            File.Create(outputfile).Close();

            Mission job = new Mission();
            job.InitMission(pkg.ScriptFiles, MarcoMap, pkg.RunParameters);


            Dictionary<string, bool> mapMacroUnset = new Dictionary<string, bool>();//未设置的变量名
            ParseResult val = job.TryParsing(mapMacroUnset);//尝试解析结果,如果变量名未设置,就输出到文件
            string Error=val.Error; List<ScriptItem> CommandNodes=val.CommandNodes;
            foreach (var marcoName in mapMacroUnset)
            {
                File.AppendAllText(outputfile, marcoName.Key + Environment.NewLine);
            }

            if (Error != "")//如果解析错误,就返回
            {
                return Error;
            }
            return "";//解析成功
        }


        public void Run(PackageTask pak)
        {
            List<string> scripts = new List<string>();
            List<string> nodes = new List<string>();
            foreach (var task in pak.Tasks)
            {
                task.PreProcess(task.Scriptfilename, pak.RunParameters,pak.records);
                scripts.Add(task.Scriptfilename);
                nodes.Add(task.NodeValue);
                //AddFile(task.Scriptfilename);
                //TODO 依赖添加脚本流程不够准确   依赖脚本执行条数来确定
                curSubProcess = task.NodeValue;
                Console.WriteLine("Excute:"+task.Scriptfilename + "NodeName:" +task.NodeValue);
                task.WaitStable();
                task.PostProcess(task.Scriptfilename, pak.RunParameters, pak.records);
            }
            //总体导入脚本  无关 暂停等待
            AddFiles(scripts,nodes);
        }
    }
    //当前要执行的命令信息
    public class RunningPackage
    {
        public int StartIndex { get;  set; }//发送前的执行条数
        private List<ScriptItem> Scripts;//脚本及其命令
        private List<string> NodeNames;//脚本对应子流程名称
        private List<ICmd> CmdList { get; set; }//命令列表
        public void SetNodeNames(List<string> Nodes)
        {
            NodeNames = Nodes;
        }
        public void SetScripts(List<ScriptItem> commandNodes)
        {
            Scripts=commandNodes;
            ToCmdList();
        }
        public void ToCmdList()
        {
            int sum = 0;
            //TODO  加入字脚本流程监控？
            Scripts.ForEach(x => sum += x.CommandList.Count);

            //using (var e1 = Scripts.GetEnumerator())
            //using (var e2 = NodeNames.GetEnumerator())
            //{
            //    while (e1.MoveNext() && e2.MoveNext())
            //    {
            //        var item1 = e1.Current;
            //        var item2 = e2.Current;
            //        // use item1 and item2
            //        sum += item1.CommandList.Count;
                    
            //    }
            //}


            // (Scripts, NodeNames).Fr
            var list = new List<ICmd>(sum);
            foreach (var item in Scripts)
            {
                foreach (var item2 in item.CommandList)
                {
                    list.Add(item2);
                }
            }
            CmdList=list;
        }

        public List<ICmd> GetCmdList()
        {
            return CmdList;
        }
        //获取指定安全点开始之后的命令,包含了@ GMark命令
        public List<ICmd> GetSumList(List<string> points)
        {
            if (points.Count == 0)//整个任务
                return GetCmdList();
            if(points.Count == 1)//选择到文件
            {
                var file = points[0];

                bool begin = false;
                var list = new List<ICmd>();
                foreach (var script in Scripts)
                {
                    if (begin)
                    {
                        foreach (var item2 in script.CommandList)
                        {
                            list.Add(item2);
                        }
                    }
                    if (script.Scriptfilename == file)
                    {
                        begin = true;
                    }

                }
                return list;
            }
            if (points.Count == 2)
            {  
                //只能搜索2层
                var file = points[0];
                var point = points[1];
                bool begin1 = false;
                bool begin2 = false;
                var list = new List<ICmd>();
                foreach (var script in Scripts)
                {
                    if (begin1&& begin2)
                    {
                        foreach (var item2 in script.CommandList)
                        {
                            list.Add(item2);
                        }
                    }
                    if (script.Scriptfilename == file)
                    {
                        begin1 = true;

                        foreach (var item2 in script.CommandList)
                        {
                            if(begin2)
                            {
                                list.Add(item2);
                            }
                            else
                            {
                                var cmdstring = item2.ToString();
                                if (cmdstring.StartsWith("@"))
                                {
                                    if (cmdstring == point)
                                    {
                                        begin2 = true;
                                    }
                                }
                            }
             
                        }
                    }
                   

                }
                return list;
            }
            return new List<ICmd>();
        }
        // 获取运行目录数
        public Tree GetRunningTree()
        {
            Tree root=new Tree() { Title= "未命名流程"  };//故意不设置Value,跳转时候排除这个节点
            foreach (var item in Scripts)
            {
                var filename= System.IO.Path.GetFileName(item.Scriptfilename);
                var child = new Tree() { Title= filename ,Value= item.Scriptfilename };
                root.Childs.Add(child);
                foreach (var item2 in item.CommandList)
                {
                    if(item2 is GMark)
                        child.Childs.Add(new Tree() { Title=item2.ToString(),Value= item2.ToString() });
                }
            }
            return root;
        }


    }


    //可运行单元,包含了脚本列表,宏命令字典,运行参数字典
    public class Package
    {
        public List<string> ScriptFiles { get; set; }//脚本列表
        public List<string> NodeNames { get; set; } = new List<string>();
        //public Dictionary<string, string> MarcoMap { get; set; }//宏命令字典, 如果需要传入宏字典再开启, 默认在启动时候,BLL.Tsystem会执行SetMarco方法,设置CmdBuilder2023的MarcoMap
        public Dictionary<string, string> RunParameters { get; set; }//运行参数字典

        public void SetAbsolutePath(string root)
        {
            for (int i = 0; i < ScriptFiles.Count; i++)
            {
               var filepath= ScriptFiles[i];
                if (filepath.StartsWith("//"))
                {
                    ScriptFiles[i] = root + filepath;
                }
            }
        }
    }

    //命令字符串
    public class CommandItem : ICloneable
    {
        public string Cmd { get; set; }//命令名
        public string Param { get; set; }//参数字符串

        public object Clone()
        {
            var cr = (CommandItem)MemberwiseClone();
            return cr; 
        }
    }

    //脚本及翻译后的命令列表
    public class ScriptItem
    {
        public string Scriptfilename;//脚本名称
        public List<ICmd> CommandList=new List<ICmd>();//解析出来的命令

    }
    //运行节点
    public class Tree
    {
        public string Title;//节点名称
        public string Value;//节点值
        public List<Tree> Childs;//子节点
        public Tree()
        {
            Childs=new List<Tree>();
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
    //单任务过程 
    public class ProcessTask
    {
        public string Scriptfilename;
        public string NodeValue;
        //前处理  //默认函数
        public Action<string, Dictionary<string, string>, T_ExpRecords> PreProcess = (script, str, record) =>
        {
            //TODO  更新进度  获取当前子流程 
            Console.WriteLine("Default " + script + " is in PreProcess");
        };
        //后处理  //默认函数
        public Action<string, Dictionary<string, string>, T_ExpRecords> PostProcess = (script, str, record) =>
        {
            Console.WriteLine("Default " + script + " is in PostProcess");
        };
        public Action WaitStable = () =>
        {
            Console.WriteLine("Default is in WaitStable");
        };
        public ProcessTask(string scriptfilename, Action<string, Dictionary<string, string>, T_ExpRecords> preProcess, Action<string, Dictionary<string, string>, T_ExpRecords> postProcess)
        {
            Scriptfilename = scriptfilename;
            if(preProcess!=null)
                PreProcess = preProcess;
            if (postProcess != null)
                PostProcess = postProcess;
        }
        public ProcessTask(string scriptfilename, string nodeValue="")
        {
            Scriptfilename = scriptfilename;
            NodeValue = nodeValue;
        }

        public void BindNodeFunc(INodeTask node)
        {
            PreProcess = node.PreProcess;
            WaitStable = node.WaitStable;
            PostProcess = node.PostProcess;
        }
    
    }
    public class PackageTask
    {
        public List<ProcessTask> Tasks { get; set; }//脚本列表
        public Dictionary<string, string> RunParameters { get; set; }//运行参数字典
        public T_ExpRecords records { get; set; }
        public void SetAbsolutePath(string root)
        {
            for (int i = 0; i < Tasks.Count; i++)
            {
                var filepath = Tasks[i];
                if (filepath.Scriptfilename.StartsWith("//"))
                {
                    Tasks[i].Scriptfilename = root + filepath.Scriptfilename;
                }
            }
        }
    }
}
