using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandBuilder.CommunicationPackage
{
    /// <summary>
    /// DSP－> PC 通用数据通讯返回包协议0xb0
    /// DSP_com_BackCMD_b0()
    /// </summary>
    public class B0PackagePCGet
    {
        public UInt32 package_header;//包头
        public UInt32 length;//长度
        public UInt32 identifier;//识别码
        public UInt32 ip;      //ip地址
        public UInt32 protocol_type;//协议类型
        public UInt32 cmd_code;//命令码
        public UInt32 module_addr;//模块地址
        //信息码 1-124
        public UInt32 info_code1; //版本年
        public UInt32 info_code2; //版本月
        public UInt32 info_code3; //版本日
        public UInt32 info_code4; //版本子码
        public UInt32 info_code5; //机器运行状态: 0通常状态, 1:std状态 (b3包), 2:spc状态(b4包)
        public UInt32 info_code6; //手动模式DSP实际运行条号
        public UInt32 info_code7; //手动模式DSP 实际运行条号
        public UInt32 info_code8; //自动模式DSP 实际接收条号
        public UInt32 info_code9; //手动模式DSP 实际接收条号
        public UInt32 info_code10; //Reserved
        public UInt32 info_code11; //Reserved
        public UInt32 info_code12; //轴1实际运行速度Feedrate.Axis1 
        public UInt32 info_code13; //轴2实际运行速度Feedrate.Axis2
        public UInt32 info_code14; //轴3实际运行速度Feedrate.Axis2
        public UInt32 info_code15; //轴4实际运行速度Feedrate.Axis2
        public UInt32 info_code16; //轴5实际运行速度Feedrate.Axis2
        public UInt32 info_code17; //轴6实际运行速度Feedrate.Axis2
        public UInt32 info_code18; //轴7实际运行速度Feedrate.Axis2
        public UInt32 info_code19; //轴8实际运行速度Feedrate.Axis2
        public UInt32 info_code20; //轴9实际运行速度Feedrate.Axis2
        public UInt32 info_code21; //轴10实际运行速度Feedrate.Axis2
        public UInt32 info_code22; //轴11实际运行速度Feedrate.Axis2
        public UInt32 info_code23; //轴12实际运行速度Feedrate.Axis2
        public UInt32 info_code24; //轴13实际运行速度Feedrate.Axis2
        public UInt32 info_code25; //轴14实际运行速度Feedrate.Axis2
        public UInt32 info_code26; //轴15实际运行速度Feedrate.Axis2
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
        public UInt32 info_code65; //轴4机床坐标
        public UInt32 info_code66; //轴5机床坐标
        public UInt32 info_code67; //轴6机床坐标
        public UInt32 info_code68; //轴7机床坐标
        public UInt32 info_code69; //轴8机床坐标
        public UInt32 info_code70; //轴9机床坐标
        public UInt32 info_code71; //轴10机床坐标
        public UInt32 info_code72; //轴11机床坐标
        public UInt32 info_code73; //轴12机床坐标
        public UInt32 info_code74; //轴13机床坐标
        public UInt32 info_code75; //轴14机床坐标
        public UInt32 info_code76; //轴15机床坐标
        public UInt32 info_code77; //轴16机床坐标
        public UInt32 info_code78; //轴17机床坐标
        public UInt32 info_code79; //轴18机床坐标
        public UInt32 info_code80; //轴19机床坐标
        public UInt32 info_code81; //轴20机床坐标
        public UInt32 info_code82; //轴21机床坐标
        public UInt32 info_code83; //轴22机床坐标
        public UInt32 info_code84; //轴23机床坐标
        public UInt32 info_code85; //轴24机床坐标
     public  UInt32 info_code86; //轴25机床坐标MACH_COORDINATE_Clone.Axis25
     public  UInt32 info_code87; //B4 包缓冲区未满，可以发送B4 包CanSendCodeSign_B4
     public  UInt32 info_code88; //B3 包缓冲区未满，可以发送B3 包CanSendCodeSign_B3
     public  UInt32 info_code89; //DSP 主错误标志位Error_main_Clone.MainErrorSign；1 有效
     public  UInt32 info_code90; //DSP 已获取稳定坐标标志GetPositionSign；1 有效
     public  UInt32 info_code91; //DSP 最后一条代码走完标志LastCodeOverSign；1 有效
     public  UInt32 info_code92; //上位向DSP 参数写入完成标志ParameterWriteCompleteSign；1 有效
     public  UInt32 info_code93; //正向硬限位标志positive hard limit, Bit0=>Axis1 positive hard limit；1 有效
     public  UInt32 info_code94; //负向硬限位标志negative hard limit, Bit0=>Axis1 negative hard limit；1 有效
     public  UInt32 info_code95; //定位完成标志position complete signal,Bit0=>Axis1 position complete signal；1 有效
     public  UInt32 info_code96; //原点复归完成标志Home complete signal,Bit0=>Axis1 Home complete signal；1 有效
     public  UInt32 info_code97; //DSP 报警信息（89：DSP 主错误标志位有效时）,Bit0=> DSP 芯片内部RAM 检验出错；1 有效
     public  UInt32 info_code98; //伺服报警信息Servo Alarm signal , Bit0=>Axis1 servo alarm signal；1 有效
     public  UInt32 info_code99; //编码器/光栅尺报警信息Encoder Alarm signal , Bit0=>Axis1 Encoder alarm signal；1 有效
     public  UInt32 info_code100; //输入口信号GPIN1 signal , Bit0=> GPIN1-in1 signal；0 输入有效，1 输入无效
     public  UInt32 info_code101; //输入口信号GPIN2 signal , Bit0=> GPIN2-in1 signal；0 输入有效，1 输入无效
     public  UInt32 info_code102; //输出口信号GPOUT1 signal , Bit0=> GPOUT1 - out 1 signal；0 输出有效，1 输出无效
     public  UInt32 info_code103; //输出口信号GPOUT2 signal（reserved） , Bit0=> GPOUT2 - out 1 signal
     public  UInt32 info_code104; //主控制命令1 信息MainCmd1 information（测试用for test）, Bit0=> MainCmd1 comand1 information；1 有效
     public  UInt32 info_code105; //主控制命令2 信息MainCmd2 information（测试用for test）,Bit0=> MainCmd2 comand1 information；1 有
     public  UInt32 info_code106; //主状态1 信息MainStatus1information（测试用for test）,Bit0=> MainStatus1 Status1 information；1 有效
     public  UInt32 info_code107; //主状态2 信息MainStatus2 information（测试用for test）,Bit0=> MainStatus2 Status1 information；1 有效
     public  UInt32 info_code108; //正向软限位标志positive soft limit , Bit0=>Axis1 positive soft limit；1 有效
     public  UInt32 info_code109; //负向软限位标志negative soft limit , Bit0=>Axis1 negative soft limit；1 有效
     public  UInt32 info_code110; //DSP 轨迹超程报警TrackRunOver  ,  Bit0=>Axis1 TrackRunOver alarm signal；1 有效
     public  UInt32 info_code111; //DSP 插补量过大报警InterpolationOver ,Bit0=>Axis1 InterpolationOver alarm signal；1 有效
     public  UInt32 info_code112; //轴非线性补偿数据出错报警CompensationDataCheckError ,Bit0=>Axis1CompensationDataCheckError signal；1 有效
     public  UInt32 info_code113; //reserved
     public  UInt32 info_code114; //reserved
     public  UInt32 info_code115; //reserved
     public  UInt32 info_code116; //reserved
     public  UInt32 info_code117; //reserved
     public  UInt32 info_code118; //reserved
     public  UInt32 info_code119; //reserved
     public  UInt32 info_code120; //reserved
     public  UInt32 info_code121; //reserved
     public  UInt32 info_code122; //reserved
     public  UInt32 info_code123; //reserved
     public  UInt32 info_code124; //crc32

     public  UInt32 crc;//CRC校验码
     public  UInt32 send_num;//通讯总条数
     public  UInt32 package_footer;//命令码
    }
}
