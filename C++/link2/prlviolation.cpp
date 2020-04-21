#include <iostream>
#include "base.h"
#include  <vector>
#include<fstream>
#include<string>
using namespace std;

void RoutingSpace_3D::setlayerwidth(int layerid, vector<double>& layerwidth)
{
	for (int i = 0; i < 4; i++)
	{
		double width, w;
		width = this->Plane_3D[layerid].minwirewidth;  //只是参照 
		w = width * 0.5 * (i + 2);
		layerwidth.push_back(w);
		//		cout<<"WIDTH"<<": "<<width<<" ";
		//		cout<<"W"<<": "<<w<<"   ";
		//		cout<<endl;
	}
	//	cout<<endl;
}

void RoutingSpace_3D::setlayerPRL(int layerid, vector <double>& layerPRL)
{

	for (int i = 1; i < 11; i++)
	{
		double length, prlvalue;
		if (Plane_3D[layerid].type == 1)//shuiping
		{
			length = this->Plane_3D[layerid].RightUpPoint.x - this->Plane_3D[layerid].LeftDownPoint.x;
			prlvalue = 0.1 * i * length;
			layerPRL.push_back(prlvalue);
		}
		if (Plane_3D[layerid].type == 2)//chuizhi
		{
			length = this->Plane_3D[layerid].RightUpPoint.y - this->Plane_3D[layerid].LeftDownPoint.y;
			prlvalue = 0.1 * i * length;
			layerPRL.push_back(prlvalue);
		}
		//		cout<<"Length: "<<length<<"   ";
		//		cout<<"id: "<<layerid<<"   ";
		//		cout<<"type: "<<Plane_3D[layerid].type<<"   ";
		//		cout<<"Prl: "<<prlvalue<<endl;
	}
	//	cout<<endl;
}

void RoutingSpace_3D::createtable(int layerid, double(&table)[4][10])
{
	double init = this->Plane_3D[layerid].defaultspace;  //已修改后
//	cout<<"1111:  "<<this->Plane_3D[layerid].minspacing; 
//	cout<<"init: "<<init<<endl;
	for (int i = 0; i < 4; i++)
	{
		for (int j = 0; j < 10; j++)
		{
			table[i][j] = i * 0.08 * init + j * 0.05 * init + init;
			//			table[i][j]=this->Plane_3D[]
			//			table[i][j]=10;
			//			cout<<table[i][j]<<"  ";
		}
		//		cout<<endl; 
	}
	//	cout<<endl; 
}

void RoutingSpace_3D::writetable(vector <double> layerprl, vector<double> layerwidth, double table[4][10])
{
	ofstream outfile("F:\\unityproject\\Deom-master\\Deom-master\\Demo\\Elastic_ball\\主要界面\\Assets\\StreamingAssets\\prlviolation.txt", ios::binary | ios::app | ios::in | ios::out);
	if (!outfile.is_open())
	{
		cout << "未成功打开文件" << endl;
	}
	string s;
	s = "according";
	outfile << s << endl;

	int n, m;
	n = 4;
	m = 10;
	outfile << n << "\t" << m << endl;

	for (int i = 0; i < n; i++)
	{
		for (int j = 0; j < m; j++)
			outfile << table[i][j] << "\t";
		outfile << endl;
	}
	int prlnum, widthnum;
	prlnum = layerprl.size();
	widthnum = layerwidth.size();
	outfile << prlnum << endl;
	for (int i = 0; i < prlnum; i++)
		outfile << layerprl[i] << "\t";
	outfile << endl;
	outfile << widthnum << endl;
	for (int i = 0; i < widthnum; i++)
		outfile << layerwidth[i] << "\t";
	outfile << endl;
	s = "message";
	outfile << s << endl;
}

void RoutingSpace_3D::computePRL(int id, vector<Wire>& wirelist1, vector<Wire>& wirelist2, double table[4][10])
{
	int num1 = wirelist1.size();
	int num2 = wirelist2.size();
	double length;

	for (int i = 0; i < num1; i++)  //wirelist1
		for (int j = i + 1; j < num2; j++)  //wirelist2
		{
			length = 0;
			//			cout<<"wire1type: "<<wirelist1[i].type<<"  "<<"wire2type: "<<wirelist2[j].type<<endl;
			//			cout<<wirelist1[i].N1.x<<" "<<wirelist1[i].N1.y<<" "
			//			<<wirelist1[i].N2.x<<" "<<wirelist1[i].N2.y<<" "<<endl
			//			<<wirelist2[j].N1.x<<" "<<wirelist2[j].N1.y<<" "
			//			<<wirelist2[j].N2.x<<" "<<wirelist2[j].N2.y<<" "<<endl<<endl;
			if (!isshort(wirelist1[i], wirelist2[j]) && !isequal(wirelist1[i], wirelist2[j]))
			{
				if (wirelist2[j].type != 3 && wirelist1[i].layerid == id &&
					wirelist2[j].layerid == id && wirelist1[i].type != 3)  //1垂直  2水平  4金属片 
				{
					double maxx;
					maxx = maxwidth(wirelist1[i], wirelist2[j]);
					if ((wirelist1[i].type == 1 && wirelist2[j].type == 4) || (wirelist1[i].type == 4 && wirelist2[j].type == 1) ||
						(wirelist1[i].type == 1 && wirelist2[j].type == 1))
					{
						if (wirelist1[i].N1.y <= wirelist2[j].N1.y && wirelist1[i].N1.y >= wirelist2[j].N2.y)
						{
							if (wirelist1[i].N2.y <= wirelist2[j].N2.y)
								length = wirelist1[i].N1.y - wirelist2[j].N2.y;
							else length = wirelist1[i].N1.y - wirelist1[i].N2.y;

						}//j-i
						else if (wirelist1[i].N1.y >= wirelist2[j].N1.y && wirelist1[i].N2.y <= wirelist2[j].N1.y)
						{
							if (wirelist2[j].N2.y <= wirelist1[i].N2.y)
								length = wirelist2[j].N1.y - wirelist1[i].N2.y;
							else length = wirelist2[j].N1.y - wirelist2[j].N2.y;
						}
					}
					else if ((wirelist1[i].type == 2 && wirelist2[j].type == 5) || (wirelist1[i].type == 5 && wirelist2[j].type == 2)
						|| (wirelist1[i].type == 2 && wirelist2[j].type == 2))
					{
						//						cout<<"111"<<endl; 
						if (wirelist1[i].N1.x <= wirelist2[j].N1.x && wirelist1[i].N1.x >= wirelist2[j].N2.x)
						{
							if (wirelist1[i].N2.x <= wirelist2[j].N2.x)
								length = wirelist1[i].N1.x - wirelist2[j].N2.x;
							else
								length = wirelist1[i].N1.x - wirelist1[i].N2.x;
						}//j-i
						else if (wirelist1[i].N2.x <= wirelist2[j].N1.x && wirelist1[i].N1.x >= wirelist2[j].N1.x)
						{
							if (wirelist2[j].N2.x <= wirelist1[i].N2.x)
								length = wirelist2[j].N1.x - wirelist1[i].N2.x;
							else length = wirelist2[j].N1.x - wirelist2[j].N2.x;

						}

					}
					else if (wirelist1[i].type == 5 && wirelist2[j].type == 5)
						if (wirelist1[i].N1.x == wirelist2[j].N1.x && wirelist1[i].N2.x == wirelist2[j].N2.x)
							length = wirelist1[i].metalwidth;
					if (wirelist1[i].type == 4 && wirelist2[j].type == 4)
					{
						//						cout<<"length444: "<<length<<endl;
						if (wirelist1[i].N1.y == wirelist2[j].N1.y && wirelist1[i].N2.y == wirelist2[j].N2.y)
						{
							length = wirelist1[i].metalwidth;
							//								cout<<"length: "<<length<<endl;
						}
					}
					//					cout<<"length: "<<length<<endl;
					if (length > 0)

					{
						isviolation(id, length, maxx, layerPRL,
							layerwidth, wirelist1[i], wirelist2[j], table);
						//						cout<<"length: "<<length<<endl;
					}
				}
			}


		}
}


void  RoutingSpace_3D::isviolation(int id, double actualprl, double actualwidth, vector<double> layerprl,
	vector <double > layerwidth, Wire& wire1, Wire& wire2, double table[4][10])
{
	double needspacing;
	int p = 3, q = 9;

	for (int i = 0; i < layerwidth.size(); i++)
		if (actualwidth < layerwidth[i])  //不可能小于第0个 
		{
			p = i - 1;
			break;
		}

	if (actualprl < layerprl[0])
		needspacing = table[p][0];
	else
	{
		for (int i = 0; i < layerprl.size(); i++)  //不小于第0个 
			if (actualprl < layerprl[i])
			{
				q = i - 1;  //空出一列 
				break;
			}
		needspacing = table[p][q];

	}
	double actualspacing;
	actualspacing = computespacing(wire1, wire2);
	//	cout<<"actualprl: "<< actualprl<<" "<<"actualspacing: "<<actualspacing<<endl;
	//	cout<<"needspacing: "<<needspacing<<endl;
	if (actualspacing < needspacing && actualspacing>0)
	{
		//		cout<<"write"<<endl;
		writemessage(wire1, wire2, actualprl, actualspacing);
		//		cout<<"prl: "<< actualprl<<" "<<"spacing: "<<actualspacing<<endl;
	}
}

void  RoutingSpace_3D::writemessage(Wire wire1, Wire wire2, double prl, double space)
{
	ofstream outfile("F:\\unityproject\\Deom-master\\Deom-master\\Demo\\Elastic_ball\\主要界面\\Assets\\StreamingAssets\\prlviolation.txt", ios::binary | ios::app | ios::in | ios::out);
	if (!outfile.is_open())
	{
		cout << "未成功打开文件" << endl;
	}
	int ctype;
	double width1, width2;
	if (wire1.wiretype)
		width1 = wire1.wirewidth;
	else
		width1 = wire1.metalwidth;

	if (wire2.wiretype)
		width2 = wire2.wirewidth;
	else
		width2 = wire2.metalwidth;
	ctype = judgewire(wire1, wire2);

	int z1, z2, z3, z4;
	double x1, x2, x3, x4, y1, y2, y3, y4;
	if (ctype == 1)
	{
		pair<Coordinate_3D, Coordinate_3D>  p;
		if (wire1.type == 4) //1m2m
		{
			x1 = wire1.N2.x;
			y1 = wire1.N2.y + 0.5 * wire1.metalwidth;
			z1 = wire1.N2.z;
			x2 = wire2.N2.x;
			y2 = wire2.N2.y + 0.5 * wire1.metalwidth;
			z2 = wire2.N2.z;

			if (isprlmm(mmpair, x1, y1, z1, x2, y2, z2))
			{
				outfile << ctype << endl;
				p.first.x = x1;
				p.first.y = y1;
				p.first.z = z1;
				p.second.x = x2;
				p.second.y = y2;
				p.second.z = z2;
				outfile << x1 << "\t" << y1 << "\t" << z1 << "\t" << x2 << "\t" << y2 << "\t" << z2 << endl;
				mmpair.push_back(p);
				outfile << width1 << "\t" << width2 << "\t" << prl << "\t" << space << endl;
			}

		}
		else if (wire1.type == 5)
		{
			x1 = wire1.N2.x + 0.5 * wire1.metalwidth;
			y1 = wire1.N2.y;
			z1 = wire1.N2.z;
			x2 = wire2.N2.x + 0.5 * wire1.metalwidth;
			y2 = wire2.N2.y;
			z2 = wire2.N2.z;
			if (isprlmm(mmpair, x1, y1, z1, x2, y2, z2))
			{
				outfile << ctype << endl;
				p.first.x = x1;
				p.first.y = y1;
				p.first.z = z1;
				p.second.x = x2;
				p.second.y = y2;
				p.second.z = z2;
				outfile << x1 << "\t" << y1 << "\t" << z1 << "\t" << x2 << "\t" << y2 << "\t" << z2 << endl;
				mmpair.push_back(p);
				outfile << width1 << "\t" << width2 << "\t" << prl << "\t" << space << endl;
			}
		}
	}
	else if (ctype == 2)  //1m2w
	{
		if (wire1.type == 4)
		{
			x1 = wire1.N2.x;
			y1 = wire1.N2.y + 0.5 * wire1.metalwidth;
			z1 = wire1.N2.z;
		}
		else if (wire1.type == 5)
		{
			x1 = wire1.N2.x + 0.5 * wire1.metalwidth;
			y1 = wire1.N2.y;
			z1 = wire1.N2.z;
		}
		x2 = wire2.N1.x;
		y2 = wire2.N1.y;
		z2 = wire2.N1.z;
		x3 = wire2.N2.x;
		y3 = wire2.N2.y;
		z3 = wire2.N2.z;
		outfile << ctype << endl;
		outfile << x1 << "\t" << y1 << "\t" << z1 << "\t" << x2 << "\t" << y2 << "\t" << z2
			<< "\t" << x3 << "\t" << y3 << "\t" << z3 << endl;
		outfile << width1 << "\t" << width2 << "\t" << prl << "\t" << space << endl;
	}
	else if (ctype == 3)  //1w2m
	{
		if (wire2.type == 4)
		{
			x3 = wire2.N2.x;
			y3 = wire2.N2.y + 0.5 * wire2.metalwidth;
			z3 = wire2.N2.z;
		}
		else if (wire2.type == 5)
		{
			x3 = wire2.N2.x + 0.5 * wire2.metalwidth;
			y3 = wire2.N2.y;
			z3 = wire2.N2.z;
		}
		x1 = wire1.N1.x;
		y1 = wire1.N1.y;
		z1 = wire1.N1.z;
		x2 = wire1.N2.x;
		y2 = wire1.N2.y;
		z2 = wire1.N2.z;
		outfile << ctype << endl;
		outfile << x1 << "\t" << y1 << "\t" << z1 << "\t" << x2 << "\t" << y2 << "\t" << z2
			<< "\t" << x3 << "\t" << y3 << "\t" << z3 << endl;
		outfile << width1 << "\t" << width2 << "\t" << prl << "\t" << space << endl;
	}
	else if (ctype == 4)
	{
		x1 = wire1.N1.x;
		y1 = wire1.N1.y;
		z1 = wire1.N1.z;
		x2 = wire1.N2.x;
		y2 = wire1.N2.y;
		z2 = wire1.N2.z;
		x3 = wire2.N1.x;
		y3 = wire2.N1.y;
		z3 = wire2.N1.z;
		x4 = wire2.N2.x;
		y4 = wire2.N2.y;
		z4 = wire2.N2.z;
		outfile << ctype << endl;
		outfile << x1 << "\t" << y1 << "\t" << z1 << "\t" << x2 << "\t" << y2 << "\t" << z2
			<< "\t" << x3 << "\t" << y3 << "\t" << z3 << "\t" << x4 << "\t" << y4 << "\t" << z4 << endl;
		outfile << width1 << "\t" << width2 << "\t" << prl << "\t" << space << endl;

	}
	outfile.close();
}