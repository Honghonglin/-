#include <iostream>
#include "base.h"
#include  <vector>
#include <math.h>
using namespace std;
bool  RoutingSpace_3D::isequal(Wire wire1, Wire wire2)
{
	if (wire1.wiretype && !wire2.wiretype)  //1导线2金属片 
	{
		double x, y;
		int z;
		z = wire1.N2.z;
		if (wire2.type == 4)
		{
			x = wire2.N1.x;
			y = wire2.N1.y - 0.5 * this->Plane_3D[z].minmetalwidth;

			if ((wire1.N1.x == x && wire1.N1.y == y && wire1.N1.z == z) ||
				(wire1.N2.x == x && wire1.N2.y == y && wire1.N2.z == z))
				return true;
		}
		else if (wire2.type == 5)
		{
			x = wire2.N1.x - 0.5 * this->Plane_3D[z].minmetalwidth;
			y = wire2.N1.y;

			if ((wire1.N1.x == x && wire1.N1.y == y && wire1.N1.z == z) ||
				(wire1.N2.x == x && wire1.N2.y == y && wire1.N2.z == z))
				return true;
		}

	}

	if (wire2.wiretype && !wire1.wiretype)
	{
		double x, y;
		int z;
		z = wire2.N2.z;
		if (wire1.type == 4)
		{
			x = wire1.N1.x;
			y = wire1.N1.y - 0.5 * this->Plane_3D[z].minmetalwidth;

			if ((wire2.N1.x == x && wire2.N1.y == y && wire2.N1.z == z)
				|| (wire2.N2.x == x && wire2.N2.y == y && wire2.N2.z == z))
				return true;
		}
		else if (wire1.type == 5)
		{
			x = wire1.N1.x - 0.5 * this->Plane_3D[z].minmetalwidth;
			y = wire1.N1.y;

			if ((wire2.N1.x == x && wire2.N1.y == y && wire2.N1.z == z) ||
				(wire2.N2.x == x && wire2.N2.y == y && wire2.N2.z == z))
				return true;
		}

	}
	return false;
}


bool  RoutingSpace_3D::isconnect(Wire wire1, Wire wire2)
{
	if (wire1.wiretype && !wire2.wiretype)
	{
		double x, y;
		int z;
		z = wire1.N2.z;
		if (wire2.type == 4)
		{
			x = wire2.N1.x;
			y = wire2.N1.y - 0.5 * this->Plane_3D[z].minmetalwidth;
			if ((wire1.N1.x == x && wire1.N1.y >= y && y >= wire1.N2.y && wire1.N1.z == z))
				return true;
		}
		else if (wire2.type == 5)
		{
			x = wire2.N1.x - 0.5 * this->Plane_3D[z].minmetalwidth;
			y = wire2.N1.y;
			if ((wire1.N1.x >= x && x >= wire1.N2.x && wire1.N1.y == y && wire1.N1.z == z))
				return true;
		}

	}

	if (wire2.wiretype && !wire1.wiretype)
	{
		double x, y;
		int z;
		z = wire2.N2.z;
		if (wire1.type == 4)
		{
			x = wire1.N1.x;
			y = wire1.N1.y - 0.5 * this->Plane_3D[z].minmetalwidth;
			if ((wire2.N1.x == x && wire2.N1.y >= y && y >= wire2.N2.y && wire2.N1.z == z))
				return true;
		}
		else if (wire1.type == 5)
		{
			x = wire1.N1.x - 0.5 * this->Plane_3D[z].minmetalwidth;
			y = wire1.N1.y;
			if ((wire2.N1.x >= x && x >= wire2.N2.x && wire2.N1.y == y && wire2.N1.z == z))
				return true;
		}

	}
	return false;
}
bool  RoutingSpace_3D::isprlmm(vector< pair<Coordinate_3D, Coordinate_3D> > mmpair, double x1, double y1, double z1
	, double x2, double y2, double z2)
{
	for (int i = 0; i < mmpair.size(); i++)
	{
		if (x1 == mmpair[i].first.x && y1 == mmpair[i].first.y && z1 == mmpair[i].first.z)
			if (x2 == mmpair[i].second.x && y2 == mmpair[i].second.y && z2 == mmpair[i].second.z)
				return false;
	}
	return true;
}

bool  RoutingSpace_3D::areamental(double x, double y, int z, vector <Coordinate_3D>  temmental)
{
	for (int i = 0; i < temmental.size(); i++)
	{
		if (x == temmental[i].x && z == temmental[i].z && y == temmental[i].y)
			return false;
	}
	return true;
}

bool  RoutingSpace_3D::checkwire(Wire wire1, Wire wire2, vector< pair<Wire, Wire> > wirelist)
{
	if (wirelist.size() != 0)
	{
		for (int i = 0; i < wirelist.size(); i++)
		{
			if (wire1.N1.x == wirelist[i].second.N1.x && wire1.N1.y == wirelist[i].second.N1.y && wire1.N1.z == wirelist[i].second.N1.z)
				if (wire1.N2.x == wirelist[i].second.N2.x && wire1.N2.y == wirelist[i].second.N2.y && wire1.N2.z == wirelist[i].second.N2.z)
					if (wire2.N1.x == wirelist[i].first.N1.x && wire2.N1.y == wirelist[i].first.N1.y && wire2.N1.z == wirelist[i].first.N1.z)
						if (wire2.N2.x == wirelist[i].first.N2.x && wire2.N2.y == wirelist[i].first.N2.y && wire2.N2.z == wirelist[i].first.N2.z)
							return true;
			if (wire1.N1.x == wirelist[i].first.N1.x && wire1.N1.y == wirelist[i].first.N1.y && wire1.N1.z == wirelist[i].first.N1.z)
				if (wire1.N2.x == wirelist[i].first.N2.x && wire1.N2.y == wirelist[i].first.N2.y && wire1.N2.z == wirelist[i].first.N2.z)
					if (wire2.N1.x == wirelist[i].second.N1.x && wire2.N1.y == wirelist[i].second.N1.y && wire2.N1.z == wirelist[i].second.N1.z)
						if (wire2.N2.x == wirelist[i].second.N2.x && wire2.N2.y == wirelist[i].second.N2.y && wire2.N2.z == wirelist[i].second.N2.z)
							return true;
		}
	}
	return false;
}

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
	return this->Plane_3D[id].minspacing * 0.8;
}

double RoutingSpace_3D::computespacing(Wire wire1, Wire wire2)
{
	int t; //1ww  2wm  3mw  4mm
	int id;
	id = wire1.layerid;
	double l, s;
	if (wire1.type == 4 && wire2.type == 4)
		l = fabs(wire1.N1.x - wire2.N1.x);
	if (wire1.type == 5 && wire2.type == 5)
		l = fabs(wire1.N1.y - wire2.N1.y);
	if ((wire1.type == 1 && wire2.type == 4) || (wire1.type == 1 && wire2.type == 1)
		|| (wire1.type == 4 && wire2.type == 1))
		l = fabs(wire1.N1.x - wire2.N1.x);
	if ((wire1.type == 2 && wire2.type == 5) || (wire1.type == 5 && wire2.type == 2)
		|| (wire1.type == 2 && wire2.type == 2))
		l = fabs(wire1.N1.y - wire2.N1.y);
	if (wire1.type != 3 && wire2.type != 3)
	{
		if (wire1.wiretype && wire2.wiretype)  //1w2w
			s = l - this->Plane_3D[id].minwirewidth;
		if (wire1.wiretype && !wire2.wiretype)  //1w2m
			s = l - 0.5 * (this->Plane_3D[id].minwirewidth + this->Plane_3D[id].minmetalwidth);
		if (!wire1.wiretype && wire2.wiretype)  //1m2w
		{
			s = l - 0.5 * (this->Plane_3D[id].minwirewidth + this->Plane_3D[id].minmetalwidth);
		}
		if (!wire1.wiretype && !wire2.wiretype)  //1m2m
			s = l - this->Plane_3D[id].minmetalwidth;
	}
	return s;
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
