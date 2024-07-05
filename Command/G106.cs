using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandBuilder.Command
{
    /// <summary>
    /// 功能码7：LED电压控制（0~5V）
    /// 格式
    /// 
    /// </summary>
    public class G106 : ICmd
    {
        private string name = "G106";
        public List<G106Param> paramList = new List<G106Param>();

        //解析参数,因为入栈,所以倒序输出
        public void ParserParam(string cmdstring)
        {
            paramList.Clear();
            Stack<G106Param> st = new Stack<G106Param>();//参数栈

            G106Param p01 = null;
            string statu = ""; //记录上次的解析的是AEF的哪个,从而设置它的值
            string number = "";//数值
            foreach (var item in cmdstring)//逐个字符解析参数
            {
                if (item == 'C')
                {
                    if (statu == "C" && number != "")//上次解析的是E,今次是A,那么把number赋值给E
                    {
                        p01.CValue = int.Parse(number);
                        number = "";
                    }
                    if (statu == "V" && number != "")//上次解析的是E,今次是A,那么把number赋值给E
                    {
                        p01.VValue = int.Parse(number);
                        number = "";
                    }

                    p01 = new G106Param() { CValue = 0, VValue = 0 };//遇到A,就新建一个参数
                    st.Push(p01);//入栈
                    statu = "C";
                }
                else if (item == 'V')
                {
                    if (number != "")//上次解析的是A,今次是E,那么把number赋值给A
                        p01.CValue = int.Parse(number);
                    number = "";
                    statu = "V";
                }
                else if (helper.CharHelper.IsNumeric(item.ToString()) || item == '.' || item == '-')//输入是数字,就累加
                {
                    number += item.ToString();
                }
                else
                    continue;
            }
            if (number != "")//最后一定是F, 没有结束标志,所以直接赋值了
                p01.VValue = int.Parse(number);

            //输出参数
            int lastV = 0;//把缺少的EF,填上去
            int lastC = 0;
            G106Param endparam = st.Pop();//获取最后一个参数
            paramList.Add(endparam);// Console.WriteLine(endparam);
            lastV = endparam.VValue;
            lastC = endparam.CValue;
            while (st.Count > 0)
            {
                var x = st.Pop();
                if (x.VValue == 0)//如果E没有值,就去它后面一个E值
                    x.VValue = lastV;
                
                else
                    lastV = x.VValue;

                if (x.CValue == 0)//如果F没有值,就去后面一个F值
                    x.CValue = lastC;
                else
                    lastC = x.CValue;
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
    /// G108的参数格式
    /// </summary>
    public class G106Param
    {
        public int CValue { get; set; }
        public int VValue { get; set; }
        public override string ToString()
        {
            return string.Format("C:{0},V:{1}", CValue, VValue);
        }

    }
}
