// CPPtest.h : main header file for the CPPtest DLL
//

#pragma once

#ifndef __AFXWIN_H__
	#error "include 'pch.h' before including this file for PCH"
#endif

#include "resource.h"		// main symbols


// CCPPtestApp
// See CPPtest.cpp for the implementation of this class
//

extern "C"
{
	// ...[»ý·«]...
	__declspec(dllexport)  int fnWin32Project2(int value);
	__declspec(dllexport) int SimpleTest(char* path);
};


class CCPPtestApp : public CWinApp
{
public:
	CCPPtestApp();

// Overrides
public:
	virtual BOOL InitInstance();

	DECLARE_MESSAGE_MAP()
};
