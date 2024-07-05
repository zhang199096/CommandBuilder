using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandBuilder.Command
{
    public class G114 : ICmd
    {
        private string name = "G114";
        public List<G114Param> paramList = new List<G114Param>();

        //解析参数,因为入栈,所以倒序输出
        public void ParserParam(string cmdstring)
        {
            paramList.Clear();

            G114Param p01 = null;
            string status = "";//记录上次的解析的
            string number = "";//数值
            foreach (var item in cmdstring)//逐个字符解析参数
            {
                if(item =='M')
                {
                    if(status=="M"&&number!="")
                    {
                        p01.MValue = int.Parse(number);
                        number = "";
                    }
                    if(status=="H"&&number!="")
                    {
                        p01.HValue = int.Parse(number);
                        number = "";
                    }
                    p01 = new G114Param() { MValue=0, HValue = 0 };
                    status = "M";
                }
                else if (item == 'H')
                {
                    if (number != "")//上次解析的是A,今次是E,那么把number赋值给A
                        p01.MValue = int.Parse(number);
                    if(status=="")//如果第一次进入这里 原始版本
                    {
                        p01 = new G114Param() { MValue = 0, HValue = 0 };
                    }
                    number = "";
                    status = "H";
                    //p01 = new G114Param() { HValue = 0 };
                }
                else if (CommandBuilder.helper.CharHelper.IsNumeric(item.ToString()) || item == '.' || item == '-')//输入是数字,就累加
                {
                    number += item.ToString();
                }
                else
                    continue;
            }
            if (number != "")//最后一定是H
                p01.HValue = int.Parse(number);

            paramList.Add(p01);

        }

        public override string ToString()
        {
            var tmp = name + "\t";
            foreach (var item in paramList)
            {
                tmp += item;
            }
            return tmp;
        }
    }
    /// <summary>
    /// G01的参数格式
    /// </summary>
    public class G114Param
    {
        public int MValue { get; set; }
        public int HValue { get; set; }
        public override string ToString()
        {
            return string.Format("M:{0},H:{1}",MValue, HValue);
        }

    }
}
