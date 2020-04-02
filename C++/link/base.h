#include<iostream>
#include<vector>
#include<algorithm> 
#include<string>
#include <stdlib.h>
#include<fstream>
using namespace std;
class Coordinate_2D {
public:
	double x;
	double y;
};

class Coordinate_3D {
public:
	double x;
	double y;
	int z;
};

class Pin {
public:
	Coordinate_3D pos;
	int type;  //1代表source，2代表sink 
	double pinwidth;  //1.5*width 
	bool used;  //保证一个引脚只属于一个线网 
	bool havedin;
};

class Wire {
public:

	int type;  //1代表垂直，2水平，3通孔,4金属片垂直，5金属片水平 
	Coordinate_3D  N1, N2;  //导线中心的两点坐标 ,上右为N1,下左为N2 
	double wirewidth;  //导线宽度 
	double metalwidth;
	bool wiretype; //true为导线，否则为金属片 
	int layerid;  //导线所在的层号，如果是通孔则为两者中较小的 
	bool isPRLviolation;  //false
	bool iseofviolation;  //true
	bool cutviolation;   //false
	bool ctcviolation;   //false
	bool areaviolation;  //false 
	vector< pair<Coordinate_3D, Coordinate_3D> >  shortlist;
};



class Net {
public:
	string NetName;
	vector<Pin> pinlist; //vector[0]代表source,其余为sink 
	vector<Wire> wirelist; //包括金属片，且一个金属片对用两个，即水平和垂直 
	int pinnum;
	int wirenum;//导线数

};

class  Track {
public:
	Coordinate_2D start;
	Coordinate_2D end;
};

class Edge {
	Coordinate_3D N1, N2;
	//int type;  //1代表垂直，2水平，3通孔
	int weight;
};

class RoutingSpace_2D {
public:
	Coordinate_2D LeftDownPoint;
	Coordinate_2D RightUpPoint;
	vector< pair< Coordinate_3D, Coordinate_3D > > wires;  //这层接受到的导线 
	vector< Coordinate_3D > mentallist;  //这层接受到的金属片
	vector< vector< pair<Coordinate_3D, Coordinate_3D> > >  shortwirelist;   //导线的短路导线 
	vector< vector< pair<Coordinate_3D, Coordinate_3D> > >  shortmentallist;  //金属片的短路导线 
	vector< Wire > connectwire;  //最终版本 
	vector< Track > TrackList;
	int layerid;
	int type;  //1水平 2垂直 
	double minwirewidth;  //导线宽度  minspacing的 1/10 
	double minmetalwidth; // 金属片宽度 minwirewidth的2倍 
	double minspacing;   //默认两个导线间距
	int maxpinnum;
	int tracknum;
	////candidate pin list for layer1
	////the height and the width of the legal box of each candidate pin can be the miximum spacing of each DRCs
	RoutingSpace_2D()
	{
		LeftDownPoint.x = 0;
		LeftDownPoint.y = 0;
		RightUpPoint.x = 90;
		RightUpPoint.y = 40;
		maxpinnum = 0;
		layerid = 0;
		type = 1;
		minwirewidth = 0;
		minmetalwidth = 0;
		minspacing = 0;
		tracknum = 10;

	};
};

class Line
{
public:
	pair<Coordinate_3D, Coordinate_3D> _pair;
	vector< pair<Coordinate_3D, Coordinate_3D> >  shortwire;

};

class Mental
{
public:
	Coordinate_3D point;
	vector< pair<Coordinate_3D, Coordinate_3D> >  shortmental;
};



class Violationregion
{
public:
	Coordinate_2D leftlower;
	Coordinate_2D rightup;
};

class RoutingSpace_3D {
public:
	//	double testPRL(Wire & wirelist1,Wire & wirelist2); 
	//	double  test (double actualprl,double actualwidth,vector<double> layerprl,
	//vector <double > layerwidth,double table[4][5]) ;
	//void testregion(Wire wire1,Violationregion & region1,
	//Violationregion & region2,double eofwidth,double eofwithin,double eofspace); 





	vector<Line> Linelist;  //unity 3D
	vector <Mental> Mentallist;  //unity 3D
	vector< RoutingSpace_2D > Plane_3D;

	vector< Edge > EdgeList;
	vector<vector<Pin> > netpins;
	int layernum;
	double maxwidth(Wire wire1, Wire wire2); //两个导线的最终宽度
	double computespacing(Wire wire1, Wire wire2);  //两个导线最终间距
	bool haveoverlap(vector<double>& rec1, vector<double>& rec2);  //两个区域是否有重叠 
	Wire findbigger(Wire wire1, Wire wire2);  //找到相邻两导线中较大的 
	Wire findsmaller(Wire wire1, Wire wire2);  //找到较小的
	int  judgewire(Wire wire1, Wire wire2);
	double minspacing(Wire wire1, Wire wire2);
	void fileEmpty(const char fileName[]);

	//spacingtable
	vector<double> layerPRL;
	vector<double> layerwidth;
	void setlayerPRL(int layerid, vector <double>& layerPRL);  //设置表中的prl 
	void setlayerwidth(int layerid, vector<double>& layerwidth);   //设置表中width  
	void createtable(int layerid, double(&table)[4][5]);   //生成表 
	void computePRL(int id, vector<Wire>& _Wirelist, vector<Wire>& _Wirelist_, double table[4][5]);  //计算实际prl 
	void isviolation(int id, double actualprl, double actualwidth, vector<double> layerprl,
		vector <double > layerwidth, Wire& wire1, Wire& wire2, double table[4][5]);
	void writetable(vector <double> layerprl, vector<double> layerwidth, double table[4][5]);
	void  writemessage(Wire wire1, Wire wire2, double prl, double space);
	//eoftable
	double eofwithin;
	double eofspace;
	double eofwidth;
	void writeeof(int i, vector < RoutingSpace_2D > p);
	void writeeofmessage(Wire wire1, Wire wire2);
	void seteofvalue(int layerid, vector<Wire> wirelist1, vector<Wire> wirelist2,
		double& eofwidth, double& eofwithin, double& eofspace);
	void createregion(Wire wire1, Wire wire2, Violationregion& region1,
		Violationregion& region2, double eofwidth, double eofwithin, double eofspace);

	//adjcutspacing
	double within;
	double adjspacing;
	int num;
	vector <Wire> cutwires;
	int sum = 0;
	void writecuts();
	void writecutsmessage(Wire wire1, Wire wire2);
	void setwithin(int layerid, double spacing, double& within);
	void setadjspacing(int layerid, double spacing, double& adjspacing);
	void setnum(int layerid, int& num);
	void computesum(int id, Wire wire, vector <Wire>& wirelist2, double cutwithin, vector<Wire>& cutwires, int& sum);
	void iscutviolation(int i, Wire wire, vector <Wire>wirelist, double adjspacing, int num, int sum);
	//ctctable
	vector <pair <double, double> > ctctable;
	double ctcspacing;
	bool ctcflag;  //false
	void writectcmessage(Wire wire1, Wire wire2);
	void createtable(double width, vector <pair <double, double> >& ctctable);
	void setspacing(int id, vector<Wire> wirelist1, vector<Wire> wirelist2,
		vector <pair <double, double> >& table, double& spacing);
	void createregion(int id, Wire wire1, Wire wire2, Violationregion& region1, Violationregion& region2,
		Violationregion& region3, Violationregion& region4, double width, double spacing);
	void haveprl(int id, Wire& wire1, Wire& wire2, Violationregion& region1, Violationregion& region2,
		Violationregion& region3, Violationregion& region4);
	void writectc();


	//area
	double area;
	void isareaviolation(int id, vector<Wire>& wirelist, double area);
	void setarea(int id, double width, double& area);
	void writeareamessage(Wire wire1);
	void writearea();
	//findpin
	vector <Coordinate_3D> crosspoint;
	vector <Pin> pins; //候选引脚 
	void findcross(RoutingSpace_2D p0, RoutingSpace_2D p1, vector <Coordinate_3D>& crosspoint);
	void findpin(vector <Pin>& pinlist, double finalspacing,
		vector <Coordinate_3D> crosspoint);
	void randpin(int maxpinnum, int netnum, int pinnum,
		vector<vector<Pin>>& netpins, vector<Pin>& pins);

	//init
	bool  isshort(Wire wire1, Wire wire2);
	void  initunity(vector< RoutingSpace_2D >& Plane_3D, int id, vector <Line>& Linelist,
		vector <Mental>& Mentallist);
	void  init2D(double length, double width, int tracksnum, int id);
	void  initwire(vector< RoutingSpace_2D >& Plane_3D, int id);
	void  changemental(vector< RoutingSpace_2D >& Plane_3D, int id);
	void  readroutingspace();
	void  writeallpin(vector <Pin> pinlist);
	void  readpin(int& netnum);
	void  readmental();
	void  readwire();
	//构造函数 
	RoutingSpace_3D()
	{
		//		this->layernum =9; 
		cout << "start" << endl;
		//读入层数，布线区域长宽，轨道数量
		readroutingspace();

		readwire();
		readmental();

		//		cout<<layernum<<endl;
		//		for(int i=0;i<layernum;i++)
		//		{
		//			cout<<i<<"层的长宽为：  "<<this->Plane_3D[i].RightUpPoint.x<<"  "<<this->Plane_3D[i].RightUpPoint.y;
		//			cout<<endl;
		//			cout<<i<<"层轨道数为：  "<<this->Plane_3D[i].tracknum<<endl<<endl; 
		//		}
				//init2D
		//		for(int i=0;i<layernum;i++)
		//		{             
		//			initunity( Plane_3D,i,
		//				 Linelist, Mentallist)
		//		}

				//initwire
		for (int i = 0; i < layernum; i++)
		{
			changemental(Plane_3D, i);
			initwire(Plane_3D, i);
		}
		cout << "initwire" << endl;
		//findpin
		findcross(Plane_3D[0], Plane_3D[1], crosspoint);
		//		for(int i=0;i<crosspoint.size() ;i++)
		//		{
		//			cout<<"第"<<i<<"个点为"<<crosspoint[i].x<<"  "<<crosspoint[i].y <<" "<<crosspoint[i].z;
		//			cout<<endl;
		//		}
		cout << "findcross" << endl;
		Pin firstpin;
		firstpin.pos.x = crosspoint[0].x;
		firstpin.pos.y = crosspoint[0].y;
		firstpin.pos.z = 0;

		firstpin.used = false;
		firstpin.type = 1;//source
		pins.push_back(firstpin);
		//		cout<<"第一个候选引脚为:  "<<pins[0].pos.x<<"  "<< pins[0].pos.y<<"  "<<pins[0].pos.z<<"  ";
		cout << "1pin" << endl;
		//未设置 
		double needspacing1 = 1.1, needspacing2 = 2.2, needspacing3 = 3.3, needspacing4 = 3.1, finalspacing;//参考 		
		finalspacing = max(max(max(needspacing1, needspacing2), needspacing3), needspacing4);
		//		cout<<"finalspacing: "<<finalspacing;
		cout << "maxspacing" << endl;
		findpin(pins, finalspacing, crosspoint);
		cout << "findpin" << endl;
		writeallpin(pins);
		cout << "writepin" << endl;
		//		for(int i=0;i<pins.size() ;i++) 
		//		{
		//			cout<<"第"<<i<<"个引脚为"<<pins[i].pos.x<<"  "<< pins[i].pos.y<<"  "<<pins[i].pos.z;
		//			cout<<endl;
		//		 } 
		int netnum, pinnum, maxpinnum;//未设置   
		pinnum = pins.size();
		readpin(netnum);
		maxpinnum = min(pinnum - 2 * (netnum - 1), int(pinnum / netnum));
		//		cout<<netnum<<"  "<<maxpinnum<<pinnum<<endl;
		randpin(maxpinnum, netnum, pinnum, netpins, pins);  // 线网最大引脚数， 线网数， 候选引脚数

		cout << "randpin" << endl;
		//		//spacingtable
		//		this->layernum=this->Plane_3D.size() ;
		//		int t;//记录层数
		//		fileEmpty("C:\\Users\\Administrator\\Desktop\\file\\data\\prlviolation.txt"); 
		//		for(int i=1;i<layernum;i++)  //第一层不布置导线 
		//		{
		//			double table[4][5];
		//			setlayerPRL( i,layerPRL);
		//			setlayerwidth(i, layerwidth);
		//			createtable(i,table);
		//			writetable(layerPRL,layerwidth,table);
		//			computePRL(i,Plane_3D[i].connectwire,Plane_3D[i].connectwire,table);
		////			if(i==1)
		////			{
		////				Wire w1,w2;
		////				double r;
		////				r=test(22.5,1.5,layerPRL,layerwidth,table);
		////				cout<<"needspacing: "<<r<<endl;
		////			}
		//			layerwidth.clear();
		//			layerPRL.clear();
		//		}
		//		
		//		cout<<"prl"<<endl;
		//		//eoftable
		//		fileEmpty("C:\\Users\\Administrator\\Desktop\\file\\data\\eofviolation.txt");
		//		for(int i=1;i<layernum;i++)
		//		{
		//			writeeof(i,Plane_3D);
		//			seteofvalue(i,Plane_3D[i].connectwire,Plane_3D[i].connectwire,eofwidth,eofwithin,eofspace);
		//			
		//		}
		//		cout<<"eof"<<endl;
		//		//adjcutspacing
		//		fileEmpty("C:\\Users\\Administrator\\Desktop\\file\\data\\cutsviolation.txt");
		//		for(int i=0;i<layernum-1;i++)  //注意减一，通孔的layerid为下一层的 
		//		{
		//				setwithin(i, this->Plane_3D[i].minspacing*0.8,within);
		//				setadjspacing(i,this->Plane_3D[i].minspacing*0.8,adjspacing);
		//				setnum (i,num);
		//				writecuts(); 
		//				for(int j=0;j<Plane_3D[i].connectwire.size();j++)
		//				{
		//					computesum(i,Plane_3D[i].connectwire[j],Plane_3D[i].connectwire,within,cutwires,sum);
		//					iscutviolation(i,Plane_3D[i].connectwire[j],cutwires,adjspacing,num,sum);
		//					cutwires.clear() ;
		//				 } 
		//			
		//		}
		//		cout<<"adjcut"<<endl;
		//		//ctcspacing
		//		fileEmpty("C:\\Users\\Administrator\\Desktop\\file\\data\\ctcviolation.txt");
		//		for(int i=1;i<layernum;i++)
		//		{
		//			createtable(this->Plane_3D[i].minwirewidth,ctctable);  //参照 
		//			writectc();	
		//			setspacing(i,Plane_3D[i].connectwire,Plane_3D[i].connectwire,ctctable,ctcspacing);
		//			ctctable.clear() ;
		//		}
		//		cout<<"ctc"<<endl;
		////		areaviolation
		//		fileEmpty("C:\\Users\\Administrator\\Desktop\\file\\data\\areaviolation.txt");
		//		for(int i=0;i<layernum;i++)
		//		{
		//			setarea(i,this->Plane_3D[i].minwirewidth,area);
		//			writearea();
		////			isareaviolation(i,Plane_3D[i].connectwire,area);
		//			
		//		 } 

	}

};


