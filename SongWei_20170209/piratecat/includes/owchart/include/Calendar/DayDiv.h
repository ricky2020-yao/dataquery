/*****************************************************************************\
*                                                                             *
* DayDiv.cs -  Day div functions, types, and definitions                      *
*                                                                             *
*               Version 4.00 ������                                       *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's Calendar. All rights reserved.*
*                                                                             *
*******************************************************************************/

#ifndef __DAYDIV_H__
#define __DAYDIV_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "DayButton.h"
#include "CYears.h"
#include "CMonth.h"
#include "CDay.h"
#include "HeadDiv.h"

namespace OwLib
{
	class DayButton;
	class HeadDiv;
	class CalendarA;

	class DayDiv
	{
	private:
		int m_am_ClickRowFrom;
		int m_am_ClickRowTo;
		int m_am_Direction;
		int m_am_Tick;
		int m_am_TotalTick;
		CalendarA *m_calendar;
		vector<DayButton*> m_dayButtons;
		vector<DayButton*> m_dayButtons_am;
	public:
		DayDiv(CalendarA *calendar);
		virtual ~DayDiv();
		CalendarA* GetCalendar();
		void SetCalendar(CalendarA *calendar);
	public:
		void Hide();
		void OnClick(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		void OnLoad();
		void OnPaint(CPaint *paint, const RECT& clipRect);
		void OnResetDiv(int state);
		void OnTimer();
		void SelectDay(CDay *selectedDay);
		void Show();
		void Update();
	};
}
#endif