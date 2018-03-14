/*****************************************************************************\
*                                                                             *
* TextBoxA.cs -  Text box functions, types, and definitions                  *
*                                                                             *
*               Version 4.00 ¡ï¡ï¡ï¡ï¡ï                                       *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's text box. All rights reserved.*
*               Checked 2016/9/25 by Lord.                                    *
*                                                                             *
*******************************************************************************/

#ifndef __TEXTBOX_H__
#define __TEXTBOX_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "..\\ScrollBar\\HScrollBar.h"
#include "..\\ScrollBar\\VScrollBar.h"
#include "..\\Layout\\Div.h"
#include <stack>

namespace OwLib
{
	struct WordLine
	{
	public:
		int m_end;
		int m_start;
		WordLine(int start, int end)
		{
			m_start = start;
			m_end = end;
		}
	};

	class TextBoxA : public DivA
	{
	protected:
		bool m_isKeyDown;
		bool m_isMouseDown;
		int m_lineHeight;
		vector<WordLine> m_lines;
		bool m_multiline;
		int m_offsetX;
		wchar_t m_passwordChar;
		vector<RECTF> m_ranges;
		stack<String> m_redoStack;
		bool m_readOnly;
		bool m_rightToLeft;
		_int64 m_selectionBackColor;
		_int64 m_selectionForeColor;
		int m_selectionLength;
		int m_selectionStart;
		bool m_showCursor;
		int m_startMovingIndex;
		int m_stopMovingIndex;
		HorizontalAlignA m_textAlign;
		bool m_textChanged;
		int m_tick;
		int TICK;
		int m_timerID;
		stack<String> m_undoStack;
		bool m_wordWrap;
		void CursorDown();
		void CursorEnd();
		void CursorHome();
		void CursorLeft();
		void CursorRight();
		void CursorUp();
		void DeleteWord();
		void InsertWord(const String& str);
		bool IsLineVisible(int index, double visiblePercent);
		void SetMovingIndex(int sIndex, int eIndex);
	public:
		TextBoxA();
		virtual ~TextBoxA();
		int GetLinesCount();
		int GetLineHeight();
		void SetLineHeight(int lineHeight);
		vector<WordLine> GetLines();
		bool IsMultiline();
		void SetMultiline(bool multiline);
		wchar_t GetPasswordChar();
		void SetPasswordChar(wchar_t passwordChar);
		bool IsReadOnly();
		void SetReadOnly(bool readOnly);
		bool IsRightToLeft();
		void SetRightToLeft(bool rightToLeft);
		_int64 GetSelectionBackColor();
		void SetSelectionBackColor(_int64 selectionBackColor);
		_int64 GetSelectionForeColor();
		void SetSelectionForeColor(_int64 selectionForeColor);
		int GetSelectionLength();
		void SetSelectionLength(int selectionLength);
		int GetSelectionStart();
		void SetSelectionStart(int selectionStart);
		HorizontalAlignA GetTextAlign();
		void SetTextAlign(HorizontalAlignA textAlign);
		bool IsWordWrap();
		void SetWordWrap(bool wordWrap);
	public:
		bool CanRedo();
		bool CanUndo();
		void ClearRedoUndo();
		virtual int GetContentHeight();
		virtual int GetContentWidth();
		virtual String GetControlType();
		virtual void GetProperty(const String& name, String *value, String *type);
		virtual vector<String> GetPropertyNames();
		String GetSelectionText();
		virtual void OnChar(wchar_t ch);
		virtual void OnCopy();
		virtual void OnCut();
		virtual void OnGotFocus();
		virtual void OnKeyDown(char key);
		virtual void OnKeyUp(char key);
		virtual void OnLostFocus();
		virtual void OnMouseDown(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnMouseMove(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnMouseUp(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnPaintForeground(CPaint *paint, const RECT& clipRect);
		virtual void OnPaste();
		virtual void OnSizeChanged();
		virtual void OnTabStop();
		virtual void OnTextChanged();
		virtual void OnTimer(int timerID);
		void Redo();
		void SelectAll();
		virtual void SetProperty(const String& name, const String& value);
		virtual String TextValidation(const String& text);
		void Undo();
		virtual void Update();
	};
}

#endif