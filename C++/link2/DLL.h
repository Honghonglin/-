#pragma once
#if defined(EXPORTBUILD)
#define _DLLExport __declspec(dllexport)
#else
#define _DLLExport __declspec(dllimport)
#endif
extern "C" void /*��Ҫ���õķ�������*/ _DLLExport LimitFun()/*��Ҫ���õķ���*/;