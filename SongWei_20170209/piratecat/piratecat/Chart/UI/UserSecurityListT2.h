/*******************************************************************************\
*                                                                               *
* UserSecurityListT2.h - User security list functions, types, and definitions.  *
*                                                                               *
*               Version 1.00  ¡ï¡ï¡ï                                            *
*                                                                               *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.        *
*               Created by Lord 2016/3/31.                                      *
*               Modified by QiChunyou,Wangshaoxu.                               *
*                                                                               *
********************************************************************************/

#ifndef __USERSECURITYLISTT2__H__
#define __USERSECURITYLISTT2__H__
#pragma once
#include "CDraw.h"
#include "OwChart.h"
#include "UIXmlEx.h"
#include "WindowEx.h"
#include "..\\..\\..\\includes\\Sock\\BaseService.h"
#include "..\\..\\..\\includes\\Sock\\QuoteService.h"
#include "..\\Service\\SecurityServiceEx.h"
#include "..\\Service\\UserSecurityServiceEx.h"

class UserSecurityListT2;

class UserSecurityCellT2 : public ButtonA
{
private:
	int m_clicks;
	int m_index;
	UserSecurityListT2 *m_list;
	int m_mode;
	RECT m_paintRect;
	String m_securityCode;
public:
	UserSecurityCellT2();
	virtual ~UserSecurityCellT2();
	int GetClicks();
	void SetClicks(int clicks);
	int GetIndex();
	void SetIndex(int index);
	UserSecurityListT2* GetList();
	void SetList(UserSecurityListT2 *list);
	int GetMode();
	void SetMode(int mode);
	RECT GetPaintRect();
	void SetPaintRect(const RECT& paintRect);
	double GetRate();
	String GetSecurityCode();
	void SetSecurityCode(String securityCode);
public:
	virtual void OnClick(const POINT& mp, MouseButtonsA button, int clicks, int delta);
	virtual bool OnDragBegin();
	virtual void OnDragging();
	virtual void OnMouseDown(const POINT& mp, MouseButtonsA button, int clicks, int delta);
	virtual void OnMouseMove(const POINT& mp, MouseButtonsA button, int clicks, int delta);
	virtual void OnMouseUp(const POINT& mp, MouseButtonsA button, int clicks, int delta);
	virtual void OnMouseWheel(const POINT& mp, MouseButtonsA button, int clicks, int delta);
	virtual void OnPaint(CPaint *paint, const RECT& clipRect);
};

class UserSecurityListT2 : public DivA
{
private:
	vector<UserSecurityCellT2*> m_cells;
	int m_cellState;
	OwChart *m_chart;
	bool m_isEditing;
	int m_latestDataRequestID;
	ListenerMessageCallBack m_latestDataCallBack;
	QuoteService *m_quoteService;
	SecurityServiceEx *m_securityService;
	UserSecurityCellT2 *m_selectedCell;
	int m_tick;
	int m_tick2;
	int m_timerID;
	bool m_useAnimation;
	ListenerMessageCallBack m_userSecurityDataCallBack;
	UserSecurityServiceEx *m_userSecurityService;
private:
	int GetAnimationPos(int now, int to);
	static void __stdcall LatestDataCallBack(CMessage *message, void *pInvoke);
	void LoadCells();
	static void __stdcall UserSecurityDataCallBack(CMessage *message, void *pInvoke);
public:
	static bool UserSecurityCellT2Asc(UserSecurityCellT2 *x, UserSecurityCellT2 *y);
	static bool UserSecurityCellT2Desc(UserSecurityCellT2 *x, UserSecurityCellT2 *y);
public:
	UserSecurityListT2();
	virtual ~UserSecurityListT2();
	OwChart* GetChart();
	void SetChart(OwChart *chart);
	bool IsEditing();
	void SetIsEditing(bool isEditing);
	UserSecurityCellT2* GetSelectedCell();
	void SetSelectedCell(UserSecurityCellT2 *selectedCell);
	bool UseAnimation();
	void SetUseAnimation(bool useAnimation);
public:
	map<String, SecurityLatestData> m_latestDatas;
	virtual void OnAdd();
	void OnLatestDataCallBack(CMessage *message);
	void OnCellDragging(UserSecurityCellT2 *cell);
	void OnClickCell(UserSecurityCellT2 *cell, const POINT& mp, MouseButtonsA button, int clicks, int delta);
	void OnCellMouseDown(UserSecurityCellT2 *cell, const POINT& mp, MouseButtonsA button, int clicks, int delta);
	void OnCellMouseMove(UserSecurityCellT2 *cell, const POINT& mp, MouseButtonsA button, int clicks, int delta);
	void OnCellMouseUp(UserSecurityCellT2 *cell, const POINT& mp, MouseButtonsA button, int clicks, int delta);
	void OnCellMouseWheel(UserSecurityCellT2 *cell, const POINT& mp, MouseButtonsA button, int clicks, int delta);
	virtual void OnInvoke(void *args);
	virtual void OnTimer(int timerID);
	void OnUserSecurityDataCallBack(CMessage *message);
	virtual void Update();
};
#endif