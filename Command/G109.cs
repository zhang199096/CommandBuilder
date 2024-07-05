using CommandBuilder.helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandBuilder.Command
{
    /// <summary>
    /// 功能码10：移液泵移液、放液
    /// 格式
    /// </summary>
   public class G109 : ICmd
    {
        private string name = "G109";
        public List<G109Param> paramList = new List<G109Param>();

        //解析参数,因为入栈,所以倒序输出
        public void ParserParam(string cmdstring)
        {
            paramList.Clear();
            Stack<G109Param> st = new Stack<G109Param>();//参数栈

            G109Param p01 = null;
            string statu = ""; //记录上次的解析的是AEF的哪个,从而设置它的值
            string number = "";//数值
            foreach (var item in cmdstring)//逐个字符解析参数
            {
                if (item == 'A')
                {
                    if (statu == "A" && number != "")//上次解析的是E,今次是A,那么把number赋值给E
                    {
                        p01.AValue = int.Parse(number);
                        number = "";
                    }
                    if (statu == "E" && number != "")//上次解析的是E,今次是A,那么把number赋值给E
                    {
                        p01.EValue = int.Parse(number);
                        number = "";
                    }
                    if (statu == "F" && number != "")//上次解析的是F,今次是A,那么把number赋值给F
                    {
                        p01.FValue = int.Parse(number);
                        number = "";
                    }

                    p01 = new G109Param() { AValue = 0, EValue = 0, FValue = 0 };//遇到A,就新建一个参数
                    st.Push(p01);//入栈
                    statu = "A";
                }
                else if (item == 'E')
                {
                    if (number != "")//上次解析的是A,今次是E,那么把number赋值给A
                        p01.AValue = int.Parse(number);
                    number = "";
                    statu = "E";
                }
                else if (item == 'F')
                {
                    if (number != "")//上次解析的是E,今次是F,那么把number赋值给E, F前一个只会是E,不会是A
                    {
                        var tmp = double.Parse(number);
                        p01.EValue = Convert.ToInt32(tmp);
                    }
                    number = "";
                    statu = "F";
                }
                else if (CharHelper.IsNumeric(item.ToString()) || item == '.' || item == '-')//输入是数字,就累加
                {
                    number += item.ToString();
                }
                else
                    continue;
            }
            if (number != "")//最后一定是F, 没有结束标志,所以直接赋值了
                p01.FValue = int.Parse(number);

            //输出参数
            int lastE = 0;//把缺少的EF,填上去
            int lastF = 0;

            G109Param endparam = st.Pop();//获取最后一个参数
            paramList.Add(endparam);// Console.WriteLine(endparam);
            lastE = endparam.EValue;
            lastF = endparam.FValue;
            while (st.Count > 0)
            {
                var x = st.Pop();
                if (x.EValue == 0)//如果E没有值,就去它后面一个E值
                    x.EValue = lastE;
                else
                    lastE = x.EValue;

                if (x.FValue == 0)//如果F没有值,就去后面一个F值
                    x.FValue = lastF;
                else
                    lastF = x.FValue;

                paramList.Add(x);   //Console.WriteLine(x);
            }
        }
        public override string ToString()
        {
            var tmp =name + "\t";
            foreach (var item in paramList)
            {
                tmp += item+" ";
            }
            return tmp;
        }

    }
    /// <summary>
    /// G01的参数格式
    /// </summary>
    public class G109Param
    {
        public int AValue { get; set; }
        public int EValue { get; set; }//E的值有没有小数?
        public int FValue { get; set; }
        public override string ToString()
        {
            return string.Format("A:{0},E:{1},F:{2} ", AValue, EValue, FValue);
        }

    }
}
