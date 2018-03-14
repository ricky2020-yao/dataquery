/*****************************************************************************\
*                                                                             *
* DateTitle.h -  Date title functions, types, and definitions                 *
*                                                                             *
*               Version 4.00 ¡ï¡ï¡ï¡ï¡ï                                       *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's calendar. All rights reserved.*
*                                                                             *
*******************************************************************************/

#ifndef __DATETITLE_H__
#define __DATETITLE_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "..\\Button\\Button.h"
#include "Calendar.h"

namespace OwLib
{
	class CalendarA;

	class DateTitle : public ButtonA
	{
	private:
		CalendarA *m_calendar;
	protected:
		virtual _int64 GetPaintingBackColor();
		virtual _int64 GetPaintingForeColor();
	public:
		DateTitle(CalendarA *calendar);
		virtual ~DateTitle();
		CalendarA* GetCalendar();
		void SetCalendar(CalendarA *calendar);
	public:
		virtual String GetControlType();
		virtual void OnClick(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnPaintForeground(CPaint *paint, const RECT& clipRect);
	};
}
#endif