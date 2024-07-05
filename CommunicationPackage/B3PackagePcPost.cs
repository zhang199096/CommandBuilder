using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandBuilder.CommunicationPackage
{
    public class B3PackagePcPost
    {
        public UInt32 package_header;//包头
        public UInt32 length;//长度
        public UInt32 identifier;//识别码
        public UInt32 ip;      //ip地址
        public UInt32 protocol_type;//协议类型
        public UInt32 cmd_code;//命令码
        public UInt32 module_addr;//模块地址
        public UInt32 info_code1;//信息码
        public UInt32 info_code2;//信息码
        public UInt32 info_code3;//信息码
        public UInt32 info_code4;//信息码
        public UInt32 info_code5;//信息码
        public UInt32 info_code6;//信息码
        public UInt32 info_code7;//信息码
        public UInt32 info_code8;//信息码
        public UInt32 info_code9;//信息码
        public UInt32 info_code10;//信息码
        public UInt32 info_code11;//信息码
        public UInt32 info_code12;//信息码
        public UInt32 info_code13;//信息码
        public UInt32 info_code14;//信息码
        public UInt32 info_code15;//信息码
        public UInt32 info_code16;//信息码
        public UInt32 info_code17;//信息码
        public UInt32 info_code18;//信息码
        public UInt32 info_code19;//信息码
        public UInt32 info_code20;//信息码
        public UInt32 info_code21;//信息码
        public UInt32 info_code22;//信息码
        public UInt32 info_code23;//信息码
        public UInt32 info_code24;//信息码
        public UInt32 info_code25;//信息码
        public UInt32 info_code26;//信息码
        public UInt32 info_code27; //轴16实际运行速度Feedrate.Axis2
        public UInt32 info_code28; //轴17实际运行速度Feedrate.Axis2
        public UInt32 info_code29; //轴18实际运行速度
        public UInt32 info_code30; //轴19实际运行速度
        public UInt32 info_code31; //轴20实际运行速度
        public UInt32 info_code32; //轴21实际运行速度
        public UInt32 info_code33; //轴22实际运行速度
        public UInt32 info_code34; //轴23实际运行速度
        public UInt32 info_code35; //轴24实际运行速度
        public UInt32 info_code36; //轴25实际运行速度
        public UInt32 info_code37; //轴1绝对坐标ABS_COORDINATE_Clone.Axis1
        public UInt32 info_code38; //轴2绝对坐标
        public UInt32 info_code39; //轴3绝对坐标
        public UInt32 info_code40; //轴4绝对坐标
        public UInt32 info_code41; //轴5绝对坐标
        public UInt32 info_code42; //轴6绝对坐标
        public UInt32 info_code43; //轴7绝对坐标
        public UInt32 info_code44; //轴8绝对坐标
        public UInt32 info_code45; //轴9绝对坐标
        public UInt32 info_code46; //轴10绝对坐标
        public UInt32 info_code47; //轴11绝对坐标
        public UInt32 info_code48; //轴12绝对坐标
        public UInt32 info_code49; //轴13绝对坐标
        public UInt32 info_code50; //轴14绝对坐标
        public UInt32 info_code51; //轴15绝对坐标
        public UInt32 info_code52; //轴16绝对坐标
        public UInt32 info_code53; //轴17绝对坐标
        public UInt32 info_code54; //轴18绝对坐标
        public UInt32 info_code55; //轴19绝对坐标
        public UInt32 info_code56; //轴20绝对坐标
        public UInt32 info_code57; //轴21绝对坐标
        public UInt32 info_code58; //轴22绝对坐标
        public UInt32 info_code59; //轴23绝对坐标
        public UInt32 info_code60; //轴24绝对坐标
        public UInt32 info_code61; //轴25绝对坐标ABS_COORDINATE_Clone.Axis25
        public UInt32 info_code62; //轴1机床坐标MACH_COORDINATE_Clone.Axis1
        public UInt32 info_code63; //轴2机床坐标
        public UInt32 info_code64; //轴3机床坐标
        public UInt32 crc;//CRC校验码
        public UInt32 send_num;//通讯总条数
        public UInt32 package_footer;//命令码
    }
}
