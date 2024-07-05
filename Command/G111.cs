using CommandBuilder.helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandBuilder.Command
{
    /// <summary>
    /// 功能码12：振动加热
    /// 格式
    /// </summary>
    public class G111 : ICmd
    {
        private string name = "G111";
        public List<G111Param> paramList = new List<G111Param>();


        //解析参数,因为入栈,所以倒序输出
        public void ParserParam(string cmdstring)
        {
            paramList.Clear();
            Stack<G111Param> st = new Stack<G111Param>();//参数栈

            G111Param p01 = null;
            string statu = ""; //记录上次的解析的是AEF的哪个,从而设置它的值
            string number = "";//数值
            int cyc_count = 0; //记录第几次循环
            int curr_module = 0;
            foreach (var item in cmdstring)//逐个字符解析参数
            {
                if (item == 'V')
                {
                    if (statu == "R" && number != "")
                    {
                        if (curr_module == 1) p01.RValue = int.Parse(number);
                        if (curr_module == 2) p01.RValue2 = int.Parse(number);
                        if (curr_module == 3) p01.RValue3 = int.Parse(number);
                        if (curr_module == 4) p01.RValue4 = int.Parse(number);
                        if (curr_module == 5) p01.RValue5 = int.Parse(number);
                    }
                    if (statu == "V" && number != "")//上次解析的是E,今次是A,那么把number赋值给E
                    {
                        p01.VValue = int.Parse(number);
                        number = "";
                    }
                    if (statu == "M" && number != "")//上次解析的是E,今次是A,那么把number赋值给E
                    {
                        p01.MValue = int.Parse(number);
                        number = "";
                    }
                    if (statu == "H" && number != "")//上次解析的是F,今次是A,那么把number赋值给F
                    {
                        p01.HValue = int.Parse(number);
                        number = "";
                    }
                    number = "";
                    if (cyc_count != 0)
                        continue; //第二次  跳出
                    p01 = new G111Param()
                    {
                        VValue = 0,
                        MValue = 0,
                        HValue = 0,
                        RValue = 0,
                        VValue2 = 0,
                        MValue2 = 0,
                        HValue2 = 0,
                        RValue2 = 0,
                        VValue3 = 0,
                        MValue3 = 0,
                        HValue3 = 0,
                        RValue3 = 0,
                        VValue4 = 0,
                        MValue4 = 0,
                        HValue4 = 0,
                        RValue4 = 0,
                        VValue5 = 0,
                        MValue5 = 0,
                        HValue5 = 0,
                        RValue5 = 0
                    };//遇到A,就新建一个参数
                    st.Push(p01);//入栈
                    statu = "V";
                }
                else if (item == 'M')
                {//确认当前模组
                    if (number != "")
                        curr_module = int.Parse(number); //设置当前模组
                    if (curr_module == 1) p01.VValue = int.Parse(number);
                    if (curr_module == 2) p01.VValue2 = int.Parse(number);
                    if (curr_module == 3) p01.VValue3 = int.Parse(number);
                    if (curr_module == 4) p01.VValue4 = int.Parse(number);
                    if (curr_module == 5) p01.VValue5 = int.Parse(number);
                    number = "";
                    statu = "M";
                }
                else if (item == 'H')
                {
                    if (number != "" && curr_module == 1) p01.MValue = int.Parse(number);
                    if (number != "" && curr_module == 2) p01.MValue2 = int.Parse(number);
                    if (number != "" && curr_module == 3) p01.MValue3 = int.Parse(number);
                    if (number != "" && curr_module == 4) p01.MValue4 = int.Parse(number);
                    if (number != "" && curr_module == 5) p01.MValue5 = int.Parse(number);
                    number = "";
                    statu = "H";//fix error 
                }
                else if (item == 'R')
                {
                    if (number != "" && curr_module == 1) p01.HValue = int.Parse(number);
                    if (number != "" && curr_module == 2) p01.HValue2 = int.Parse(number);
                    if (number != "" && curr_module == 3) p01.HValue3 = int.Parse(number);
                    if (number != "" && curr_module == 4) p01.HValue4 = int.Parse(number);
                    if (number != "" && curr_module == 5) p01.HValue5 = int.Parse(number);
                    number = "";
                    statu = "R";
                    cyc_count++;
                }
                else if (CharHelper.IsNumeric(item.ToString()) || item == '.' || item == '-')//输入是数字,就累加
                {
                    number += item.ToString();
                }
                else
                    continue;
            }
            if (number != "")//最后一定是R, 没有结束标志,所以直接赋值了
            {
                if (statu == "H") //如果到这里是H代表第一版本脚本
                {
                    if (curr_module == 1) p01.HValue = int.Parse(number);
                    if (curr_module == 2) p01.HValue2 = int.Parse(number);
                    if (curr_module == 3) p01.HValue3 = int.Parse(number);
                    if (curr_module == 4) p01.HValue4 = int.Parse(number);
                    if (curr_module == 5) p01.HValue5 = int.Parse(number);
                    //  p01.HValue = int.Parse(number);
                    p01.RValue = 0; //老脚本不存在R值
                }
                else
                {
                    if (curr_module == 1) p01.RValue = int.Parse(number);
                    if (curr_module == 2) p01.RValue2 = int.Parse(number);
                    if (curr_module == 3) p01.RValue3 = int.Parse(number);
                    if (curr_module == 4) p01.RValue4 = int.Parse(number);
                    if (curr_module == 5) p01.RValue5 = int.Parse(number);
                    // p01.RValue3 = int.Parse(number);
                }

            }

            //输出参数
            int lastE = 0;//把缺少的EF,填上去
            int lastF = 0;

            G111Param endparam = st.Pop();//获取最后一个参数
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
    public class G111Param
    {
        public int VValue { get; set; }
        public int VValue2 { get; set; }
        public int VValue3 { get; set; }
        public int VValue4 { get; set; }
        public int VValue5 { get; set; }
        public int MValue { get; set; }
        public int MValue2 { get; set; }
        public int MValue3 { get; set; }
        public int MValue4 { get; set; }
        public int MValue5 { get; set; }
        public int HValue { get; set; }
        public int HValue2 { get; set; }
        public int HValue3 { get; set; }
        public int HValue4 { get; set; }
        public int HValue5 { get; set; }
        public int RValue { get; set; }//转速
        public int RValue2 { get; set; }//转速
        public int RValue3 { get; set; }//转速
        public int RValue4 { get; set; }//转速
        public int RValue5 { get; set; }//转速
        public override string ToString()
        {
            return string.Format("CH1=V:{0},M:{1},H:{2},R:{3}", VValue, MValue, HValue, RValue) +
                   string.Format("CH2=V:{0},M:{1},H:{2},R:{3}", VValue2, MValue2, HValue2, RValue2)+
                   string.Format("CH3=V:{0},M:{1},H:{2},R:{3}", VValue3, MValue3, HValue3, RValue3)+
                   string.Format("CH4=V:{0},M:{1},H:{2},R:{3}", VValue4, MValue4, HValue4, RValue4)+
                   string.Format("CH5=V:{0},M:{1},H:{2},R:{3}", VValue5, MValue5, HValue5, RValue5);
        }

    }
}
