/*****************************************************************************\
*                                                                             *
* GridCell.h -  Grid cell functions, types, and definitions                   *
*                                                                             *
*               Version 4.00 ¡ï¡ï¡ï¡ï¡ï                                       *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's grid. All rights reserved.    *
*               Check right 2016/9/22 by Lord.                                *
*                                                                             *
*******************************************************************************/

#ifndef __GRIDCELL_H__
#define __GRIDCELL_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "..\\Base\\CProperty.h"
#include "GridColumn.h"
#include "GridRow.h"
#include "Grid.h"

namespace OwLib
{
	class GridColumn;
	class GridRow;
	class GridA;

	class GridCellStyle
	{
	private:
		_int64 m_backColor;
		FONT *m_font;
		_int64 m_foreColor;
	public:
		GridCellStyle();
		virtual ~GridCellStyle();
		_int64 GetBackColor();
		void SetBackColor(_int64 backColor);
		FONT* GetFont();
		void SetFont(FONT *font);
		_int64 GetForeColor();
		void SetForeColor(_int64 foreColor);
	public:
		void Copy(GridCellStyle *style);
	};

	class GridCell : public CProperty
	{
	private:
		bool m_allowEdit;
		int m_colSpan;
		GridColumn *m_column;
		GridA *m_grid;
		String m_name;
		GridRow *m_row;
		int m_rowSpan;
		GridCellStyle *m_style;
	public:
		GridCell();
		virtual ~GridCell();
		bool AllowEdit();
		void SetAllowEdit(bool allowEdit);
		int GetColSpan();
		void SetColSpan(int colSpan);
		GridColumn* GetColumn();
		void SetColumn(GridColumn *column);
		GridA* GetGrid();
		void SetGrid(GridA *grid);
		String GetName();
		void SetName(const String& name);
		GridRow* GetRow();
		void SetRow(GridRow *row);
		int GetRowSpan();
		void SetRowSpan(int rowSpan);
		GridCellStyle* GetStyle();
		void SetStyle(GridCellStyle *style);
		String GetText();
		void SetText(const String& text);
	public:
		virtual int CompareTo(GridCell *cell);
		virtual bool GetBool();
		virtual double GetDouble();
		virtual float GetFloat();
		virtual int GetInt();
		virtual _int64 GetLong();
		virtual String GetPaintText();
		virtual void GetProperty(const String& name, String *value, String *type);
		virtual vector<String> GetPropertyNames();
		virtual String GetString();
		virtual void OnAdd();
		virtual void OnPaint(CPaint *paint, const RECT& rect, const RECT& clipRect, bool isAlternate);
		virtual void OnRemove();
        virtual void SetBool(bool value);
        virtual void SetDouble(double value);
        virtual void SetFloat(float value);
        virtual void SetInt(int value);
        virtual void SetLong(_int64 value);
		virtual void SetProperty(const String& name, const String& value);
        virtual void SetString(const String& value);
	};

	class GridControlCell : public GridCell
	{
	private:
		ControlA *m_control;
		ControlMouseEvent m_mouseDownEvent;
		ControlMouseEvent m_mouseMoveEvent;
		ControlMouseEvent m_mouseUpEvent;
		ControlMouseEvent m_mouseWheelEvent;
	private:
		static void ControlMouseDown(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke);
		static void ControlMouseMove(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke);
		static void ControlMouseUp(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke);
		static void ControlMouseWheel(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke);
	public:
		GridControlCell();
		virtual ~GridControlCell();
		ControlA* GetControl();
		void SetControl(ControlA *control);
		virtual String GetString();
		virtual String GetPaintText();
		virtual void OnAdd();
		virtual void OnControlMouseDown(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnControlMouseMove(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnControlMouseUp(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnControlMouseWheel(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnPaint(CPaint *paint, const RECT& rect, const RECT& clipRect, bool isAlternate);
		virtual void OnPaintControl(CPaint *paint, const RECT& rect, const RECT& clipRect);
		virtual void OnRemove();
		virtual void SetString(const String& value);
	};
}

#endif