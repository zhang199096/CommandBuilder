using CommandBuilder.helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandBuilder.Command
{
    /// <summary>
    /// 功能码1：空代码
    /// 格式
    /// </summary>
    public class G100 : ICmd
    {
        private string name = "G100";
        public List<G100Param> paramList = new List<G100Param>();

        //解析参数,因为入栈,所以倒序输出
        public void ParserParam(string cmdstring)
        {
            paramList.Clear();
            // Stack<G04Param> st = new Stack<G04Param>();//参数栈

            G100Param p01 = null;
            string number = "";//数值
            foreach (var item in cmdstring)//逐个字符解析参数
            {
                if (item == 'M')
                {
                    p01 = new G100Param() { MValue = 0 };
                    //st.Push(p01);//入栈
                    //statu = "H";
                }
                else if (CharHelper.IsNumeric(item.ToString()) || item == '.' || item == '-')//输入是数字,就累加
                {
                    number += item.ToString();
                }
                else
                    continue;
            }
            if (number != "")//最后一定是F, 没有结束标志,所以直接赋值了
                p01.MValue = int.Parse(number);

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
    /// G01的参数格式
    /// </summary>
    public class G100Param
    {
        public int MValue { get; set; }
        public override string ToString()
        {
            return string.Format("M:{0}", MValue);
        }

    }
}
