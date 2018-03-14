/*****************************************************************************\
*                                                                             *
* IndicatorList.h - Indicator list functions, types, and definitions.         *
*                                                                             *
*               Version 1.00  ¡ï¡ï¡ï                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/1/20.                                    *
*                                                                             *
******************************************************************************/

#ifndef __INDICATORLIST_H__
#define __INDICATORLIST_H__
#pragma once
#include "UIXmlEx.h"
#include "CDraw.h"
#include "WindowEx.h"
#include "OwChart.h"
#include "..\\..\\..\\includes\\Sock\\Security.h"
#include "..\\Service\\IndicatorServiceEx.h"
#include "..\\Service\\DataCenter.h"

class IndicatorList : public WindowXmlEx
{
private:
	OwChart *m_chart;
	CDiv *m_div;
	ControlEvent m_gridSelectedRowsChangedEvent;
	Indicator m_indicator;
	ListenerMessageCallBack m_indicatorDataCallBackEvent;
	IndicatorServiceEx *m_indicatorService;
	ControlInvokeEvent m_invokeEvent;
	NativeBase *m_native;
	bool m_isSubmit;
	ControlTimerEvent m_timerEvent;
	int m_timerID;
	TreeA *m_tvList;
	WindowEx *m_window;
private:
	static void CallTimer(void *sender, int timerID, void *pInvoke);
	static void ClickButton(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke);
	static void GridSelectedRowsChanged(void *sender, void *pInvoke);
	static void __stdcall IndicatorDataCallBack(CMessage *message, void *pInvoke);
	static void Invoke(void *sender, void *args, void *pInvoke);
	void RegisterEvents(ControlA *control);
public:
	IndicatorList(NativeBase *native);
	virtual ~IndicatorList();
	OwChart* GetChart();
	void SetChart(OwChart *chart);
	CDiv* GetDiv();
	void SetDiv(CDiv *div);
	bool IsSubmit();
	WindowEx* GetWindow();
public:
	void AddIndicatorsToTree(vector<Indicator> *indicators);
	void ChangeUsePassword();
	virtual void Close();
	void Delete();
	map<String, TreeNodeA*> GetIndicatorsCategoryNodes();
	map<String, TreeNodeA*> GetIndicatorsNodes();
	void GetIndicator(Indicator *indicator);
	void LoadIndicator(Indicator *indicator);
	void OnGridSelectedRowsChanged();
	void OnIndicatorDataCallBack(CMessage *message);
	void OnInvoke(void *args);
	void OnTimer(int timerID);
	void Save(bool saveAsNew);
	virtual void Show();
	void Submit();
};

#endif