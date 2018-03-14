/********************************************************************************\
*                                                                                *
* SecurityFilterList.h - Security filter list functions, types, and definitions. *
*                                                                                *
*               Version 1.00  ¡ï¡ï¡ï                                             *
*                                                                                *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.         *
*               Created by Lord 2016/1/22.                                       *
*                                                                                *
*********************************************************************************/

#ifndef __SECURITYFILTERLIST_H__
#define __SECURITYFILTERLIST_H__
#pragma once
#include "UIXmlEx.h"
#include "CDraw.h"
#include "WindowEx.h"
#include "IndicatorList.h"
#include "SecurityList.h"
#include "SecurityFilterResult.h"
#include "..\\Service\\DataCenter.h"
#include "..\\Service\\SecurityServiceEx.h"
#include "..\\Service\\SecurityFilterServiceEx.h"

class IndicatorList;
class SecurityList;
class SecurityFilterResult;
class SecurityFilterServiceEx;

class SecurityFilterList : public WindowXmlEx
{
private:
	OwChart *m_chart;
	String m_codes;
	ControlEvent m_gridSelectedRowsChangedEvent;
	GridA *m_gridTemplate;
	ControlInvokeEvent m_invokeEvent;
	NativeBase *m_native;
	SecurityServiceEx *m_securityService;
	ListenerMessageCallBack m_securityFilterDataCallBackEvent;
	SecurityFilterServiceEx *m_securityFilterService;
	SecurityFilterTemplate m_template;
	ControlTimerEvent m_timerEvent;
	int m_timerID;
	WindowEx *m_window;
private:
	void AddTemplatesToGrid(vector<SecurityFilterTemplate> *templates);
	static void CallTimer(void *sender, int timerID, void *pInvoke);
	static void ClickButton(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke);
	map<String, GridRow*> GetTemplateRows();
	static void GridSelectedRowsChanged(void *sender, void *pInvoke);
	static void IndicatorListWindowClosed(void *sender, bool *cancel, void *pInvoke);
	static void Invoke(void *sender, void *args, void *pInvoke);
	void LoadTemplate(SecurityFilterTemplate *sfTemplate);
	void RegisterEvents(ControlA *control);
	static void __stdcall SecurityFilterDataCallBack(CMessage *message, void *pInvoke);
	static void SecurityListWindowClosed(void *sender, bool *cancel, void *pInvoke);
public:
	SecurityFilterList(NativeBase *native);
	virtual ~SecurityFilterList();
	OwChart* GetChart();
	void SetChart(OwChart *chart);
	WindowEx* GetWindow();
public:
	virtual void Close();
	void Create();
	void Delete();
	void GetTemplate(SecurityFilterTemplate *sfTemplate);
	void OnGridSelectedRowsChanged();
	void OnIndicatorListWindowClosed(IndicatorList *indicatorList, bool *cancel);
	void OnInvoke(void *args);
	void OnSecurityFilterDataCallBack(CMessage *message);
	void OnSecurityListWindowClosed(SecurityList *securityList, bool *cancel);
	void OnTimer(int timerID);
	void Run();
	void Save();
	void SelectIndicator();
	void SelectSecurities();
	virtual void Show();
	void Watch();
};

#endif