using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandBuilder.Command
{
    public class G113 : ICmd
    {
        private string name = "G113";
        public List<G113Param> paramList = new List<G113Param>();


        //解析参数,因为入栈,所以倒序输出
        public void ParserParam(string cmdstring)
        {
            paramList.Clear();
            Stack<G113Param> st = new Stack<G113Param>();//参数栈

            G113Param p01 = null;
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
                    if (statu == "H" && number != "")//上次解析的是E,今次是A,那么把number赋值给E
                    {
                        p01.HValue = int.Parse(number);
                        number = "";
                    }
                    if (statu == "L" && number != "")//上次解析的是F,今次是A,那么把number赋值给F
                    {
                        p01.LValue = int.Parse(number);
                        number = "";
                    }
                    if (statu == "P" && number != "")//上次解析的是F,今次是A,那么把number赋值给F
                    {
                        p01.PValue = int.Parse(number);
                        number = "";
                    }

                    p01 = new G113Param() { EValue = 0, HValue = 0, LValue = 0,PValue=0 };//遇到A,就新建一个参数
                    st.Push(p01);//入栈
                    statu = "E";
                }
                else if (item == 'H')
                {
                    if (number != "")//上次解析的是A,今次是E,那么把number赋值给A
                        p01.EValue = int.Parse(number);
                    number = "";
                    statu = "H";
                }
                else if (item == 'L')
                {
                    if (number != "")//上次解析的是E,今次是F,那么把number赋值给E, F前一个只会是E,不会是A
                        p01.HValue = int.Parse(number);
                    number = "";
                    statu = "L";
                }
                else if (item == 'P')
                {
                    if (number != "")//上次解析的是E,今次是F,那么把number赋值给E, F前一个只会是E,不会是A
                        p01.LValue = int.Parse(number);
                    number = "";
                    statu = "P";
                }
                else if (helper.CharHelper.IsNumeric(item.ToString()) || item == '.' || item == '-')//输入是数字,就累加
                {
                    number += item.ToString();
                }
                else
                    continue;
            }
            if (number != "")//最后一定是F, 没有结束标志,所以直接赋值了
                p01.PValue = int.Parse(number);

            //输出参数
            int lastH = 0;//把缺少的EF,填上去
            int lastL = 0;
            int lastP = 0;

            G113Param endparam = st.Pop();//获取最后一个参数
            paramList.Add(endparam);// Console.WriteLine(endparam);
            lastH = endparam.HValue;
            lastL = endparam.LValue;
            lastP = endparam.PValue;
            while (st.Count > 0)
            {
                var x = st.Pop();
                if (x.HValue == 0)//如果E没有值,就去它后面一个E值
                    x.HValue = lastH;
                else
                    lastH = x.HValue;

                if (x.LValue == 0)//如果F没有值,就去后面一个F值
                    x.LValue = lastL;
                else
                    lastL = x.LValue;

                if (x.PValue == 0)//如果F没有值,就去后面一个F值
                    x.PValue = lastP;
                else
                    lastP = x.PValue;

                paramList.Add(x);   //Console.WriteLine(x);
            }
        }

        public override string ToString()
        {
            var tmp =  name + "\t";
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
    public class G113Param
    {
        public int EValue { get; set; }
        public int HValue { get; set; }
        public int LValue { get; set; }
        public int PValue { get; set; }
        public override string ToString()
        {
            return string.Format("E:{0},H:{1},L:{2},P:{3}", EValue, HValue, LValue, PValue);
        }

    }
}
