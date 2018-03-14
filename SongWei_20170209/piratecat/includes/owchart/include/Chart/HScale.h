/*****************************************************************************\
*                                                                             *
* HScale.h -    Horizontal scale functions, types, and definitions            *
*                                                                             *
*               Version 4.00 ¡ï¡ï¡ï¡ï¡ï                                       *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's owchart. All rights reserved. *
*               Modified by Wang Shaoxu.                                      *
*******************************************************************************/


#ifndef __HSCALE_H__
#define __HSCALE_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "..\\Base\\CPaint.h"
#include "Enums.h"
#include "CrossLineTip.h"
#include "CDiv.h"

namespace OwLib
{
	class CDiv;
	class CrossLineTip;

	class HScale : public CProperty
	{
	private:
		bool m_allowUserPaint;
		CrossLineTip *m_crossLineTip;
		map<DateType, _int64> m_dateColors;
		FONT *m_font;
		_int64 m_foreColor;
		int m_height;
		HScaleType m_hScaleType;
		int m_interval;
		_int64 m_scaleColor;
		vector<double> m_scaleSteps;
		bool m_visible;
	public:
		HScale();
		virtual ~HScale();
		bool AllowUserPaint();
		void SetAllowUserPaint(bool allowUserPaint);
		CrossLineTip* GetCrossLineTip();
		_int64 GetDateColor(DateType dateType);
		void SetDateColor(DateType dateType, _int64 color);
		FONT* GetFont();
		void SetFont(FONT *font);
		_int64 GetForeColor();
		void SetForeColor(_int64 foreColor);
		int GetHeight();
		void SetHeight(int height);
		HScaleType GetHScaleType();
		void SetHScaleType(HScaleType hScaleType);
		int GetInterval();
		void SetInterval(int interval);
		_int64 GetScaleColor();
		void SetScaleColor(_int64 scaleColor);
		bool IsVisible();
		void SetVisible(bool visible);
	public:
		virtual void GetProperty(const String& name, String *value, String *type);
		virtual vector<String> GetPropertyNames();
		vector<double> GetScaleSteps();
		virtual void OnPaint(CPaint *paint, CDiv *div, const RECT& rect);
		virtual void SetProperty(const String& name, const String& value);
		void SetScaleSteps(vector<double> scaleSteps);
	};
}
#endif