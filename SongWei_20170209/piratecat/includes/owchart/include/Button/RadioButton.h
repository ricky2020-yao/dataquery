/*****************************************************************************\
*                                                                             *
* RaioButton.h -  Radio button functions, types, and definitions              *
*                                                                             *
*               Version 4.00 ¡ï¡ï¡ï¡ï¡ï                                       *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's button. All rights reserved.  *
*               Checked 2016/10/8 by Lord.                                    *
*                                                                             *
*******************************************************************************/

#ifndef __RADIOBUTTON_H__
#define __RADIOBUTTON_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "..\\Base\\CStr.h"
#include "CheckBox.h"

namespace OwLib
{
	class RadioButtonA:public CheckBoxA
	{
	private:
		String m_groupName;
	public:
		RadioButtonA();
		virtual ~RadioButtonA();
		String GetGroupName();
		void SetGroupName(const String& groupName);
	public:
		virtual String GetControlType();
		virtual void GetProperty(const String& name, String *value, String *type);
		virtual vector<String> GetPropertyNames();
		virtual void OnClick(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnPaintCheckButton(CPaint *paint, const RECT& clipRect);
		virtual void SetProperty(const String& name, const String& value);
		virtual void Update();
	};
}

#endif