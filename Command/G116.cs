using CommandBuilder.helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandBuilder.Command
{
    /// <summary>
    /// 功能码9：装载、卸载tip头
    /// 格式
    /// </summary>
    public class G116 : ICmd
    {
        private string name = "G116";
        public List<G116Param> paramList = new List<G116Param>();

        //解析参数,因为入栈,所以倒序输出
        public void ParserParam(string cmdstring)
        {
            paramList.Clear();
            Stack<G116Param> st = new Stack<G116Param>();//参数栈

            G116Param p01 = null;
            string statu = ""; //记录上次的解析的是AEF的哪个,从而设置它的值
            string number = "";//数值
            foreach (var item in cmdstring)//逐个字符解析参数
            {
                if (item == 'L')
                {
                    if (number != "")//上次解析的是E,今次是A,那么把number赋值给E
                    {
                        p01.LUName = statu;
                        p01.LUValue = int.Parse(number);
                        number = "";
                    }

                    p01 = new G116Param() { LUValue = 0 };
                    st.Push(p01);//入栈
                    statu = "L";
                }
                else if (item == 'U')
                {
                    if (number != "")//上次解析的是E,今次是A,那么把number赋值给E
                    {
                        p01.LUName = statu;
                        p01.LUValue = int.Parse(number);
                        number = "";
                    }

                    p01 = new G116Param() { LUValue = 0 };
                    st.Push(p01);//入栈
                    statu = "U";
                }
                else if (CharHelper.IsNumeric(item.ToString()) || item == '.' || item == '-')//输入是数字,就累加
                {
                    number += item.ToString();
                }
                else
                    continue;
            }
            if (number != "")//最后一定是F, 没有结束标志,所以直接赋值了
            {
                p01.LUName = statu;
                p01.LUValue = int.Parse(number);
            }

            //输出参数
            G116Param endparam = st.Pop();//获取最后一个参数
            paramList.Add(endparam);// Console.WriteLine(endparam);
            while (st.Count > 0)
            {
                var x = st.Pop();
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
                tmp += item+" ";
            }
            return tmp;
        }
    }

    /// <summary>
    /// G116的参数格式
    /// </summary>
    public class G116Param
    {
        public string LUName { get; set; }
        public int LUValue { get; set; }
        public override string ToString()
        {
            return string.Format("{0}:{1}", LUName,LUValue);
        }

    }
}
