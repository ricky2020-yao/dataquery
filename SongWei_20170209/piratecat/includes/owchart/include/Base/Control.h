/*****************************************************************************\
*                                                                             *
* Control.h - Base control functions, types, and definitions              *
*                                                                             *
*               Version 4.00 ¡ï¡ï¡ï¡ï¡ï                                       *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's owchart. All rights reserved. *
*               Check 2016/9/15 by Lord.                                      *
*                                                                             *
*******************************************************************************/

#ifndef __BASECONTROL_H__
#define __BASECONTROL_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "NativeBase.h"
#include "CPaint.h"
#include "CStr.h"
#include "CProperty.h"

namespace OwLib
{
	static int timerID = 0;
	class NativeBase;
	class ControlA;

	typedef void (*ControlEvent)(void*, void*);
	typedef void (*ControlInvokeEvent)(void*, void*, void*);
	typedef void (*ControlKeyEvent)(void*, char, void*);
	typedef void (*ControlMouseEvent)(void*, const POINT&, MouseButtonsA, int, int, void*);
	typedef void (*ControlPaintEvent)(void*, CPaint*, const RECT&, void*);
	typedef void (*ControlTimerEvent)(void*, int, void*);
	typedef void (*ControlTouchEvent)(void*, vector<CTouch>*, void*);

	class EVENTID
	{
	public:
        static const int ADD = 0;
        static const int AUTOSIZECHANGED = 1;
        static const int BACKCOLORCHANGED = 2;
        static const int BACKIMAGECHANGED = 3;
        static const int CHAR = 4;
        static const int CLICK = 5;
        static const int COPY = 6;
        static const int CUT = 7;
        static const int DOCKCHANGED = 8;
        static const int DOUBLECLICK = 9;
        static const int DRAGBEGIN = 10;
        static const int DRAGEND = 11;
        static const int DRAGGING = 12;
        static const int ENABLECHANGED = 13;
        static const int FONTCHANGED = 14;
        static const int FORECOLORCHANGED = 15;
        static const int GOTFOCUS = 16;
        static const int INVOKE = 17;
        static const int KEYDOWN = 18;
        static const int KEYUP = 19;
        static const int LOAD = 20;
        static const int LOCATIONCHANGED = 21;
        static const int LOSTFOCUS = 22;
        static const int MARGINCHANGED = 23;
        static const int MOUSEDOWN = 24;
        static const int MOUSEENTER = 25;
        static const int MOUSELEAVE = 26;
        static const int MOUSEMOVE = 27;
        static const int MOUSEUP = 28;
        static const int MOUSEWHEEL = 29;
        static const int PADDINGCHANGED = 30;
        static const int PARENTCHANGED = 31;
        static const int PAINT = 32;
        static const int PAINTBORDER = 33;
        static const int PASTE = 34;
        static const int REGIONCHANGED = 36;
        static const int REMOVE = 37;
        static const int SIZECHANGED = 38;
        static const int TABINDEXCHANGED = 39;
		static const int TABSTOP = 40;
        static const int TABSTOPCHANGED = 41;
        static const int TEXTCHANGED = 42;
        static const int TIMER = 43;
        static const int VISIBLECHANGED = 44;
        static const int CHECKEDCHANGED = 45;
        static const int SELECTEDDAYCHANGED = 46;
        static const int GRIDCELLCLICK = 47;
        static const int GRIDCELLEDITBEGIN = 48;
        static const int GRIDCELLEDITEND = 49;
        static const int GRIDCELLMOUSEDOWN = 50;
        static const int GRIDCELLMOUSEMOVE = 51;
        static const int GRIDCELLMOUSEUP = 52;
        static const int GRIDSELECTEDCELLSCHANGED = 53;
        static const int GRIDSELECTEDCOLUMNSSCHANGED = 54;
        static const int GRIDSELECTEDROWSCHANGED = 55;
        static const int MENUITEMCLICK = 56;
        static const int SELECTEDINDEXCHANGED = 57;
        static const int SELECTEDTABPAGECHANGED = 58;
        static const int SCROLLED = 59;
        static const int VALUECHANGED = 60;
        static const int WINDOWCLOSED = 61;
        static const int WINDOWCLOSING = 62;
		static const int TOUCHBEGIN = 10000;
		static const int TOUCHMOVE = 10001;
		static const int TOUCHEND = 10002;
		static const int TOUCHCANCEL = 10003;
        static const int USER = 100000;
	};

	class ControlA : public CProperty
	{
	protected:
		bool m_allowDrag;
		bool m_allowPreviewsEvent;
		ANCHOR m_anchor;
		bool m_autoEllipsis;
		bool m_autoSize;
		_int64 m_backColor;
		String m_backImage;
		_int64 m_borderColor;
		bool m_canFocus;
		bool m_canRaiseEvents;
		CursorsA m_cursor;
		bool m_displayOffset;
		DockStyleA m_dock;
		bool m_enabled;
		bool m_focused;
		FONT *m_font;
		_int64 m_foreColor;
		bool m_isDragging;
		bool m_isWindow;
		POINT m_location;
		PADDING m_margin;
		SIZE m_maximumSize;
		SIZE m_minimumSize;
		String m_name;
		NativeBase *m_native;
		SIZE m_oldSize;
		float m_opacity;
		PADDING m_padding;
		ControlA *m_parent;
		RECT m_region;
		String m_resourcePath;
		SIZE m_size;
		int m_tabIndex;
		bool m_tabStop;
		void *m_tag;
		String m_text;
		bool m_topMost;
		bool m_useRegion;
		bool m_visible;
	protected:
		map<int, vector<void*>*> m_events;
		map<int, vector<void*>*> m_invokes;
		void CallEvents(int eventID);
		void CallInvokeEvents(int eventID, void *args);
		void CallKeyEvents(int eventID, char key);
		void CallMouseEvents(int eventID, const POINT& mp, MouseButtonsA button, int clicks, int delta);
		void CallPaintEvents(int eventID, CPaint *paint, const RECT& clipRect);
		void CallTimerEvents(int eventID, int timerID);
		void CallTouchEvents(int eventID, vector<CTouch> *touches);
        virtual _int64 GetPaintingBackColor();
        virtual String GetPaintingBackImage();
        virtual _int64 GetPaintingBorderColor();
        virtual _int64 GetPaintingForeColor();
		bool IsPaintEnabled(ControlA *control);
	public:
		vector<ControlA*> m_controls;
		ControlA();
		virtual ~ControlA();
		bool AllowDrag();
		void SetAllowDrag(bool allowDrag);
		bool AllowPreviewsEvent();
		void SetAllowPreviewsEvent(bool allowPreviewsEvent);
		ANCHOR GetAnchor();
		void SetAnchor(const ANCHOR& anchor);
		bool IsAutoEllipsis();
		void SetAutoEllipsis(bool autoEllipsis);
		bool IsAutoSize();
		void SetAutoSize(bool autoSize);
		_int64 GetBackColor();
		void SetBackColor(_int64 backColor);
		String GetBackImage();
		void SetBackImage(const String& backImage);
		_int64 GetBorderColor();
		void SetBorderColor(_int64 borderColor);
		int GetBottom();
		RECT GetBounds();
		void SetBounds(const RECT& rect);
		bool CanFocus();
		void SetCanFocus(bool canFocus);
		bool CanRaiseEvents();
		void SetCanRaiseEvents(bool canRaiseEvents);
		bool IsCapture();
		CursorsA GetCursor();
		void SetCursor(CursorsA cursor);
		bool IsDisplayOffset();
		void SetDisplayOffset(bool displayOffset);
		RECT GetDisplayRect();
		DockStyleA GetDock();
		void SetDock(DockStyleA dock);
		bool IsEnabled();
		void SetEnabled(bool enabled);
		bool IsFocused();
		void SetFocused(bool focused);
		FONT* GetFont();
		void SetFont(FONT *font);
		_int64 GetForeColor();
		void SetForeColor(_int64 foreColor);
		int GetHeight();
		void SetHeight(int height);
		bool IsDragging();
		bool IsWindow();
		void SetWindow(bool isWindow);
		int GetLeft();
		void SetLeft(int left);
		POINT GetLocation();
		void SetLocation(const POINT& location);
		PADDING GetMargin();
		void SetMargin(const PADDING& margin);
		SIZE GetMaximumSize();
		void SetMaximumSize(SIZE maxinumSize);
		SIZE GetMinimumSize();
		void SetMinimumSize(SIZE minimumSize);
		POINT GetMousePoint();
		String GetName();
		void SetName(const String& name);
		NativeBase* GetNative();
		void SetNative(NativeBase *native);
		float GetOpacity();
		void SetOpacity(float opacity);
		PADDING GetPadding();
		void SetPadding(const PADDING& padding);
		ControlA* GetParent();
		void SetParent(ControlA *control);
		RECT GetRegion();
		void SetRegion(const RECT& region);
		String GetResourcePath();
		void SetResourcePath(const String& resourcePath);
		int GetRight();
		SIZE GetSize();
		void SetSize(const SIZE& size);
		int GetTabIndex();
		void SetTabIndex(int tabIndex);
		bool IsTabStop();
		void SetTabStop(bool tabStop);
		void* GetTag();
		void SetTag(void *tag);
		String GetText();
		void SetText(const String& text);
		int GetTop();
		void SetTop(int top);
		bool IsTopMost();
		void SetTopMost(bool topMost);
		bool UseRegion();
		bool IsVisible();
		void SetVisible(bool visible);
		int GetWidth();
		void SetWidth(int width);
	public:
		virtual void AddControl(ControlA *control);
		virtual void BeginInvoke(void *args);
		virtual void BringChildToFront(ControlA *childControl);
		virtual void BringToFront();
		virtual void ClearControls();
		virtual bool ContainsControl(ControlA *control);
		virtual bool ContainsPoint(const POINT& mp);
		virtual void Focus();
		virtual vector<ControlA*> GetControls();
		virtual String GetControlType();
		virtual POINT GetDisplayOffset();
		static int GetNewTimerID();
		virtual void GetProperty(const String& name, String *value, String *type);
		virtual vector<String> GetPropertyNames();
		virtual bool HasChildren();
		virtual void Hide();
		virtual void InsertControl(int index, ControlA *control);
		virtual void Invalidate();
		virtual void Invoke(void *args);
		virtual void OnAdd();
		virtual void OnAutoSizeChanged();
		virtual void OnBackColorChanged();
		virtual void OnBackImageChanged();
		virtual void OnChar(wchar_t ch);
		virtual void OnClick(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnCopy();
		virtual void OnCut();
		virtual void OnDockChanged();
		virtual void OnDoubleClick(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual bool OnDragBegin();
		virtual void OnDragEnd();
		virtual void OnDragging();
		virtual void OnDragReady(POINT *startOffset);
		virtual void OnEnableChanged();
		virtual void OnFontChanged();
		virtual void OnForeColorChanged();
		virtual void OnGotFocus();
		virtual void OnInvoke(void *args);
		virtual void OnLoad();
		virtual void OnLocationChanged();
		virtual void OnLostFocus();
		virtual void OnKeyDown(char key);
		virtual void OnKeyUp(char key);
		virtual void OnMarginChanged();
		virtual void OnMouseDown(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnMouseEnter(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnMouseLeave(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnMouseMove(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnMouseUp(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnMouseWheel(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnPaddingChanged();
		virtual void OnPaint(CPaint *paint, const RECT& clipRect);
		virtual void OnPaintBackground(CPaint *paint, const RECT& clipRect);
		virtual void OnPaintBorder(CPaint *paint, const RECT& clipRect);
		virtual void OnPaintForeground(CPaint *paint, const RECT& clipRect);
		virtual void OnParentChanged();
		virtual void OnPaste();
		virtual bool OnPreviewsKeyEvent(int eventID, char key);
		virtual bool OnPreviewsMouseEvent(int eventID, const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnRegionChanged();
		virtual void OnRemove();
		virtual void OnSizeChanged();
		virtual void OnTabIndexChanged();
		virtual void OnTabStop();
		virtual void OnTabStopChanged();
		virtual void OnTextChanged();
		virtual void OnTimer(int timerID);
		virtual void OnTouchBegin(vector<CTouch> *touches);
		virtual void OnTouchCancel(vector<CTouch> *touches);
		virtual void OnTouchEnd(vector<CTouch> *touches);
		virtual void OnTouchMove(vector<CTouch> *touches);
		virtual void OnVisibleChanged();
		virtual POINT PointToControl(const POINT& mp);
		virtual POINT PointToNative(const POINT& mp);
		virtual void RegisterEvent(void *func, int eventID, void *pInvoke);
		virtual void RemoveControl(ControlA *control);
		virtual void SetProperty(const String& name, const String& value);
		virtual void Show();
		virtual void StartTimer(int timerID, int interval);
		virtual void StopTimer(int timerID);
		virtual void SendChildToBack(ControlA *childControl);
		virtual void SendToBack();
		virtual void UnRegisterEvent(void *func, int eventID);
		virtual void Update();
	};
}
#endif