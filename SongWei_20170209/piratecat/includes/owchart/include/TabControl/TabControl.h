/*********************************************************************************\
*                                                                                 *
* TabControl.h -  Tab control functions, types, and definitions                   *
*                                                                                 *
*               Version 4.00 ������                                           *
*                                                                                 *
*               Copyright (c) 2016-2016, Lord's tab control. All rights reserved. *
*               Check 2016/9/24 by Lord.                                          *
*                                                                                 *
**********************************************************************************/

#ifndef __TABCONTROL_H__
#define __TABCONTROL_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "..\\Base\\CStr.h"
#include "..\\Layout\\Div.h"
#include "TabPage.h"

namespace OwLib
{
	typedef enum TabPageLayout
	{
		TabPageLayout_Bottom,
		TabPageLayout_Left,
		TabPageLayout_Right,
		TabPageLayout_Top
	};
	
	class TabPageA;

	class TabControlA:public DivA
	{
	private:
		int m_animationState;
		TabPageLayout m_layout;
		int m_selectedIndex;	
		int m_timerID;
		bool m_useAnimation;
		void DrawMoving();
		int GetTabPageIndex(TabPageA *tabPage);
	public:
		vector<TabPageA*> m_tabPages;
		TabControlA();
		virtual ~TabControlA();
		TabPageLayout GetLayout();
		void SetLayout(TabPageLayout layout);
		int GetSelectedIndex();
		void SetSelectedIndex(int selectedIndex);
		TabPageA* GetSelectedTabPage();
		void SetSelectedTabPage(TabPageA *selectedTabPage);
		bool UseAnimation();
		void SetUseAnimation(bool useAnimation);
	public:
		virtual void AddControl(ControlA *control);
		virtual String GetControlType();
		virtual void ClearControls();
		virtual void GetProperty(const String& name, String *value, String *type);
		virtual vector<String> GetPropertyNames();
		virtual void InsertControl(int index, ControlA *control);
        virtual void OnDragTabHeaderBegin(TabPageA *tabPage);
        virtual void OnDragTabHeaderEnd(TabPageA *tabPage);
        virtual void OnDraggingTabHeader(TabPageA *tabPage);
		virtual void OnLoad();
		virtual void OnSelectedTabPageChanged();
		virtual void OnTimer(int timerID);
		virtual void RemoveControl(ControlA *control);
		virtual void SetProperty(const String& name, const String& value);
		virtual void Update();
	};
}

#endif