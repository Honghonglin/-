#pragma once
#if defined(EXPORTBUILD)
#define _DLLExport __declspec(dllexport)
#else
#define _DLLExport __declspec(dllimport)
#endif


extern "C" double /*需要调用的方法类型*/ _DLLExport Multip(double x, double y)/*需要调用的方法*/;
extern "C" void /*需要调用的方法类型*/ _DLLExport Func();
extern "C" double /*需要调用的方法类型*/ _DLLExport ssss(double x, double y)/*需要调用的方法*/;
