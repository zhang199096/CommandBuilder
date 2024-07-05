using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandBuilder.Command
{
    /// <summary>
    /// 功能码2：LED控制
    /// 格式
    /// </summary>
    public class G101 : ICmd
    {
        private string name = "G101";
        public List<G101Param2> paramList = new List<G101Param2>();

        //解析参数,因为入栈,所以倒序输出
        public void ParserParam(string cmdstring)
        {
            paramList.Clear();

            G101Param2 p01 = null;
            string number = "";//数值
            foreach (var item in cmdstring)//逐个字符解析参数
            {
                if (item == 'T')
                {
                    p01 = new G101Param2() { T1Value = 0, T2Value=0 };
                }
                else if (item == ',')
                {
                    if (number != "")//上次解析的是A,今次是E,那么把number赋值给A
                        p01.T1Value = int.Parse(number);
                    number = "";
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
                int tmp = int.Parse(number);
                p01.T2Value += tmp;
            }

            paramList.Add(p01);
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
    /// G101的参数格式
    /// </summary>
    public class G101Param2
    {
        public int T1Value { get; set; }
        public int T2Value { get; set; }
        public override string ToString()
        {
            return string.Format("T1:{0},T2:{1}", T1Value, T2Value);
        }

    }
}
