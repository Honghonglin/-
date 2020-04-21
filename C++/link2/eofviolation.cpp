#include <iostream>
#include "base.h"
#include  <vector>
#include<fstream>

using namespace std;
void RoutingSpace_3D::seteofvalue(int id, vector<Wire> wirelist1, vector<Wire> wirelist2,
	double& eofwidth, double& eofwithin, double& eofspace)
{
	int num1 = wirelist1.size();
	int num2 = wirelist2.size();
	double spacing1 = this->Plane_3D[id].defaultspace * 0.5;
	//	cout<<"spacing: "<<spacing1<<endl;
	eofwithin = spacing1 * 0.9;
	eofspace = spacing1 * 1.1;
	for (int i = 0; i < num1; i++)
		for (int j = i + 1; j < num2; j++)
		{
			if ((!isshort(wirelist1[i], wirelist2[j])) && (!isequal(wirelist1[i], wirelist2[j])))
			{
				if (wirelist1[i].layerid == wirelist2[j].layerid && wirelist1[i].layerid == id &&
					wirelist1[i].type != 3 && wirelist2[j].type != 3)
				{

					double width = maxwidth(wirelist1[i], wirelist2[j]);
					eofwidth = width;

					Violationregion  region1;
					Violationregion  region2;
					Wire biggerwire, smallerwire;
					smallerwire = findsmaller(wirelist1[i], wirelist2[j]);
					biggerwire = findbigger(wirelist1[i], wirelist2[j]);//由它产生违反的区域
					//更大导线产生的违反区域更大，因此只要检测这个更大的导线 
					createregion(biggerwire, smallerwire, region1,
						region2, eofwidth, eofwithin, eofspace);
				}
			}
		}
}


void RoutingSpace_3D::createregion(Wire wire1, Wire wire2, Violationregion& region1,
	Violationregion& region2, double eofwidth, double eofwithin, double eofspace)
{
	if (wire1.type != 3)
	{
		vector<double> v1;   //违反区域 
		vector<double> v2;  //违反区域 
		vector<double> v3;	//更小的导线 
		if (wire1.type == 1 || wire1.type == 4)//1垂直，2水平
		//金属片同时加入两个方向 
		{
			//					cout<<"当前导线的eolspace: "<<eofspace<<endl
			//					<<"当前导线的eolwithin: "<<eofwithin<<endl
			//					<<"当前导线的eolwidth: "<<eofwidth<<endl;

			region1.leftlower.x = wire1.N1.x - 0.5 * eofwidth - eofwithin;
			region1.leftlower.y = wire1.N1.y;
			region1.rightup.x = wire1.N1.x + 0.5 * eofwidth + eofwithin;
			region1.rightup.y = wire1.N1.y + eofspace;
			//上方的区域 n1
			region2.leftlower.x = wire1.N2.x - 0.5 * eofwidth - eofwithin;
			region2.leftlower.y = wire1.N2.y - eofspace;
			region2.rightup.x = wire1.N2.x + 0.5 * eofwidth + eofwithin;
			region2.rightup.y = wire1.N2.y;//下方的区域 n2
			v1.push_back(region1.leftlower.x);
			v1.push_back(region1.leftlower.y);
			v1.push_back(region1.rightup.x);
			v1.push_back(region1.rightup.y);
			v2.push_back(region2.leftlower.x);
			v2.push_back(region2.leftlower.y);
			v2.push_back(region2.rightup.x);
			v2.push_back(region2.rightup.y);

			if (!wire2.wiretype)
			{


				v3.push_back(wire2.N2.x);
				v3.push_back(wire2.N2.y - 0.5 * eofwidth);
				v3.push_back(wire2.N1.x);
				v3.push_back(wire2.N1.y + 0.5 * eofwidth);

			}
			else if (wire2.wiretype)
			{
				double w;
				w = wire2.wirewidth;

				v3.push_back(wire2.N2.x - 0.5 * w);
				v3.push_back(wire2.N2.y);
				v3.push_back(wire2.N1.x + 0.5 * w);
				v3.push_back(wire2.N1.y);

			}

			bool flag1, flag2;
			flag1 = haveoverlap(v1, v3);
			flag2 = haveoverlap(v2, v3);
			//					cout<<"flag1:"<<flag1<<endl;
			//					cout<<"flag2:"<<flag2<<endl;
			if ((flag1 || flag2) && (wire2.type == 4 || wire2.type == 1))
			{
				//						cout<<"11111write111111"<<endl;
				writeeofmessage(wire1, wire2);

				//						cout<<"region1的坐标为： "<<endl;
				//						for(int i=0;i<4;i++)
				//						{
				//							cout<<v1[i]<<" ";
				//						 }
				//						cout<<endl; 
				//						cout<<"region2的坐标为： "<<endl;
				//						for(int i=0;i<4;i++)
				//						{
				//							cout<<v2[i]<<" ";
				//						 }
				//						cout<<endl;
				//						cout<<"region3的坐标为： "<<endl;
				//						for(int i=0;i<4;i++)
				//						{
				//							cout<<v3[i]<<" ";
				//						 }
				//						cout<<endl<<endl;

			}
			v1.clear();
			v2.clear();
			v3.clear();
		}
		if (wire1.type == 2 || wire1.type == 5)
		{

			region1.leftlower.x = wire1.N1.x;
			region1.leftlower.y = wire1.N1.y - 0.5 * eofwidth - eofwithin;
			region1.rightup.x = wire1.N1.x + eofspace;
			region1.rightup.y = wire1.N1.y + 0.5 * eofwidth + eofwithin;//右边的区域 n1
			region2.leftlower.x = wire1.N2.x - eofspace;
			region2.leftlower.y = wire1.N2.y - 0.5 * eofwidth - eofwithin;
			region2.rightup.x = wire1.N2.x;
			region2.rightup.y = wire1.N2.y + 0.5 * eofwidth + eofwithin;//左边的区域 n2
			v1.push_back(region1.leftlower.x);
			v1.push_back(region1.leftlower.y);
			v1.push_back(region1.rightup.x);
			v1.push_back(region1.rightup.y);
			v2.push_back(region2.leftlower.x);
			v2.push_back(region2.leftlower.y);
			v2.push_back(region2.rightup.x);
			v2.push_back(region2.rightup.y);
			if (!wire2.wiretype)
			{
				v3.push_back(wire2.N2.x - 0.5 * eofwidth);
				v3.push_back(wire2.N2.y);
				v3.push_back(wire2.N1.x + 0.5 * eofwidth);
				v3.push_back(wire2.N1.y);
			}
			else if (wire2.wiretype)
			{
				double w;
				w = wire2.wirewidth;
				v3.push_back(wire2.N2.x);
				v3.push_back(wire2.N2.y - 0.5 * w);
				v3.push_back(wire2.N1.x);
				v3.push_back(wire2.N1.y + 0.5 * w);
			}
			bool flag1, flag2;
			flag1 = haveoverlap(v1, v3);
			flag2 = haveoverlap(v2, v3);
			if ((flag1 || flag2) && (wire2.type == 2 || wire2.type == 5))
			{
				//						cout<<"22222write2222"<<endl;
				writeeofmessage(wire1, wire2);
			}
			v1.clear();
			v2.clear();
			v3.clear();
		}

	}

}


void RoutingSpace_3D::writeeof(int i, vector < RoutingSpace_2D > p)
{
	ofstream outfile("F:\\unityproject\\Deom-master\\Deom-master\\Demo\\Elastic_ball\\主要界面\\Assets\\StreamingAssets\\eofviolation.txt", ios::binary | ios::app | ios::in | ios::out);
	if (!outfile.is_open())
	{
		cout << "未成功打开文件" << endl;
	}
	string s;
	s = "according";
	outfile << s << endl;
	double wwidth, wwithin, wspace;
	wwidth = p[i].defaultspace * 0.5;

	wwithin = wwidth * 0.9;
	wspace = wwidth * 1.1;

	outfile << wwithin << "\t" << wspace << endl;
	s = "message";
	outfile << s << endl;
}

void  RoutingSpace_3D::writeeofmessage(Wire wire1, Wire wire2)
{
	ofstream outfile("F:\\unityproject\\Deom-master\\Deom-master\\Demo\\Elastic_ball\\主要界面\\Assets\\StreamingAssets\\eofviolation.txt", ios::binary | ios::app | ios::in | ios::out);
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
				outfile << width1 << "\t" << width2 << endl;
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
				outfile << width1 << "\t" << width2 << endl;
			}
		}
	}
	else if (ctype == 2)  //1m2w
	{
		outfile << ctype << endl;
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
		outfile << x1 << "\t" << y1 << "\t" << z1 << "\t" << x2 << "\t" << y2 << "\t" << z2
			<< "\t" << x3 << "\t" << y3 << "\t" << z3 << endl;
		outfile << width1 << "\t" << width2 << endl;
	}
	else if (ctype == 3)  //1w2m
	{
		outfile << ctype << endl;
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
		outfile << x1 << "\t" << y1 << "\t" << z1 << "\t" << x2 << "\t" << y2 << "\t" << z2
			<< "\t" << x3 << "\t" << y3 << "\t" << z3 << endl;
		outfile << width1 << "\t" << width2 << endl;
	}
	else if (ctype == 4)
	{
		outfile << ctype << endl;
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
		outfile << x1 << "\t" << y1 << "\t" << z1 << "\t" << x2 << "\t" << y2 << "\t" << z2
			<< "\t" << x3 << "\t" << y3 << "\t" << z3 << "\t" << x4 << "\t" << y4 << "\t" << z4 << endl;
		outfile << width1 << "\t" << width2 << endl;

	}
	outfile.close();
}