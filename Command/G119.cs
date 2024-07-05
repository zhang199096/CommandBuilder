
using CommandBuilder.helper;
using System;
using System.Collections.Generic;

namespace CommandBuilder.Command
{
    public class G119 : ICmd
    {
        private string name = "G119";
        public List<G119Param> paramList = new List<G119Param>();

        //解析参数,因为入栈,所以倒序输出
        public void ParserParam(string cmdstring)
        {
            paramList.Clear();
            Stack<G119Param> st = new Stack<G119Param>();//参数栈

            G119Param p01 = null;
            string statu = ""; //记录上次的解析的是AEF的哪个,从而设置它的值
            string number = "";//数值
            foreach (var item in cmdstring)//逐个字符解析参数
            {
                if (item == 'A')
                {
                    if (statu == "G" && number != "")//上次解析的是E,今次是A,那么把number赋值给E
                    {
                        p01.GValue = int.Parse(number);
                        number = "";
                    }
                    p01 = new G119Param() { AValue = 0, IValue = 0, PULLValue = 0, OValue = 0, PUSHValue = 0, GValue = 0 };//遇到A,就新建一个参数
                    st.Push(p01);//入栈
                    statu = "A";
                }
                else if (item == 'I')
                {
                    if (number != "")//上次解析的是A,今次是E,那么把number赋值给A
                        p01.AValue = int.Parse(number);
                    number = "";
                    statu = "I";
                }
                else if (item == 'E' && statu == "I")
                {
                    if (number != "")//上次解析的是E,今次是F,那么把number赋值给E, F前一个只会是E,不会是A
                    {
                        p01.IValue = int.Parse(number);
                    }
                    number = "";
                    statu = "E";
                }
                else if (item == 'O')
                {
                    if (number != "")//上次解析的是A,今次是E,那么把number赋值给A
                        p01.PULLValue = int.Parse(number);
                    number = "";
                    statu = "O";
                }
                else if (item == 'E' && statu == "O")
                {
                    if (number != "")//上次解析的是A,今次是E,那么把number赋值给A
                        p01.OValue = int.Parse(number);
                    number = "";
                    statu = "E";
                }
                else if (item == 'G')
                {
                    if (number != "")//上次解析的是A,今次是E,那么把number赋值给A
                        p01.PUSHValue = int.Parse(number);
                    number = "";
                    statu = "G";
                }
                else if (CharHelper.IsNumeric(item.ToString()) || item == '.' || item == '-')//输入是数字,就累加
                {
                    number += item.ToString();
                }
                else
                    continue;
            }
            if (number != "" && statu=="G")//最后一定是F, 没有结束标志,所以直接赋值了
                p01.GValue = int.Parse(number);

            //输出参数
            int lastI = 0;//把缺少的EF,填上去
            int lastPULL = 0;
            int lastO = 0;
            int lastPUSH = 0;
            G119Param endparam = st.Pop();//获取最后一个参数
            paramList.Add(endparam);// Console.WriteLine(endparam);
            lastI = endparam.IValue;
            lastPULL = endparam.PULLValue;
            lastO = endparam.OValue;
            lastPUSH = endparam.PUSHValue;
            while (st.Count > 0)
            {
                var x = st.Pop();
                if (x.IValue == 0)//如果E没有值,就去它后面一个E值
                    x.IValue = lastI;
                else
                    lastI = x.IValue;

                if (x.PULLValue == 0)//如果E没有值,就去它后面一个E值
                    x.PULLValue = lastPULL;
                else
                    lastPULL = x.PULLValue;
                if (x.OValue == 0)//如果E没有值,就去它后面一个E值
                    x.OValue = lastO;
                else
                    lastO = x.OValue;
                if (x.PUSHValue == 0)//如果E没有值,就去它后面一个E值
                    x.PUSHValue = lastPUSH;
                else
                    lastPUSH = x.PUSHValue;
               

                paramList.Add(x);   //Console.WriteLine(x);
            }
        }
        public override string ToString()
        {
            var tmp = name + "\t";
            foreach (var item in paramList)
            {
                tmp += item + " ";
            }
            return tmp;
        }

    }
    /// <summary>
    /// G01的参数格式
    /// </summary>
    public class G119Param
    {
        public int AValue { get; set; }
        public int IValue { get; set; }
        public int PULLValue { get; set; }
        public int OValue { get; set; }
        public int PUSHValue { get; set; }//E的值有没有小数?
        public int GValue { get; set; }
        public override string ToString()
        {
            return string.Format("A:{0},I:{1},E:{2},O:{3},E:{4},G:{5} ", AValue, IValue, PULLValue,OValue,PUSHValue,GValue);
        }

    }
}


