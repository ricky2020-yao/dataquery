/*****************************************************************************\
*                                                                             *
* SecurityList.h - Security list functions, types, and definitions.           *
*                                                                             *
*               Version 1.00  ¡ï¡ï¡ï                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/1/23.                                    *
*                                                                             *
******************************************************************************/

#ifndef __SECURITYLIST_H__
#define __SECURITYLIST_H__
#pragma once
#include "UIXmlEx.h"
#include "CDraw.h"
#include "WindowEx.h"
#include "..\\Service\\DataCenter.h"
#include "..\\Service\\SecurityServiceEx.h"

class UserSecurityServiceEx;

class SecurityList : public WindowXmlEx
{
private:
	GridA *m_gridSecurities;
	ControlEvent m_gridSelectedRowsChangedEvent;
	GridA *m_gridSelectSecurities;
	ControlInvokeEvent m_invokeEvent;
	bool m_isSubmit;
	NativeBase *m_native;
	ListenerMessageCallBack m_securityDataCallBack;
	int m_securitiesRequestID;
	SecurityServiceEx *m_securityService;
	TreeA *m_tvBlock;
	UserSecurityServiceEx *m_userSecurityService;
	WindowEx *m_window;
private:
	static void ClickButton(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke);
	static void GridSelectedRowsChanged(void *sender, void *pInvoke);
	static void Invoke(void *sender, void *args, void *pInvoke);
	void RegisterEvents(ControlA *control);
	static void __stdcall SecurityDataCallBack(CMessage *message, void *pInvoke);
public:
	SecurityList(NativeBase *native);
	virtual ~SecurityList();
	bool IsSubmit();
	WindowEx* GetWindow();
public:
	void AddSecuritiesToSecuritiesGrid(vector<Security> *securities);
	void BindSecuritiesToSelectSecuritiesGrid(vector<Security> *securities);
	virtual void Close();
	void GetSelectedSecurities(vector<Security> *securities);
	void LoadUserSecurityCategories(vector<UserSecurityCategory> *categories);
	void OnGridSelectedRowsChanged();
	void OnInvoke(void *args);
	void OnSecurityDataCallBack(CMessage *message);
	void RemoveSecuritiesFromSecuritiesGrid(vector<Security> *securities);
	void SelectSecurities();
	void SelectAllSecurities();
	virtual void Show();
	void Submit();
	void UnSelectAllSecurities();
	void UnSelectSecurities();
};

#endif