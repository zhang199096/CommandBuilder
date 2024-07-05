using CommandBuilder.helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandBuilder.Command
{
    /// <summary>
    /// 定时暂停
    /// 格式
    /// </summary>
    public class G04 : ICmd
    {
        private string name = "G04";
        public List<G04Param> paramList = new List<G04Param>();

        //解析参数,因为入栈,所以倒序输出
        public void ParserParam(string cmdstring)
        {
            paramList.Clear();
           // Stack<G04Param> st = new Stack<G04Param>();//参数栈

            G04Param p01 = null;
            string status = "";
            string number = "";//数值
            foreach (var item in cmdstring)//逐个字符解析参数
            {
                if (item == 'H')
                {
                    p01 = new G04Param() { HValue = 0 };
                    //st.Push(p01);//入栈
                    status = "H";
                }
                else if(item == 'E')
                { 
                    if (number != "" && status == "H")//最后一定是F, 没有结束标志,所以直接赋值了
                        p01.HValue = int.Parse(number);
                    number = "";
                    status = "E";
                }
                else if (CharHelper.IsNumeric(item.ToString()) || item == '.' || item == '-')//输入是数字,就累加
                {
                    number += item.ToString();
                }
                else
                    continue;
            }
            if (number != "" && status == "E")
                p01.EVAlue = int.Parse(number);
            else if(number != "" && status == "H")
                p01.HValue = int.Parse(number);

            paramList.Add(p01);
          
        }
        internal void show()//输出字符串,用于debug
        {
            Console.Write(name + "\t");
            foreach (var item in paramList)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();
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
    public class G04Param
    {
        public int HValue { get; set; }
        public int EVAlue { get; set; }
        public override string ToString()
        {
            return string.Format("H:{0},E:{1}", HValue,EVAlue);
        }

    }
}
