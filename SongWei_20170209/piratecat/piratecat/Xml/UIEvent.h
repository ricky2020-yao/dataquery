/*****************************************************************************\
*                                                                             *
* UIEvent.h -    Event functions, types, and definitions                      *
*                                                                             *
*               Version 4.00 ¡ï¡ï¡ï                                           *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's UI. All rights reserved.      *
*                                                                             *
*******************************************************************************/

#ifndef __UIEVENT_H__
#define __UIEVENT_H__
#pragma once
#include "UIScript.h"
#include "UIXml.h"

namespace OwLib
{
	class UIScript;
	class UIXml;

	class EventInfo
	{
	private:
		map<int, String> m_functions;
	public:
		EventInfo();
		virtual ~EventInfo();
	public:
		void AddEvent(int eventID, const String& function);
		String GetFunction(int eventID);
		void RemoveEvent(int eventID);
	};

	class UIEvent
	{
	private:
		UIScript *m_script;
		String m_sender;
		UIXml *m_xml;
	public:
		map<ControlA*, EventInfo*> m_events;
	public:
		UIEvent(UIXml *xml);
		virtual ~UIEvent();
		UIScript* GetScript();
		void SetScript(UIScript *script);
		String GetSender();
		void SetSender(const String& sender);
		UIXml* GetXml();
		void SetXml(UIXml *xml);
	public:
        static void CallAdd(void *sender, void *pInvoke);
        static void CallChar(void *sender, char ch, void *pInvoke);
        static void CallCheckedChanged(void *sender, void *pInvoke);
        static void CallClick(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke);
        static void CallCopy(void *sender, void *pInvoke);
        static void CallDragBegin(void *sender, void *pInvoke);
        static void CallDragEnd(void *sender, void *pInvoke);
        static void CallDragging(void *sender, void *pInvoke);
        static void CallEnableChanged(void *sender, void *pInvoke);
        static String CallFunction(void *sender, int eventID, void *pInvoke);
        static void CallGotFocus(void *sender, void *pInvoke);
        static void CallInvoke(void *sender, void *args, void *pInvoke);
        static void CallLoad(void *sender, void *pInvoke);
        static void CallLocationChanged(void *sender, void *pInvoke);
        static void CallLostFocus(void *sender, void *pInvoke);
        static void CallKeyDown(void *sender, char key, void *pInvoke);
        static void CallKeyUp(void *sender, char key, void *pInvoke);
        static void CallMenuItemClick(void *sender, MenuItemA *item, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke);
        static void CallMouseDown(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke);
        static void CallMouseEnter(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke);
        static void CallMouseLeave(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke);
        static void CallMouseMove(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke);
        static void CallMouseUp(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke);
        static void CallMouseWheel(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke);
        static void CallPaint(void *sender, CPaint *paint, const RECT& clipRect, void *pInvoke);
        static void CallPaintBorder(void *sender, CPaint *paint, const RECT& clipRect, void *pInvoke);
        static void CallPaste(void *sender, void *pInvoke);
        static void CallRemove(void *sender, void *pInvoke);
        static void CallSelectedDayChanged(void *sender, void *pInvoke);
        static void CallSelectedIndexChanged(void *sender, void *pInvoke);
        static void CallSelectedTabPageChanged(void *sender, void *pInvoke);
        static void CallScrolled(void *sender, void *pInvoke);
        static void CallSizeChanged(void *sender, void *pInvoke);
        static void CallTextChanged(void *sender, void *pInvoke);
        static void CallTimer(void *sender, int timerID, void *pInvoke);
        static void CallVisibleChanged(void *sender, void *pInvoke);
        static void CallValueChanged(void *sender, void *pInvoke);
		virtual int GetEventID(const String& eventName);
		virtual void RegisterEvent(ControlA *control, const String& eventName, const String& function);
	};
}
#endif