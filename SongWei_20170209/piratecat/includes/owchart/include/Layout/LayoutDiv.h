/*****************************************************************************\
*                                                                             *
* LayoutDiv.h -  Layout functions, types, and definitions                     *
*                                                                             *
*               Version 4.00 ¡ï¡ï¡ï¡ï¡ï                                       *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's layout. All rights reserved.  *
*               Check right 2016/9/22 by Lord.                                *
*                                                                             *
*******************************************************************************/

#ifndef __LAYOUTDIV_H__
#define __LAYOUTDIV_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "..\\Button\\Button.h"
#include "Div.h"

namespace OwLib
{
	class LayoutDivA : public DivA
	{
	private:
		LayoutStyleA m_layoutStyle;
	public:
		LayoutDivA();
		virtual ~LayoutDivA();
		LayoutStyleA GetLayoutStyle();
		void SetLayoutStyle(LayoutStyleA layoutStyle);
	public:
		virtual String GetControlType();
		virtual void GetProperty(const String& name, String *value, String *type);
		virtual vector<String> GetPropertyNames();
		virtual bool OnResetLayout();
		virtual void SetProperty(const String& name, const String& value);
		virtual void Update();
	};
}

#endif