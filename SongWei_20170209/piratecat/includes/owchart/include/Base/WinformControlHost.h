/*****************************************************************************\
*                                                                             *
* WinformControlHost.h -Winform control host, types, and definitions          *
*                                                                             *
*               Version 4.00 ¡ï¡ï¡ï¡ï¡ï                                       *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's owchart. All rights reserved. *
*                                                                             *
*******************************************************************************/

#ifndef __WINFORMCONTROLHOST_H__
#define __WINFORMCONTROLHOST_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "Control.h"
#include "CStr.h"
#include "CPaint.h"
#include "CMathLib.h"
#include "GdiPaint.h"
#include "GdiPlusPaint.h"
#include "NativeBase.h"
#include "ControlHost.h"

namespace OwLib
{
	class NativeBase;

	class WinformControlHost : public ControlHost
	{
	private:
		CRITICAL_SECTION _csLock;
		bool m_allowOperate;
		bool m_allowPartialPaint;
		HIMC m_hImc;
		HWND m_hWnd;
		int m_invokeSerialID;
		map<int, void*> m_invokeArgs;
		map<int, ControlA*> m_invokeControls;
		NativeBase *m_native;
		int m_pInvokeMsgID;
	private:
		SIZE GetClientSize();
	public:
		WinformControlHost();
		virtual ~WinformControlHost();
		HWND GetHWnd();
		void SetHWnd(HWND hWnd);
		virtual NativeBase* GetNative();
		virtual void SetNative(NativeBase *native);
		int GetPInvokeMsgID();
		void SetPInvokeMsgID(int pInvokeMsgID);
	public:
		virtual bool AllowOperate();
		virtual bool AllowPartialPaint();
		virtual void BeginInvoke(ControlA *control, void *args);
		virtual void Copy(string text);
		virtual ControlA* CreateInternalControl(ControlA *parent, const String& clsid);
		static string GetAppPath();
		virtual CursorsA GetCursor();
		virtual int GetIntersectRect(LPRECT lpDestRect, const RECT *lpSrc1Rect, const RECT *lpSrc2Rect);
        virtual POINT GetMousePoint();
        virtual SIZE GetSize();
		virtual int GetUnionRect(LPRECT lpDestRect, const RECT *lpSrc1Rect, const RECT *lpSrc2Rect);
		void Lock();
		virtual void Invalidate();
        virtual void Invalidate(const RECT& rect);
		virtual void Invoke(ControlA *control, void *args);
		virtual bool IsKeyPress(char key);
		void OnInvoke(int invokeSerialID);
		int OnMessage(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam);
		void OnPaint(HDC hDC, const RECT& rect);
		virtual string Paste();
		virtual void SetAllowOperate(bool allowOperate);
		virtual void SetAllowPartialPaint(bool allowPartialPaint);
        virtual void SetCursor(CursorsA cursor);
        virtual void StartTimer(int timerID, int interval);
        virtual void StopTimer(int timerID);
		void UnLock();
	};
}
#endif