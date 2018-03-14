/*************************************************************************************\
*                                                                                     *
* Spin.h -      Numeric up down functions, types, and definitions                     *
*                                                                                     *
*               Version 4.00 ¡ï¡ï¡ï¡ï¡ï                                               *
*                                                                                     *
*               Copyright (c) 2016-2016, Lord's numeric up down. All rights reserved. *
*               Checked 2016/9/25 by Lord.                                            *
*                                                                                     *
**************************************************************************************/

#ifndef __SPIN_H__
#define __SPIN_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "..\\Base\\CStr.h"
#include "TextBox.h"
#include "..\\Button\\Button.h"

namespace OwLib
{
	class SpinA : public TextBoxA
	{
	private:
		int m_digit;
		ButtonA *m_downButton;
		ControlMouseEvent m_downButtonMouseDownEvent;
		double m_maximum;
		double m_minimum;
		double m_step;
		ButtonA *m_upButton;
		ControlMouseEvent m_upButtonMouseDownEvent;
		double m_value;
		static void DownButtonMouseDown(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke);
		static void UpButtonMouseDown(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke);
	public:
		SpinA();
		virtual ~SpinA();
		int GetDigit();
		void SetDigit(int digit);
		ButtonA* GetDownButton();
		double GetMaximum();
		void SetMaximum(double maximum);
		double GetMinimum();
		void SetMinimum(double minimum);
		double GetStep();
		void SetStep(double step);
		ButtonA* GetUpButton();
		double GetValue();
		void SetValue(double value);
	public:
		void Add();
		virtual String GetControlType();
		virtual void GetProperty(const String& name, String *value, String *type);
		virtual vector<String> GetPropertyNames();
		virtual void OnKeyDown(char key);
		virtual void OnLoad();
		virtual void OnMouseWheel(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnValueChanged();
		void Reduce();
		virtual void SetProperty(const String& name, const String& value);
		virtual String TextValidation(const String& text);
		virtual void Update();
	};
}

#endif