/*****************************************************************************\
*                                                                             *
* UserSecurityList.h - User security list functions, types, and definitions.  *
*                                                                             *
*               Version 1.00  ¡ï¡ï¡ï                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/1/8.                                     *
*                                                                             *
******************************************************************************/

#ifndef __USERSECURITYLIST_H__
#define __USERSECURITYLIST_H__
#pragma once
#include "UIXmlEx.h"
#include "CDraw.h"
#include "SecurityList.h"
#include "OwChart.h"
#include "WindowEx.h"
#include "..\\..\\..\\includes\\Sock\\Security.h"
#include "..\\Service\\DataCenter.h"
#include "..\\Service\\SecurityServiceEx.h"
#include "..\\Service\\UserSecurityServiceEx.h"
#include "..\\..\\..\\includes\\Sock\\QuoteService.h"

class SecurityList;

class UserSecurityList : public WindowXmlEx
{
private:
	OwChart *m_chart;
	GridA *m_gridCategory;
	GridCellMouseEvent m_gridCellClick;
	GridCellEvent m_gridCellEditEndEvent;
	GridA *m_gridSecurities;
	ControlEvent m_gridSelectedRowsChangedEvent;
	ControlInvokeEvent m_invokeEvent;
	ListenerMessageCallBack m_latestDataCallBack;
	int m_latestDataRequestID;
	NativeBase *m_native;
	QuoteService *m_quoteService;
	ControlEvent m_searchTextBoxGotFocus;
	SecurityServiceEx *m_securityService;
	ControlTimerEvent m_timerEvent;
	int m_timerID;
	TextBoxA *m_txtSearch;
	ListenerMessageCallBack m_userSecurityDataCallBack;
	UserSecurityServiceEx *m_userSecurityService;
	WindowEx *m_window;
private:
	static void CallTimer(void *sender, int timerID, void *pInvoke);
	static void ClickButton(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke);
	static void CategoryGridCellEditEnd(void *sender, GridCell *cell, void *pInvoke);
	static void GridCellClick(void *sender, GridCell *cell, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke);
	static void GridSelectedRowsChanged(void *sender, void *pInvoke);
	static void Invoke(void *sender, void *args, void *pInvoke);
	static void __stdcall LatestDataCallBack(CMessage *message, void *pInvoke);
	void RegisterEvents(ControlA *control);
	static void SearchTextBoxGotFocus(void *sender, void *pInvoke);
	static void SecurityListWindowClosed(void *sender, bool *cancel, void *pInvoke);
	static void __stdcall UserSecurityDataCallBack(CMessage *message, void *pInvoke);
public:
	map<String, SecurityLatestData> m_latestDatas;
	UserSecurityList(NativeBase *native);
	virtual ~UserSecurityList();
	OwChart* GetChart();
	void SetChart(OwChart *chart);
	WindowEx* GetWindow();
public:
	void AddCategory();
	void AddCategoriesToCategoryGrid(vector<UserSecurityCategory> *categories);
	void AddSecurities();
	void AddSecuritiesToSecuritiesGrid(vector<Security> *securities);
	void BindSecuritiesToSecuritiesGrid(UserSecurityCategory *category);
	virtual void Close();
	void DeleteCategory();
	void DeleteSecurities();
	void LoadCategoriesToCategoryGrid(vector<UserSecurityCategory> *categories);
	void OnCategoryGridCellEditEnd(GridCell *cell);
	void OnGridCellDoubleClick(GridCell *cell);
	void OnInvoke(void *args);
	void OnLatestDataCallBack(CMessage *message);
	void OnGridSelectedRowsChanged();
	void OnSearchTextBoxGotFocus();
	void OnSecurityListWindowClosed(SecurityList *securityList, bool *cancel);
	void OnTimer(int timerID);
	void OnUserSecurityDataCallBack(CMessage *message);
	void RemoveSecuritiesFromSecuritiesGrid(vector<Security> *securities);
	void RemoveCategoriesFromCategoryGrid(vector<UserSecurityCategory> *categories);
	virtual void Show();
	void UpdateCategoriesToCategoryGrid(vector<UserSecurityCategory> *categories);
};


class UserSecurityDoubleCell : public GridDoubleCell
{
private:
	UserSecurityList *m_userSecurityList;
public:
	UserSecurityDoubleCell();
	~UserSecurityDoubleCell();
	UserSecurityList* GetUserSecurityList();
	void SetUserSecurityList(UserSecurityList *userSecurityList);
public:
	virtual double GetDouble();
	static int GetPriceColor(double price, double comparePrice);
	virtual void OnPaint(CPaint *paint, const RECT& rect, const RECT& clipRect, bool isAlternate);
};

#endif