/*********************************************************************************\
*                                                                                 *
* VScrollBar.h - Vertical scroll bar functions, types, and definitions          *
*                                                                                 *
*               Version 4.00 ������                                           *
*                                                                                 *
*               Copyright (c) 2016-2016, Lord's scroll bar. All rights reserved.  *
*                                                                                 *
***********************************************************************************/

#ifndef __VSCROLLBAR_H__
#define __VSCROLLBAR_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "ScrollBar.h"

namespace OwLib
{
	class VScrollBarA:public ScrollBarA
	{
	private:
        ControlMouseEvent m_backButtonMouseDownEvent;
		ControlMouseEvent m_backButtonMouseUpEvent;
		static void BackButtonMouseDown(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke);
		static void BackButtonMouseUp(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke);
	public:
		VScrollBarA();
		virtual ~VScrollBarA();
		virtual String GetControlType();
		void OnBackButtonMouseDown(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		void OnBackButtonMouseUp(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnDragScroll();
		virtual void OnLoad();
		virtual void Update();
	};
}

#endif