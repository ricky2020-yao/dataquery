/***********************************************************************************\
*                                                                                   *
* IndicatorLayoutList.h - Indicator layout list functions, types, and definitions.  *
*                                                                                   *
*               Version 1.00  ¡ï¡ï¡ï                                                *
*                                                                                   *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.            *
*               Created by Lord 2016/5/26.                                          *
*                                                                                   *
************************************************************************************/

#ifndef __INDICATORLAYOUTLIST_H__
#define __INDICATORLAYOUTLIST_H__
#pragma once
#include "UIXmlEx.h"
#include "CDraw.h"
#include "WindowEx.h"
#include "LayoutScaleDiv.h"
#include "..\\Service\\DataCenter.h"
#include "..\\Service\\IndicatorLayoutServiceEx.h"

class IndicatorLayoutList : public WindowXmlEx
{
private:
	OwChart *m_chart;
	ControlEvent m_gridSelectedRowsChangedEvent;
	GridA *m_gridLayouts;
	ListenerMessageCallBack m_indicatorLayoutDataCallBack;
	IndicatorLayoutServiceEx *m_indicatorLayoutService;
	ControlInvokeEvent m_invokeEvent;
	NativeBase *m_native;
	IndicatorLayout m_layout;
	WindowEx *m_window;
private:
	void AddLayoutsToGrid(vector<IndicatorLayout> *layouts);
	static void ClickButton(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke);
	map<String, GridRow*> GetIndicatorLayoutsRows();
	static void GridSelectedRowsChanged(void *sender, void *pInvoke);
	static void __stdcall IndicatorLayoutDataCallBack(CMessage *message, void *pInvoke);
	static void Invoke(void *sender, void *args, void *pInvoke);
	void LoadLayout(IndicatorLayout *layout);
	void RegisterEvents(ControlA *control);
public:
	IndicatorLayoutList(NativeBase *native);
	virtual ~IndicatorLayoutList();
	OwChart* GetChart();
	void SetChart(OwChart *chart);
	WindowEx* GetWindow();
public:
	void Apply();
	virtual void Close();
	void Create();
	void Delete();
	void GetLayout(IndicatorLayout *layout);
	void OnGridSelectedRowsChanged();
	void OnInvoke(void *args);
	void OnIndicatorLayoutDataCallBack(CMessage *message);
	void Save();
	virtual void Show();
};

#endif