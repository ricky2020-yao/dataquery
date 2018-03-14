/*****************************************************************************\
*                                                                             *
* LoginWindow.h - Login window functions, types, and definitions.             *
*                                                                             *
*               Version 1.00  ¡ï¡ï¡ï                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/4/9.                                     *
*                                                                             *
******************************************************************************/

#ifndef __LOGINWINDOW_H__
#define __LOGINWINDOW_H__
#pragma once
#include "UIXmlEx.h"
#include "CDraw.h"
#include "OwChart.h"
#include "WindowEx.h"
#include "ConnectWindow.h"
#include "Sky.h"
#include "..\\Service\\UserCookieService.h"
#include "..\\Service\\DataCenter.h"
#include "..\\..\\..\\includes\\Sock\\LoginService.h"

class LoginWindow : public WindowXmlEx
{
private:
	OwChart *m_chart;
	ControlInvokeEvent m_invokeEvent;
	ListenerMessageCallBack m_loginDataCallBackEvent;
	int m_loginRequestID;
	LoginService *m_loginService;
	NativeBase *m_native;
	Sky *m_sky;
	WindowEx *m_window;
private:
	static void ClickButton(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke);
	static void Invoke(void *sender, void *args, void *pInvoke);
	static void __stdcall LoginDataCallBack(CMessage *message, void *pInvoke);
	void RegisterEvents(ControlA *control);
	static void WindowLocationChanged(void *sender, void *pInvoke);
public:
	LoginWindow(NativeBase *native);
	virtual ~LoginWindow();
	OwChart *GetChart();
	void SetChart(OwChart *chart);
	WindowEx* GetWindow();
public:
	virtual void Close();
	void Login();
	void OnInvoke(void *args);
	void OnLoginDataCallBack(CMessage *message);
	void OnWindowLocationChanged();
	virtual void Show();
	void ShowNetSetting();
};

#endif