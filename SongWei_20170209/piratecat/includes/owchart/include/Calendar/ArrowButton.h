/*****************************************************************************\
*                                                                             *
* ArrowButton.h -  Arrow button functions, types, and definitions             *
*                                                                             *
*               Version 4.00 ¡ï¡ï¡ï¡ï¡ï                                       *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's calendar. All rights reserved.*
*                                                                             *
*******************************************************************************/

#ifndef __ARROWBUTTON_H__
#define __ARROWBUTTON_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "..\\Button\\Button.h"
#include "Calendar.h"
#include "YearDiv.h"

namespace OwLib
{
	class CalendarA;

	class ArrowButton:public ButtonA
	{
	private:
		CalendarA *m_calendar;
		bool m_toLast;
	protected:
		virtual _int64 GetPaintingBackColor();
	public:
		ArrowButton(CalendarA *calendar);
		virtual ~ArrowButton();
		CalendarA* GetCalendar();
		void SetCalendar(CalendarA *calendar);
		bool IsToLast();
		void SetToLast(bool toLast);
	public:
		virtual String GetControlType();
		virtual void OnClick(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnPaintBackground(CPaint *paint, const RECT& clipRect);
	};
}
#endif