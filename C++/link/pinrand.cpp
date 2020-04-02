#include <iostream>
#include "base.h"
#include <vector>
#include<fstream>
#include <algorithm>
#include <stdlib.h>
#include <math.h>
#include<ctime>  
using namespace std;
void  RoutingSpace_3D::findcross(RoutingSpace_2D p0, RoutingSpace_2D p1, vector <Coordinate_3D>& crosspoint)
{
	double x, y, orginx, orginy;
	double routingwidthx, routingwidthy;
	int numx, numy;
	numx = p0.tracknum;
	routingwidthx = p0.RightUpPoint.x - p0.LeftDownPoint.x;
	x = routingwidthx / numx;
	orginx = x / 2;
	numy = p0.tracknum;
	routingwidthy = p0.RightUpPoint.y - p0.LeftDownPoint.y;
	y = routingwidthy / numy;
	orginy = y / 2;
	for (int i = 0; i * x + orginx < routingwidthx; i++)
		for (int j = 0; j * y + orginy < routingwidthy; j++)
		{
			Coordinate_3D point;
			point.x = i * x + orginx;
			point.y = j * y + orginy;
			point.z = 0;
			crosspoint.push_back(point);
		}

}


void  RoutingSpace_3D::findpin(vector <Pin>& pinlist, double finalspacing,
	vector <Coordinate_3D> crosspoint)
{
	int num;
	num = pinlist.size();
	for (int i = 0; i < crosspoint.size(); i++)
	{
		bool f;
		f = true;
		for (int j = 0; j < num; j++)
		{
			double dx, dy;
			dx = fabs(crosspoint[i].x - pinlist[j].pos.x);
			dy = fabs(crosspoint[i].y - pinlist[j].pos.y);
			if (dx < finalspacing && dy < finalspacing)
			{
				f = false;
				break;
			}
			//			double d;
			//			d=(crosspoint[i].x-pinlist[j].pos.x)*(crosspoint[i].x-pinlist[j].pos.x)+
			//			(crosspoint[i].y-pinlist[j].pos.y)*(crosspoint[i].y-pinlist[j].pos.y);
			//			if(d<finalspacing*finalspacing)
			//			{
			//				f=false;
			//				break;
			//			}
		}
		if (f)
		{
			Pin tem;
			tem.pos.x = crosspoint[i].x;
			tem.pos.y = crosspoint[i].y;
			tem.pos.z = 0;
			tem.pinwidth = (crosspoint[1].y - crosspoint[0].y) * 0.15;
			tem.type = 2;  //sink
			tem.used = false;
			bool haved = true;
			for (int k = 0; k < pinlist.size(); k++)
			{
				if (tem.pos.x == pinlist[k].pos.x && tem.pos.y == pinlist[k].pos.y)
				{
					haved = false;
					break;
				}
			}
			if (haved)
			{
				pinlist.push_back(tem);
				num++;
			}
		}
	}
}

void RoutingSpace_3D::writeallpin(vector <Pin> pinlist)
{
	int num;
	num = pinlist.size();
	ofstream outfile("F:\\unityproject\\Deom-master\\Deom-master\\Demo\\Elastic_ball\\��Ҫ����\\Assets\\StreamingAssets\\candidatepin.txt");
	if (!outfile.is_open())
	{
		cout << "δ�ɹ����ļ�" << endl;
	}
	double x, y;
	int z;
	outfile << num << endl;
	for (int i = 0; i < num; i++)
	{
		outfile << pinlist[i].pos.x << "\t" << pinlist[i].pos.y << "\t" << pinlist[i].pos.z << endl;
	}
	outfile.close();
}

void RoutingSpace_3D::readpin(int& netnum)
{
	ifstream infile("F:\\unityproject\\Deom-master\\Deom-master\\Demo\\Elastic_ball\\��Ҫ����\\Assets\\StreamingAssets\\pinmessage.txt");
	if (!infile.is_open())
	{
		cout << "δ�ɹ����ļ�" << endl;
	}
	infile >> netnum;

	infile.close();
}

void RoutingSpace_3D::randpin(int maxpinnum, int netnum, int pinnum,
	vector<vector<Pin>>& nets, vector<Pin>& pins)  // ��������������� �������� ��ѡ������
{
	ofstream outfile("F:\\unityproject\\Deom-master\\Deom-master\\Demo\\Elastic_ball\\��Ҫ����\\Assets\\StreamingAssets\\randpin.txt");
	if (!outfile.is_open())
	{
		cout << "δ�ɹ����ļ�" << endl;
	}
	srand((int)time(0));
	for (int i = 0; i < netnum; i++)
	{

		int maxxpin;  //ʵ��������������� 
		vector <Pin> singlenet;
		singlenet.clear();
		//		cout<<"�û�ѡ�������������� "<<maxpinnum<<endl;
		//		cout<<"��ѡ�������� "<<pinnum<<endl;

		//		int t;
		//		t=pinnum-2*(netnum-1)
		maxxpin = rand() % (maxpinnum - 1) + 2;  //2��maxpinnum
		outfile << maxxpin << endl;
		int number;
		cout << " ��" << i + 1 << "��������" << maxxpin << "������,�ֱ�Ϊ" << endl;
		for (int j = 0; j < maxxpin; j++)
		{
			//			srand((int)time(0));
			number = rand() % pinnum;  //0��size of pinlist
//			cout<<"number: "<<number<<endl;
//			cout<<" ��"<<i+1<<"��������"<<maxxpin<<"������,�ֱ�Ϊ"<<endl;
			if (!pins[number].used)
			{

				if (singlenet.size() == 0)
					pins[number].type = 1;//source
//				cout<<"  �Ƿ�ʹ�ù�:  "<< pins[number].used<<endl;
				singlenet.push_back(pins[number]);
				pins[number].used = true;
				//				cout<< "��"<<j+1<<"����������Ϊ��"<<pins[number].type<<endl;
				outfile << pins[number].pos.x << "\t" << pins[number].pos.y << "\t" << pins[number].pos.z << endl;
				cout << number << "����������Ϊ�� " << pins[number].pos.x << " " << pins[number].pos.y << " " << pins[number].pos.z << " " << endl;
				//				cout<<endl;
			}
			else
				j--;  //ע��  
		}

		//		cout<<endl<<endl;
		nets.push_back(singlenet);
		singlenet.clear();
		outfile << endl;
	}
	outfile.close();
}

