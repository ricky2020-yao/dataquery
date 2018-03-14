/*******************************************************************************************\
*                                                                                           *
* MacroService.h -  Macro service functions, types, and definitions.                        *
*                                                                                           *
*               Version 1.00  ¡ï¡ï¡ï                                                        *
*                                                                                           *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.                    *
*               Created by Lord 2016/10/7.                                                  *
*                                                                                           *
********************************************************************************************/

#ifndef __MACROSERVICEEX_H__
#define __MACROSERVICEEX_H__
#pragma once
#include "IndicatorLayoutServiceEx.h"
#include "SecurityServiceEx.h"
#include "..\\UI\\OwChart.h"
#include "..\\UI\\WinformControlHostEx.h"
#include "..\\..\\..\\includes\\Sock\\MacroService.h"

class OwChart;

class CFunctionEx : public CFunction
{
private:
	OwChart *m_chart;
	CIndicator *m_indicator;
public:
	CFunctionEx(OwChart *chart, CIndicator *indicator, int id, String name, bool withParameters);
	virtual ~CFunctionEx();
public:
	String GetText(String text);
	bool IsNewData();
	virtual double OnCalculate(CVariable *var);
public:
	double Alert(CVariable *var);
	double ScrollLeft(CVariable *var);
	double ScrollRight(CVariable *var);
	double SetCode(CVariable *var);
	double SetCycle(CVariable *var);
	double SetLayout(CVariable *var);
	double ShowWindow(CVariable *var);
	double Sleep(CVariable *var);
	double WinExecute(CVariable *var);
	double WinGetValue(CVariable *var);
	double WinSendKey(CVariable *var);
	double WinMouseEvent(CVariable *var);
	double WinSetText(CVariable *var);
	double ZoomIn(CVariable *var);
	double ZoomOut(CVariable *var);
};

class MacroServiceEx : public MacroService
{
private:
	OwChart *m_chart;
	int m_getListRequestID;
	bool m_loaded;
	vector<Macro> m_macros;
	int m_operatorRequestID;
	map<String, String> m_runningMacros;
	static void ChartInvoke(void *sender, void *args, void *pInvoke);
private:
	CLockEx m_lock;
public:
	MacroServiceEx();
	virtual ~MacroServiceEx();
	OwChart* GetChart();
	void SetChart(OwChart* chart);
	int GetListRequestID();
	bool IsLoaded();
	int GetOperatorRequestID();
public:
	bool GetMacroByID(const String& macroID, Macro *macro);
	bool GetMacros(vector<Macro> *macros);
	void OnChartInvoke(void *args);
	virtual int OnMacroEnd(Macro *macro);
	virtual int OnMacroRunning(Macro *macro);
	virtual int OnMacroStart(Macro *macro);
	virtual void OnReceive(CMessage *message);
	void Run(Macro *macro);
};
#endif