/*****************************************************************************\
*                                                                             *
* ChatWindow.h - Chart window functions, types, and definitions.              *
*                                                                             *
*               Version 1.00  ¡ï¡ï¡ï                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/6/10.                                    *
*                                                                             *
******************************************************************************/

#ifndef __CHATWINDOW__H__
#define __CHATWINDOW__H__
#pragma once
#include "UIXmlEx.h"
#include "CDraw.h"
#include "OwChart.h"
#include "WindowEx.h"
#include "..\\Service\\DataCenter.h"
#include "..\\Service\\ChatServiceEx.h"
#include "..\\..\\..\\includes\\Sock\\ChatService.h"

class OwChart;

class ChatWindow : public WindowXmlEx
{
private:
	OwChart *m_chart;
	ListenerMessageCallBack m_chatDataCallBackEvent;
	ChatServiceEx *m_chatService;
	ControlInvokeEvent m_invokeEvent;
	NativeBase *m_native;
	WindowEx *m_window;
private:
	static void ClickButton(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke);
	static void Invoke(void *sender, void *args, void *pInvoke);
	static void __stdcall ChatDataCallBack(CMessage *message, void *pInvoke);
	void RegisterEvents(ControlA *control);
public:
	ChatWindow(NativeBase *native);
	virtual ~ChatWindow();
	OwChart *GetChart();
	void SetChart(OwChart *chart);
	WindowEx* GetWindow();
public:
	virtual void Close();
	void OnInvoke(void *args);
	void OnChatDataCallBack(CMessage *message);
	void Send();
	virtual void Show();
};

#endif