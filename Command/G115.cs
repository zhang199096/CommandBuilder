using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandBuilder.Command
{
    public class G115 : ICmd
    {
        private string name = "G115";
        public List<G115Param> paramList = new List<G115Param>();

        //解析参数,因为入栈,所以倒序输出
        public void ParserParam(string cmdstring)
        {
            paramList.Clear();
            Stack<G115Param> st = new Stack<G115Param>();//参数栈

            G115Param p01 = null;
            string statu = ""; //记录上次的解析的是AEF的哪个,从而设置它的值
            string number = "";//数值
            foreach (var item in cmdstring)//逐个字符解析参数
            {
                if (item == 'S')
                {
                    if (statu == "S" && number != "")//上次解析的是E,今次是A,那么把number赋值给E
                    {
                        p01.SValue = int.Parse(number);
                        number = "";
                    }
                    if (statu == "C" && number != "")//上次解析的是E,今次是A,那么把number赋值给E
                    {
                        p01.CValue = int.Parse(number);
                        number = "";
                    }

                    p01 = new G115Param() { SValue = 0, CValue = 0 };//遇到A,就新建一个参数
                    st.Push(p01);//入栈
                    statu = "S";
                }
                else if (item == 'C')
                {
                    if (number != "")//上次解析的是A,今次是E,那么把number赋值给A
                        p01.SValue = int.Parse(number);
                    number = "";
                    statu = "C";
                }
                else if (helper.CharHelper.IsNumeric(item.ToString()) || item == '.' || item == '-')//输入是数字,就累加
                {
                    number += item.ToString();
                }
                else
                    continue;
            }
            if (number != "")//最后一定是F, 没有结束标志,所以直接赋值了
                p01.CValue = int.Parse(number);

            //输出参数
            int lastS = 0;//把缺少的EF,填上去
            int lastC = 0;
            G115Param endparam = st.Pop();//获取最后一个参数
            paramList.Add(endparam);// Console.WriteLine(endparam);
            lastC = endparam.CValue;
            lastS = endparam.SValue;
            while (st.Count > 0)
            {
                var x = st.Pop();
                if (x.CValue == 0)//如果E没有值,就去它后面一个E值
                    x.CValue = lastC;

                else
                    lastC = x.CValue;

                if (x.SValue == 0)//如果F没有值,就去后面一个F值
                    x.SValue = lastS;
                else
                    lastS = x.SValue;
                paramList.Add(x);   //Console.WriteLine(x);
            }
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
    public class G115Param
    {
        public int SValue { get; set; }
        public int CValue { get; set; }
        public override string ToString()
        {
            return string.Format("S:{0},C:{1}", SValue,CValue);
        }

    }
}
