/*****************************************************************************\
*                                                                             *
* Grid.h -    Grid functions, types, and definitions                          *
*                                                                             *
*               Version 4.00 ¡ï¡ï¡ï¡ï¡ï                                       *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's grid. All rights reserved.    *
*               Check right 2016/9/22 by Lord.                                *
*                                                                             *
*******************************************************************************/

#ifndef __GRID_H__
#define __GRID_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "GridColumn.h"
#include "GridRow.h"
#include "GridCell.h"
#include "..\\ScrollBar\\HScrollBar.h"
#include "..\\ScrollBar\\VScrollBar.h"
#include "..\\Layout\\Div.h"
#include "..\\TextBox\\TextBox.h"

namespace OwLib
{
	class GridCell;
	class GridColumn;
	class GridRow;
	class GridRowStyle;
	
	typedef void (*GridCellEvent) (void*, GridCell*, void*);

	typedef void (*GridCellMouseEvent)(void*, GridCell*, const POINT&, MouseButtonsA, int, int, void*);

	class GridRowCompare
	{
	public:
		int m_columnIndex;
		int m_type;
	public:
		GridRowCompare();
		virtual ~GridRowCompare();
		bool operator()(GridRow *x, GridRow *y);
	};

	class GridA:public DivA
	{
	private:
		ControlEvent m_editTextBoxLostFocusEvent;
		ControlKeyEvent m_editTextBoxKeyDownEvent;
	private:
		bool m_allowDragRow;
		GridRowStyle *m_alternateRowStyle;
		GridCellEditMode m_cellEditMode;
		GridCell *m_editingCell;
		TextBoxA *m_editTextBox;
		_int64 m_gridLineColor;
		bool m_headerVisible;
		int m_headerHeight;
		int m_horizontalOffset;
		GridRow *m_hoveredRow;
		bool m_multiSelect;
		GridRowStyle *m_rowStyle;
		GridSelectionMode m_selectionMode;
		int m_timerID;
		bool m_useAnimation;
		int m_verticalOffset;
		void CallCellEvents(int eventID, GridCell *cell);
		void CallCellMouseEvents(int eventID, GridCell *cell, const POINT& mp, MouseButtonsA button, int clicks, int delta);
		void MouseEvent(const POINT& mp, MouseButtonsA button, int clicks, int delta, int state);
		static void EditTextBoxLostFocus(void *sender, void *pInvoke);
		static void EditTextBoxKeyDown(void *sender, char key, void *pInvoke);
	protected:
		bool m_lockUpdate;
		vector<GridCell*> m_selectedCells;
		vector<GridColumn*> m_selectedColumns;
		vector<GridRow*> m_selectedRows;
		int GetAllVisibleColumnsWidth();
		int GetAllVisibleRowsHeight();
	public:
		vector<GridColumn*> m_columns;
		vector<GridRow*> m_rows;
		GridA();
		virtual ~GridA();
		bool AllowDragRow();
		void SetAllowDragRow(bool allowDragRow);
		GridRowStyle* GetAlternateRowStyle();
		void SetAlternateRowStyle(GridRowStyle *alternateRowStyle);
		GridCellEditMode GetCellEditMode();
		void SetCellEditMode(GridCellEditMode cellEditMode);
		TextBoxA* GetEditTextBox();
		_int64 GetGridLineColor();
		void SetGridLineColor(_int64 gridLineColor);
		bool IsHeaderVisible();
		void SetHeaderVisible(bool headerVisible);
		int GetHeaderHeight();
		void SetHeaderHeight(int headerHeight);
		int GetHorizontalOffset();
		void SetHorizontalOffset(int horizontalOffset);
		GridRow* GetHoveredRow();
		bool IsMultiSelect();
		void SetMultiSelect(bool multiSelect);
		GridRowStyle* GetRowStyle();
		void SetRowStyle(GridRowStyle *rowStyle);
		vector<GridCell*> GetSelectedCells();
		void SetSelectedCells(vector<GridCell*> selectedCells);
		vector<GridColumn*> GetSelectedColumns();
		void SetSelectedColumns(vector<GridColumn*> selectedColumns);
		vector<GridRow*> GetSelectedRows();
		void SetSelectedRows(vector<GridRow*> selectedRows);
		GridSelectionMode GetSelectionMode();
		void SetSelectionMode(GridSelectionMode selectionMode);
		bool UseAnimation();
		void SetUseAnimation(bool useAnimation);
		int GetVerticalOffset();
		void SetVerticalOffset(int verticalOffset);
	public:
		virtual void AddColumn(GridColumn *column);
		void AddRow(GridRow *row);
		void BeginUpdate();
		void Clear();
		virtual void ClearColumns();
		void ClearRows();
		void EndUpdate();
		GridColumn* GetColumn(int columnIndex);
		vector<GridColumn*> GetColumns();
		virtual int GetContentHeight();
		virtual int GetContentWidth();
		virtual String GetControlType();
		virtual POINT GetDisplayOffset();
		virtual void GetProperty(const String& name, String *value, String *type);
		virtual vector<String> GetPropertyNames();
		GridRow* GetRow(const POINT& mp);
		GridRow* GetRow(int rowIndex);
		vector<GridRow*> GetRows();
		void GetVisibleRowsIndex(double visiblePercent, int *firstVisibleRowIndex, int *lastVisibleRowIndex);
		void InsertRow(int index, GridRow *row);
		bool IsRowVisible(GridRow *row, double visiblePercent);
		void MoveRow(int oldIndex, int newIndex);
		virtual void OnCellClick(GridCell *cell, const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnCellEditBegin(GridCell *cell);
		virtual void OnCellEditEnd(GridCell *cell);
		virtual void OnCellMouseDown(GridCell *cell, const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnCellMouseMove(GridCell *cell, const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnCellMouseUp(GridCell *cell, const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnKeyDown(char key);
		virtual void OnLoad();
		virtual void OnLostFocus();
		virtual void OnMouseDown(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnMouseLeave(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnMouseMove(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnMouseUp(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnPaintForeground(CPaint *paint, const RECT& clipRect);
		virtual void OnPaintEditTextBox(GridCell *cell, CPaint *paint, const RECT& rect, const RECT& clipRect);
		virtual void OnSelectedCellsChanged();
		virtual void OnSelectedColumnsChanged();
		virtual void OnSelectedRowsChanged();
		virtual void OnSetEmptyClipRegion();
		virtual void OnTimer(int timerID);
		virtual void OnVisibleChanged();
		virtual void RemoveColumn(GridColumn *column);
		void RemoveRow(GridRow *row);
		virtual void ResetHeaderLayout();
		GridRow* SelectFrontRow();
		GridRow* SelectNextRow();
		virtual void SetProperty(const String& name, const String& value);
		void Sort(GridColumn *column, GridColumnSortMode sortMode);
		virtual void Update();
		void UpdateSortColumn();
	};
}

#endif