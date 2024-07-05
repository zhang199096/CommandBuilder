using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandBuilder.Command
{
    /// <summary>
    /// 功能码5：荧光控制
    /// 格式
    /// </summary>
   public class G104 : ICmd
    {
        private string name = "G104";
        public List<G104Param> paramList = new List<G104Param>();

        //解析参数,因为入栈,所以倒序输出
        public void ParserParam(string cmdstring)
        {
            paramList.Clear();
            Stack<G104Param> st = new Stack<G104Param>();//参数栈

            G104Param p01 = null;
            string statu = ""; //记录上次的解析的是AEF的哪个,从而设置它的值
            string number = "";//数值
            foreach (var item in cmdstring)//逐个字符解析参数
            {
                if (item == 'F')
                {
                    if (number != "")//上次解析的是E,今次是A,那么把number赋值给E
                    {
                        p01.FName = statu;
                        p01.FValue = int.Parse(number);
                        number = "";
                    }
                    p01 = new G104Param() { FValue = 0 };
                    st.Push(p01);//入栈
                    statu = "F";
                }
                else if (helper.CharHelper.IsNumeric(item.ToString()) || item == '.' || item == '-')//输入是数字,就累加
                {
                    number += item.ToString();
                }
                else
                    continue;
            }
            if (number != "")//最后一定是F, 没有结束标志,所以直接赋值了
            {
                p01.FValue = int.Parse(number);
                p01.FName = statu;
            }
               

            //输出参数
            G104Param endparam = st.Pop();//获取最后一个参数
            paramList.Add(endparam);// Console.WriteLine(endparam);
            while (st.Count > 0)
            {
                var x = st.Pop();
                paramList.Add(x);   //Console.WriteLine(x);
            }
            //paramList.Add(p01);

        }

        public override string ToString()
        {
            var tmp = name + "\t";
            foreach (var item in paramList)
            {
                tmp += item;
                tmp += " ";
            }
            return tmp;
        }
    }
    /// <summary>
    /// G101的参数格式
    /// </summary>
    public class G104Param
    {
        public string FName { get; set; }
        public double FValue { get; set; }
        public override string ToString()
        {
            return string.Format("{0}:{1}", FName,FValue);
        }

    }
}
