/*****************************************************************************\
*                                                                             *
* YearDiv.h    Year div functions, types, and definitions                      *
*                                                                             *
*               Version 4.00 ¡ï¡ï¡ï¡ï¡ï                                       *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's Calendar. All rights reserved.*
*                                                                             *
*******************************************************************************/

#ifndef __YEARDIV_H__
#define __YEARDIV_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "YearButton.h"
#include "HeadDiv.h"

namespace OwLib
{
	class CalendarA;
	class HeadDiv;
	class YearButton;

	class YearDiv 
	{
	private:
		int m_am_Direction;
		int m_am_Tick;
		int m_am_TotalTick;
		CalendarA *m_calendar;
		int m_startYear;
		vector<YearButton*> m_yearButtons;
		vector<YearButton*> m_yearButtons_am;
	public:
		YearDiv(CalendarA *calendar);
		virtual ~YearDiv();
		CalendarA* GetCalendar();
		void SetCalendar(CalendarA *calendar);
		int GetStartYear();
	public:
		void Hide();
		void OnClick(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		void OnLoad();
		void OnPaint(CPaint *paint, const RECT& clipRect);
		void OnResetDiv(int state);
		void OnTimer();
		void SelectStartYear(int startYear);
		void Show();
		void Update();
	};
}
#endif