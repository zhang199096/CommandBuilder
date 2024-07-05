using CommandBuilder.Command;
using System;

namespace CommandBuilder
{
    //将字符串转成命令
    public class Parser
    {
        public ICmd Parse(CommandItem row)
        {
            ICmd ic = ParseParam(row.Cmd, row.Param);
            return ic;
        }

        private ICmd ParseParam(string cmd, string param)
        {
            if(cmd.StartsWith("@"))//安全点以@开头,用于给用户选择运行位置
            {
                GMark gm = new GMark(cmd);
                return gm;
            }
            switch (cmd)
            {
                case "G01": //G01, 用G99是针对单个命令来测试的
                    G01 g01 = new G01();
                    g01.ParserParam(param);
                    return g01;
                case "G00":
                    G01 g00 = new G01("G00");
                    g00.ParserParam(param);
                    return g00;
                case "G04":
                    G04 g04 = new G04();
                    g04.ParserParam(param);
                    return g04;
                case "G100":
                    var g100 = new G100();
                    g100.ParserParam(param);
                    return g100;
                case "G101":
                    var g101 = new G101();
                    g101.ParserParam(param);
                    return g101;
                case "G102":
                    G102 g102 = new G102();
                    g102.ParserParam(param);
                    return g102;
                case "G103":
                    G103 g103 = new G103();
                    g103.ParserParam(param);
                    return g103;
                case "G104":
                    var g104 = new G104();
                    g104.ParserParam(param);
                    return g104;
                case "G105":
                    var g105 = new G105();
                    g105.ParserParam(param);
                    return g105;
                case "G106":
                    var g106 = new G106();
                    g106.ParserParam(param);
                    return g106;
                case "G107":
                    var g107 = new G107();
                    g107.ParserParam(param);
                    return g107;
                case "G108":
                    G108 g108 = new G108();
                    g108.ParserParam(param);
                    return g108;
                case "G109":
                    G109 g109 = new G109();
                    g109.ParserParam(param);
                    return g109;
                case "G110":
                    G110 g110 = new G110();
                    g110.ParserParam(param);
                    return g110;
                case "G111":
                    G111 g111 = new G111();
                    g111.ParserParam(param);
                    return g111;
                case "G112":
                    G112 g112 = new G112();
                    g112.ParserParam(param);
                    return g112;
                case "G113":
                    var g113 = new G113();
                    g113.ParserParam(param);
                    return g113;
                case "G114":
                    var g114 = new G114();
                    g114.ParserParam(param);
                    return g114;
                case "G115":
                    var g115 = new G115();
                    g115.ParserParam(param);
                    return g115;
                case "G116":
                    var g116 = new G116();
                    g116.ParserParam(param);
                    return g116;
                case "G117":
                    var g117 = new G117();
                    g117.ParserParam(param);
                    return g117;
                case "G118":
                    var g118 = new G118();
                    g118.ParserParam(param);
                    return g118;
                case "G119":
                    var g119 = new G119();
                    g119.ParserParam(param);
                    return g119;
                case "G201":
                    var g201 = new G201();
                    g201.ParserParam(param);
                    return g201;
                default:
                    Console.WriteLine("无法识别的命令:" + cmd);
                    break;
            }
            return null;
        }

    }

}
