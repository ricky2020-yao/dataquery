/*****************************************************************************\
*                                                                             *
* SplitLayoutDiv.h -  Split layout functions, types, and definitions          *
*                                                                             *
*               Version 4.00 ������                                       *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's layout. All rights reserved.  *
*               Check right 2016/9/22 by Lord.                                *
*                                                                             *
*******************************************************************************/

#ifndef __SPLITLAYOUTDIV_H__
#define __SPLITLAYOUTDIV_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "..\\Button\\Button.h"
#include "Div.h"
#include "LayoutDiv.h"
#include "TableLayoutDiv.h"

namespace OwLib
{
	class SplitLayoutDivA : public LayoutDivA
	{
	private:
		ControlA *m_firstControl;
		SIZE m_oldSize;
		ControlA *m_secondControl;
		SizeTypeA m_splitMode;
		ButtonA *m_splitter;
		ControlEvent m_splitterDraggingEvent;
		static void SplitterDragging(void *sender, void *pInvoke);
	public:
		SplitLayoutDivA();
		virtual ~SplitLayoutDivA();
		ControlA* GetFirstControl();
		void SetFirstControl(ControlA *firstControl);
		ControlA* GetSecondControl();
		void SetSecondControl(ControlA *secondControl);
		SizeTypeA GetSplitMode();
		void SetSplitMode(SizeTypeA splitMode);
		ButtonA* GetSplitter();
	public:
		virtual String GetControlType();
		virtual void GetProperty(const String& name, String *value, String *type);
		virtual vector<String> GetPropertyNames();
		virtual void OnLoad();
		virtual bool OnResetLayout();
		virtual void Update();
		virtual void SetProperty(const String& name, const String& value);
	};
}

#endif