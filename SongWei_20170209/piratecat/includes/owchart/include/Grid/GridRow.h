/*****************************************************************************\
*                                                                             *
* GridRow.h -   Grid row functions, types, and definitions                    *
*                                                                             *
*               Version 4.00 ¡ï¡ï¡ï¡ï¡ï                                       *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's grid. All rights reserved.    *
*               Check right 2016/9/22 by Lord.                                *
*                                                                             *
*******************************************************************************/

#ifndef __GRIDROW_H__
#define __GRIDROW_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "..\\Base\\CProperty.h"
#include "GridCell.h"
#include "Grid.h"

namespace OwLib
{
	class GridCell;
	class GridA;

	class GridRowStyle
	{
	private:
		_int64 m_backColor;
		FONT *m_font;
		_int64 m_foreColor;
		_int64 m_hoveredBackColor;
		_int64 m_hoveredForeColor;
		_int64 m_selectedBackColor;
		_int64 m_selectedForeColor;
	public:
		GridRowStyle();
		virtual ~GridRowStyle();
		_int64 GetBackColor();
		void SetBackColor(_int64 backColor);
		FONT* GetFont();
		void SetFont(FONT *font);
		_int64 GetForeColor();
		void SetForeColor(_int64 foreColor);
		_int64 GetHoveredBackColor();
		void SetHoveredBackColor(_int64 hoveredBackColor);
		_int64 GetHoveredForeColor();
		void SetHoveredForeColor(_int64 hoveredForeColor);
		_int64 GetSelectedBackColor();
		void SetSelectedBackColor(_int64 selectedBackColor);
		_int64 GetSelectedForeColor();
		void SetSelectedForeColor(_int64 selectedForeColor);
	public:
		void Copy(GridRowStyle *style);
	};

	class GridRow : public CProperty
	{
	private:
		RECT m_bounds;
		GridA *m_grid;
		int m_height;
		int m_index;
		bool m_visible;
		int m_visibleIndex;
	public:
		vector<GridCell*> m_cells;
		GridRow();
		virtual ~GridRow();
		RECT GetBounds();
		void SetBounds(RECT bounds);
		GridA* GetGrid();
		void SetGrid(GridA *grid);
		int GetHeight();
		void SetHeight(int height);
		int GetIndex();
		void SetIndex(int index);
		bool IsVisible();
		void SetVisible(bool visible);
		int GetVisibleIndex();
		void SetVisibleIndex(int visibleIndex);
	public:
		void AddCell(int columnIndex, GridCell *cell);
		void ClearCells();
		vector<GridCell*> GetCells();
		GridCell* GetCell(int columnIndex);
		GridCell* GetCell(String columnName);
		virtual void GetProperty(const String& name, String *value, String *type);
		virtual vector<String> GetPropertyNames();
		virtual void OnAdd();
		virtual void OnRemove();
		void RemoveCell(int columnIndex);
		virtual void SetProperty(const String& name, const String& value);
	};
}

#endif