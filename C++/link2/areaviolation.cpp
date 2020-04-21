#include <iostream>
#include "base.h"
#include  <vector>
#include<fstream>
 
using namespace std;
void RoutingSpace_3D::setarea(int id,double width,double & area)
{
	area=width*width;
//	area=10000;
}

void RoutingSpace_3D::writearea()
{
	ofstream outfile("F:\\unityproject\\Deom-master\\Deom-master\\Demo\\Elastic_ball\\主要界面\\Assets\\StreamingAssets\\areaviolation.txt",ios::binary | ios::app | ios::in | ios::out);
	if (!outfile.is_open()) 
    { 
        cout << "未成功打开文件" << endl; 
    }
    string s;
	s="according";
	outfile << s<<endl;
	outfile << area << endl;
	s="message";
	outfile<<s<<endl;
}

void RoutingSpace_3D::isareaviolation(int id,vector<Wire> & wirelist,double area,vector <Coordinate_3D> & temmental)
{
	double s,width,length;
	for(int i=0;i<wirelist.size() ;i++)
	{
		if(wirelist[i].layerid==id&&wirelist[i].type!=3)
		{
			if(wirelist[i].type==4||wirelist[i].type==5)
			{
				width=wirelist[i].metalwidth;
				length=wirelist[i].metalwidth;
			}
			
			else if (wirelist[i].type==1)
			{
				width=wirelist[i].wirewidth;
				length=wirelist[i].N1.y-wirelist[i].N2.y;
			}
			else if (wirelist[i].type==2)
			{
				width=wirelist[i].wirewidth;
				length=wirelist[i].N1.x-wirelist[i].N2.x;
			}
			s=width*length;
			if(s<area)
			{
				writeareamessage(wirelist[i],temmental);
//				cout<<id<<"  "<<wirelist[i].type<<"  "<<area<<"  "<<width<<"  "<<length<<"  "<<s<<endl; 
			}
		} 
	}
}

void RoutingSpace_3D::writeareamessage(Wire wire1,vector <Coordinate_3D> & temmental)
{
	ofstream outfile("F:\\unityproject\\Deom-master\\Deom-master\\Demo\\Elastic_ball\\主要界面\\Assets\\StreamingAssets\\areaviolation.txt",ios::app|ios::out);
	if (!outfile.is_open()) 
    { 
        cout << "未成功打开文件" << endl; 
    }
    double x,y;
    int z;
    if(wire1.wiretype)
    	outfile<<"1"<<endl;
    else if(!wire1.wiretype)
    	outfile<<"2"<<endl;
	if (wire1.type==4)  //金属片垂直  
	{
		x=wire1.N1.x;
		y=wire1.N1.y-0.5*wire1.metalwidth;
		z=wire1.N1.z;
		if(areamental(x,y,z,temmental))
		{
			Coordinate_3D tem;
			tem.x=x;
			tem.y=y;
			tem.z=z;
			temmental.push_back(tem);
			outfile<<x<<"\t"<<y<<"\t"<<z<<endl;
			outfile<<wire1.metalwidth<<endl;
		}
		
	}
	else if(wire1.type==5)
	{
		x=wire1.N1.x-0.5*wire1.metalwidth;
		y=wire1.N1.y;
		z=wire1.N1.z;
		if(areamental(x,y,z,temmental))
		{
			Coordinate_3D tem;
			tem.x=x;
			tem.y=y;
			tem.z=z;
			temmental.push_back(tem);
			outfile<<x<<"\t"<<y<<"\t"<<z<<endl;
			outfile<<wire1.metalwidth<<endl;
		}
		
	}
	else if(wire1.type==1||wire1.type==2)
	{
		outfile<<wire1.N1.x<<"\t"<<wire1.N1.y<<"\t"<<wire1.N1.z<<"\t"
		<<wire1.N2.x<<"\t"<<wire1.N2.y<<"\t"<<wire1.N2.z<<endl;
		outfile<<wire1.wirewidth<<endl;
	}
	outfile<<endl;
	outfile.close() ;
}


