/*****************************************************************************\
*                                                                             *
* GridCellExtends.h -  Grid cell functions, types, and definitions            *
*                                                                             *
*               Version 4.00 ¡ï¡ï¡ï¡ï¡ï                                       *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's grid. All rights reserved.    *
*               Check right 2016/9/22 by Lord.                                *
*                                                                             *
*******************************************************************************/

#ifndef __GRIDCELLEXTENDS_H__
#define __GRIDCELLEXTENDS_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "GridCell.h"
#include "GridColumn.h"
#include "GridRow.h"
#include "Grid.h"

namespace OwLib
{
	class GridCell;
	class GridColumn;
	class GridRow;
	class GridA;
	class ButtonA;
	class CheckBoxA;
	class ComboBoxA;
	class DatePickerA;
	class LabelA;
	class SpinA;
	class RadioButtonA;
	class TextBoxA;

	class GridBoolCell : public GridCell
	{
	private:
		bool m_value;
	public:
		GridBoolCell();
		GridBoolCell(bool value);
		virtual ~GridBoolCell();
	public:
		virtual int CompareTo(GridCell *cell);
		virtual bool GetBool();
		virtual double GetDouble();
		virtual float GetFloat();
		virtual int GetInt();
		virtual _int64 GetLong();
		virtual String GetString();
	    virtual void SetBool(bool value);
        virtual void SetDouble(double value);
        virtual void SetFloat(float value);
        virtual void SetInt(int value);
        virtual void SetLong(_int64 value);
        virtual void SetString(const String& value);
	};

	class GridButtonCell: public GridControlCell
	{
	public:
		GridButtonCell();
		virtual ~GridButtonCell();
		ButtonA* GetButton();
	};

	class GridCheckBoxCell : public GridControlCell
	{
	private:
	public:
		GridCheckBoxCell();
		virtual ~GridCheckBoxCell();
		CheckBoxA* GetCheckBox();
	public:
		virtual bool GetBool();
		virtual double GetDouble();
		virtual float GetFloat();
		virtual int GetInt();
		virtual _int64 GetLong();
		virtual String GetString();
		virtual void SetBool(bool value);
		virtual void SetDouble(double value);
        virtual void SetFloat(float value);
        virtual void SetInt(int value);
        virtual void SetLong(_int64 value);
        virtual void SetString(const String& value);
	};

	class GridComboBoxCell : public GridControlCell
	{
	public:
		GridComboBoxCell();
		virtual ~GridComboBoxCell();
		ComboBoxA* GetComboBox();
	public:
		virtual bool GetBool();
		virtual double GetDouble();
		virtual float GetFloat();
		virtual int GetInt();
		virtual _int64 GetLong();
		virtual String GetString();
		virtual void SetBool(bool value);
		virtual void SetDouble(double value);
        virtual void SetFloat(float value);
        virtual void SetInt(int value);
        virtual void SetLong(_int64 value);
        virtual void SetString(const String& value);
	};

	class GridDatePickerCell : public GridControlCell
	{
	public:
		GridDatePickerCell();
		~GridDatePickerCell();
		DatePickerA* GetDatePicker();
	};

	class GridDivCell : public GridControlCell
	{
	public:
		GridDivCell();
		~GridDivCell();
		DivA* GetDiv();
	};

	class GridDoubleCell : public GridCell
	{
	private:
		double m_value;
	public:
		GridDoubleCell();
		GridDoubleCell(double value);
		virtual ~GridDoubleCell();
	public:
		virtual int CompareTo(GridCell *cell);
		virtual bool GetBool();
		virtual double GetDouble();
		virtual float GetFloat();
		virtual int GetInt();
		virtual _int64 GetLong();
		virtual String GetString();
		virtual void SetBool(bool value);
        virtual void SetDouble(double value);
        virtual void SetFloat(float value);
        virtual void SetInt(int value);
        virtual void SetLong(_int64 value);
        virtual void SetString(const String& value);
	};

	class GridFloatCell : public GridCell
	{
	private:
		float m_value;
	public:
		GridFloatCell();
		GridFloatCell(float value);
		virtual ~GridFloatCell();
	public:
		virtual int CompareTo(GridCell *cell);
		virtual bool GetBool();
		virtual double GetDouble();
		virtual float GetFloat();
		virtual int GetInt();
		virtual _int64 GetLong();
		virtual String GetString();
		virtual void SetBool(bool value);
        virtual void SetDouble(double value);
        virtual void SetFloat(float value);
        virtual void SetInt(int value);
        virtual void SetLong(_int64 value);
        virtual void SetString(const String& value);
	};

	class GridIntCell : public GridCell
	{
	private:
		int m_value;
	public:
		GridIntCell();
		GridIntCell(int value);
		virtual ~GridIntCell();
	public:
		virtual int CompareTo(GridCell *cell);
		virtual bool GetBool();
		virtual double GetDouble();
		virtual float GetFloat();
		virtual int GetInt();
		virtual _int64 GetLong();
		virtual String GetString();
		virtual void SetBool(bool value);
        virtual void SetDouble(double value);
        virtual void SetFloat(float value);
        virtual void SetInt(int value);
        virtual void SetLong(_int64 value);
        virtual void SetString(const String& value);
	};

	class GridLabelCell : public GridControlCell
	{
	public:
		GridLabelCell();
		~GridLabelCell();
		LabelA* GetLabel();
	};

	class GridLongCell : public GridCell
	{
	private:
		_int64 m_value;
	public:
		GridLongCell();
		GridLongCell(_int64 value);
		virtual ~GridLongCell();
	public:
		virtual int CompareTo(GridCell *cell);
		virtual bool GetBool();
		virtual double GetDouble();
		virtual float GetFloat();
		virtual int GetInt();
		virtual _int64 GetLong();
		virtual String GetString();
		virtual void SetBool(bool value);
        virtual void SetDouble(double value);
        virtual void SetFloat(float value);
        virtual void SetInt(int value);
        virtual void SetLong(_int64 value);
        virtual void SetString(const String& value);
	};

	class GridRadioButtonCell : public GridControlCell
	{
	public:
		GridRadioButtonCell();
		~GridRadioButtonCell();
		RadioButtonA* GetRadioButton();
	public:
		virtual bool GetBool();
		virtual double GetDouble();
		virtual float GetFloat();
		virtual int GetInt();
		virtual _int64 GetLong();
		virtual String GetString();
		virtual void SetBool(bool value);
		virtual void SetDouble(double value);
        virtual void SetFloat(float value);
        virtual void SetInt(int value);
        virtual void SetLong(_int64 value);
        virtual void SetString(const String& value);
	};

	class GridSpinCell : public GridControlCell
	{
	public:
		GridSpinCell();
		virtual ~GridSpinCell();
		SpinA* GetSpin();
	public:
		virtual bool GetBool();
		virtual double GetDouble();
		virtual float GetFloat();
		virtual int GetInt();
		virtual _int64 GetLong();
		virtual void SetBool(bool value);
		virtual void SetDouble(double value);
        virtual void SetFloat(float value);
        virtual void SetInt(int value);
        virtual void SetLong(_int64 value);
	};

	class GridStringCell : public GridCell
	{
	private:
		String m_value;
	public:
		GridStringCell();
		GridStringCell(const String& value);
		~GridStringCell();
	public:
		virtual int CompareTo(GridCell *cell);
		virtual bool GetBool();
		virtual double GetDouble();
		virtual float GetFloat();
		virtual int GetInt();
		virtual _int64 GetLong();
		virtual String GetString();
		virtual void SetString(const String& value);
	};

	class GridTextBoxCell: public GridControlCell
	{
	public:
		GridTextBoxCell();
		virtual ~GridTextBoxCell();
		TextBoxA* GetTextBox();
	};
}
#endif