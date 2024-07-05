using CommandBuilder.helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandBuilder.Command
{
    public class G201 : ICmd
    {
        private string name = "G201";
        public List<G201Param> paramList = new List<G201Param>();

        public G201()
        {
            name = "G201";
        }
        public G201(string cmdname)
        {
            name = cmdname;
        }

        //解析参数,因为入栈,所以倒序输出
        public void ParserParam(string cmdstring)
        {
            paramList.Clear();
            Stack<G201Param> st = new Stack<G201Param>();//参数栈

            G201Param p201 = null;
            string statu = ""; //记录上次的解析的是AEF的哪个,从而设置它的值
            string number = "";//数值
            foreach (var item in cmdstring)//逐个字符解析参数
            {
                if (item == 'D')
                {
                    p201 = new G201Param() { DValue = 0, CValue = 0, PValue = 0, RValue = 0, AValue = 0, HValue = 0, TValue = 0 };//遇到D,就新建一个参数
                    st.Push(p201);//入栈
                    statu = "D";
                }
                else if (item == 'C')
                {
                    if (statu == "D" && number != "")
                        p201.DValue = int.Parse(number);
                    number = "";
                    statu = "C";
                }
                else if (item == 'P')
                {
                    if (statu == "D" && number != "")
                        p201.DValue = int.Parse(number);
                    number = "";
                    statu = "P";
                }
                else if (item == 'R')
                {
                    if (statu == "D" && number != "")
                        p201.DValue = int.Parse(number);
                    number = "";
                    statu = "R";
                }
                else if (item == 'A')
                {
                    if (statu == "R" && number != "")
                        p201.RValue = int.Parse(number);
                    number = "";
                    statu = "A";
                }
                else if (item == 'H')
                {
                    if (statu == "A" && number != "")
                        p201.AValue = int.Parse(number);
                    number = "";
                    statu = "H";
                }
                else if (item == 'T')
                {
                    if (statu == "H" && number != "")//上次解析的是D,今次是T,那么把number赋值给D
                        p201.HValue = int.Parse(number);
                    number = "";
                    statu = "T";
                }
                
                else if (CharHelper.IsNumeric(item.ToString()) || item == '.' || item == '-')//输入是数字,就累加
                {
                    number += item.ToString();
                }
                else
                    continue;
            }

            if (number != "")//
            {
                if (statu == "D")
                    p201.DValue = int.Parse(number);
                if (statu == "C")
                    p201.CValue = int.Parse(number);
                if (statu == "P")
                    p201.PValue = int.Parse(number);
                if (statu == "T")
                    p201.TValue = int.Parse(number);
            }

            //输出参数
            G201Param endparam = st.Pop();//获取最后一个参数
            paramList.Add(endparam);// Console.WriteLine(endparam);

            while (st.Count > 0)
            {
                var x = st.Pop();
                paramList.Add(x);   //Console.WriteLine(x);
            }

            //输出参数
            //int lastE = 0;//把缺少的EF,填上去
            //int lastF = 0;

            //G202Param endparam = st.Pop();//获取最后一个参数
            //paramList.Add(endparam);// Console.WriteLine(endparam);
            //lastE = endparam.EValue;
            //lastF = endparam.FValue;
            //while (st.Count > 0)
            //{
            //    var x = st.Pop();
            //    if (x.EValue == 0)//如果E没有值,就去它后面一个E值
            //        x.EValue = lastE;
            //    else
            //        lastE = x.EValue;

            //    if (x.FValue == 0)//如果F没有值,就去后面一个F值
            //        x.FValue = lastF;
            //    else
            //        lastF = x.FValue;

            //    paramList.Add(x);   //Console.WriteLine(x);
            //}
        }
        //internal void show()//输出字符串,用于debug
        //{
        //    //show2();
        //    //return;
        //    Console.Write(name+"\t");
        //    foreach (var item in paramList)
        //    {
        //        Console.WriteLine(item);
        //    }
        //    Console.WriteLine();
        //}
        public override string ToString()
        {
            var tmp = name + " \t";
            foreach (var item in paramList)
            {
                tmp += item;
            }
            return tmp;
        }

        internal void show2()//输出字符串,用于debug
        {
            File.AppendAllText("d:/a.txt", name + "\r\n", Encoding.Default);
            // Console.WriteLine(name);
            foreach (var item in paramList)
            {
                //  Console.WriteLine(item);
                File.AppendAllText("d:/a.txt", item + "\r\n", Encoding.Default);
            }
            File.AppendAllText("d:/a.txt", "\r\n", Encoding.Default);
            //Console.WriteLine();
        }



    }
    /// <summary>
    /// G01的参数格式
    /// </summary>
    public class G201Param
    {
        public int DValue { get; set; }
        public int CValue { get; set; }
        public int PValue { get; set; }
        public int RValue { get; set; }
        public int AValue { get; set; }
        public int HValue { get; set; }
        public int TValue { get; set; }
        public override string ToString()
        {
            return string.Format("D:{0},\tC:{1},\tP:{2},\tR:{3},\tA:{4},\tH:{5},\tT:{6}\t", DValue, CValue, PValue, RValue, AValue, HValue, TValue);
        }

    }
}
