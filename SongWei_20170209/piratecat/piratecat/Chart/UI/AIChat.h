/*******************************************************************************\
*                                                                               *
* AIChat.h - AI chat list functions, types, and definitions.  *
*                                                                               *
*               Version 1.00  ¡ï¡ï¡ï                                            *
*                                                                               *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.        *
*               Created by YaoHaifan 2016/5/12.                                 *
*                                                                               *
********************************************************************************/

#ifndef __AICHAT__H__
#define __AICHAT__H__
#pragma once
#include "CDraw.h"
#include "OwChart.h"
#include "UIXmlEx.h"
#include "WindowEx.h"
#include "..\\..\\..\\includes\\Sock\\AIService.h"

class AIChatList;

class AIChatCell : public TextBoxA
{
private:
	AIChatList *m_chatList;
	int m_index;
	bool m_isSelf;
public:
	AIChatCell();
	virtual ~AIChatCell();
	AIChatList* GetChatList();
	void SetChatList(AIChatList *chat);
	int GetIndex();
	void SetIndex(int index);
	bool IsSelf();
	void SetIsSelf(bool isSelf);
public:
	virtual void OnAdd();
	virtual void OnMouseWheel(const POINT& mp, MouseButtonsA button, int clicks, int delta);
	virtual void OnPaint(CPaint *paint, const RECT& clipRect);
};

class AIChatList : public DivA
{
private:
	AIService *m_aiService;
	vector<AIChatCell*> m_cells;
	ListenerMessageCallBack m_aiDataCallBack;
	int m_requestID;
private:
	static void __stdcall AiDataCallBack(CMessage *message, void *pInvoke);
public:
	AIChatList();
	virtual ~AIChatList();
public:
	virtual void OnAdd();
	void OnAiDataCallBack(CMessage *message);
	void OnCellMouseWheel(AIChatCell *cell, const POINT& mp, MouseButtonsA button, int clicks, int delta);
	virtual void OnInvoke(void *args);
	virtual void OnPaint(CPaint *paint, const RECT& clipRect);
	void Send(const String& str);
};

class AIChat : public ControlA
{
private:
	ButtonA *m_btnSend;
	AIChatList *m_chatList;
	TextBoxA *m_txtChat;
private:
	static void ClickButton(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke);
	void RegisterEvents(ControlA *control);
public:
	AIChat();
	virtual ~AIChat();
public:
	virtual void OnAdd();
	void Send();
	virtual void Update();
};
#endif