/*****************************************************************************\
*                                                                             *
* MonthDiv.h   Month div functions, types, and definitions                    *
*                                                                             *
*               Version 4.00 ¡ï¡ï¡ï¡ï¡ï                                       *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's Calendar. All rights reserved.*
*                                                                             *
*******************************************************************************/

#ifndef __MONTHDIV_H__
#define __MONTHDIV_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "MonthButton.h"
#include "HeadDiv.h"

namespace OwLib
{
	class MonthButton;
	class HeadDiv;
	class CalendarA;

	class MonthDiv
	{
	private:
		int m_am_Direction;
		int m_am_Tick;
		int m_am_TotalTick;
		CalendarA *m_calendar;
		int m_year;
		vector<MonthButton*> m_monthButtons;
		vector<MonthButton*> m_monthButtons_am;
	public:
		MonthDiv(CalendarA *calendar);
		virtual ~MonthDiv();
		CalendarA* GetCalendar();
		void SetCalendar(CalendarA *calendar);
		int GetYear();
	public:
		virtual void Hide();
		void OnClick(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnLoad();
		void OnPaint(CPaint *paint, const RECT& clipRect);
		void OnResetDiv(int state);
		virtual void OnTimer();
		virtual void SelectYear(int year);
		virtual void Show();
		void Update();
	};
}
#endif