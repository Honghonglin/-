#include <iostream>
#include "base.h"
#include  <vector>
#include<fstream>
#include<math.h>
using namespace std;
void RoutingSpace_3D::setwithin(int layerid,double spacing,double & within)
{
	within=2*this->Plane_3D[layerid].minspacing;
//	within=85;
}

void RoutingSpace_3D::setadjspacing(int layerid,double spacing,double & adjspacing)
{
	adjspacing=this->Plane_3D[layerid].minspacing; 
//	adjspacing=85;
}

void RoutingSpace_3D::setnum(int layerid,int & num)
{
	num=3;
	
}

void RoutingSpace_3D::writecuts()
{
	ofstream outfile("F:\\unityproject\\Deom-master\\Deom-master\\Demo\\Elastic_ball\\主要界面\\Assets\\StreamingAssets\\cutsviolation.txt",ios::binary | ios::app | ios::in | ios::out);	
	if (!outfile.is_open()) 
	{ 
	    cout << "未成功打开文件" << endl; 
	}
	string s;
	s="according";
	outfile<<s<<endl;
	outfile<<this->within<<"\t"<<this->adjspacing<<"\t"<<this->num<<endl;
	s="message";
	outfile<<s<<endl;
	outfile.close() ;
}

void RoutingSpace_3D::computesum(int id ,Wire wire,vector <Wire>& wirelist2,
double cutwithin,vector<Wire> & cutwires,int & sum)
{

		for(int j=0;j<wirelist2.size();j++)   
		{
			if(wire.type==3&&wirelist2[j].type==3&&wire.layerid==id &&wirelist2[j].layerid==id)
			{
				if(!isshort(wire,wirelist2[j]))
				{
					double d;
					d=(wire.N1.x-wirelist2[j].N1.x )*(wire.N1.x-wirelist2[j].N1.x )
					+(wire.N1.y-wirelist2[j].N1.y )*(wire.N1.y-wirelist2[j].N1.y );
					if(d<cutwithin*cutwithin)
					{
						sum++;
						if(d!=0)
							cutwires.push_back(wirelist2[j]); 
					}
				}
			}
		}
}

void RoutingSpace_3D::iscutviolation(int i,Wire wire,vector <Wire> wirelist,
double adjspacing,int num,int sum,vector< pair<Wire,Wire> > & temwire)
{	
	
	if(sum>=num)
		for(int j=0;j<wirelist.size();j++)
		{
			if(wirelist[j].layerid==i)
			{
				double d;
				pair <Wire,Wire> pairwire;	
				d=(wire.N1.x-wirelist[j].N1.x )*(wire.N1.x-wirelist[j].N1.x )
				+(wire.N1.y-wirelist[j].N1.y )*(wire.N1.y-wirelist[j].N1.y );
				if(d<adjspacing*adjspacing)
				{
					
					if (!checkwire(wire,wirelist[j],temwire))
					{
						pairwire=make_pair(wire,wirelist[j]);
//						cout<<"wire is: "<<pairwire.first.N1.x<<"  "
//						<<pairwire.first.N1.y<<"  "
//						<<pairwire.first.N1.z<<"  "<<endl;
						temwire.push_back(pairwire); 
						writecutsmessage(wire,wirelist[j]);
					}
				}
			}
		}
}

void RoutingSpace_3D::writecutsmessage(Wire wire1,Wire wire2)
{
//	cout<<"have a wire "<<endl;
	double x1,x2,y1,y2,x3,x4,y3,y4;
	int z1,z2,z3,z4;
	x1=wire1.N1.x;
	y1=wire1.N1.y;
	z1=wire1.N1.z;
	x2=wire1.N2.x;
	y2=wire1.N2.y;
	z2=wire1.N2.z;
	
	x3=wire2.N1.x;
	y3=wire2.N1.y;
	z3=wire2.N1.z;
	x4=wire2.N2.x;
	y4=wire2.N2.y;
	z4=wire2.N2.z;
	ofstream outfile("F:\\unityproject\\Deom-master\\Deom-master\\Demo\\Elastic_ball\\主要界面\\Assets\\StreamingAssets\\cutsviolation.txt",ios::binary | ios::app | ios::in | ios::out);	
	if (!outfile.is_open()) 
	{ 
	    cout << "未成功打开文件" << endl; 
	}
	double d;
	d=sqrt( (x1-x3)*(x1-x3)+(y1-y3)*(y1-y3) );
	outfile << "4" << endl;
	outfile<<x1<<"\t"<<y1<<"\t"<<z1<<"\t"<<x2<<"\t"<<y2<<"\t"<<z2<<"\t"
	<<x3<<"\t"<<y3<<"\t"<<z3<<"\t"<<x4<<"\t"<<y4<<"\t"<<z4<<"\t"<<endl 
	<<d<<endl;
	
}
