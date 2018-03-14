/*****************************************************************************\
*                                                                             *
* CrossLine.h - Cross line functions, types, and definitions                  *
*                                                                             *
*               Version 4.00 ¡ï¡ï¡ï¡ï¡ï                                       *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's owchart. All rights reserved. *
*               Modified by Wang Shaoxu.                                      *
*******************************************************************************/

#ifndef __CROSSLINE_H__
#define __CROSSLINE_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "Enums.h"
#include "CDiv.h"

namespace OwLib
{
	class CDiv;
	class CrossLine : CProperty
	{
	private:
		bool m_allowUserPaint;
		bool m_allowDoubleClick;
		AttachVScale m_attachVScale;
		_int64 m_lineColor;
	public:
		CrossLine();
		virtual ~CrossLine();
		bool AllowDoubleClick();
		void SetAllowDoubleClick(bool allowDoubleClick);
		bool AllowUserPaint();
		void SetAllowUserPaint(bool allowUserPaint);
		AttachVScale GetAttachVScale();
		void SetAttachVScale(AttachVScale attachVScale);
		_int64 GetLineColor();
		void SetLineColor(_int64 lineColor);
	public:
		virtual void GetProperty(const String& name, String *value, String *type);
		virtual vector<String> GetPropertyNames();
		virtual void OnPaint(CPaint *paint, CDiv *div, const RECT& rect);
		virtual void SetProperty(const String& name, const String& value);
	};
}
#endif