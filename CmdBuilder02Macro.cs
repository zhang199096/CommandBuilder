using ExpressionEvaluator;
using System;
using System.Collections.Generic;
using System.IO;

namespace CommandBuilder
{
    //宏变量及表达式处理
    public class Marco
    {
        public Dictionary<string, string> MarcoMap = new Dictionary<string, string>();//宏转换
        public Marco(Dictionary<string, string> map)
        {
            MarcoMap = map;
        }
     
        public string MarcoParse(string line)
        {
            #region 处理一层带括号的计算式,不含宏变量
            string tmpLine = line.Replace("（","(").Replace("）",")");
            var expBracket = tmpLine.IndexOf("(");
            if (expBracket > -1)
                line = ExpressionCalcLineBracket(tmpLine);
            #endregion

            //提取变量名
            var listMarcoNameList = new List<string>();
            var symbol = '#';
            var indexlist = new List<int>();
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == symbol)
                    indexlist.Add(i);
            }

            for (int i = 0; i < indexlist.Count; i = i + 2)
            {
                if (indexlist.Count <= i + 1)//只有开始符号,没有结束符号
                    break;
                var len = indexlist[i + 1] - indexlist[i] + 1;
                var s1 = line.Substring(indexlist[i], len);
                listMarcoNameList.Add(s1);
            }

            //执行替换
            foreach (var marcoName in listMarcoNameList)
            {
                if (MarcoMap.ContainsKey(marcoName))   //直接报错,由开发员补齐资料继续
                {
                    var value = MarcoMap[marcoName];
                    if (string.IsNullOrEmpty(value))
                        value = "0";
                    line = line.Replace(marcoName, value);
                }
                else {
                    throw new Exception($"宏定义参数 {marcoName} 不存在，替换失败,请检查宏定义文件,line:{line}");
                }
            }
            var expFlag = line.IndexOf("$");

            if (expFlag > -1)
                line = ExpressionCalcLine(line);
            return line;
        }
        private string ExpressionCalcLineBracket(string line)
        {
            //没有处理多层括号的情况
            TextMatch textMatch = new TextMatch();
            var beginend=FindExpItem(line).Split('_');
            int start = int.Parse(beginend[0]);//  line.LastIndexOf('(');
            int end= int.Parse(beginend[1]);//line.IndexOf(")");
            if(start==-1|| end==-1)//括号查找失败
                return line;
            var exp2 = line.Substring(start, end- start+1);
            var expression = new CompiledExpression(exp2);
            var expValue = expression.Eval().ToString();//计算表达式的值

            line = line.Replace(exp2, expValue);//替换表达式
            return line;
        }
        //当有多个括号时候,查找最内层括号
        private string FindExpItem(string line)
        {
            int begin = -1;
            int end = -1;
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '(')
                    begin = i;
                if (line[i] == ')')
                {
                    end = i;
                    break;
                }
            }
            return begin.ToString() + "_" + end.ToString();
        }
        private string ExpressionCalcLine(string line)
        {
            //try
            //{
            TextMatch textMatch = new TextMatch();
            var listMarcoNameList = textMatch.ParseText(line, '$');//找出所有表达式
            foreach (var exp in listMarcoNameList)
            {
                var exp2 = exp.Substring(1, exp.Length - 2);
                var expression = new CompiledExpression(exp2);
                var expValue = expression.Eval().ToString();//计算表达式的值

                line = line.Replace(exp, expValue);//替换表达式
            }
            //}
            //catch
            //{
            //    throw new Exception("脚本表达式计算错误.");
            //}

            return line;
        }


        public string TryMarcoParse(string line, Dictionary<string, bool> mapMacroUnset, string scriptfile)
        {
            //提取变量名
            var listMarcoNameList = new List<string>();
            var symbol = '#';
            var indexlist = new List<int>();
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == symbol)
                    indexlist.Add(i);
            }

            for (int i = 0; i < indexlist.Count; i = i + 2)
            {
                if (indexlist.Count <= i + 1)//只有开始符号,没有结束符号
                    break;
                var len = indexlist[i + 1] - indexlist[i] + 1;
                var s1 = line.Substring(indexlist[i], len);
                listMarcoNameList.Add(s1);
            }

            //执行替换
            foreach (var marcoName in listMarcoNameList)
            {
                if (!MarcoMap.ContainsKey(marcoName))//将Macro.json没有的变量名输出到文件
                {
                    mapMacroUnset[marcoName] = true;
                    continue;
                }
                //if (MarcoMap.ContainsKey(marcoName))   //直接报错,由开发员补齐资料继续
                //{
                    var value = MarcoMap[marcoName];
                    if (string.IsNullOrEmpty(value))
                        value = "0";
                    line = line.Replace(marcoName, value);
                //}
            }
            var expFlag = line.IndexOf("$");
            if (expFlag > -1)
            {
                if(line.IndexOf("#") <0)//因为宏变量名替换失败,计算表达式会出现异常. 宏变量已经在上面的代码输出到文件了
                    line = TryExpressionCalcLine(line, scriptfile);
            }
            return line;
        }
        private string TryExpressionCalcLine(string line, string scriptfile)
        {
            TextMatch textMatch = new TextMatch();
            var listMarcoNameList = textMatch.ParseText(line, '$');//找出所有表达式
            foreach (var exp in listMarcoNameList)
            {
                var exp2 = exp.Substring(1, exp.Length - 2);
                var expression = new CompiledExpression(exp2);
                var expValue = expression.Eval().ToString();//计算表达式的值

                line = line.Replace(exp, expValue);//替换表达式
            }
            return line;
        }

    }
    public class TextMatch
    {
        public List<string> ParseText(string line, char symbol)//找出#变量
        {
            List<string> list = new List<string>();

            // var symbol = '#';
            var indexlist = new List<int>();
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == symbol)
                    indexlist.Add(i);
            }

            for (int i = 0; i < indexlist.Count; i = i + 2)
            {
                if (indexlist.Count <= i + 1)//只有开始符号,没有结束符号
                    break;
                var len = indexlist[i + 1] - indexlist[i] + 1;
                var s1 = line.Substring(indexlist[i], len);
                list.Add(s1);
            }
            return list;
        }
    }
}
