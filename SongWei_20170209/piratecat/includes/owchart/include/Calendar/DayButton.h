/*****************************************************************************\
*                                                                             *
* DayButton.h -  Day button functions, types, and definitions                 *
*                                                                             *
*               Version 4.00 ¡ï¡ï¡ï¡ï¡ï                                       *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's calendar. All rights reserved.*
*                                                                             *
*******************************************************************************/

#ifndef __DAYBUTTON_H__
#define __DAYBUTTON_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "CDay.h"
#include "Calendar.h"

namespace OwLib
{
	class CalendarA;

	class DayButton
	{
	private:
		RECT m_bounds;
		CalendarA *m_calendar;
		CDay *m_day;
		bool m_inThisMonth;
		bool m_selected;
		bool m_visible;
	protected:
		virtual _int64 GetPaintingBackColor();
		virtual _int64 GetPaintingBorderColor();
		virtual _int64 GetPaintingForeColor();
	public:
		DayButton(CalendarA *calendar);
		virtual ~DayButton();
			RECT GetBounds();
		void SetBounds(const RECT& bounds);
		CalendarA* GetCalendar();
		void SetCalendar(CalendarA *calendar);
		CDay* GetDay();
		void SetDay(CDay *day);
		bool InThisMonth();
		void SetThisMonth(bool inThisMonth);
		bool IsSelected();
		void SetSelected(bool selected);
		bool IsVisible();
		void SetVisible(bool visible);
	public:
		virtual void OnClick(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnPaintBackGround(CPaint *paint, const RECT& clipRect);
		virtual void OnPaintBorder(CPaint *paint, const RECT& clipRect);
		virtual void OnPaintForeground(CPaint *paint, const RECT& clipRect);
	};
}
#endif