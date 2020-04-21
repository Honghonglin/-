#pragma once
#if defined(EXPORTBUILD)
#define _DLLExport __declspec(dllexport)
#else
#define _DLLExport __declspec(dllimport)
#endif
extern "C" void /*需要调用的方法类型*/ _DLLExport LimitFun()/*需要调用的方法*/;