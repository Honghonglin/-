#include <iostream>
#include "base.h"
#include  <vector>
using namespace std;

void RoutingSpace_3D::fileEmpty(const char fileName[])
{
	fstream file(fileName, ios::out);
	file.close();
	return;
}

int  RoutingSpace_3D::judgewire(Wire wire1, Wire wire2)
{
	if (wire1.wiretype && wire2.wiretype)  //1w2w
		return 4;
	if (wire1.wiretype && !wire2.wiretype)  //1w2m
		return 3;
	if (!wire1.wiretype && wire2.wiretype)  //1m2w
		return 2;
	if (!wire1.wiretype && !wire2.wiretype)  //1m2m
		return 1;
}
bool  RoutingSpace_3D::isshort(Wire wire1, Wire wire2)
{
	int num;
	num = wire1.shortlist.size();
	for (int i = 0; i < num; i++)
	{
		if (wire2.N1.x == wire1.shortlist[i].first.x && wire2.N2.x == wire1.shortlist[i].second.x)
			if (wire2.N1.y == wire1.shortlist[i].first.y && wire2.N2.y == wire1.shortlist[i].second.y)
				if (wire2.N1.z == wire1.shortlist[i].first.z && wire2.N2.z == wire1.shortlist[i].second.z)
					return true;
	}
	int num1;
	num1 = wire2.shortlist.size();
	for (int i = 0; i < num1; i++)
	{
		if (wire1.N1.x == wire2.shortlist[i].first.x && wire1.N2.x == wire2.shortlist[i].second.x)
			if (wire1.N1.y == wire2.shortlist[i].first.y && wire1.N2.y == wire2.shortlist[i].second.y)
				if (wire1.N1.z == wire2.shortlist[i].first.z && wire1.N2.z == wire2.shortlist[i].second.z)
					return true;
	}
	return false;
}

double RoutingSpace_3D::minspacing(Wire wire1, Wire wire2)
{
	int id;
	id = wire1.layerid;
	return this->Plane_3D[id].minspacing * 0.6;
}

double RoutingSpace_3D::computespacing(Wire wire1, Wire wire2)
{
	int t; //1ww  2wm  3mw  4mm
	int id;
	id = wire1.layerid;
	if (wire1.type != 3 && wire2.type != 3)
	{
		if (wire1.wiretype && wire2.wiretype)  //1w2w
			return this->Plane_3D[id].minspacing * 0.8;
		if (wire1.wiretype && !wire2.wiretype)  //1w2m
			return this->Plane_3D[id].minspacing * 0.7;
		if (!wire1.wiretype && wire2.wiretype)  //1m2w
		{
			return this->Plane_3D[id].minspacing * 0.7;
		}
		if (!wire1.wiretype && !wire2.wiretype)  //1m2m
			return this->Plane_3D[id].minspacing * 0.6;
	}
}

double RoutingSpace_3D::maxwidth(Wire wire1, Wire wire2)
{
	if (wire1.wiretype && wire2.wiretype)  //1w2w
		return wire2.wirewidth;
	if (wire1.wiretype && !wire2.wiretype)  //1w2m
		return wire2.metalwidth;
	if (!wire1.wiretype && wire2.wiretype)  //1m2w
		return wire1.metalwidth;
	if (!wire1.wiretype && !wire2.wiretype)  //1m2m
		return wire1.metalwidth;

}

Wire RoutingSpace_3D::findbigger(Wire wire1, Wire wire2)
{
	if (wire1.wiretype == wire2.wiretype)
		return wire1;
	else
	{
		if (wire1.wiretype)
			return wire2;
		else if (wire2.wiretype)
			return wire1;
	}
}


Wire RoutingSpace_3D::findsmaller(Wire wire1, Wire wire2)
{
	if (wire1.wiretype == wire2.wiretype)
		return wire2; //注意和更大的不能是同一个导线 
	else
	{
		if (wire1.wiretype)
			return wire1;
		else if (wire2.wiretype)
			return wire2;
	}
}

bool RoutingSpace_3D::haveoverlap(vector<double>& rec1, vector<double>& rec2) {
	if ((rec1[0] >= rec2[0]) && (rec1[0] < rec2[2]))
	{
		if ((rec1[1] >= rec2[1]) && (rec1[1] < rec2[3]))
			return true;

		if ((rec1[3] > rec2[1]) && (rec1[3] <= rec2[3]))
			return true;

		if ((rec2[1] >= rec1[1]) && (rec2[1] < rec1[3]))
			return true;

		if ((rec2[3] > rec1[1]) && (rec2[3] <= rec1[3]))
			return true;
	}

	if ((rec1[2] > rec2[0]) && (rec1[2] <= rec2[2]))
	{
		if ((rec1[1] >= rec2[1]) && (rec1[1] < rec2[3]))
			return true;

		if ((rec1[3] > rec2[1]) && (rec1[3] <= rec2[3]))
			return true;

		if ((rec2[1] >= rec1[1]) && (rec2[1] < rec1[3]))
			return true;

		if ((rec2[3] > rec1[1]) && (rec2[3] <= rec1[3]))
			return true;
	}

	if ((rec2[0] >= rec1[0]) && (rec2[0] < rec1[2]))
	{
		if ((rec1[1] >= rec2[1]) && (rec1[1] < rec2[3]))
			return true;

		if ((rec1[3] > rec2[1]) && (rec1[3] <= rec2[3]))
			return true;

		if ((rec2[1] >= rec1[1]) && (rec2[1] < rec1[3]))
			return true;

		if ((rec2[3] > rec1[1]) && (rec2[3] <= rec1[3]))
			return true;
	}

	if ((rec2[2] > rec1[0]) && (rec2[2] <= rec1[2]))
	{
		if ((rec1[1] >= rec2[1]) && (rec1[1] < rec2[3]))
			return true;

		if ((rec1[3] > rec2[1]) && (rec1[3] <= rec2[3]))
			return true;

		if ((rec2[1] >= rec1[1]) && (rec2[1] < rec1[3]))
			return true;

		if ((rec2[3] > rec1[1]) && (rec2[3] <= rec1[3]))
			return true;
	}

	return false;
}
