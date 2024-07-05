using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandBuilder.CommunicationPackage
{
    //class B1PackagePcPost
    //{
    //    UInt32 package_header;//包头
    //    UInt32 length;//长度
    //    UInt32 identifier;//识别码
    //    UInt32 ip;      //ip地址
    //    UInt32 protocol_type;//协议类型
    //    UInt32 cmd_code;//命令码
    //    UInt32 module_addr;//模块地址
    //    UInt32 info_code1;//运动控制插补周期单位: 0.1ms //设定范围：（5-20）；初始值:10
    //    UInt32 info_code2;//运动轴从站总数 单位：个 //设定范围：（0-25）；初始值:0
    //    UInt32 info_code3;//位置偏差过大报警设定值单位：mm //设定范围：（0-100）；初始值:10
    //    UInt32 info_code4;// 轴1 加速度1 单位：mm/s^2//设定范围：（100-10000）；初始值: 4000
    //    UInt32 info_code5; // 轴2
    //    UInt32 info_code6; // 轴3
    //    UInt32 info_code7; // 轴4
    //    UInt32 info_code8; // 轴5
    //    UInt32 info_code9; // 轴6
    //    UInt32 info_code10;// 轴7
    //    UInt32 info_code11;// 轴8
    //    UInt32 info_code12;// 轴9
    //    UInt32 info_code13;// 轴10
    //    UInt32 info_code14;// 轴11
    //    UInt32 info_code15;// 轴12
    //    UInt32 info_code16;// 轴13
    //    UInt32 info_code17;// 轴14
    //    UInt32 info_code18;// 轴15
    //    UInt32 info_code19;// 轴16
    //    UInt32 info_code20;// 轴17
    //    UInt32 info_code21;// 轴18
    //    UInt32 info_code22;// 轴19
    //    UInt32 info_code23;// 轴20
    //    UInt32 info_code24;// 轴21  
    //    UInt32 info_code25;// 轴22
    //    UInt32 info_code26;// 轴23
    //    UInt32 info_code27;// 轴24
    //    UInt32 info_code28;// 轴25
    //    UInt32 info_code29;//轴1 减速度单位：mm/s^2 //设定范围：（100-10000）；初始值: 4000
    //    UInt32 info_code30;// 轴2
    //    UInt32 info_code31;// 轴3
    //    UInt32 info_code32;// 轴4
    //    UInt32 info_code33;// 轴5
    //    UInt32 info_code34;// 轴6
    //    UInt32 info_code35;// 轴7
    //    UInt32 info_code36;// 轴8
    //    UInt32 info_code37;// 轴9
    //    UInt32 info_code38;// 轴10
    //    UInt32 info_code39;// 轴11
    //    UInt32 info_code40;// 轴12
    //    UInt32 info_code41;// 轴13
    //    UInt32 info_code42;// 轴14
    //    UInt32 info_code43;// 轴15
    //    UInt32 info_code44;// 轴16
    //    UInt32 info_code45;// 轴17
    //    UInt32 info_code46;// 轴18
    //    UInt32 info_code47;// 轴19
    //    UInt32 info_code48;// 轴20
    //    UInt32 info_code49;// 轴21
    //    UInt32 info_code50;// 轴22
    //    UInt32 info_code51;// 轴23
    //    UInt32 info_code52;// 轴24
    //    UInt32 info_code53;// 轴25
    //    UInt32 info_code54;//运动轴最小速度单位:0.1mm/s //设定范围：（5-100）；初始值:30
    //    UInt32 info_code55;//运动轴精准最小速度单位:0.1mm/s //设定范围：（5-50）；初始值:10
    //    UInt32 info_code56;//运动轴转向过程时间常数单位:0.1s //设定范围：（5-100）；初始值:30
    //    UInt32 info_code57; //运动轴精准转向过程时间常数单位:0.1s //设定范围：（5-50）；初始值:10
    //    UInt32 info_code58;//各轴最小输入单位单位 nm //设定范围：（50~10000）；初始值:1000
    //    UInt32 info_code59;//各轴最小输出单位单位 nm/pulse//设定范围：（50~10000）；初始值:1000
    //    UInt32 info_code60;//快速进给速度1 单位mm/s //设定范围：（10~2000）；初始值:100
    //    UInt32 info_code61;//快速进给速度2 单位mm/s //设定范围：（10~2000）；初始值:100
    //    UInt32 info_code62; //工进进给速度1 单位mm/s //设定范围：（1~1000）；初始值:10
    //    UInt32 info_code63;//工进进给速度2 单位mm/s //设定范围：（1~1000）；初始值:10
    //    UInt32 info_code64; //原点复归速度1 单位mm/s //设定范围：（1~50）；初始值:20
    //    UInt32 info_code65; //原点复归回退速度1 单位mm/s //设定范围：（1~5）；初始值:1
    //    UInt32 info_code66;//原点复归回退距离1 单位mm //设定范围：（5~30）；初始值:15
    //    UInt32 info_code67;//原点复归速度2 单位mm/s // 设定范围：（ 1~50 ）； 初始值:20
    //    UInt32 info_code68; //原点复归回退速度2 单位mm/s //设定范围：（1~5）；初始值:1
    //    UInt32 info_code69;//原点复归回退距离1 单位mm //设定范围：（5~30）；初始值:15
    //    UInt32 info_code70; //轴1 原点复归停止位置校验标志 0：不校验；1：校验
    //    UInt32 info_code71; // 轴2
    //    UInt32 info_code72; // 轴3
    //    UInt32 info_code73; // 轴4
    //    UInt32 info_code74; // 轴5
    //    UInt32 info_code75; // 轴6
    //    UInt32 info_code76; // 轴7
    //    UInt32 info_code77; // 轴8
    //    UInt32 info_code78; // 轴9
    //    UInt32 info_code79; // 轴10
    //    UInt32 info_code80; // 轴11
    //    UInt32 info_code81; // 轴12
    //    UInt32 info_code82; // 轴13
    //    UInt32 info_code83; // 轴14
    //    UInt32 info_code84; // 轴15
    //    UInt32 info_code85; // 轴16
    //    UInt32 info_code86; // 轴17
    //    UInt32 info_code87; // 轴18
    //    UInt32 info_code88; // 轴19
    //    UInt32 info_code89; // 轴20
    //    UInt32 info_code90; // 轴21
    //    UInt32 info_code91; // 轴22
    //    UInt32 info_code92; // 轴23
    //    UInt32 info_code93; // 轴24
    //    UInt32 info_code94; // 轴25
    //    UInt32 info_code95; //轴1 原点复归方向设置 0：负方向；1：正方向 //设定范围：（0,1）；初始值:0
    //    UInt32 info_code96; // 轴2
    //    UInt32 info_code97; // 轴3
    //    UInt32 info_code98; // 轴4
    //    UInt32 info_code99; // 轴5
    //    UInt32 info_code100;// 轴6
    //    UInt32 info_code101;// 轴7
    //    UInt32 info_code102;// 轴8
    //    UInt32 info_code103;// 轴9
    //    UInt32 info_code104;// 轴10
    //    UInt32 info_code105;// 轴11
    //    UInt32 info_code106;// 轴12
    //    UInt32 info_code107;// 轴13
    //    UInt32 info_code108;// 轴14
    //    UInt32 info_code109;// 轴15
    //    UInt32 info_code110;// 轴16
    //    UInt32 info_code111;// 轴17
    //    UInt32 info_code112;// 轴18
    //    UInt32 info_code113;// 轴19
    //    UInt32 info_code114;// 轴20
    //    UInt32 info_code115;// 轴21
    //    UInt32 info_code116;// 轴22
    //    UInt32 info_code117;// 轴23
    //    UInt32 info_code118;// 轴24
    //    UInt32 info_code119;// 轴25
    //    UInt32 info_code120;//轴1 编码器/光栅尺检测标志 0：不校验；1：校验
    //    UInt32 info_code121;// 轴2
    //    UInt32 info_code122;// 轴3
    //    UInt32 info_code123;// 轴4
    //    UInt32 info_code124;// 轴5
    //    UInt32 info_code125;// 轴6
    //    UInt32 info_code126;// 轴7
    //    UInt32 info_code127;// 轴8
    //    UInt32 info_code128;// 轴9
    //    UInt32 info_code129;// 轴10
    //    UInt32 info_code130;// 轴11
    //    UInt32 info_code131;// 轴12
    //    UInt32 info_code132;// 轴13
    //    UInt32 info_code133;// 轴14
    //    UInt32 info_code134;// 轴15
    //    UInt32 info_code135;// 轴16
    //    UInt32 info_code136;// 轴17
    //    UInt32 info_code137;// 轴18
    //    UInt32 info_code138;// 轴19
    //    UInt32 info_code139;// 轴20
    //    UInt32 info_code140;// 轴21
    //    UInt32 info_code141;// 轴22
    //    UInt32 info_code142;// 轴23
    //    UInt32 info_code143;// 轴24
    //    UInt32 info_code144;// 轴25
    //    UInt32 info_code145;// 轴1 进给方向 0：不切换；1：切换 //设定范围：（0,1）；初始值:0
    //    UInt32 info_code146;// 轴2
    //    UInt32 info_code147;// 轴3
    //    UInt32 info_code148;// 轴4
    //    UInt32 info_code149;// 轴5
    //    UInt32 info_code150;// 轴6
    //    UInt32 info_code151;// 轴7
    //    UInt32 info_code152;// 轴8
    //    UInt32 info_code153;// 轴9
    //    UInt32 info_code154;// 轴10
    //    UInt32 info_code155;// 轴11
    //    UInt32 info_code156;// 轴12
    //    UInt32 info_code157;// 轴13
    //    UInt32 info_code158;// 轴14
    //    UInt32 info_code159;// 轴15
    //    UInt32 info_code160;// 轴16
    //    UInt32 info_code161;// 轴17
    //    UInt32 info_code162;// 轴18
    //    UInt32 info_code163;// 轴19
    //    UInt32 info_code164;// 轴20
    //    UInt32 info_code165;// 轴21
    //    UInt32 info_code166;// 轴22
    //    UInt32 info_code167;// 轴23
    //    UInt32 info_code168;// 轴24
    //    UInt32 info_code169;// 轴25
    //    UInt32 info_code170;//轴1 编码器/光栅尺读数方向 0：不切换；1：切换//设定范围：（0,1）；初始值:0
    //    UInt32 info_code171;// 轴2
    //    UInt32 info_code172;// 轴3
    //    UInt32 info_code173;// 轴4
    //    UInt32 info_code174;// 轴5
    //    UInt32 info_code175;// 轴6
    //    UInt32 info_code176;// 轴7
    //    UInt32 info_code177;// 轴8
    //    UInt32 info_code178;// 轴9
    //    UInt32 info_code179;// 轴10
    //    UInt32 info_code180;// 轴11
    //    UInt32 info_code181;// 轴12
    //    UInt32 info_code182;// 轴13
    //    UInt32 info_code183;// 轴14
    //    UInt32 info_code184;// 轴15
    //    UInt32 info_code185;// 轴16
    //    UInt32 info_code186;// 轴17
    //    UInt32 info_code187;// 轴18
    //    UInt32 info_code188;// 轴19
    //    UInt32 info_code189;// 轴20
    //    UInt32 info_code190;// 轴21
    //    UInt32 info_code191;// 轴22
    //    UInt32 info_code192;// 轴23
    //    UInt32 info_code193;// 轴24
    //    UInt32 info_code194;// 轴25
    //    UInt32 info_code195;//轴1 编码器分辨率单位：nm//设定范围：（50,20000）；初始值:1000
    //    UInt32 info_code196;// 轴2
    //    UInt32 info_code197;// 轴3
    //    UInt32 info_code198;// 轴4
    //    UInt32 info_code199;// 轴5
    //    UInt32 info_code200;// 轴6
    //    UInt32 info_code201;// 轴7
    //    UInt32 info_code202;// 轴8
    //    UInt32 info_code203;// 轴9
    //    UInt32 info_code204;// 轴10
    //    UInt32 info_code205;// 轴11
    //    UInt32 info_code206;// 轴12
    //    UInt32 info_code207;// 轴13
    //    UInt32 info_code208;// 轴14
    //    UInt32 info_code209;// 轴15
    //    UInt32 info_code210;// 轴16
    //    UInt32 info_code211;// 轴17
    //    UInt32 info_code212;// 轴18
    //    UInt32 info_code213;// 轴19
    //    UInt32 info_code214;// 轴20
    //    UInt32 info_code215;// 轴21
    //    UInt32 info_code216;// 轴22
    //    UInt32 info_code217;// 轴23
    //    UInt32 info_code218;// 轴24
    //    UInt32 info_code219;// 轴25
    //    UInt32 info_code220;// 轴1 绝对坐标原点单位：um //设定范围：（-1000000,1000000）；初始值:0
    //    UInt32 info_code221;// 轴2
    //    UInt32 info_code222;// 轴3
    //    UInt32 info_code223;// 轴4
    //    UInt32 info_code224;// 轴5
    //    UInt32 info_code225;// 轴6
    //    UInt32 info_code226;// 轴7
    //    UInt32 info_code227;// 轴8
    //    UInt32 info_code228;// 轴9
    //    UInt32 info_code229;// 轴10
    //    UInt32 info_code230;// 轴11
    //    UInt32 info_code231;// 轴12
    //    UInt32 info_code232;// 轴13
    //    UInt32 info_code233;// 轴14
    //    UInt32 info_code234;// 轴15
    //    UInt32 info_code235;// 轴16
    //    UInt32 info_code236;// 轴17
    //    UInt32 info_code237;// 轴18
    //    UInt32 info_code238;// 轴19
    //    UInt32 info_code239;// 轴20
    //    UInt32 info_code240;// 轴21
    //    UInt32 info_code241;// 轴22
    //    UInt32 info_code242;// 轴23
    //    UInt32 info_code243;// 轴24
    //    UInt32 info_code244;// 轴25
    //    UInt32 info_code245;// 轴1 偏移坐标单位：um //设定范围：（-1000000,1000000）；初始值:0
    //    UInt32 info_code246;// 轴2
    //    UInt32 info_code247;// 轴3
    //    UInt32 info_code248;// 轴4
    //    UInt32 info_code249;// 轴5
    //    UInt32 info_code250;// 轴6
    //    UInt32 info_code251;// 轴7
    //    UInt32 info_code252;// 轴8
    //    UInt32 info_code253;// 轴9
    //    UInt32 info_code254;// 轴10
    //    UInt32 info_code255;// 轴11
    //    UInt32 info_code256;// 轴12
    //    UInt32 info_code257;// 轴13
    //    UInt32 info_code258;// 轴14
    //    UInt32 info_code259;// 轴15
    //    UInt32 info_code260;// 轴16
    //    UInt32 info_code261;// 轴17
    //    UInt32 info_code262;// 轴18
    //    UInt32 info_code263;// 轴19
    //    UInt32 info_code264;// 轴20
    //    UInt32 info_code265;// 轴21
    //    UInt32 info_code266;// 轴22
    //    UInt32 info_code267;// 轴23
    //    UInt32 info_code268;// 轴24
    //    UInt32 info_code269;// 轴25
    //    UInt32 info_code270;// 轴1 安全位置坐标单位：um //设定范围：（-1000000,1000000）；初始值:0
    //    UInt32 info_code271;// 轴2
    //    UInt32 info_code272;// 轴3
    //    UInt32 info_code273;// 轴4
    //    UInt32 info_code274;// 轴5
    //    UInt32 info_code275;// 轴6
    //    UInt32 info_code276;// 轴7
    //    UInt32 info_code277;// 轴8
    //    UInt32 info_code278;// 轴9
    //    UInt32 info_code279;// 轴10
    //    UInt32 info_code280;// 轴11
    //    UInt32 info_code281;// 轴12
    //    UInt32 info_code282;// 轴13
    //    UInt32 info_code283;// 轴14
    //    UInt32 info_code284;// 轴15
    //    UInt32 info_code285;// 轴16
    //    UInt32 info_code286;// 轴17
    //    UInt32 info_code287;// 轴18
    //    UInt32 info_code288;// 轴19
    //    UInt32 info_code289;// 轴20
    //    UInt32 info_code290;// 轴21
    //    UInt32 info_code291;// 轴22
    //    UInt32 info_code292;// 轴23
    //    UInt32 info_code293;// 轴24
    //    UInt32 info_code294;// 轴25
    //    UInt32 info_code295;// 轴1 回退坐标单位：um //设定范围：（-1000000,1000000）；初始值:0
    //    UInt32 info_code296;// 轴2
    //    UInt32 info_code297;// 轴3
    //    UInt32 info_code298;// 轴4
    //    UInt32 info_code299;// 轴5
    //    UInt32 info_code300;// 轴6
    //    UInt32 info_code301;// 轴7
    //    UInt32 info_code302;// 轴8
    //    UInt32 info_code303;// 轴9
    //    UInt32 info_code304;// 轴10
    //    UInt32 info_code305;// 轴11
    //    UInt32 info_code306;// 轴12
    //    UInt32 info_code307;// 轴13
    //    UInt32 info_code308;// 轴14
    //    UInt32 info_code309;// 轴15
    //    UInt32 info_code310;// 轴16
    //    UInt32 info_code311;// 轴17
    //    UInt32 info_code312;// 轴18
    //    UInt32 info_code313;// 轴19
    //    UInt32 info_code314;// 轴20
    //    UInt32 info_code315;// 轴21
    //    UInt32 info_code316;// 轴22
    //    UInt32 info_code317;// 轴23
    //    UInt32 info_code318;// 轴24
    //    UInt32 info_code319;// 轴25
    //    UInt32 info_code320;// 轴1 定位坐标1 单位：um //设定范围：（-1000000,1000000）；初始值:0
    //    UInt32 info_code321;// 轴2
    //    UInt32 info_code322;// 轴3
    //    UInt32 info_code323;// 轴4
    //    UInt32 info_code324;// 轴5
    //    UInt32 info_code325;// 轴6
    //    UInt32 info_code326;// 轴7
    //    UInt32 info_code327;// 轴8
    //    UInt32 info_code328;// 轴9
    //    UInt32 info_code329;// 轴10
    //    UInt32 info_code330;// 轴11
    //    UInt32 info_code331;// 轴12
    //    UInt32 info_code332;// 轴13
    //    UInt32 info_code333;// 轴14
    //    UInt32 info_code334;// 轴15
    //    UInt32 info_code335;// 轴16
    //    UInt32 info_code336;// 轴17
    //    UInt32 info_code337;// 轴18
    //    UInt32 info_code338;// 轴19
    //    UInt32 info_code339;// 轴20
    //    UInt32 info_code340;// 轴21
    //    UInt32 info_code341;// 轴22
    //    UInt32 info_code342;// 轴23
    //    UInt32 info_code343;// 轴24
    //    UInt32 info_code344;// 轴25
    //    UInt32 info_code345;// 轴1 定位坐标2 单位：um //设定范围：（-1000000,1000000）；初始值:0
    //    UInt32 info_code346;// 轴2
    //    UInt32 info_code347;// 轴3
    //    UInt32 info_code348;// 轴4
    //    UInt32 info_code349;// 轴5
    //    UInt32 info_code350;// 轴6
    //    UInt32 info_code351;// 轴7
    //    UInt32 info_code352;// 轴8
    //    UInt32 info_code353;// 轴9
    //    UInt32 info_code354;// 轴10
    //    UInt32 info_code355;// 轴11
    //    UInt32 info_code356;// 轴12
    //    UInt32 info_code357;// 轴13
    //    UInt32 info_code358;// 轴14
    //    UInt32 info_code359;// 轴15
    //    UInt32 info_code360;// 轴16
    //    UInt32 info_code361;// 轴17
    //    UInt32 info_code362;// 轴18
    //    UInt32 info_code363;// 轴19
    //    UInt32 info_code364;// 轴20
    //    UInt32 info_code365;// 轴21
    //    UInt32 info_code366;// 轴22
    //    UInt32 info_code367;// 轴23
    //    UInt32 info_code368;// 轴24
    //    UInt32 info_code369;// 轴25
    //    UInt32 info_code370;//轴1 最大进给速度单位：mm/s//设定范围：（10,2000）；初始值:100
    //    UInt32 info_code371;// 轴2
    //    UInt32 info_code372;// 轴3
    //    UInt32 info_code373;// 轴4
    //    UInt32 info_code374;// 轴5
    //    UInt32 info_code375;// 轴6
    //    UInt32 info_code376;// 轴7
    //    UInt32 info_code377;// 轴8
    //    UInt32 info_code378;// 轴9
    //    UInt32 info_code379;// 轴10
    //    UInt32 info_code380;// 轴11
    //    UInt32 info_code381;// 轴12
    //    UInt32 info_code382;// 轴13
    //    UInt32 info_code383;// 轴14
    //    UInt32 info_code384;// 轴15
    //    UInt32 info_code385;// 轴16
    //    UInt32 info_code386;// 轴17
    //    UInt32 info_code387;// 轴18
    //    UInt32 info_code388;// 轴19
    //    UInt32 info_code389;// 轴20
    //    UInt32 info_code390;// 轴21
    //    UInt32 info_code391;// 轴22
    //    UInt32 info_code392;// 轴23
    //    UInt32 info_code393;// 轴24
    //    UInt32 info_code394;// 轴25
    //    UInt32 info_code395;//轴1 原点复归停止位置单位：um //设定范围：（-30000,30000）；初始值:0
    //    UInt32 info_code396;// 轴2
    //    UInt32 info_code397;// 轴3
    //    UInt32 info_code398;// 轴4
    //    UInt32 info_code399;// 轴5
    //    UInt32 info_code400;// 轴6
    //    UInt32 info_code401;// 轴7
    //    UInt32 info_code402;// 轴8
    //    UInt32 info_code403;// 轴9
    //    UInt32 info_code404;// 轴10
    //    UInt32 info_code405;// 轴11
    //    UInt32 info_code406;// 轴12
    //    UInt32 info_code407;// 轴13
    //    UInt32 info_code408;// 轴14
    //    UInt32 info_code409;// 轴15
    //    UInt32 info_code410;// 轴16
    //    UInt32 info_code411;// 轴17
    //    UInt32 info_code412;// 轴18
    //    UInt32 info_code413;// 轴19
    //    UInt32 info_code414;// 轴20
    //    UInt32 info_code415;// 轴21
    //    UInt32 info_code416;// 轴22
    //    UInt32 info_code417;// 轴23
    //    UInt32 info_code418;// 轴24
    //    UInt32 info_code419;// 轴25
    //    UInt32 info_code420;//轴1 正向软限位单位：um //设定范围：（-1000000,1000000）；初始值:1000000
    //    UInt32 info_code421;// 轴2
    //    UInt32 info_code422;// 轴3
    //    UInt32 info_code423;// 轴4
    //    UInt32 info_code424;// 轴5
    //    UInt32 info_code425;// 轴6
    //    UInt32 info_code426;// 轴7
    //    UInt32 info_code427;// 轴8
    //    UInt32 info_code428;// 轴9
    //    UInt32 info_code429;// 轴10
    //    UInt32 info_code430;// 轴11
    //    UInt32 info_code431;// 轴12
    //    UInt32 info_code432;// 轴13
    //    UInt32 info_code433;// 轴14
    //    UInt32 info_code434;// 轴15
    //    UInt32 info_code435;// 轴16
    //    UInt32 info_code436;// 轴17
    //    UInt32 info_code437;// 轴18
    //    UInt32 info_code438;// 轴19
    //    UInt32 info_code439;// 轴20
    //    UInt32 info_code440;// 轴21
    //    UInt32 info_code441;// 轴22
    //    UInt32 info_code442;// 轴23
    //    UInt32 info_code443;// 轴24
    //    UInt32 info_code444;// 轴25
    //    UInt32 info_code445;//轴1 负向软限位单位：um //设定范围：（-1000000, 1000000）；初始值:-1000000
    //    UInt32 info_code446;// 轴2
    //    UInt32 info_code447;// 轴3
    //    UInt32 info_code448;// 轴4
    //    UInt32 info_code449;// 轴5
    //    UInt32 info_code450;// 轴6
    //    UInt32 info_code451;// 轴7
    //    UInt32 info_code452;// 轴8
    //    UInt32 info_code453;// 轴9
    //    UInt32 info_code454;// 轴10
    //    UInt32 info_code455;// 轴11
    //    UInt32 info_code456;// 轴12
    //    UInt32 info_code457;// 轴13
    //    UInt32 info_code458;// 轴14
    //    UInt32 info_code459;// 轴15
    //    UInt32 info_code460;// 轴16
    //    UInt32 info_code461;// 轴17
    //    UInt32 info_code462;// 轴18
    //    UInt32 info_code463;// 轴19
    //    UInt32 info_code464;// 轴20
    //    UInt32 info_code465;// 轴21
    //    UInt32 info_code466;// 轴22
    //    UInt32 info_code467;// 轴23
    //    UInt32 info_code468;// 轴24
    //    UInt32 info_code469;// 轴25
    //    UInt32 info_code470;// 功能选择1 //设定范围：（0,0xFFFFFFFF）；初始值:0:
    //    UInt32 info_code471;// 功能选择2
    //    UInt32 info_code472;// 功能选择3
    //    UInt32 info_code473;// 功能选择4
    //    UInt32 info_code474;// 功能选择5
    //    UInt32 info_code475;// 功能选择6
    //    UInt32 info_code476;// 功能选择7
    //    UInt32 info_code477;// 功能选择8
    //    UInt32 info_code478;// 功能选择9
    //    UInt32 info_code479;// 功能选择10
    //    UInt32 info_code480;// 弓字形扫描时 LEDFlashTime1 //单位:us;0xffff 常亮
    //    UInt32 info_code481;// 弓字形扫描时LEDFlashTime2 //单位:us;0xffff 常亮
    //    UInt32 info_code482;//保留
    //    UInt32 info_code483;//保留
    //    UInt32 info_code484;//保留
    //    UInt32 info_code485;//保留
    //    UInt32 info_code486;//保留

    //    UInt32 crc;//CRC校验码
    //    UInt32 send_num;//通讯总条数
    //    UInt32 package_footer;//命令码
    //}
}
