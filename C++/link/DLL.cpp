//宏定义
#define EXPORTBUILD

//加载头文件
#include "DLL.h"
#include "base.h"

class s
{
public:
	double t;
	void fu()
	{
		t += 5.0;
	}
};
//设置函数(需要调用的方法)
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