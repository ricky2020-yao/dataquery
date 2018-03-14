/*****************************************************************************\
*                                                                             *
* ScaleGrid.h -     Scale grid functions, types, and definitions              *
*                                                                             *
*               Version 4.00 ¡ï¡ï¡ï¡ï¡ï                                       *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's owchart. All rights reserved. *
*               Modified by Wang Shaoxu.                                      *
*******************************************************************************/

#ifndef __SCALEGRID_H__
#define __SCALEGRID_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "CDiv.h"

namespace OwLib
{
	class CDiv;

	class ScaleGrid : public CProperty
	{
	private:
		bool m_allowUserPaint;
		int m_distance;
		_int64 m_gridColor;
		int m_lineStyle;
		bool m_visible;
	public:
		ScaleGrid();
		virtual ~ScaleGrid();
		bool AllowUserPaint();
		void SetAllowUserPaint(bool allowUserPaint);
		int GetDistance();
		void SetDistance(int distance);
		_int64 GetGridColor();
		void SetGridColor(_int64 gridColor);
		int GetLineStyle();
		void SetLineStyle(int lineStyle);
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