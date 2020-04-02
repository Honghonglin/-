#include <iostream>
#include  "base.h"
#include <vector>
#include <fstream>
using namespace std;


void  RoutingSpace_3D::readroutingspace()
{

	ifstream infile("F:\\unityproject\\Deom-master\\Deom-master\\Demo\\Elastic_ball\\主要界面\\Assets\\StreamingAssets\\routingspace.txt");
	if (!infile.is_open())
	{
		cout << "未成功打开文件" << endl;
	}

	int routingnum, tracksnum;
	double length, width;
	infile >> routingnum;
	infile >> length >> width;
	infile >> tracksnum;

	this->layernum = routingnum;

	for (int i = 0; i < routingnum; i++)
	{
		init2D(length, width, tracksnum, i);
	}

	infile.close();
}

void  RoutingSpace_3D::init2D(double length, double width, int tracksnum, int id)
{
	RoutingSpace_2D tem;
	Track temtrack;
	tem.layerid = id;
	tem.LeftDownPoint.x = 0.0;
	tem.LeftDownPoint.y = 0.0;
	tem.RightUpPoint.x = length;  //unity 
	tem.RightUpPoint.y = width;  //unity 
	tem.tracknum = tracksnum - id / 3;//假设第一层为10，每三层减1  unity

	if (id == 0)
		tem.maxpinnum = 30;//假设第一层最大引脚数为0
	if (id % 2 == 0)  //水平 
	{
		tem.type = 1;
		tem.minspacing = tem.RightUpPoint.y / tem.tracknum;
		tem.minwirewidth = tem.minspacing / 10;
		tem.minmetalwidth = tem.minwirewidth * 2;
		double ox;
		ox = tem.minspacing / 2;
		for (int i = 0; ox + i * 2 * ox < tem.RightUpPoint.y; i++)
		{
			temtrack.start.x = 0.0;
			temtrack.start.y = ox + i * 2 * ox;
			temtrack.end.x = tem.RightUpPoint.x;
			temtrack.end.y = ox + i * 2 * ox;
			tem.TrackList.push_back(temtrack);
		}
	}
	else
	{
		tem.type = 2;
		tem.minspacing = tem.RightUpPoint.x / tem.tracknum;
		tem.minwirewidth = tem.minspacing / 10;
		tem.minmetalwidth = tem.minwirewidth * 2;
		double oy;
		oy = tem.minspacing / 2;
		for (int i = 0; oy + i * 2 * oy < tem.RightUpPoint.x; i++)
		{
			temtrack.start.x = oy + i * 2 * oy;
			temtrack.start.y = 0.0;
			temtrack.end.x = oy + i * 2 * oy;
			temtrack.end.y = tem.RightUpPoint.y;
			tem.TrackList.push_back(temtrack);
		}
	}

	Plane_3D.push_back(tem);
	tem.TrackList.clear();
}

void  RoutingSpace_3D::readwire()
{
	int z1, z2, num;
	double x1, x2, y1, y2;  //daoxian
	int sz1, sz2;  //导线的短路 
	double sx1, sx2, sy1, sy2;
	ifstream infile("F:\\unityproject\\Deom-master\\Deom-master\\Demo\\Elastic_ball\\主要界面\\Assets\\StreamingAssets\\wire.txt");
	if (!infile.is_open())
	{
		cout << "未成功打开文件" << endl;
	}
	infile >> num;
	for (int i = 0; i < num; i++)  //导线数量 
	{
		Line line;
		pair<Coordinate_3D, Coordinate_3D> p;
		infile >> x1 >> y1 >> z1 >> x2 >> y2 >> z2;
		line._pair.first.x = x1;
		line._pair.first.y = y1;
		line._pair.first.z = z1;
		line._pair.second.x = x2;
		line._pair.second.y = y2;
		line._pair.second.z = z2;
		int shortnum;
		infile >> shortnum;
		for (int j = 0; j < shortnum; j++)
		{
			infile >> sx1 >> sy1 >> sz1 >> sx2 >> sy2 >> sz2;
			p.first.x = sx1;
			p.first.y = sy1;
			p.first.z = sz1;
			p.second.x = sx2;
			p.second.y = sy2;
			p.second.z = sz2;
			line.shortwire.push_back(p);
		}
		this->Linelist.push_back(line);
		int lnum;
		lnum = this->layernum;
		for (int k = 0; k < lnum; k++)
		{
			if (line._pair.second.z == k)
			{
				cout << "layer:  " << line._pair.second.z << endl;
				//				this->Plane_3D[k].wires.push_back(line._pair);
				this->Plane_3D[k].shortwirelist.push_back(line.shortwire);
				//				cout<<this->Plane_3D[k].shortwirelist[0][0].first.x<<endl;
			}
		}
	}
	infile.close();
}


void  RoutingSpace_3D::readmental()
{
	ifstream infile("F:\\unityproject\\Deom-master\\Deom-master\\Demo\\Elastic_ball\\主要界面\\Assets\\StreamingAssets\\mental.txt");
	if (!infile.is_open())
	{
		cout << "未成功打开文件" << endl;
	}
	int num1, num2, z, sz1, sz2;
	double x, y, sx1, sy1, sx2, sy2;
	infile >> num1;  //金属片数量

	for (int i = 0; i < num1; i++)
	{
		infile >> x >> y >> z;
		Mental mental;
		pair<Coordinate_3D, Coordinate_3D> p;
		mental.point.x = x;
		mental.point.y = y;
		mental.point.z = z;

		infile >> num2;
		for (int j = 0; j < num2; j++)
		{
			infile >> sx1 >> sy1 >> sz1 >> sx2 >> sy2 >> sz2;
			p.first.x = sx1;
			p.first.y = sy1;
			p.first.z = sz1;
			p.second.x = sx2;
			p.second.y = sy2;
			p.second.z = sz2;
			mental.shortmental.push_back(p);
		}
		this->Mentallist.push_back(mental);
		int lnum;
		lnum = this->layernum;

		for (int k = 0; k < lnum; k++)
		{
			if (mental.point.z == k)
			{
				cout << "layer:  " << mental.point.z << endl;
				//				this->Plane_3D[k].mentallist.push_back(mental.point); 
				this->Plane_3D[k].shortmentallist.push_back(mental.shortmental);
			}
		}
	}
	infile.close();
}

//void  RoutingSpace_3D::initunity(vector< RoutingSpace_2D > & Plane_3D,int id,
//vector <Line> & Linelist,vector <Mental> & Mentallist)
//{
//	cout<<"111"<<endl;
//	for(int j=0;j<Linelist.size();j++)
//		if(Linelist[j]._pair.second.z==id)
//		{
//			Plane_3D[id].wires.push_back(Linelist[j]._pair); 
//			Plane_3D[id].shortwirelist.push_back(Linelist[j].shortwire);	
//		} 
//	cout<<"111"<<endl;
//	for(int j=0;j<Mentallist.size();j++)
//		if(Mentallist[j].point.z==id) 
//		{
//			Plane_3D[id].mentallist.push_back(Mentallist[j].point);
//			Plane_3D[id].shortmentallist.push_back(Mentallist[j].shortmental); 
//		} 
//	cout<<"111"<<endl;
//}

void  RoutingSpace_3D::changemental(vector< RoutingSpace_2D >& Plane_3D, int id)
{
	int num = Plane_3D[id].mentallist.size();
	for (int i = 0; i < num; i++)
	{
		Wire wire1, wire2;
		//wire1  水平 
		wire1.N1.x = Plane_3D[id].mentallist[i].x + 0.5 * Plane_3D[id].minmetalwidth;
		wire1.N1.y = Plane_3D[id].mentallist[i].y;
		wire1.N1.z = Plane_3D[id].mentallist[i].z;
		wire1.N2.x = Plane_3D[id].mentallist[i].x - 0.5 * Plane_3D[id].minmetalwidth;
		wire1.N2.y = Plane_3D[id].mentallist[i].y;
		wire1.N2.z = Plane_3D[id].mentallist[i].z;
		wire1.layerid = Plane_3D[id].mentallist[i].z;
		wire1.metalwidth = Plane_3D[id].minmetalwidth;
		wire1.type = 5;
		wire1.wiretype = false;
		wire1.wirewidth = 0;  //不存在
		//wire1 短路导线 mental
		int shortnum;
		shortnum = Plane_3D[id].shortmentallist[i].size();
		for (int j = 0; j < shortnum; j++)
		{
			wire1.shortlist[j].first.x = Plane_3D[id].shortmentallist[i][j].first.x;
			wire1.shortlist[j].first.y = Plane_3D[id].shortmentallist[i][j].first.y;
			wire1.shortlist[j].first.z = Plane_3D[id].shortmentallist[i][j].first.z;
			wire1.shortlist[j].second.x = Plane_3D[id].shortmentallist[i][j].second.x;
			wire1.shortlist[j].second.y = Plane_3D[id].shortmentallist[i][j].second.y;
			wire1.shortlist[j].second.z = Plane_3D[id].shortmentallist[i][j].second.z;
		}
		Plane_3D[id].connectwire.push_back(wire1);

		//wire2  垂直 
		wire2.N1.x = Plane_3D[id].mentallist[i].x;
		wire2.N1.y = Plane_3D[id].mentallist[i].y + 0.5 * Plane_3D[id].minmetalwidth;
		wire2.N1.z = Plane_3D[id].mentallist[i].z;
		wire2.N2.x = Plane_3D[id].mentallist[i].x;
		wire2.N2.y = Plane_3D[id].mentallist[i].y - 0.5 * Plane_3D[id].minmetalwidth;
		wire2.N2.z = Plane_3D[id].mentallist[i].z;
		wire2.layerid = Plane_3D[id].mentallist[i].z;
		wire2.metalwidth = Plane_3D[id].minmetalwidth;
		wire2.type = 4;
		wire2.wiretype = false;
		wire2.wirewidth = 0;  //不存在
		//wire1 短路导线 mental
		int shortnum1;
		shortnum1 = Plane_3D[id].shortmentallist[i].size();
		for (int j = 0; j < shortnum1; j++)
		{
			wire2.shortlist[j].first.x = Plane_3D[id].shortmentallist[i][j].first.x;
			wire2.shortlist[j].first.y = Plane_3D[id].shortmentallist[i][j].first.y;
			wire2.shortlist[j].first.z = Plane_3D[id].shortmentallist[i][j].first.z;
			wire2.shortlist[j].second.x = Plane_3D[id].shortmentallist[i][j].second.x;
			wire2.shortlist[j].second.y = Plane_3D[id].shortmentallist[i][j].second.y;
			wire2.shortlist[j].second.z = Plane_3D[id].shortmentallist[i][j].second.z;
		}
		Plane_3D[id].connectwire.push_back(wire2);
	}
}

void  RoutingSpace_3D::initwire(vector< RoutingSpace_2D >& Plane_3D, int id)
{
	int num;
	num = Plane_3D[id].wires.size();
	Wire wire1;
	for (int i = 0; i < num; i++)
	{
		wire1.N1.x = Plane_3D[id].wires[i].first.x;
		wire1.N1.y = Plane_3D[id].wires[i].first.y;
		wire1.N1.z = Plane_3D[id].wires[i].first.z;
		wire1.N2.x = Plane_3D[id].wires[i].second.x;
		wire1.N2.y = Plane_3D[id].wires[i].second.y;
		wire1.N2.z = Plane_3D[id].wires[i].second.z;
		wire1.metalwidth = 0;//不存在
		wire1.wiretype = true;
		wire1.wirewidth = Plane_3D[id].minwirewidth;
		wire1.layerid = Plane_3D[id].wires[i].first.z;//第一个端点的z 
		if (wire1.N1.x == wire1.N2.x && wire1.N1.y != wire1.N2.y && wire1.N1.z == wire1.N2.z) //垂直 
			wire1.type = 1;
		else if (wire1.N1.y == wire1.N2.y && wire1.N1.x != wire1.N2.x && wire1.N1.z == wire1.N2.z) //水平
			wire1.type = 2;
		else if (wire1.N1.z != wire1.N2.z && wire1.N1.x == wire1.N2.x && wire1.N1.y == wire1.N2.y)  //通孔 
			wire1.type = 3;
		int shortnum;
		shortnum = Plane_3D[id].shortwirelist[i].size();
		for (int j = 0; j < shortnum; j++)
		{
			wire1.shortlist[j].first.x = Plane_3D[id].shortwirelist[i][j].first.x;
			wire1.shortlist[j].first.y = Plane_3D[id].shortwirelist[i][j].first.y;
			wire1.shortlist[j].first.z = Plane_3D[id].shortwirelist[i][j].first.z;
			wire1.shortlist[j].second.x = Plane_3D[id].shortwirelist[i][j].second.x;
			wire1.shortlist[j].second.y = Plane_3D[id].shortwirelist[i][j].second.y;
			wire1.shortlist[j].second.z = Plane_3D[id].shortwirelist[i][j].second.z;
		}
		Plane_3D[id].connectwire.push_back(wire1);
	}
}