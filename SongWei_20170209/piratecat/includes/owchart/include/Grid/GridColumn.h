/*****************************************************************************\
*                                                                             *
* GridColumn.h -  Grid column functions, types, and definitions               *
*                                                                             *
*               Version 4.00 ¡ï¡ï¡ï¡ï¡ï                                       *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's grid. All rights reserved.    *
*               Check right 2016/9/22 by Lord.                                *
*                                                                             *
*******************************************************************************/

#ifndef __GRIDCOLUMN_H__
#define __GRIDCOLUMN_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "GridEnums.h"
#include "Grid.h"
#include "..\\Button\\Button.h"

namespace OwLib
{
	class GridA;

	class GridColumn : public ButtonA
	{
	private:
		bool m_allowSort;
		bool m_allowResize;
		String m_columnType;
		bool m_frozen;
		GridA *m_grid;
		RECT m_headerRect;
		HorizontalAlignA m_horizontalAlign;
		int m_index;
		GridColumnSortMode m_sortMode;
	protected:
		int m_beginWidth;
        POINT m_mouseDownPoint;
		int m_resizeState;
	public:
		GridColumn();
		GridColumn(const String& text);
		virtual ~GridColumn();
		bool AllowResize();
		void SetAllowResize(bool allowResize);
		bool AllowSort();
		void SetAllowSort(bool allowSort);
		String GetColumnType();
		void SetColumnType(String columnType);
		bool IsFrozen();
		void SetFrozen(bool frozen);
		GridA* GetGrid();
		void SetGrid(GridA *grid);
		RECT GetHeaderRect();
		void SetHeaderRect(RECT headerRect);
		HorizontalAlignA GetHorizontalAlign();
		void SetHorizontalAlign(HorizontalAlignA horizontalAlign);
		int GetIndex();
		void SetIndex(int index);
		GridColumnSortMode GetSortMode();
		void SetSortMode(GridColumnSortMode sortMode);
	public:
		virtual String GetControlType();
		virtual void GetProperty(const String& name, String *value, String *type);
		virtual vector<String> GetPropertyNames();
		virtual void OnClick(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual bool OnDragBegin();
		virtual void OnDragging();
		virtual void OnMouseDown(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnMouseMove(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnMouseUp(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnPaintForeground(CPaint *paint, const RECT& clipRect);
		virtual void SetProperty(const String& name, const String& value);
	};
}

#endif