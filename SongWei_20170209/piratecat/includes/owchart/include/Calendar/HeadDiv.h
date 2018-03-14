/*****************************************************************************\
*                                                                             *
* HeadDiv.h -   HeadDiv functions, types, and definitions                     *
*                                                                             *
*               Version 4.00 ������                                       *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's calendar. All rights reserved.*
*                                                                             *
*******************************************************************************/

#ifndef __HEADDIV_H__
#define __HEADDIV_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "CDay.h"
#include "Calendar.h"
#include "DateTitle.h"

namespace OwLib
{
	class ArrowButton;
	class DateTitle;

	class HeadDiv:public ControlA
	{
	private:
		CalendarA *m_calendar;
		DateTitle *m_dateTitle;
		ArrowButton *m_lastBtn;
		ArrowButton *m_nextBtn;
		String m_weekStrings[7];
	public:
		HeadDiv(CalendarA *calendar);
		virtual ~HeadDiv();
		CalendarA* GetCalendar();
		void SetCalendar(CalendarA *calendar);
		DateTitle* GetDateTitle();
		void SetDateTitle(DateTitle *dateTitle);
		ArrowButton* GetLastBtn();
		void SetLastBtn(ArrowButton *lastBtn);
		ArrowButton* GetNextBtn();
		void SetNextBtn(ArrowButton *nextBtn);
	public:
		virtual String GetControlType();
		virtual void OnLoad();
		virtual void OnPaintBackground(CPaint *paint, const RECT& clipRect);
		virtual void OnPaintBorder(CPaint *paint, const RECT& clipRect);
		virtual void OnPaintForeground(CPaint *paint, const RECT& clipRect);
		virtual void Update();
	};
}

#endif