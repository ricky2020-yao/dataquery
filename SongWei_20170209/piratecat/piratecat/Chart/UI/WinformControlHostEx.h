/*********************************************************************************\
*                                                                                 *
* WinformControlHostEx.h -Winform control host functions, types, and definitions.*
*                                                                                 *
*               Version 6.00                                                      *
*                                                                                 *
*               Copyright (c) 2016-2016, Pirate. All rights reserved.             *
*               Created by Lord 2016/12/2.                                        *
*                                                                                 *
**********************************************************************************/

#ifndef __WINFORMCONTROLHOSTEX_H__
#define __WINFORMCONTROLHOSTEX_H__
#pragma once
#include "RibbonButton.h"
#include "GdiPlusPaintEx.h"

class WinformControlHostEx : public WinformControlHost
{
public:
	WinformControlHostEx();
	virtual ~WinformControlHostEx();
public:
	virtual ControlA* CreateInternalControl(ControlA *parent, const String& clsid);
	virtual void Execute(String cmd);
	virtual String GetText();
	virtual void MouseEvent(String eventID, int dx, int dy, int data);
	virtual void SendKey(String key);
	virtual void SetText(String text);
};
#endif