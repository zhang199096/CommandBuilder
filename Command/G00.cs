using CommandBuilder.helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandBuilder.Command
{
    /// <summary>
    /// 快速定位
    /// 格式：命令码 + 各轴坐标与速度，分号或回车结束当前条；“//”后面不解析，直到回车
    /// 约定：坐标单位为毫米(mm)，速度为毫米每秒(mm/s)，如果所有轴速度一样，可以在最后写一个(见示例)；
    /// A表示Axis的缩写，A+数字表示第几个电机轴，E表示需要移动到的终点坐标，中间空格。
    /// 如A8E300.000F30 表示以第8轴从当前位置以30mm/s的速度运行到300的坐标处。
    /// </summary>
    public class G00:ICmd
    {
        private string name = "G00";
        public List<G00Param> paramList = new List<G00Param>();

        //解析参数,因为入栈,所以倒序输出
        public void ParserParam(string cmdstring)
        {
            paramList.Clear();
            Stack<G00Param> st = new Stack<G00Param>();//参数栈

            G00Param p01 = null;
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

                    p01 = new G00Param() { AValue = 0, EValue = 0, FValue = 0 };//遇到A,就新建一个参数
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
                        p01.EValue = Convert.ToInt32(tmp * 1000);
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

            G00Param endparam = st.Pop();//获取最后一个参数
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
        internal void show()//输出字符串,用于debug
        {
            Console.WriteLine(name);
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
    /// G01的参数格式
    /// </summary>
    public class G00Param
    {
        public int AValue { get; set; }
        public int EValue { get; set; }//E的值有没有小数?
        public int FValue { get; set; }
        public override string ToString()
        {
            return string.Format("A:{0},E:{1},F:{2}", AValue, EValue, FValue);
        }

    }
}
