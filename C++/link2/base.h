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
	int type;  //1����source��2����sink 
	double pinwidth;  //1.5*width 
	bool used;  //��֤һ������ֻ����һ������ 
	bool havedin;
};

class Wire {
public:

	int type;  //1����ֱ��2ˮƽ��3ͨ��,4����Ƭ��ֱ��5����Ƭˮƽ 
	Coordinate_3D  N1, N2;  //�������ĵ��������� ,����ΪN1,����ΪN2 
	double wirewidth;  //���߿�� 
	double metalwidth;
	bool wiretype; //trueΪ���ߣ�����Ϊ����Ƭ 
	int layerid;  //�������ڵĲ�ţ������ͨ����Ϊ�����н�С�� 

	vector< pair<Coordinate_3D, Coordinate_3D> >  shortlist;
};



class Net {
public:
	string NetName;
	vector<Pin> pinlist; //vector[0]����source,����Ϊsink 
	vector<Wire> wirelist; //��������Ƭ����һ������Ƭ������������ˮƽ�ʹ�ֱ 
	int pinnum;
	int wirenum;//������

};

class  Track {
public:
	Coordinate_2D start;
	Coordinate_2D end;
};

class Edge {
	Coordinate_3D N1, N2;
	//int type;  //1����ֱ��2ˮƽ��3ͨ��
	int weight;
};

class Line
{
public:
	pair<Coordinate_3D, Coordinate_3D> _pair;
	vector< pair<Coordinate_3D, Coordinate_3D> >  shortwire;   //��·���� 

};

class Mental
{
public:
	Coordinate_3D point;
	vector< pair<Coordinate_3D, Coordinate_3D> >  shortmental;  //��·�Ľ���Ƭ�͵��� 

};

class RoutingSpace_2D {
public:
	Coordinate_2D LeftDownPoint;
	Coordinate_2D RightUpPoint;
	vector< pair<Coordinate_3D, Coordinate_3D> > wires;  //�����ܵ��ĵ��� 
	vector< Coordinate_3D > mentallist;  //�����ܵ��Ľ���Ƭ
	vector< vector< pair<Coordinate_3D, Coordinate_3D> > >  shortwirelist;   //���ߵĶ�·���� 
	vector< vector< pair<Coordinate_3D, Coordinate_3D> > >  shortmentallist;  //����Ƭ�Ķ�·���� 

	vector< Wire > connectwire;  //���հ汾 
	vector< Track > TrackList;
	int layerid;
	int type;  //1ˮƽ 2��ֱ 
	double minwirewidth;  //���߿��  minspacing�� 1/10 
	double minmetalwidth; // ����Ƭ��� minwirewidth��2�� 
	double minspacing;   //����������
	double defaultspace;  //Ĭ�ϼ�ࣨ��������֮�䣩 
	int maxpinnum;
	int tracknum;
	////candidate pin list for layer1
	////the height and the width of the legal box of each candidate pin can be the miximum spacing of each DRCs

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
	vector< pair<Coordinate_3D, Coordinate_3D> >  mmpair;
	double maxwidth(Wire wire1, Wire wire2); //�������ߵ����տ��
	double computespacing(Wire wire1, Wire wire2);  //�����������ռ��
	bool haveoverlap(vector<double>& rec1, vector<double>& rec2);  //���������Ƿ����ص� 
	Wire findbigger(Wire wire1, Wire wire2);  //�ҵ������������нϴ�� 
	Wire findsmaller(Wire wire1, Wire wire2);  //�ҵ���С��
	int  judgewire(Wire wire1, Wire wire2);
	double minspacing(Wire wire1, Wire wire2);
	void  fileEmpty(const char fileName[]);
	bool  checkwire(Wire wire1, Wire wire2, vector< pair<Wire, Wire> > wirelist);
	bool  areamental(double x, double y, int z, vector <Coordinate_3D>  temmental);
	bool  isprlmm(vector< pair<Coordinate_3D, Coordinate_3D> > mmpair, double x1, double y1, double z1
		, double x2, double y2, double z2);
	bool  isconnect(Wire wire1, Wire wire2);
	bool  isequal(Wire wire1, Wire wire2);
	//spacingtable
	vector<double> layerPRL;
	vector<double> layerwidth;
	void setlayerPRL(int layerid, vector <double>& layerPRL);  //���ñ��е�prl 
	void setlayerwidth(int layerid, vector<double>& layerwidth);   //���ñ���width  
	void createtable(int layerid, double(&table)[4][10]);   //���ɱ� 
	void computePRL(int id, vector<Wire>& _Wirelist, vector<Wire>& _Wirelist_, double table[4][10]);  //����ʵ��prl 
	void isviolation(int id, double actualprl, double actualwidth, vector<double> layerprl,
		vector <double > layerwidth, Wire& wire1, Wire& wire2, double table[4][10]);
	void writetable(vector <double> layerprl, vector<double> layerwidth, double table[4][10]);
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
	void iscutviolation(int i, Wire wire, vector <Wire>wirelist, double adjspacing, int num, int sum, vector< pair<Wire, Wire> >& temwire);
	//ctctable
	vector <pair <double, double> > ctctable;
	double ctcspacing;
	bool ctcflag;  //false
	void writectcmessage(Wire wire1, Wire wire2);
	void createtable(int i, double width, vector <pair <double, double> >& ctctable);
	void setspacing(int id, vector<Wire> wirelist1, vector<Wire> wirelist2,
		vector <pair <double, double> >& table, double& spacing);
	void createregion(int id, Wire wire1, Wire wire2, Violationregion& region1, Violationregion& region2,
		Violationregion& region3, Violationregion& region4, double width, double spacing);
	void haveprl(int id, Wire& wire1, Wire& wire2, Violationregion& region1, Violationregion& region2,
		Violationregion& region3, Violationregion& region4);
	void writectc();


	//area
	double area;
	void isareaviolation(int id, vector<Wire>& wirelist, double area, vector <Coordinate_3D>& temmental);
	void setarea(int id, double width, double& area);
	void writeareamessage(Wire wire1, vector <Coordinate_3D>& temmental);
	void writearea();
	//findpin
	vector <Coordinate_3D> crosspoint;
	vector <Pin> pins; //��ѡ���� 
	void findcross(RoutingSpace_2D p0, RoutingSpace_2D p1, vector <Coordinate_3D>& crosspoint);
	void findpin(vector <Pin>& pinlist, double finalspacing,
		vector <Coordinate_3D> crosspoint);
	void randpin(int maxpinnum, int netnum, int pinnum,
		vector<vector<Pin>>& netpins, vector<Pin>& pins);

	//init
	bool  isshort(Wire wire1, Wire wire2);
	void  initunity(vector< RoutingSpace_2D >& Plane_3D, int id, vector <Line>& Linelist,
		vector <Mental>& Mentallist);
	void  init2D(double length, double width, int tracksnum, int id, double& temspace);
	void  initwire(vector< RoutingSpace_2D >& Plane_3D, int id);
	void  changemental(vector< RoutingSpace_2D >& Plane_3D, int id);
	void  readroutingspace();
	void  writeallpin(vector <Pin> pinlist);
	void  readpin(int& netnum);
	void  readmental();
	void  readwire();
	void  readmental1();
	//���캯�� 
	RoutingSpace_3D()
	{
		//		this->layernum =9; 
		cout << "start" << endl;
		//����������������򳤿��������
		readroutingspace();

		readwire();

		readmental();
		readmental1();
		cout << layernum << endl;
		for (int i = 0; i < layernum; i++)
		{
			cout << i << "��ĳ���Ϊ��  " << this->Plane_3D[i].RightUpPoint.x << "  " << this->Plane_3D[i].RightUpPoint.y;
			cout << endl;
			cout << "����Ϊ�� " << this->Plane_3D[i].type << endl;
			cout << "�߿�Ϊ�� " << this->Plane_3D[i].minwirewidth << endl;
			cout << "����Ƭ���Ϊ�� " << this->Plane_3D[i].minmetalwidth << endl;
			cout << "�������Ϊ�� " << this->Plane_3D[i].minspacing << endl;
			cout << "Ĭ�ϼ��Ϊ�� " << this->Plane_3D[i].defaultspace << endl;
			cout << "�����Ϊ��  " << this->Plane_3D[i].tracknum << endl << endl;
		}
		//init2D
		for (int i = 0; i < layernum; i++)
		{
			initunity(Plane_3D, i, Linelist, Mentallist);
		}

		//initwire
		for (int i = 0; i < layernum; i++)
		{
			initwire(Plane_3D, i);
			changemental(Plane_3D, i);
		}
		cout << "initwire" << endl;
		//		for(int i=0;i<layernum;i++)
		//		{
		//			cout<<"��"<<i<<"��Ĺ����Ϊ��  "<< this->Plane_3D[i].connectwire.size()<<endl;
		//		}
		//		//testwire
		//	for(int i=0;i<layernum;i++)   
		//		{  
		//			int num;
		//			num=this->Plane_3D[i].connectwire.size();
		//			for(int j=0;j<num;j++)
		//			{
		//				cout<<"��"<<i<<"��"<<"��"<<j<<"�����ߵ�����Ϊ�� ";
		//				cout<<this->Plane_3D[i].connectwire[j].N1.x<<" "
		//				    <<this->Plane_3D[i].connectwire[j].N1.y<<" "
		//				    <<this->Plane_3D[i].connectwire[j].N1.z<<" "
		//				    <<this->Plane_3D[i].connectwire[j].N2.x<<" "
		//				    <<this->Plane_3D[i].connectwire[j].N2.y<<" "
		//				    <<this->Plane_3D[i].connectwire[j].N2.z<<" "<<endl;
		//				cout<<"��������Ϊ�� "<< this->Plane_3D[i].connectwire[j].type<<endl;
		//				int snum=this->Plane_3D[i].connectwire[j].shortlist.size() ;
		//				cout<<"���·�ĵ�����Ϊ: "<<snum<<endl;
		//				for (int k=0;k<snum;k++)
		//				{
		//					cout<<"��"<<i<<"��"<<"��"<<j<<"�����ߵ�"<<"��"<<k<<"����·����Ϊ�� ";
		//					cout<<this->Plane_3D[i].connectwire[j].shortlist[k].first.x<<" " 
		//					<<this->Plane_3D[i].connectwire[j].shortlist[k].first.y<<" " 
		//					<<this->Plane_3D[i].connectwire[j].shortlist[k].first.z<<" " 
		//					<<this->Plane_3D[i].connectwire[j].shortlist[k].second.x<<" " 
		//					<<this->Plane_3D[i].connectwire[j].shortlist[k].second.y<<" " 
		//					<<this->Plane_3D[i].connectwire[j].shortlist[k].second.z<<" " ; 
		//				}
		//				cout<<endl<<endl;
		//			}
		//			cout<<endl;
		//	}


		//		//findpin
		//		findcross(Plane_3D[0],Plane_3D[1],crosspoint);
		////		for(int i=0;i<crosspoint.size() ;i++)
		////		{
		////			cout<<"��"<<i<<"����Ϊ"<<crosspoint[i].x<<"  "<<crosspoint[i].y <<" "<<crosspoint[i].z;
		////			cout<<endl;
		////		}
		//		cout<<"findcross"<<endl; 
		//		Pin firstpin;
		//		firstpin.pos.x=crosspoint[0].x;
		//		firstpin.pos.y=crosspoint[0].y;
		//		firstpin.pos.z=0;
		//
		//		firstpin.used=false;
		//		firstpin.type=1;//source
		//		pins.push_back(firstpin);
		////		cout<<"��һ����ѡ����Ϊ:  "<<pins[0].pos.x<<"  "<< pins[0].pos.y<<"  "<<pins[0].pos.z<<"  ";
		//		cout<<"1pin"<<endl;
		//		//δ���� 
		//		double needspacing1=1.1,needspacing2=2.2,needspacing3=3.3,needspacing4=3.1,finalspacing;//�ο� 		
		//		finalspacing=max(max(max(needspacing1,needspacing2),needspacing3),needspacing4);
		////		cout<<"finalspacing: "<<finalspacing;
		//		cout<<"maxspacing"<<endl;
		//		findpin(pins,finalspacing,crosspoint);
		//		cout<<"findpin"<<endl;
		//		writeallpin(pins);
		//		cout<<"writepin"<<endl;
		////		for(int i=0;i<pins.size() ;i++) 
		////		{
		////			cout<<"��"<<i<<"������Ϊ"<<pins[i].pos.x<<"  "<< pins[i].pos.y<<"  "<<pins[i].pos.z;
		////			cout<<endl;
		////		 } 
		//		int netnum,pinnum,maxpinnum;//δ����   
		//		pinnum=pins.size();
		//		readpin(netnum);
		//		maxpinnum=min(pinnum-2*(netnum-1),int(pinnum/netnum));
		////		cout<<netnum<<"  "<<maxpinnum<<pinnum<<endl;
		//		randpin(maxpinnum,netnum,pinnum,netpins,pins);  // ��������������� �������� ��ѡ������
		//
		//		cout<<"randpin"<<endl;
				//spacingtable
		this->layernum = this->Plane_3D.size();
		fileEmpty("F:\\unityproject\\Deom-master\\Deom-master\\Demo\\Elastic_ball\\��Ҫ����\\Assets\\StreamingAssets\\prlviolation.txt");
		for (int i = 1; i < layernum; i++)  //��һ�㲻���õ��� 
		{
			double table[4][10];

			setlayerPRL(i, layerPRL);
			setlayerwidth(i, layerwidth);
			createtable(i, table);
			writetable(layerPRL, layerwidth, table);
			computePRL(i, Plane_3D[i].connectwire, Plane_3D[i].connectwire, table);
			layerwidth.clear();
			layerPRL.clear();
			mmpair.clear();
			//			if(i==1)
			//			{
			//				Wire w1,w2;
			//				double r;
			//				r=test(22.5,1.5,layerPRL,layerwidth,table);
			//				cout<<"needspacing: "<<r<<endl;
			//			}
		}

		cout << "prl" << endl;
		//eoftable
		fileEmpty("F:\\unityproject\\Deom-master\\Deom-master\\Demo\\Elastic_ball\\��Ҫ����\\Assets\\StreamingAssets\\eofviolation.txt");
		for (int i = 1; i < layernum; i++)
		{
			writeeof(i, Plane_3D);
			seteofvalue(i, Plane_3D[i].connectwire, Plane_3D[i].connectwire, eofwidth, eofwithin, eofspace);
			mmpair.clear();
		}
		cout << "eof" << endl;
		//adjcutspacing
		fileEmpty("F:\\unityproject\\Deom-master\\Deom-master\\Demo\\Elastic_ball\\��Ҫ����\\Assets\\StreamingAssets\\cutsviolation.txt");
		for (int i = 0; i < layernum; i++)  //ע���һ��ͨ�׵�layeridΪ��һ��� 
		{
			vector< pair<Wire, Wire> > temwire;

			setwithin(i, this->Plane_3D[i].minspacing * 0.8, within);
			setadjspacing(i, this->Plane_3D[i].minspacing * 0.8, adjspacing);
			setnum(i, num);
			writecuts();
			for (int j = 0; j < Plane_3D[i].connectwire.size(); j++)
			{
				if (Plane_3D[i].connectwire[j].type == 3)
				{
					sum = 0;
					computesum(i, Plane_3D[i].connectwire[j], Plane_3D[i].connectwire, within, cutwires, sum);
					sum--;  //��ȥ���� 
//						cout<<"��"<<i<<"��"<<"��"<<j<<"��ͨ����������Ϊ��  "<<sum<<",�ֱ�Ϊ�� "<<endl;
//						for(int k=0;k<sum;k++) 
//						{
//							cout<<"wiretype:  "<<cutwires[k].type<<endl;
//							cout<<cutwires[k].N1.x<<"  "<<cutwires[k].N1.y<<"  "<<cutwires[k].N1.z<<"  "
//							<<cutwires[k].N2.x<<"  "<<cutwires[k].N2.y<<"  "<<cutwires[k].N2.z<<endl;
//						}
					iscutviolation(i, Plane_3D[i].connectwire[j], cutwires, adjspacing, num, sum, temwire);
					cutwires.clear();
				}
			}
			temwire.clear();

		}
		cout << "adjcut" << endl;
		//ctcspacing
		fileEmpty("F:\\unityproject\\Deom-master\\Deom-master\\Demo\\Elastic_ball\\��Ҫ����\\Assets\\StreamingAssets\\ctcviolation.txt");
		for (int i = 1; i < layernum; i++)
		{
			createtable(i, this->Plane_3D[i].minwirewidth, ctctable);  //���� 
			writectc();
			setspacing(i, Plane_3D[i].connectwire, Plane_3D[i].connectwire, ctctable, ctcspacing);
			ctctable.clear();
			mmpair.clear();
		}
		cout << "ctc" << endl;
		//		areaviolation
		fileEmpty("F:\\unityproject\\Deom-master\\Deom-master\\Demo\\Elastic_ball\\��Ҫ����\\Assets\\StreamingAssets\\areaviolation.txt");
		for (int i = 1; i < layernum; i++)
		{
			vector <Coordinate_3D>  temmental;
			setarea(i, this->Plane_3D[i].minwirewidth, area);
			writearea();
			isareaviolation(i, Plane_3D[i].connectwire, area, temmental);
			temmental.clear();
		}
		cout << "area" << endl;
	}


	//test


};


