/*****************************************************************************\
*                                                                             *
* Calendar.h -   Calendar functions, types, and definitions                   *
*                                                                             *
*               Version 4.00 ¡ï¡ï¡ï¡ï¡ï                                       *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's calendar. All rights reserved.*
*                                                                             *
*******************************************************************************/

#ifndef __CALENDAR_H__
#define __CALENDAR_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "CDay.h"
#include "CYears.h"
#include "CMonth.h"
#include "ArrowButton.h"
#include "DateTitle.h"
#include "DayButton.h"
#include "DayDiv.h"
#include "HeadDiv.h"
#include "MonthButton.h"
#include "MonthDiv.h"
#include "YearButton.h"
#include "YearDiv.h"

namespace OwLib
{
	class ArrowButton;
	class DateTitle;
	class DayButton;
	class DayDiv;
	class HeadDiv;
	class MonthButton;
	class MonthDiv;
	class YearButton;
	class YearDiv;

	typedef enum CalendarMode
	{
		CalendarMode_Day,
		CalendarMode_Month,
		CalendarMode_Year
	};

	class CalendarA : public ControlA
	{
	private:
		DayDiv *m_dayDiv;
		HeadDiv *m_headDiv;
		CalendarMode m_mode;
		int m_month;
		MonthDiv *m_monthDiv;
		CDay *m_selectedDay;
		int m_timerID;;
		bool m_useAnimation;
		int m_year;
		YearDiv *m_yearDiv;
		CYears *m_years;
	public:
		CalendarA();
		virtual ~CalendarA();
		DayDiv* GetDayDiv();
		void SetDayDiv(DayDiv *dayDiv);
		HeadDiv* GetHeadDiv();
		void SetHeadDiv(HeadDiv *headDiv);
		CalendarMode GetMode();
		void SetMode(CalendarMode mode);
		CMonth* GetMonth();
		void SetMonth(CMonth *month);
		MonthDiv* GetMonthDiv();
		void SetMonthDiv(MonthDiv *monthDiv);
		CDay* GetSelectedDay();
		void SetSelectedDay(CDay *day);
		bool UseAnimation();
		void SetUseAnimation(bool useAnimation);
		YearDiv* GetYearDiv();
		void SetYearDiv(YearDiv *yearDiv);
		CYears* GetYears();
		void SetYears(CYears *years);
	public:
		int DayOfWeek(int y, int m, int d);
		virtual String GetControlType();
		CMonth* GetLastMonth(int year, int month);
		CMonth* GetNextMonth(int year, int month);
		virtual void GetProperty(const String& name, String *value, String *type);
		virtual vector<String> GetPropertyNames();
		void GoLastMonth();
		void GoNextMonth();
		virtual void OnClick(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnLoad();
		virtual void OnKeyDown(char key);
		virtual void OnPaintBackground(CPaint *paint, const RECT& clipRect);
		virtual void OnSelectedDayChanged();
		virtual void OnTimer(int timerID);
		virtual void SetProperty(const String& name, const String& value);
		virtual void Update();
	};
}
#endif