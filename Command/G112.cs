using CommandBuilder.helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandBuilder.Command
{
    /// <summary>
    /// 功能码13：电磁脉冲振荡
    /// 格式
    /// </summary>
    public class G112 : ICmd
    {
        private string name = "G112";
        public List<G112Param> paramList = new List<G112Param>();


        //解析参数,因为入栈,所以倒序输出
        public void ParserParam(string cmdstring)
        {
            paramList.Clear();
            Stack<G112Param> st = new Stack<G112Param>();//参数栈

            G112Param p01 = null;
            string statu = ""; //记录上次的解析的是AEF的哪个,从而设置它的值
            string number = "";//数值
            foreach (var item in cmdstring)//逐个字符解析参数
            {
                if (item == 'E')
                {
                    if (statu == "E" && number != "")//上次解析的是E,今次是A,那么把number赋值给E
                    {
                        p01.EValue = int.Parse(number);
                        number = "";
                    }
                    //if (statu == "M" && number != "")//上次解析的是E,今次是A,那么把number赋值给E
                    //{
                    //    p01.MValue = int.Parse(number);
                    //    number = "";
                    //}
                    if (statu == "H" && number != "")//上次解析的是F,今次是A,那么把number赋值给F
                    {
                        p01.HValue = int.Parse(number);
                        number = "";
                    }

                    p01 = new G112Param() { EValue = 0, MValue = 0, HValue = 0 };//遇到A,就新建一个参数
                    st.Push(p01);//入栈
                    statu = "E";
                }
                //else if (item == 'M')
                //{
                //    if (number != "")//上次解析的是A,今次是E,那么把number赋值给A
                //        p01.EValue = int.Parse(number);
                //    number = "";
                //    statu = "M";
                //}
                else if (item == 'H')
                {
                    if (number != "")//上次解析的是E,今次是F,那么把number赋值给E, F前一个只会是E,不会是A
                        p01.EValue = int.Parse(number);
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
                p01.HValue = int.Parse(number);

            //输出参数
            int lastE = 0;//把缺少的EF,填上去
            int lastF = 0;

            G112Param endparam = st.Pop();//获取最后一个参数
            paramList.Add(endparam);// Console.WriteLine(endparam);
            lastE = endparam.MValue;
            lastF = endparam.HValue;
            while (st.Count > 0)
            {
                var x = st.Pop();
                if (x.MValue == 0)//如果E没有值,就去它后面一个E值
                    x.MValue = lastE;
                else
                    lastE = x.MValue;

                if (x.HValue == 0)//如果F没有值,就去后面一个F值
                    x.HValue = lastF;
                else
                    lastF = x.HValue;

                paramList.Add(x);   //Console.WriteLine(x);
            }
        }
        internal void show()//输出字符串,用于debug
        {
            Console.Write( name + "\t");
            foreach (var item in paramList)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();
        }
        public override string ToString()
        {
            var tmp =name + "\t";
            foreach (var item in paramList)
            {
                tmp += item;
            }
            return tmp;
        }
    }

    /// <summary>
    /// G112的参数格式
    /// </summary>
    public class G112Param
    {
        public int EValue { get; set; }
        public int MValue { get; set; }
        public int HValue { get; set; }
        public override string ToString()
        {
            return string.Format("E:{0},M:{1},H:{2}", EValue, MValue, HValue);
        }

    }
}
