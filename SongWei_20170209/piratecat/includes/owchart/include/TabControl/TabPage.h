/*********************************************************************************\
*                                                                                 *
* TabPageA.cs -  Tab page functions, types, and definitions                       *
*                                                                                 *
*               Version 4.00 ������                                           *
*                                                                                 *
*               Copyright (c) 2016-2016, Lord's tab control. All rights reserved. *
*               Check 2016/9/24 by Lord.                                          *
*                                                                                 *
**********************************************************************************/

#ifndef __TABPAGE_H__
#define __TABPAGE_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "..\\Layout\\Div.h"
#include "..\\Button\\Button.h"
#include "TabControl.h"

namespace OwLib
{
	class TabControlA;

	class TabPageA:public DivA
	{
	private:
        ControlEvent m_dragHeaderBeginEvent;
        ControlEvent m_dragHeaderEndEvent;
        ControlEvent m_draggingHeaderEvent;
		ButtonA *m_headerButton;
		POINT m_headerLocation;
		ControlMouseEvent m_headerMouseDownEvent;
		TabControlA *m_tabControl;
        static void DragHeaderBegin(void *sender, void *pInvoke);
        static void DragHeaderEnd(void *sender, void *pInvoke);
        static void DraggingHeader(void *sender, void *pInvoke);
		static void HeaderMouseDown(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke);
	public:
		TabPageA();
		virtual ~TabPageA();
		ButtonA* GetHeaderButton();
		void SetHeaderButton(ButtonA *headerButton);		
		POINT GetHeaderLocation();
		void SetHeaderLocation(POINT headerLocation);
		TabControlA* GetTabControl();
		void SetTabControl(TabControlA *tabControl);
	public:
		virtual String GetControlType();
		virtual void GetProperty(const String& name, String *value, String *type);
		virtual vector<String> GetPropertyNames();
		virtual void OnLoad();
		virtual void OnTextChanged();
		virtual void SetProperty(const String& name, const String& value);
	};
}

#endif