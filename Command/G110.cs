using CommandBuilder.helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandBuilder.Command
{
    /// <summary>
    /// 功能码11：离心机控制（定位、离心运行等）
    /// 格式
    /// </summary>
    public class G110 : ICmd
    {
        private string name = "G110";
        public List<G110Param> paramList = new List<G110Param>();
        //解析参数,因为入栈,所以倒序输出
        public void ParserParam(string cmdstring)
        {
            paramList.Clear();
            Stack<G110Param> st = new Stack<G110Param>();//参数栈

            G110Param p01 = null;
            string statu = ""; //记录上次的解析的是AEF的哪个,从而设置它的值
            string number = "";//数值
            foreach (var item in cmdstring)//逐个字符解析参数
            {
                if (item == 'P')
                {
                    p01 = new G110Param() { PValue = 0, RValue = 0, HValue = 0 };//遇到A,就新建一个参数
                    st.Push(p01);//入栈
                    statu = "P";
                }
                else if (item == 'R')
                {
                    p01 = new G110Param() { PValue = 0, RValue = 0, HValue = 0 };//遇到A,就新建一个参数
                    st.Push(p01);//入栈
                    statu = "R";
                }
                else if (item == 'H')
                {
                    if (number != "")//上次解析的是E,今次是F,那么把number赋值给E, F前一个只会是E,不会是A
                        p01.RValue = int.Parse(number);
                    number = "";
                    statu = "H";
                }
                else if (CharHelper.IsNumeric(item.ToString()) || item == '.' || item == '-')//输入是数字,就累加
                {
                    number += item.ToString();
                }
                else
                    continue;
            }
            if (number != "")//最后一定是F, 没有结束标志,所以直接赋值了
            {
                if (statu == "P")
                    p01.PValue = int.Parse(number);
                if (statu == "H")
                    p01.HValue = int.Parse(number);
            }  

            //输出参数
            G110Param endparam = st.Pop();//获取最后一个参数
            paramList.Add(endparam);// Console.WriteLine(endparam);

            while (st.Count > 0)
            {
                var x = st.Pop();
                paramList.Add(x);   //Console.WriteLine(x);
            }
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
    /// G110的参数格式
    /// </summary>
    public class G110Param
    {
        public int PValue { get; set; }
        public int RValue { get; set; }
        public int HValue { get; set; }
        public override string ToString()
        {
            return string.Format("P:{0},R:{1},H:{2}", PValue, RValue, HValue);
        }

    }
}
