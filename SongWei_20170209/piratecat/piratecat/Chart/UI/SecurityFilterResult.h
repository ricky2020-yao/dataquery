/************************************************************************************\
*                                                                                    *
* SecurityFilterResult.h - Security filter result functions, types, and definitions. *
*                                                                                    *
*               Version 1.00  ¡ï¡ï¡ï                                                 *
*                                                                                    *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.             *
*               Created by Lord 2016/3/17.                                           *
*                                                                                    *
*************************************************************************************/

#ifndef __SECURITYFILTERRESULT_H__
#define __SECURITYFILTERRESULT_H__
#pragma once
#include "UIXmlEx.h"
#include "CDraw.h"
#include "OwChart.h"
#include "WindowEx.h"
#include "..\\Service\\DataCenter.h"
#include "..\\Service\\SecurityFilterTask.h"

class SecurityFilterInfo;
class SecurityFilterTask;

class SecurityFilterResultRow : public GridRow
{
private:
	SecurityFilterInfo *m_info;
public:
	SecurityFilterResultRow();
	virtual ~SecurityFilterResultRow();
	SecurityFilterInfo* GetInfo();
	void SetInfo(SecurityFilterInfo *info);
};

class SecurityFilterResultStringCell : public GridStringCell
{
public:
	SecurityFilterResultStringCell();
	~SecurityFilterResultStringCell();
public:
	virtual void OnPaint(CPaint *paint, const RECT& rect, const RECT& clipRect, bool isAlternate);
};

class SecurityFilterResultDoubleCell : public GridDoubleCell
{
private:
	String m_fieldName;
public:
	SecurityFilterResultDoubleCell();
	~SecurityFilterResultDoubleCell();
	String GetFieldName();
	void SetFieldName(const String& fieldName);
public:
	virtual double GetDouble();
	virtual void OnPaint(CPaint *paint, const RECT& rect, const RECT& clipRect, bool isAlternate);
};

class SecurityFilterResult
{
private:
	OwChart *m_chart;
	GridCellMouseEvent m_gridCellClick;
	GridA *m_gridFilterResult;
	NativeBase *m_native;
	int m_runningState;
	SecurityFilterTask *m_task;
	ControlTimerEvent m_timerEvent;
	int m_timerID;
	WindowEx *m_window;
	UIXmlEx *m_xml;
private:
	void AddIndicatorColumns();
	void AddIndicatorRows();
	static void CallTimer(void *sender, int timerID, void *pInvoke);
	static void ClickButton(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke);
	static void GridCellClick(void *sender, GridCell *cell, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke);
	void RegisterEvents(ControlA *control);
public:
	SecurityFilterResult(NativeBase *native);
	virtual ~SecurityFilterResult();
	OwChart* GetChart();
	void SetChart(OwChart *chart);
	NativeBase* GetNative();
	SecurityFilterTask* GetTask();
	void SetTask(SecurityFilterTask *task);
	WindowEx* GetWindow();
	UIXmlEx* GetXml();
public:
	virtual void Close();
	void OnGridCellDoubleClick(GridCell *cell);
	void OnTimer(int timerID);
	virtual void Show();
};

#endif