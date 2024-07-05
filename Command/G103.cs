using CommandBuilder.helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandBuilder.Command
{
    /// <summary>
    /// 功能码4: 输出口控制
    /// 格式
    /// </summary>
    public class G103 : ICmd
    {
        private string name = "G103";
        public List<G103Param> paramList = new List<G103Param>();

        //解析参数,因为入栈,所以倒序输出
        public void ParserParam(string cmdstring)
        {
            paramList.Clear();
            Stack<G103Param> st = new Stack<G103Param>();//参数栈

            G103Param p01 = null;
            string statu = ""; //记录上次的解析的是AEF的哪个,从而设置它的值
            string number = "";//数值
            string HL = "";//高低电平
            foreach (var item in cmdstring)//逐个字符解析参数
            {
                if (item == 'O')
                {
                    if (statu == "O" && HL != "")//上次解析的是E,今次是A,那么把number赋值给E
                    {
                        p01.OValue = int.Parse(number);
                        p01.HLValue = HL;
                        number = "";
                        HL = "";
                    }

                    p01 = new G103Param() { OValue = 0, HLValue ="" };//遇到A,就新建一个参数
                    st.Push(p01);//入栈
                    statu = "O";
                }
                else if (item == 'H')
                {
                    if (number != "")//上次解析的是A,今次是E,那么把number赋值给A
                    {
                        p01.OValue = int.Parse(number);
                        p01.HLValue = "H";
                    }
                    number = "";
                    statu = "";
                }
                else if (item == 'L')
                {
                    if (number != "")//上次解析的是E,今次是F,那么把number赋值给E, F前一个只会是E,不会是A
                    {
                        p01.OValue = int.Parse(number);
                        p01.HLValue = "L";
                    }
                    number = "";
                    statu = "";
                }
                else if (CharHelper.IsNumeric(item.ToString()) || item == '.' || item == '-')//输入是数字,就累加
                {
                    number += item.ToString();
                }
                else
                    continue;
            }
            if (HL != "")//最后一定是F, 没有结束标志,所以直接赋值了
            {
                p01.OValue = int.Parse(number);
                p01.HLValue = HL;
            }

            //输出参数
            G103Param endparam = st.Pop();//获取最后一个参数
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
    /// G01的参数格式
    /// </summary>
    public class G103Param
    {
        public int OValue { get; set; }
        public string HLValue { get; set; }
        public override string ToString()
        {
            return string.Format("O:{0},{1} ", OValue, HLValue);
        }

    }
}
