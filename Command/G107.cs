using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandBuilder.Command
{
    /// <summary>
    ///  功能码8：LED通断时间更新
    ///  格式
    /// </summary>
    public class G107 : ICmd
    {
        private string name = "G107";
        public List<G107Param> paramList = new List<G107Param>();

        //解析参数,因为入栈,所以倒序输出
        public void ParserParam(string cmdstring)
        {
            paramList.Clear();
          //  Stack<G107Param> st = new Stack<G107Param>();//参数栈

            G107Param p01 = null;
           // string statu = ""; //记录上次的解析的是AEF的哪个,从而设置它的值
            string number = "";//数值
            foreach (var item in cmdstring)//逐个字符解析参数
            {
                if (item == 'T')
                {
                    p01 = new G107Param() { T1Value = 0, T2Value = 0 };//遇到A,就新建一个参数
                   // st.Push(p01);//入栈
                 //   statu = "T1";
                }
                else if (item == ',')
                {
                    if (number != "")//上次解析的是A,今次是E,那么把number赋值给A
                        p01.T1Value = int.Parse(number);
                    number = "";
                   // statu = "T2";
                }
                else if (helper.CharHelper.IsNumeric(item.ToString()) || item == '.' || item == '-')//输入是数字,就累加
                {
                    number += item.ToString();
                }
                else
                    continue;
            }
            if (number != "")//最后一定是F, 没有结束标志,所以直接赋值了
                p01.T2Value = int.Parse(number);
            paramList.Add(p01);
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
    public class G107Param
    {
        public int T1Value { get; set; }
        public int T2Value { get; set; }
        public override string ToString()
        {
            return string.Format("T1:{0},T2:{1}", T1Value, T2Value);
        }

    }
}
