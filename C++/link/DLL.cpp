#include<cstdio>
#include<iostream>
#include<vector>
#include<map>
#include<queue>
#include<fstream>
//�궨��
#define EXPORTBUILD

//����ͷ�ļ�
#include "DLL.h"
#include "base.h"
using namespace std;

struct Edge1 {
	long long edgeID;//每条边的编号
	long long start, end;
	long long cost;//边的代价，初始化�?，当被使用过后，每次使用加上一个很大的数尽量防止被别的线网使用
};

struct vertice {
	long long vID;//点的编号
	long long cost;//点的代价,算法使用，建图不用考虑  其实就是点到起始引脚的距�?
	//其他需要的数据。。�?
};

struct Graph {
	vector<vertice> points; //所有的�?
	vector<long long> pins; //需要连接的引脚的点的编�?
	vector<Edge1> edges; //布线图中所有的�?
	vector<vector<long long> > g;//布线图，第一层vector是所有的vertice，第二层vector是start或end连接到该vertice的边的编�?
	// 例子
	// 点集�? 1 2 3
	// �?�?-1
	// �?�?-2
	// �?�?-3
	// g�?
	// g[0] = { 0 }    �?邻接的边编号�?
	// g[1] = { 0 1 }    �?邻接的边编号�?�?
	// g[2] = { 1 2 }
	// g[3] = { 2 }
};

vector<long long> srtp(Graph graph) {

	const long long INF = 99999999;
	vector<bool> verticeTag;
	vector<bool> histVerticeTag;
	queue<long long> verticeQueue;
	vector<long long> preVertice;

	//设置容量
	verticeTag.resize(graph.points.size());
	preVertice.resize(graph.points.size());
	histVerticeTag.resize(graph.points.size());

	//初始�?
	for (long long i = 0; i < graph.points.size(); i++) {
		graph.points[i].cost = INF;       //点的代价cost初始化为无穷大（算法需要）
		verticeTag[i] = false;          //点i不在队列�?
		histVerticeTag[i] = false;        //-----�?
		preVertice[i] = INF;         //点i的前一个点编号设为无穷�?
	}

	long long startFpga = graph.pins[0];
	preVertice[startFpga] = -1;     //点startFpga的前一个点（不存在�?
	verticeTag[startFpga] = true;     //标记点startFpga入队
	histVerticeTag[startFpga] = true;     //------�?
	graph.points[startFpga].cost = 0;     //点startFpga的代价（距离）为0
	verticeQueue.push(startFpga);    //点startFpga入队

	vector<long long> selectEdge;
	selectEdge.clear();      //清空选中的边

	//SPFA
	for (long long j = 1; j < graph.pins.size(); j++) {   //遍历所有引�?
		while (!verticeQueue.empty()) {

			long long start = verticeQueue.front();     //队头的点为start
			for (long long i = 0; i < graph.g[start].size(); i++) {     //遍历点start的邻接边
				long long eID = graph.g[start][i];     //点start的邻接边设为eID
				long long end;
				if (graph.edges[eID].start == start) {     //边eID的start点若为队头的�?-------�?
					end = graph.edges[eID].end;   //end设为边eID的end
				}
				else {
					end = graph.edges[eID].start;     //否则end设为边eID的start�?
				}
				if (graph.points[end].cost > graph.points[start].cost + graph.edges[eID].cost) {    //
					graph.points[end].cost = graph.points[start].cost + graph.edges[eID].cost;    //
					preVertice[end] = eID;     //点end的前一条边更新为eID
					if (verticeTag[end] != true) {
						verticeTag[end] = true;    //
						verticeQueue.push(end);    //
					}
				}
			}
			verticeQueue.pop();
			if (histVerticeTag[start] != true) {    //------�?
				verticeTag[start] = false;
			}

		}

		long long verticeChoice = graph.pins[j];     //从最后一个引脚开始回�?
		//回溯找到路径
		long long curVertice = verticeChoice;
		while (preVertice[curVertice] != -1) {     //若点curVertice的前一个点�?1，说明回溯到了第一个引脚，则停�?
			histVerticeTag[curVertice] = true;    //-----�?
			selectEdge.push_back(preVertice[curVertice]);     //将点curVertice的前一个点放入选中的边队列
			if (curVertice != graph.edges[preVertice[curVertice]].start) {
				curVertice = graph.edges[preVertice[curVertice]].start;
			}
			else {
				curVertice = graph.edges[preVertice[curVertice]].end;
			}
		}

		for (long long i = 0; i < graph.points.size(); i++) {
			if (histVerticeTag[i] == true) {
				verticeTag[i] = true;
				preVertice[i] = -1;
				graph.points[i].cost = 0;
				verticeQueue.push(i);
			}
			else {
				verticeTag[i] = false;
				preVertice[i] = INF;
				graph.points[i].cost = INF;
			}
		}
	}

	return selectEdge;
}

void _DLLExport CreateFunc() {
	ifstream infile;
	ofstream outfile;
	infile.open("F:\\unityproject\\Deom-master\\Deom-master\\Demo\\Elastic_ball\\��Ҫ����\\Assets\\StreamingAssets\\temp.txt");
	vector<long long> proWire;     //存放重复使用的边eID


	Graph graph;
	// 构建点共 vNum �?
	long long vNum;
	cout << "Input vNum from Temp.txt" << endl;

	infile >> vNum;
	for (long long i = 0; i < vNum; i++) {
		vertice vtemp;
		vtemp.vID = i;
		graph.points.push_back(vtemp);
	}

	// 构建边共 eNum �?
	long long eNum;
	cout << "Input eNum from Temp.txt" << endl;
	infile >> eNum;
	cout << "Input points from Temp.txt" << endl;
	Edge1 etemp;
	for (long long i = 0; i < eNum; i++) {
		infile >> etemp.start >> etemp.end;
		etemp.edgeID = i;
		etemp.cost = 1;
		graph.edges.push_back(etemp);
	}

	// 构建�?
	graph.g.resize(graph.points.size());
	for (long long i = 0; i < graph.edges.size(); i++) {
		graph.g[graph.edges[i].start].push_back(i);  //放入点邻接的�?
		graph.g[graph.edges[i].end].push_back(i);
	}
	vector<long long> select_edge;

	long long netNum, pinNum, pNo;
	cout << "Input netNum from Temp.txt" << endl;
	infile >> netNum;
	outfile.open("F:\\unityproject\\Deom-master\\Deom-master\\Demo\\Elastic_ball\\��Ҫ����\\Assets\\StreamingAssets\\result.txt");
	outfile << netNum << endl;
	for (long long i = 0; i < netNum; i++) {   //线网
		long long useNetNum = i;
		cout << "Input pinNum from Temp.txt" << endl;
		infile >> pinNum;
		cout << "Input pins from Temp.txt" << endl;
		for (long long j = 0; j < pinNum; j++) {
			infile >> pNo;
			graph.pins.push_back(pNo);
		}

		// 布线
		select_edge = srtp(graph);

		// 弹出已布线线网的引脚
		graph.pins.clear();
		printf("Result in file\n");
		// 输出结果
		outfile << select_edge.size() << endl;
		for (long long n = 0; n < select_edge.size(); n++) {

			long long eID = select_edge[n];
			graph.edges[eID].cost += 100;
			outfile << graph.edges[eID].start << " " << graph.edges[eID].end << endl;

			//graph.edges[eID].useNet.push_back(useNetNum);
			if (graph.edges[eID].cost > 101) {
				proWire.push_back(eID);
			}

		}

	}

	//输出短路
	outfile << proWire.size() << endl;
	for (long long m = 0; m < proWire.size(); m++) {
		long long proID = proWire[m];
		outfile << graph.edges[proID].start << " " << graph.edges[proID].end << " " << graph.edges[proID].cost << " ";
		long long fre = (graph.edges[proID].cost) / 100;
		/*
		for(long long p=0; p<graph.edges[proID].useNet.size(); p++){
			outfile<<graph.edges[proID].useNet[p]<<" ";
		}
		*/

		outfile << fre << endl;
	}
	//*/

	infile.close();
	outfile.close();
}


class s
{
public:
	double t;
	void fu()
	{
		t += 5.0;
	}
};
//���ú���(��Ҫ���õķ���)
double _DLLExport Multip(double x, double y)
{
	s _s;
	_s.t = 5.3;
	_s.fu();
	return _s.t;
}
double _DLLExport ssss(double x, double y)
{
	return x * y;
}
void _DLLExport  Func()
{
     RoutingSpace_3D  routingSpace_3D;
}