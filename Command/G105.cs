using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandBuilder.Command
{
    /// <summary>
    ///  功能码6：更新扫描起点坐标或偏移坐标等
    ///  格式
    /// </summary>
   public class G105 : ICmd
    {
        private string name = "G105";
        public List<G105Param> paramList = new List<G105Param>();

        //解析参数,因为入栈,所以倒序输出
        public void ParserParam(string cmdstring)
        {
            paramList.Clear();
            Stack<G105Param> st = new Stack<G105Param>();//参数栈

            G105Param p01 = null;
            string statu = ""; //记录上次的解析的是AEF的哪个,从而设置它的值
            string number = "";//数值
            foreach (var item in cmdstring)//逐个字符解析参数
            {
                if (item == 'R')
                {
                    if (statu == "R" && number != "")//上次解析的是E,今次是A,那么把number赋值给E
                    {
                        p01.RValue = int.Parse(number);
                        number = "";
                    }
                    if (statu == "A" && number != "")//上次解析的是E,今次是A,那么把number赋值给E
                    {
                        p01.AValue = int.Parse(number);
                        number = "";
                    }
                    if (statu == "E" && number != "")//上次解析的是F,今次是A,那么把number赋值给F
                    {
                        var tmp = double.Parse(number);
                        p01.EValue = Convert.ToInt32(tmp * 1000);
                        //p01.EValue = int.Parse(number);
                        number = "";
                    }

                    p01 = new G105Param() { AValue = 0, EValue = 0, RValue = 0 };//遇到A,就新建一个参数
                    st.Push(p01);//入栈
                    statu = "R";
                }
                else if (item == 'A')
                {
                    if (number != "")//上次解析的是A,今次是E,那么把number赋值给A
                        p01.RValue = int.Parse(number);
                    number = "";
                    statu = "A";
                }
                else if (item == 'E')
                {
                    if (number != "")//上次解析的是E,今次是F,那么把number赋值给E, F前一个只会是E,不会是A
                    {
                        p01.AValue = int.Parse(number);
                    }
                    number = "";
                    statu = "E";
                }
                else if (helper.CharHelper.IsNumeric(item.ToString()) || item == '.' || item == '-')//输入是数字,就累加
                {
                    number += item.ToString();
                }
                else
                    continue;
            }
            if (number != "")//最后一定是F, 没有结束标志,所以直接赋值了
            {//  p01.EValue = int.Parse(number);
                var tmp = double.Parse(number);
                p01.EValue = Convert.ToInt32(tmp * 1000);
            }

            //输出参数
            int lastR = 0;
            int lastA = 0;//把缺少的EF,填上去
            int lastE = 0;

            G105Param endparam = st.Pop();//获取最后一个参数
            paramList.Add(endparam);// Console.WriteLine(endparam);
            lastR = endparam.RValue;
            lastA = endparam.AValue;
            lastE = endparam.EValue;
           
            while (st.Count > 0)
            {
                var x = st.Pop();
                if (x.RValue == 0)//如果E没有值,就去它后面一个E值
                    x.RValue = lastR;
                else
                    lastR = x.RValue;
                if (x.AValue == 0)//如果E没有值,就去它后面一个E值
                    x.AValue = lastA;
                else
                    lastE = x.EValue;

                if (x.EValue == 0)//如果F没有值,就去后面一个F值
                    x.EValue = lastE;
                else
                    lastE = x.EValue;

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
            return tmp ;
        }
    }
    /// <summary>
    /// G101的参数格式
    /// </summary>
    public class G105Param
    {
        public int RValue { get; set; }
        public int AValue { get; set; }
        public int EValue { get; set; }//E的值有没有小数?
       
        public override string ToString()
        {
            return string.Format("R:{0},A:{1},E:{2}", RValue, AValue, EValue);
        }

    }
}
