#include <iostream>
#include "base.h"
#include  <vector>
#include<fstream>

using namespace std;
void RoutingSpace_3D::createtable(int id, double width, vector <pair <double, double> >& ctctable)
{
	pair<double, double>  p;
	for (int i = 1; i < 5; i++)
	{
		double ctcwidth, ctcspacing;
		ctcwidth = width * (i) / 2;
		ctcspacing = this->Plane_3D[id].defaultspace * (i) / 4;
		//		ctcwidth=0.01;
		//		ctcspacing=10000;
		p = make_pair(ctcwidth, ctcspacing);
		ctctable.push_back(p);
	}
}

void RoutingSpace_3D::writectc()
{
	ofstream outfile("F:\\unityproject\\Deom-master\\Deom-master\\Demo\\Elastic_ball\\主要界面\\Assets\\StreamingAssets\\ctcviolation.txt", ios::binary | ios::app | ios::in | ios::out);
	if (!outfile.is_open())
	{
		cout << "未成功打开文件" << endl;
	}
	string s;
	s = "according";
	outfile << s << endl;
	int sctc;
	sctc = this->ctctable.size();
	outfile << "2" << "\t" << sctc << endl;
	for (int i = 0; i < sctc; i++)
	{
		outfile << this->ctctable[i].first << "\t" << this->ctctable[i].second << endl;
	}
	s = "message";
	outfile << s << endl;
}

void RoutingSpace_3D::setspacing(int id, vector<Wire> wirelist1, vector<Wire> wirelist2, vector <pair <double, double> >& table, double& spacing)
{
	int num1 = wirelist1.size();
	int num2 = wirelist2.size();
	spacing = 0;
	for (int i = 0; i < num1; i++)
		for (int j = i + 1; j < num2; j++)
		{
			if ((!isshort(wirelist1[i], wirelist2[j])) && (!isequal(wirelist1[i], wirelist2[j])))
			{
				if (wirelist1[i].layerid == wirelist2[j].layerid && wirelist1[i].layerid == id &&
					wirelist1[i].layerid != 3 && wirelist2[j].layerid != 3)
				{
					double width;
					width = maxwidth(wirelist1[i], wirelist2[j]);
					//						cout<<"ctcwidth:  "<<width<<endl;
					int p = 3;
					for (int i = 0; i < 4; i++)
					{
						if (table[i].first > width)
						{
							p = i - 1;
							break;
						}
					}
					if (p < 0)
						spacing = table[0].second;
					else
						spacing = table[p].second;

					//						cout<<"ctcp:  "<<p<<endl;
					//						cout<<"ctcspacing:  "<<spacing<<endl; 
					Wire biggerwire, smallerwire;
					Violationregion  region1;
					Violationregion  region2;
					Violationregion  region3;
					Violationregion  region4;
					biggerwire = findbigger(wirelist1[i], wirelist2[j]);
					smallerwire = findsmaller(wirelist1[i], wirelist2[j]);
					createregion(id, biggerwire, smallerwire, region1, region2,
						region3, region4, width, spacing);  //和第二个约束相同 
				}
			}
		}
}


void RoutingSpace_3D::createregion(int id, Wire wire1, Wire wire2, Violationregion& region1, Violationregion& region2,
	Violationregion& region3, Violationregion& region4, double width, double spacing)
{
	//1右上，2左上，3右下，4左下 
	////1代表垂直，2水平，3通孔,4金属片垂直，5金属片水平

	if (wire1.layerid == id && wire2.layerid == id)
	{
		if (wire1.type != 3)
		{
			if (wire1.type == 1 || wire1.type == 4) //垂直
			{
				region1.leftlower.x = wire1.N1.x + 0.5 * width;
				region1.leftlower.y = wire1.N1.y;
				region1.rightup.x = wire1.N1.x + 0.5 * width + spacing;
				region1.rightup.y = wire1.N1.y + spacing;//右上 

				region2.leftlower.x = wire1.N1.x - 0.5 * width - spacing;
				region2.leftlower.y = wire1.N1.y;
				region2.rightup.x = wire1.N1.x - 0.5 * width;
				region2.rightup.y = wire1.N1.y + spacing;//左上 

				region3.leftlower.x = wire1.N2.x + 0.5 * width;
				region3.leftlower.y = wire1.N2.y - spacing;
				region3.rightup.x = wire1.N2.x + 0.5 * width + spacing;
				region3.rightup.y = wire1.N2.y;//右下 

				region4.leftlower.x = wire1.N2.x - 0.5 * width - spacing;
				region4.leftlower.y = wire1.N2.y - spacing;
				region4.rightup.x = wire1.N2.x - 0.5 * width;
				region4.rightup.y = wire1.N2.y;//左下 

//				cout<<region1.leftlower.x<<endl;
//				cout<<region1.leftlower.y<<endl;
//				cout<<region1.rightup.x<<endl;
//				cout<<region1.rightup.y<<endl;//右上 
//				cout<<endl;
//				cout<<region2.leftlower.x<<endl;
//				cout<<region2.leftlower.y<<endl;
//				cout<<region2.rightup.x<<endl;
//				cout<<region2.rightup.y<<endl;//左上 
//				cout<<endl;
//				cout<<region3.leftlower.x<<endl;
//				cout<<region3.leftlower.y<<endl;
//				cout<<region3.rightup.x<<endl;
//				cout<<region3.rightup.y<<endl;//右下 
//				cout<<endl;
//				cout<<region4.leftlower.x<<endl;
//				cout<<region4.leftlower.y<<endl;
//				cout<<region4.rightup.x<<endl;
//				cout<<region4.rightup.y<<endl;//左下 
			}
			if (wire1.type == 2 || wire1.type == 5) //水平 
			{
				region1.leftlower.x = wire1.N1.x;
				region1.leftlower.y = wire1.N1.y + 0.5 * width;
				region1.rightup.x = wire1.N1.x + spacing;
				region1.rightup.y = wire1.N1.y + 0.5 * width + spacing;//右上 

				region2.leftlower.x = wire1.N2.x - spacing;
				region2.leftlower.y = wire1.N2.y + 0.5 * width;
				region2.rightup.x = wire1.N2.x;
				region2.rightup.y = wire1.N2.y + spacing + 0.5 * width;//左上 

				region3.leftlower.x = wire1.N1.x;
				region3.leftlower.y = wire1.N1.y - spacing - 0.5 * width;
				region3.rightup.x = wire1.N1.x + spacing;
				region3.rightup.y = wire1.N1.y - 0.5 * width;//右下 

				region4.leftlower.x = wire1.N2.x - spacing;
				region4.leftlower.y = wire1.N2.y - spacing - 0.5 * width;
				region4.rightup.x = wire1.N2.x;
				region4.rightup.y = wire1.N2.y - 0.5 * width;//左下 
			}
			double prl;
			haveprl(id, wire1, wire2, region1, region2,
				region3, region4);

		}
	}
}
void RoutingSpace_3D::haveprl(int id, Wire& wire1, Wire& wire2, Violationregion& region1, Violationregion& region2,
	Violationregion& region3, Violationregion& region4)
{

	double length;
	bool f;
	f = false;
	if (wire2.layerid == wire1.layerid && wire2.layerid == id && wire1.type != 3 && wire2.type != 3)
	{
		if ((wire1.type == 1 && wire2.type == 4) || (wire1.type == 4 && wire2.type == 1) ||
			(wire1.type == 1 && wire2.type == 1))
		{
			if (wire1.N1.y <= wire2.N1.y && wire1.N1.y >= wire2.N2.y)
			{
				f = true;
			}//j-i
			else if (wire1.N1.y >= wire2.N1.y && wire1.N2.y <= wire2.N1.y)
			{
				f = true;
			}
		}
		else if ((wire1.type == 2 && wire2.type == 5) || (wire1.type == 5 && wire2.type == 2)
			|| (wire1.type == 2 && wire2.type == 2))
		{
			if (wire1.N1.x <= wire2.N1.x && wire1.N1.x >= wire2.N2.x)
			{
				f = true;
			}//j-i
			else if (wire1.N2.x <= wire2.N1.x && wire1.N1.x >= wire2.N1.x)
			{
				f = true;
			}

		}
		else if (wire1.type == 5 && wire2.type == 5)
			if (wire1.N1.x == wire2.N1.x && wire1.N2.x == wire2.N2.x)
				f = true;
		if (wire1.type == 4 && wire2.type == 4)
			if (wire1.N1.y == wire2.N1.y && wire1.N2.y == wire2.N2.y)
				f = true;
	}
	if (!f)
	{
		vector<Violationregion>  region;
		region.push_back(region1);
		region.push_back(region2);
		region.push_back(region3);
		region.push_back(region4);
		bool ctcflag = false;
		for (int k = 0; k < 4; k++)
		{

			double arr1[4], arr2[4];
			double width;
			if (wire2.type == 1 || wire2.type == 2)
				width = wire2.wirewidth;
			else if (wire2.type == 4 || wire2.type == 5)
				width = wire2.metalwidth;
			arr1[0] = region[k].leftlower.x;
			arr1[1] = region[k].leftlower.y;
			arr1[2] = region[k].rightup.x;
			arr1[3] = region[k].rightup.y;
			if (wire2.type == 1 || wire2.type == 4)
			{
				arr2[0] = wire2.N2.x - 0.5 * width;
				arr2[1] = wire2.N2.y;
				arr2[2] = wire2.N1.x + 0.5 * width;
				arr2[3] = wire2.N1.y;
			}
			else if (wire2.type == 2 || wire2.type == 5)
			{
				arr2[0] = wire2.N2.x;
				arr2[1] = wire2.N2.y - 0.5 * width;
				arr2[2] = wire2.N1.x;
				arr2[3] = wire2.N1.y + 0.5 * width;
			}
			//				for(int b=0;b<4;b++)
			//				{
			//					cout<<arr2[b]<<endl;
			//				}
			vector <double> r1;
			vector <double> r2;
			for (int p = 0; p < 4; p++)
			{
				r1.push_back(arr1[p]);
				r2.push_back(arr2[p]);
			}
			ctcflag = haveoverlap(r1, r2);
			r1.clear();
			r2.clear();
			if (ctcflag)
			{
				//			 		for(int c=0;c<4;c++)
				//			 		{
				//			 			cout<<r1[c]<<"  ";
				//					 }
				//					cout<<endl;
				//					for(int c=0;c<4;c++)
				//			 		{
				//			 			cout<<r2[c]<<"  ";
				//					 }
				//					cout<<endl;
				break;
			}
			//				else
			//					cout<<ctcflag<<endl;
		}
		//			cout<<"ctcflag:  "<<ctcflag<<endl;
		if (ctcflag && wire1.type != 3 && wire2.type != 3)
		{
			if (wire1.type == 1 || wire1.type == 4)
				if (wire2.type == 1 || wire2.type == 4)
					writectcmessage(wire1, wire2);
		}
		if (ctcflag && wire1.type != 3 && wire2.type != 3)
		{
			if (wire1.type == 2 || wire1.type == 5)
				if (wire2.type == 2 || wire2.type == 5)
					writectcmessage(wire1, wire2);
		}
	}

}


void RoutingSpace_3D::writectcmessage(Wire wire1, Wire wire2)
{
	ofstream outfile("F:\\unityproject\\Deom-master\\Deom-master\\Demo\\Elastic_ball\\主要界面\\Assets\\StreamingAssets\\ctcviolation.txt", ios::binary | ios::app | ios::in | ios::out);
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