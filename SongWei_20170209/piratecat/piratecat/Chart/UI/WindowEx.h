/*****************************************************************************\
*                                                                             *
* WindowEx.h - Window functions, types, and definitions.                      *
*                                                                             *
*               Version 1.00  ¡ï¡ï¡ï                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/5/8.                                     *
*                                                                             *
******************************************************************************/

#ifndef __WINDOWEX_H__
#define __WINDOWEX_H__
#pragma once
#include "CDraw.h"
#include "RibbonButton.h"
#include "UIXmlEx.h"

class WindowEx:public WindowA
{
private:
	int m_animateDirection;
	bool m_animateMoving;
	int m_animateType;
	RibbonButton *m_closeButton;
	bool m_isChildWindow;
	int m_timerID;
public:
	WindowEx();
	virtual ~WindowEx();
	bool IsAnimateMoving();
	RibbonButton* GetCloseButton();
	void SetCloseButton(RibbonButton* closeButton);
	bool IsChildWindow();
	void SetChildWindow(bool isChildWindow);
public:
	void AnimateHide();
	void AnimateShow(bool showDialog);
	virtual void OnAdd();
	virtual void OnDragReady(POINT *startOffset);
	virtual void OnPaintBackground(CPaint *paint, const RECT& clipRect);
	virtual void OnTimer(int timerID);
	virtual void Update();
};
#endif