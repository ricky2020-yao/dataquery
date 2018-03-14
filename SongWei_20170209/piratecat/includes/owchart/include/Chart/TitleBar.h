/*****************************************************************************\
*                                                                             *
* TitleBar.h -  Title Bar functions, types, and definitions                   *
*                                                                             *
*               Version 4.00 ¡ï¡ï¡ï¡ï¡ï                                       *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's owchart. All rights reserved. *
*               Modified by Wang Shaoxu.                                      *
*******************************************************************************/

#ifndef __TITLEBAR_H__
#define __TITLEBAR_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "..\\Base\\CPaint.h"
#include "CTable.h"
#include "Enums.h"
#include "CList.h"
#include "CDiv.h"

namespace OwLib
{
	class CDiv;

	class CTitle : public CProperty
	{
	private:
		int m_digit;
		int m_fieldName;
		String m_fieldText;
		TextMode m_fieldTextMode;
		String m_fieldTextSeparator;
		_int64 m_textColor;
		bool m_visible;
	public:
		CTitle(int fieldName, const String& fieldText, _int64 color, int digit, bool visible);
		int GetDigit();
		void SetDigit(int digit);
		int GetFieldName();
		void SetFieldName(int fieldName);
		String GetFieldText();
		void SetFieldText(const String& fieldText);
		TextMode GetFieldTextMode();
		void SetFieldTextMode(TextMode fieldTextMode);
		String GetFieldTextSeparator();
		void SetFieldTextSeparator(const String& fieldTextSeparator);
		_int64 GetTextColor();
		void SetTextColor(_int64 textColor);
		bool IsVisible();
		void SetVisible(bool visible);
	public:
		virtual void GetProperty(const String& name, String *value, String *type);
		virtual vector<String> GetPropertyNames();
		virtual void SetProperty(const String& name, const String& value);
	};

	class TitleBar : public CProperty
	{
	private:
		bool m_allowUserPaint;
		FONT *m_font;
		_int64 m_foreColor;
		int m_height;
		int m_maxLine;
		bool m_showUnderLine;
		String m_text;
		_int64 m_underLineColor;
		bool m_visible;
	public:
		TitleBar();
		virtual ~TitleBar();
		vector<CTitle*> Titles;
		bool AllowUserPaint();
		void SetAllowUserPaint(bool allowUserPaint);
		FONT* GetFont();
		void SetFont(FONT *font);
		_int64 GetForeColor();
		void SetForeColor(_int64 foreColor);
		int GetHeight();
		void SetHeight(int height);
		int GetMaxLine();
		void SetMaxLine(int maxLine);
		bool ShowUnderLine();
		void SetShowUnderLine(bool showUnderLine);
		String GetText();
		void SetText(const String& text);
		_int64 GetUnderLineColor();
		void SetUnderLineColor(_int64 underLineColor);
		bool IsVisible();
		void SetVisible(bool visible);
	public:
		virtual void GetProperty(const String& name, String *value, String *type);
		virtual vector<String> GetPropertyNames();
		virtual void OnPaint(CPaint *paint, CDiv *div, const RECT& rect);
		virtual void SetProperty(const String& name, const String& value);
	};
}
#endif