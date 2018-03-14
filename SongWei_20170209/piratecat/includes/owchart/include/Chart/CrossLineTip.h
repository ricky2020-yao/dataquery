/*****************************************************************************\
*                                                                             *
* CrossLineTip.h - Cross line tip functions, types, and definitions           *
*                                                                             *
*               Version 4.00 ¡ï¡ï¡ï¡ï¡ï                                       *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's owchart. All rights reserved. *
*               Modified by Wang Shaoxu.                                      *
*******************************************************************************/

#ifndef __CROSSLINETIP_H__
#define __CROSSLINETIP_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "..\\Base\\CPaint.h"
#include "CDiv.h"

namespace OwLib
{
	class CDiv;

	class CrossLineTip
	{
	private:
		bool m_allowUserPaint;
		_int64 m_backColor;
		FONT *m_font;
		_int64 m_foreColor;
		bool m_visible;
	public:
		CrossLineTip();
		virtual ~CrossLineTip();
		bool AllowUserPaint();
		void SetAllowUserPaint(bool allowUserPaint);
		_int64 GetBackColor();
		void SetBackColor(_int64 backColor);
		FONT* GetFont();
		void SetFont(FONT *font);
		_int64 GetForeColor();
		void SetForeColor(_int64 foreColor);
		bool IsVisible();
		void SetVisible(bool visible);
	public:
		virtual void OnPaint(CPaint *paint, CDiv *div, const RECT& rect);
		virtual void GetProperty(const String& name, String *value, String *type);
		virtual vector<String> GetPropertyNames();
		virtual void SetProperty(const String& name, const String& value);
	};
}
#endif