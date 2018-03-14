/*****************************************************************************\
*                                                                             *
* CheckBox.h -  Check box functions, types, and definitions                   *
*                                                                             *
*               Version 4.00 ¡ï¡ï¡ï¡ï¡ï                                       *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's button. All rights reserved.  *
*               Checked 2016/10/8 by Lord.                                    *
*                                                                             *
*******************************************************************************/

#ifndef __CHECKBOX_H__
#define __CHECKBOX_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "..\\Base\\CStr.h"
#include "Button.h"

namespace OwLib
{
	class CheckBoxA : public ButtonA
	{
	private:
		_int64 m_buttonBackColor;
		_int64 m_buttonBorderColor;
		SIZE m_buttonSize;
		bool m_checked;
		String m_checkedBackImage;
		String m_checkHoveredBackImage;
		String m_checkPushedBackImage;
		String m_disableCheckedBackImage;
		HorizontalAlignA m_horizontalAlign;
	protected:
		virtual _int64 GetPaintingBackColor();
		virtual _int64 GetPaintingButtonBackColor();
		virtual _int64 GetPaintingButtonBorderColor();
		virtual String GetPaintingBackImage();
	public:
		CheckBoxA();
		virtual ~CheckBoxA();
		_int64 GetButtonBackColor();
		void SetButtonBackColor(_int64 buttonBackColor);
		_int64 GetButtonBorderColor();
		void SetButtonBorderColor(_int64 buttonBorderColor);
		SIZE GetButtonSize();
		void SetButtonSize(SIZE buttonSize);
		bool IsChecked();
		void SetChecked(bool checked);
		String GetCheckedBackImage();
		void SetCheckedBackImage(const String& checkedBackImage);
		String GetCheckHoveredBackImage();
		void SetCheckHoveredBackImage(const String& checkHoveredBackImage);
		String GetCheckPushedBackImage();
		void SetCheckPushedBackImage(const String& checkPushedBackImage);
		String GetDisableCheckedBackImage();
		void SetDisableCheckedBackImage(const String& disableCheckedBackImage);
		HorizontalAlignA GetHorizontalAlign();
		void SetHorizontalAlign(HorizontalAlignA horizontalAlign);
	public:
		virtual String GetControlType();
		virtual void GetProperty(const String& name, String *value, String *type);
		virtual vector<String> GetPropertyNames();
		virtual void OnCheckedChanged();
		virtual void OnClick(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnPaintBackground(CPaint *paint, const RECT& clipRect);
		virtual void OnPaintCheckButton(CPaint *paint, const RECT& clipRect);
		virtual void OnPaintForeground(CPaint *paint, const RECT& clipRect);
		virtual void SetProperty(const String& name, const String& value);
	};
}

#endif