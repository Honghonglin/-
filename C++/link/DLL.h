#pragma once
#if defined(EXPORTBUILD)
#define _DLLExport __declspec(dllexport)
#else
#define _DLLExport __declspec(dllimport)
#endif


extern "C" double /*��Ҫ���õķ�������*/ _DLLExport Multip(double x, double y)/*��Ҫ���õķ���*/;
extern "C" void /*��Ҫ���õķ�������*/ _DLLExport Func();
extern "C" double /*��Ҫ���õķ�������*/ _DLLExport ssss(double x, double y)/*��Ҫ���õķ���*/;
