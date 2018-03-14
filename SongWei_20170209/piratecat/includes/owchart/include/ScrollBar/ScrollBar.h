/*********************************************************************************\
*                                                                                 *
* ScrollBar.h - Scroll bar functions, types, and definitions                    *
*                                                                                 *
*               Version 4.00 ¡ï¡ï¡ï¡ï¡ï                                           *
*                                                                                 *
*               Copyright (c) 2016-2016, Lord's scroll bar. All rights reserved.  *
*                                                                                 *
***********************************************************************************/

#ifndef __SCROLLBAR_H__
#define __SCROLLBAR_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "..\\Button\\Button.h"

namespace OwLib
{
	class ScrollBarA:public ControlA
	{
	private:
		ButtonA *m_addButton;
		ButtonA *m_backButton;
		int m_contentSize;
		bool m_isAdding;
		bool m_isReducing;
		int m_lineSize;
		int m_pageSize;
		int m_pos;
		ButtonA *m_reduceButton;
		ButtonA *m_scrollButton;
		int m_tick;
		int m_timerID;
	private:
		ControlMouseEvent m_addButtonMouseDownEvent;
		ControlMouseEvent m_addButtonMouseUpEvent;
		ControlMouseEvent m_reduceButtonMouseDownEvent;
		ControlMouseEvent m_reduceButtonMouseUpEvent;
		ControlEvent m_scrollButtonDraggingEvent;
		static void AddButtonMouseDown(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke);
		static void AddButtonMouseUp(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke);
		static void ReduceButtonMouseDown(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke);
		static void ReduceButtonMouseUp(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke);
		static void ScrollButtonDragging(void *sender, void *pInvoke);
	public:
		ScrollBarA();
		virtual ~ScrollBarA();
		ButtonA* GetAddButton();
		ButtonA* GetBackButton();
		int GetContentSize();
		void SetContentSize(int contentWidth);
		bool IsAdding();
		void SetIsAdding(bool isAdding);
		bool IsReducing();
		void SetIsReducing(bool isReducing);
		int GetLineSize();
		void SetLineSize(int lineSize);
		int GetPageSize();
		void SetPageSize(int pageSize);
		int GetPos();
		void SetPos(int pos);
		ButtonA* GetReduceButton();
		ButtonA* GetScrollButton();
	public:
		virtual String GetControlType();
		virtual void LineAdd();
		virtual void LineReduce();
		void OnAddButtonMouseDown(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		void OnAddButtonMouseUp(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnDragScroll();
		virtual void OnLoad();
		void OnReduceButtonMouseDown(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		void OnReduceButtonMouseUp(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnScrolled();
		virtual void OnVisibleChanged();
		virtual void PageAdd();
		virtual void PageReduce();
		virtual void ScrollToBegin();
		virtual void ScrollToEnd();
		virtual void OnTimer(int timerID);
	};
}

#endif