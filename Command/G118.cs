using CommandBuilder.helper;
using System;
using System.Collections.Generic;

namespace CommandBuilder.Command
{
    public class G118 : ICmd
    {
        private string name = "G118";
        public List<G118Param> paramList = new List<G118Param>();

        //解析参数,因为入栈,所以倒序输出
        public void ParserParam(string cmdstring)
        {
            paramList.Clear();
            Stack<G118Param> st = new Stack<G118Param>();//参数栈

            G118Param p01 = null;
            string statu = ""; //记录上次的解析的是AEF的哪个,从而设置它的值
            string number = "";//数值
            foreach (var item in cmdstring)//逐个字符解析参数
            {
                if (item == 'A')
                {
                    if (statu == "V" && number != "")//上次解析的是E,今次是A,那么把number赋值给E
                    {
                        p01.VValue = int.Parse(number);
                        number = "";
                    }
                    p01 = new G118Param() { AValue = 0, MValue = 0, VValue = 0 };//遇到A,就新建一个参数
                    st.Push(p01);//入栈
                    statu = "A";
                }
                else if (item == 'M')
                {
                    if (number != "")//上次解析的是A,今次是E,那么把number赋值给A
                        p01.AValue = int.Parse(number);
                    number = "";
                    statu = "E";
                }
                else if (item == 'V')
                {
                    if (number != "")//上次解析的是E,今次是F,那么把number赋值给E, F前一个只会是E,不会是A
                    {
                        p01.MValue = int.Parse(number);
                    }
                    number = "";
                    statu = "V";
                }
                else if (CharHelper.IsNumeric(item.ToString()) || item == '.' || item == '-')//输入是数字,就累加
                {
                    number += item.ToString();
                }
                else
                    continue;
            }
            if (number != "")//最后一定是F, 没有结束标志,所以直接赋值了
                p01.VValue = int.Parse(number);

            //输出参数
            int lastM = 0;//把缺少的EF,填上去
            int lastV = 0;

            G118Param endparam = st.Pop();//获取最后一个参数
            paramList.Add(endparam);// Console.WriteLine(endparam);
            lastM = endparam.MValue;
            lastV = endparam.VValue;
            while (st.Count > 0)
            {
                var x = st.Pop();
                if (x.MValue == 0)//如果E没有值,就去它后面一个E值
                    x.MValue = lastM;
                else
                    lastM = x.MValue;

                if (x.VValue == 0)//如果F没有值,就去后面一个F值
                    x.VValue = lastV;
                else
                    lastV = x.VValue;

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
    public class G118Param
    {
        //M:1 初始化 V 无效
        //M:2 最大速度 V 速度值
        //M:3 速度 V 速度值
        //M:4 查询 V 无效

        public int AValue { get; set; } 
        public int MValue { get; set; }
        public int VValue { get; set; }
        public override string ToString()
        {
            return string.Format("A:{0},M:{1},V:{2} ", AValue, MValue, VValue);
        }

    }
}
