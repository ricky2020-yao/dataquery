/*****************************************************************************\
*                                                                             *
* TableLayoutDiv.h -  Table layout div functions, types, and definitions      *
*                                                                             *
*               Version 4.00 ¡ï¡ï¡ï¡ï¡ï                                       *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's layout. All rights reserved.  *
*               Check right 2016/7/2 by Lord.                                 *
*                                                                             *
*******************************************************************************/
#ifndef __TABLELAYOUTDIV_H__
#define __TABLELAYOUTDIV_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "Div.h"

namespace OwLib
{	
	typedef enum SizeTypeA
	{
		SizeTypeA_AbsoluteSize,
		SizeTypeA_PercentSize
	};

	class ColumnStyleA
	{
	private:
		SizeTypeA m_sizeType;
		float m_width;
	public:
		ColumnStyleA(SizeTypeA sizeType, float width);
		virtual ~ColumnStyleA();
		SizeTypeA GetSizeType();
		void SetSizeTypeA(SizeTypeA  sizeType);
		float GetWidth();
		void SetWidth(float width);
	};

	class RowStyleA
	{
	private:
		float m_height;
		SizeTypeA m_sizeType;
	public:
		RowStyleA(SizeTypeA sizeType, float height);
		virtual ~RowStyleA();
		float GetHeight();
		void SetHeight(float height);
		SizeTypeA GetSizeType();
		void SetSizeTypeA(SizeTypeA  sizeType);
	};

	class TableLayoutDivA : public DivA
	{
	private:
		vector<int> m_columns;
		int m_columnsCount;
		vector<int> m_rows;
		int m_rowsCount;
		vector<ControlA*> m_tableControls;
	public:
		vector<ColumnStyleA> m_columnStyles;
		vector<RowStyleA> m_rowStyles;
		TableLayoutDivA();
		virtual ~TableLayoutDivA();
		int GetColumnsCount();
		void SetColumnsCount(int columnsCount);
		int GetRowsCount();
		void SetRowsCount(int rowsCount);
	public:
		virtual void AddControl(ControlA *control, int column, int row);
		virtual String GetControlType();
		virtual void GetProperty(const String& name, String *value, String *type);
		virtual vector<String> GetPropertyNames();
		virtual bool OnResetLayout();
		virtual void RemoveControl(ControlA *control);
		virtual void SetProperty(const String& name, const String& value);
		virtual void Update();
	};
}

#endif