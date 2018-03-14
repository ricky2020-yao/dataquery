/*****************************************************************************\
*                                                                             *
* SelectArea.h -Select Area functions, types, and definitions                 *
*                                                                             *
*               Version 4.00 ¡ï¡ï¡ï¡ï¡ï                                       *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's owchart. All rights reserved. *
*               Modified by Wang Shaoxu.                                      *
*******************************************************************************/

#ifndef __SELECTAREA_H__
#define __SELECTAREA_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "CDiv.h"

namespace OwLib
{
	class CDiv;

	class SelectArea : public CProperty
	{
	private:
		bool m_allowUserPaint;
		_int64 m_backColor;
		RECT m_bounds;
		bool m_canResize;
		bool m_enabled;
		_int64 m_lineColor;
		bool m_visible;
	public:
		SelectArea();
		virtual ~SelectArea();
		bool AllowUserPaint();
		void SetAllowUserPaint(bool allowUserPaint);
		_int64 GetBackColor();
		void SetBackColor(_int64 backColor);
		RECT GetBounds();
		void SetBounds(RECT bounds);
		bool CanResize();
		void SetCanResize(bool canResize);
		bool IsEnabled();
		void SetEnabled(bool enabled);
		_int64 GetLineColor();
		void SetLineColor(_int64 lineColor);
		bool IsVisible();
		void SetVisible(bool visible);
	public:
		void Close();
		virtual void GetProperty(const String& name, String *value, String *type);
		virtual vector<String> GetPropertyNames();
		virtual void OnPaint(CPaint *paint, CDiv *div, const RECT& rect);
		virtual void SetProperty(const String& name, const String& value);

	};
}
#endif