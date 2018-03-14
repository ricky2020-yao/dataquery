/*****************************************************************************\
*                                                                             *
* CToolTip.h -  Tooltip functions, types, and definitions                     *
*                                                                             *
*               Version 4.00 ¡ï¡ï¡ï¡ï¡ï                                       *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's owchart. All rights reserved. *
*               Modified by Wang Shaoxu.                                      *
*******************************************************************************/


#ifndef __CTOOLTIP_H__
#define __CTOOLTIP_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "..\\Base\\CPaint.h"
#include "CDiv.h"

namespace OwLib
{
	class CDiv;

	class CToolTip : public CProperty
	{
	private:
		bool m_allowUserPaint;
		_int64 m_backColor;
		_int64 m_borderColor;
		FONT *m_font;
		_int64 m_foreColor;
	public:
		CToolTip();
		virtual ~CToolTip();
		bool AllowUserPaint();
		void SetAllowUserPaint(bool allowUserPaint);
		_int64 GetBackColor();
		void SetBackColor(_int64 backColor);
		_int64 GetBorderColor();
		void SetBorderColor(_int64 borderColor);
		FONT* GetFont();
		void SetFont(FONT *font);
		_int64 GetForeColor();
		void SetForeColor(_int64 foreColor);
	public:
		virtual void GetProperty(const String& name, String *value, String *type);
		virtual vector<String> GetPropertyNames();
		virtual void OnPaint(CPaint *paint, CDiv *div, const RECT& rect);
		virtual void SetProperty(const String& name, const String& value);
	};
}
#endif